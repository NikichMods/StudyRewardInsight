// SPDX-License-Identifier: MPL-2.0

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace StudyRewardInsight
{
    internal static class StudyTooltip
    {
        private const string CircumspectExpression =
            "1*Ppar(\"buff_survay\")";

        private static readonly HashSet<string> WarnedUnsupportedCrafts =
            new HashSet<string>(StringComparer.Ordinal);

        internal static void Apply(
            object itemDefinition,
            object item,
            bool fullDetail,
            object result)
        {
            TooltipLanguage language;

            if (!fullDetail
                || itemDefinition == null
                || item == null
                || result == null
                || !TryGetLanguage(out language))
            {
                return;
            }

            IList rows = result as IList;
            if (rows == null)
                return;

            object surveyRow = FindNativeIncompleteSurveyRow(rows);
            if (surveyRow == null)
                return;

            object surveyCraft = GameApi.GetSurveyCraft(itemDefinition);
            if (surveyCraft == null)
                return;

            if (string.Equals(
                GameApi.EnumName(surveyCraft, "sub_type"),
                "SurveySciencePoints",
                StringComparison.Ordinal))
            {
                return;
            }

            RewardSnapshot snapshot;
            string unsupportedReason;
            if (!TryReadRewardSnapshot(
                surveyCraft,
                out snapshot,
                out unsupportedReason))
            {
                WarnUnsupportedOnce(
                    GameApi.Id(surveyCraft),
                    unsupportedReason);
                return;
            }

            if (!snapshot.HasPositiveReward)
                return;

            bool hasAlchemy =
                GameApi.GetAlchemyDecompositionCount(itemDefinition) > 0;

            string block = RewardModel.BuildBlock(
                snapshot,
                hasAlchemy,
                language);

            if (string.IsNullOrEmpty(block))
                return;

            GameApi.Set(surveyRow, "text", block);
        }

        private static object FindNativeIncompleteSurveyRow(IList rows)
        {
            string survey = GameApi.Localize("survey");
            string notComplete = GameApi.Localize("survey_not_complete");

            if (string.IsNullOrEmpty(survey)
                || string.IsNullOrEmpty(notComplete))
            {
                return null;
            }

            object found = null;

            foreach (object row in rows)
            {
                if (row == null
                    || !string.Equals(
                        row.GetType().Name,
                        "BubbleWidgetTextData",
                        StringComparison.Ordinal))
                {
                    continue;
                }

                string text = GameApi.Get(row, "text") as string;
                if (string.IsNullOrEmpty(text)
                    || text.IndexOf(survey, StringComparison.Ordinal) < 0
                    || text.IndexOf(notComplete, StringComparison.Ordinal) < 0)
                {
                    continue;
                }

                if (found != null)
                    return null;

                found = row;
            }

            return found;
        }

        private static bool TryReadRewardSnapshot(
            object surveyCraft,
            out RewardSnapshot snapshot,
            out string unsupportedReason)
        {
            snapshot = new RewardSnapshot();
            unsupportedReason = null;

            IList output = GameApi.List(surveyCraft, "output");
            if (output == null)
            {
                unsupportedReason = "Survey output is unavailable.";
                return false;
            }

            foreach (object reward in output)
            {
                string id = GameApi.Id(reward);
                if (id != "r" && id != "g" && id != "b")
                    continue;

                int points;
                string reason;
                if (!TryReadPointValue(
                    reward,
                    id,
                    out points,
                    out reason))
                {
                    unsupportedReason = reason;
                    return false;
                }

                AddPoints(snapshot, id, points);
            }

            return true;
        }

        private static bool TryReadPointValue(
            object reward,
            string id,
            out int points,
            out string reason)
        {
            points = 0;
            reason = null;

            if (IsFixedEntry(reward))
            {
                int value = GameApi.IntValue(reward, "value", 0);
                points = Math.Max(0, value);
                return true;
            }

            if (IsCircumspectBlueEntry(reward, id))
            {
                float buff;
                if (!GameApi.TryGetPlayerParam(
                    "buff_survay",
                    out buff))
                {
                    reason =
                        "Circumspect state is unavailable.";
                    return false;
                }

                if (Math.Abs(buff) <= 0.001f)
                {
                    points = 0;
                    return true;
                }

                if (Math.Abs(buff - 1f) <= 0.001f)
                {
                    points = 1;
                    return true;
                }

                reason =
                    "Circumspect parameter is outside the accepted 0/1 envelope.";
                return false;
            }

            reason =
                "Unsupported dynamic R/G/B Survey output shape.";
            return false;
        }

        private static bool IsCircumspectBlueEntry(
            object reward,
            string id)
        {
            if (id != "b"
                || GameApi.IntValue(reward, "value", 0) != 1
                || GameApi.IntValue(reward, "chance_group", -2) != -1)
            {
                return false;
            }

            return string.IsNullOrEmpty(
                    GameApi.RawExpression(
                        GameApi.Get(reward, "self_chance")))
                && string.IsNullOrEmpty(
                    GameApi.RawExpression(
                        GameApi.Get(reward, "common_chance")))
                && string.Equals(
                    NormalizeExpression(
                        GameApi.RawExpression(
                            GameApi.Get(reward, "min_value"))),
                    CircumspectExpression,
                    StringComparison.Ordinal)
                && string.IsNullOrEmpty(
                    GameApi.RawExpression(
                        GameApi.Get(reward, "max_value")));
        }

        private static bool IsFixedEntry(object entry)
        {
            return entry != null
                && GameApi.IntValue(
                    entry,
                    "chance_group",
                    -2) == -1
                && string.IsNullOrEmpty(
                    GameApi.RawExpression(
                        GameApi.Get(entry, "self_chance")))
                && string.IsNullOrEmpty(
                    GameApi.RawExpression(
                        GameApi.Get(entry, "common_chance")))
                && string.IsNullOrEmpty(
                    GameApi.RawExpression(
                        GameApi.Get(entry, "min_value")))
                && string.IsNullOrEmpty(
                    GameApi.RawExpression(
                        GameApi.Get(entry, "max_value")));
        }

        private static void AddPoints(
            RewardSnapshot snapshot,
            string id,
            int points)
        {
            if (points <= 0)
                return;

            switch (id)
            {
                case "r":
                    snapshot.Red += points;
                    break;
                case "g":
                    snapshot.Green += points;
                    break;
                case "b":
                    snapshot.Blue += points;
                    break;
            }
        }

        private static bool TryGetLanguage(
            out TooltipLanguage language)
        {
            return TooltipLocalization.TryResolve(
                GameApi.CurrentLanguage(),
                out language);
        }

        private static string NormalizeExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return string.Empty;

            StringBuilder normalized =
                new StringBuilder(expression.Length);

            foreach (char ch in expression)
            {
                if (!char.IsWhiteSpace(ch))
                    normalized.Append(ch);
            }

            return normalized.ToString();
        }

        private static void WarnUnsupportedOnce(
            string craftId,
            string reason)
        {
            string key = string.IsNullOrEmpty(craftId)
                ? "<unknown>"
                : craftId;

            if (!WarnedUnsupportedCrafts.Add(key))
                return;

            Plugin.Log.LogWarning(
                "Leaving vanilla Study tooltip unchanged for "
                + key
                + ": "
                + reason);
        }
    }
}
