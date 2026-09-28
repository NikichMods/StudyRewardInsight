// SPDX-License-Identifier: MPL-2.0

using System;
using System.Collections.Generic;
using System.Text;

namespace StudyRewardInsight
{
    internal enum RewardMagnitude
    {
        Small,
        Moderate,
        Large,
        VeryLarge
    }

    internal enum RewardModifier
    {
        None,
        Stingier,
        MoreGenerous
    }

    internal sealed class RewardSnapshot
    {
        internal int Red;
        internal int Green;
        internal int Blue;
        internal int Faith;
        internal bool FaithKnown = true;

        internal int Points(string pointId)
        {
            switch (pointId)
            {
                case "r":
                    return Red;
                case "g":
                    return Green;
                case "b":
                    return Blue;
                default:
                    return 0;
            }
        }

        internal bool HasPositiveReward
        {
            get
            {
                return Red > 0 || Green > 0 || Blue > 0;
            }
        }
    }

    internal struct RewardAssessment : IEquatable<RewardAssessment>
    {
        internal RewardAssessment(
            RewardMagnitude magnitude,
            RewardModifier modifier)
        {
            Magnitude = magnitude;
            Modifier = modifier;
        }

        internal RewardMagnitude Magnitude { get; }
        internal RewardModifier Modifier { get; }

        public bool Equals(RewardAssessment other)
        {
            return Magnitude == other.Magnitude
                && Modifier == other.Modifier;
        }

        public override bool Equals(object obj)
        {
            return obj is RewardAssessment
                && Equals((RewardAssessment)obj);
        }

        public override int GetHashCode()
        {
            return ((int)Magnitude * 397) ^ (int)Modifier;
        }
    }

    internal static class RewardModel
    {
        private sealed class RewardGroup
        {
            internal RewardGroup(RewardAssessment assessment)
            {
                Assessment = assessment;
            }

            internal RewardAssessment Assessment { get; }
            internal List<string> PointIds { get; } =
                new List<string>();
        }

        internal static RewardAssessment Assess(
            string pointId,
            int points,
            int faith,
            bool faithKnown)
        {
            return new RewardAssessment(
                Magnitude(pointId, points),
                Modifier(points, faith, faithKnown));
        }

        internal static RewardMagnitude Magnitude(
            string pointId,
            int points)
        {
            if (points <= 0)
                throw new ArgumentOutOfRangeException(nameof(points));

            switch (pointId)
            {
                case "r":
                    if (points <= 12) return RewardMagnitude.Small;
                    if (points <= 40) return RewardMagnitude.Moderate;
                    if (points <= 75) return RewardMagnitude.Large;
                    return RewardMagnitude.VeryLarge;

                case "g":
                    if (points <= 12) return RewardMagnitude.Small;
                    if (points <= 25) return RewardMagnitude.Moderate;
                    if (points <= 45) return RewardMagnitude.Large;
                    return RewardMagnitude.VeryLarge;

                case "b":
                    if (points <= 12) return RewardMagnitude.Small;
                    if (points <= 37) return RewardMagnitude.Moderate;
                    if (points <= 65) return RewardMagnitude.Large;
                    return RewardMagnitude.VeryLarge;

                default:
                    throw new ArgumentOutOfRangeException(nameof(pointId));
            }
        }

        internal static RewardModifier Modifier(
            int points,
            int faith,
            bool faithKnown)
        {
            if (!faithKnown || faith <= 0 || points <= 0)
                return RewardModifier.None;

            double ratio = (double)points / faith;
            if (ratio < 7.5d)
                return RewardModifier.Stingier;
            if (ratio > 12.5d)
                return RewardModifier.MoreGenerous;
            return RewardModifier.None;
        }

        internal static string BuildRussianBlock(
            RewardSnapshot snapshot,
            bool hasAlchemyDecomposition)
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

                RewardAssessment assessment = Assess(
                    pointId,
                    points,
                    snapshot.Faith,
                    snapshot.FaithKnown);

                RewardGroup group = null;
                foreach (RewardGroup candidate in groups)
                {
                    if (candidate.Assessment.Equals(assessment))
                    {
                        group = candidate;
                        break;
                    }
                }

                if (group == null)
                {
                    group = new RewardGroup(assessment);
                    groups.Add(group);
                }

                group.PointIds.Add(pointId);
            }

            StringBuilder text = new StringBuilder();
            text.Append("Награда за исследование:");

            foreach (RewardGroup group in groups)
            {
                text.Append("\n");
                foreach (string pointId in group.PointIds)
                {
                    text.Append("(");
                    text.Append(pointId);
                    text.Append(")");
                }

                text.Append(" ");
                text.Append(MagnitudeLabel(group.Assessment.Magnitude));

                string modifier = ModifierLabel(group.Assessment.Modifier);
                if (!string.IsNullOrEmpty(modifier))
                {
                    text.Append(", ");
                    text.Append(modifier);
                }
            }

            if (hasAlchemyDecomposition)
            {
                text.Append("\n");
                text.Append("После изучения сгодится для алхимии.");
            }

            return text.ToString();
        }

        internal static string MagnitudeLabel(
            RewardMagnitude magnitude)
        {
            switch (magnitude)
            {
                case RewardMagnitude.Small:
                    return "Небольшая";
                case RewardMagnitude.Moderate:
                    return "Умеренная";
                case RewardMagnitude.Large:
                    return "Большая";
                case RewardMagnitude.VeryLarge:
                    return "Очень большая";
                default:
                    throw new ArgumentOutOfRangeException(nameof(magnitude));
            }
        }

        internal static string ModifierLabel(
            RewardModifier modifier)
        {
            switch (modifier)
            {
                case RewardModifier.None:
                    return string.Empty;
                case RewardModifier.Stingier:
                    return "скупее обычного";
                case RewardModifier.MoreGenerous:
                    return "щедрее обычного";
                default:
                    throw new ArgumentOutOfRangeException(nameof(modifier));
            }
        }
    }
}
