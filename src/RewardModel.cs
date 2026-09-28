// SPDX-License-Identifier: MPL-2.0

using System;
using System.Collections.Generic;
using System.Text;

namespace StudyRewardInsight
{
    internal enum TooltipLanguage
    {
        Russian,
        English
    }

    internal enum RewardMagnitude
    {
        Low,
        Medium,
        High,
        VeryHigh
    }

    internal sealed class RewardSnapshot
    {
        internal int Red;
        internal int Green;
        internal int Blue;

        internal int Points(string pointId)
        {
            switch (pointId)
            {
                case "r": return Red;
                case "g": return Green;
                case "b": return Blue;
                default: return 0;
            }
        }

        internal bool HasPositiveReward
        {
            get { return Red > 0 || Green > 0 || Blue > 0; }
        }
    }

    internal static class RewardModel
    {
        private const string AtomicGap = "\u2009\u2009";

        private sealed class RewardGroup
        {
            internal RewardGroup(RewardMagnitude magnitude)
            {
                Magnitude = magnitude;
            }

            internal RewardMagnitude Magnitude { get; }
            internal List<string> PointIds { get; } = new List<string>();
        }

        internal static RewardMagnitude Magnitude(string pointId, int points)
        {
            if (points <= 0)
                throw new ArgumentOutOfRangeException(nameof(points));

            switch (pointId)
            {
                case "r":
                    if (points <= 12) return RewardMagnitude.Low;
                    if (points <= 40) return RewardMagnitude.Medium;
                    if (points <= 75) return RewardMagnitude.High;
                    return RewardMagnitude.VeryHigh;
                case "g":
                    if (points <= 12) return RewardMagnitude.Low;
                    if (points <= 25) return RewardMagnitude.Medium;
                    if (points <= 45) return RewardMagnitude.High;
                    return RewardMagnitude.VeryHigh;
                case "b":
                    if (points <= 12) return RewardMagnitude.Low;
                    if (points <= 37) return RewardMagnitude.Medium;
                    if (points <= 65) return RewardMagnitude.High;
                    return RewardMagnitude.VeryHigh;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pointId));
            }
        }

        internal static string BuildBlock(
            RewardSnapshot snapshot,
            bool hasAlchemyDecomposition,
            TooltipLanguage language)
        {
            if (snapshot == null || !snapshot.HasPositiveReward)
                return null;

            string[] pointOrder = { "r", "g", "b" };
            List<RewardGroup> groups = new List<RewardGroup>();

            foreach (string pointId in pointOrder)
            {
                int points = snapshot.Points(pointId);
                if (points <= 0)
                    continue;

                RewardMagnitude magnitude = Magnitude(pointId, points);
                RewardGroup group = null;

                foreach (RewardGroup candidate in groups)
                {
                    if (candidate.Magnitude == magnitude)
                    {
                        group = candidate;
                        break;
                    }
                }

                if (group == null)
                {
                    group = new RewardGroup(magnitude);
                    groups.Add(group);
                }

                group.PointIds.Add(pointId);
            }

            StringBuilder text = new StringBuilder();
            text.Append(Heading(language));

            bool firstGroup = true;
            foreach (RewardGroup group in groups)
            {
                text.Append(firstGroup ? " " : "\n");

                foreach (string pointId in group.PointIds)
                {
                    text.Append("(");
                    text.Append(pointId);
                    text.Append(")");
                }

                text.Append(AtomicGap);
                text.Append(Atomicize(MagnitudeLabel(group.Magnitude, language)));
                firstGroup = false;
            }

            if (hasAlchemyDecomposition)
            {
                text.Append("\n");
                text.Append(AlchemyLine(language));
            }

            return text.ToString();
        }

        internal static string MagnitudeLabel(
            RewardMagnitude magnitude,
            TooltipLanguage language)
        {
            if (language == TooltipLanguage.Russian)
            {
                switch (magnitude)
                {
                    case RewardMagnitude.Low: return "Низкая";
                    case RewardMagnitude.Medium: return "Средняя";
                    case RewardMagnitude.High: return "Высокая";
                    case RewardMagnitude.VeryHigh: return "Очень высокая";
                    default: throw new ArgumentOutOfRangeException(nameof(magnitude));
                }
            }

            if (language == TooltipLanguage.English)
            {
                switch (magnitude)
                {
                    case RewardMagnitude.Low: return "Low";
                    case RewardMagnitude.Medium: return "Medium";
                    case RewardMagnitude.High: return "High";
                    case RewardMagnitude.VeryHigh: return "Very High";
                    default: throw new ArgumentOutOfRangeException(nameof(magnitude));
                }
            }

            throw new ArgumentOutOfRangeException(nameof(language));
        }

        private static string Heading(TooltipLanguage language)
        {
            switch (language)
            {
                case TooltipLanguage.Russian:
                    return "Награда за исследование:";
                case TooltipLanguage.English:
                    return "Study reward:";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language));
            }
        }

        private static string AlchemyLine(TooltipLanguage language)
        {
            switch (language)
            {
                case TooltipLanguage.Russian:
                    return "Исследование позволит использовать этот предмет в алхимии.";
                case TooltipLanguage.English:
                    return "Studying unlocks an alchemy use.";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language));
            }
        }

        private static string Atomicize(string text)
        {
            return text.Replace(" ", AtomicGap);
        }
    }
}
