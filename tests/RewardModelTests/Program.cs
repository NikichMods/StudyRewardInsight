// SPDX-License-Identifier: MPL-2.0

using System;

namespace StudyRewardInsight
{
    internal static class Program
    {
        private static int Main()
        {
            TestMagnitudeBoundaries();
            TestModifierBoundaries();
            TestGrouping();
            TestSeparateAssessments();
            TestAlchemyLine();
            TestZeroFaithModifier();

            Console.WriteLine("RewardModel tests passed.");
            return 0;
        }

        private static void TestMagnitudeBoundaries()
        {
            AssertMagnitude("r", 1, RewardMagnitude.Small);
            AssertMagnitude("r", 12, RewardMagnitude.Small);
            AssertMagnitude("r", 13, RewardMagnitude.Moderate);
            AssertMagnitude("r", 40, RewardMagnitude.Moderate);
            AssertMagnitude("r", 41, RewardMagnitude.Large);
            AssertMagnitude("r", 75, RewardMagnitude.Large);
            AssertMagnitude("r", 76, RewardMagnitude.VeryLarge);

            AssertMagnitude("g", 12, RewardMagnitude.Small);
            AssertMagnitude("g", 13, RewardMagnitude.Moderate);
            AssertMagnitude("g", 25, RewardMagnitude.Moderate);
            AssertMagnitude("g", 26, RewardMagnitude.Large);
            AssertMagnitude("g", 45, RewardMagnitude.Large);
            AssertMagnitude("g", 46, RewardMagnitude.VeryLarge);

            AssertMagnitude("b", 12, RewardMagnitude.Small);
            AssertMagnitude("b", 13, RewardMagnitude.Moderate);
            AssertMagnitude("b", 37, RewardMagnitude.Moderate);
            AssertMagnitude("b", 38, RewardMagnitude.Large);
            AssertMagnitude("b", 65, RewardMagnitude.Large);
            AssertMagnitude("b", 66, RewardMagnitude.VeryLarge);
        }

        private static void TestModifierBoundaries()
        {
            AssertEqual(
                RewardModifier.Stingier,
                RewardModel.Modifier(14, 2, true),
                "ratio below 7.5");

            AssertEqual(
                RewardModifier.None,
                RewardModel.Modifier(15, 2, true),
                "ratio exactly 7.5");

            AssertEqual(
                RewardModifier.None,
                RewardModel.Modifier(25, 2, true),
                "ratio exactly 12.5");

            AssertEqual(
                RewardModifier.MoreGenerous,
                RewardModel.Modifier(26, 2, true),
                "ratio above 12.5");
        }

        private static void TestGrouping()
        {
            RewardSnapshot snapshot = new RewardSnapshot
            {
                Red = 10,
                Green = 10,
                Faith = 1
            };

            string actual = RewardModel.BuildRussianBlock(
                snapshot,
                false);

            string expected =
                "Награда за исследование:\n"
                + "(r)(g) Небольшая";

            AssertEqual(expected, actual, "identical assessments group");
        }

        private static void TestSeparateAssessments()
        {
            RewardSnapshot snapshot = new RewardSnapshot
            {
                Red = 10,
                Green = 5,
                Faith = 1
            };

            string actual = RewardModel.BuildRussianBlock(
                snapshot,
                false);

            string expected =
                "Награда за исследование:\n"
                + "(r) Небольшая\n"
                + "(g) Небольшая, скупее обычного";

            AssertEqual(
                expected,
                actual,
                "different modifiers must not group");
        }

        private static void TestAlchemyLine()
        {
            RewardSnapshot snapshot = new RewardSnapshot
            {
                Blue = 45,
                Faith = 2
            };

            string actual = RewardModel.BuildRussianBlock(
                snapshot,
                true);

            string expected =
                "Награда за исследование:\n"
                + "(b) Большая, щедрее обычного\n"
                + "После изучения сгодится для алхимии.";

            AssertEqual(expected, actual, "alchemy capability line");
        }

        private static void TestZeroFaithModifier()
        {
            RewardSnapshot snapshot = new RewardSnapshot
            {
                Blue = 100,
                Faith = 0
            };

            string actual = RewardModel.BuildRussianBlock(
                snapshot,
                false);

            string expected =
                "Награда за исследование:\n"
                + "(b) Очень большая";

            AssertEqual(expected, actual, "zero Faith modifier omitted");
        }

        private static void AssertMagnitude(
            string pointId,
            int points,
            RewardMagnitude expected)
        {
            AssertEqual(
                expected,
                RewardModel.Magnitude(pointId, points),
                pointId + " magnitude " + points);
        }

        private static void AssertEqual<T>(
            T expected,
            T actual,
            string name)
        {
            if (!Equals(expected, actual))
            {
                throw new InvalidOperationException(
                    name
                    + ": expected ["
                    + expected
                    + "] but got ["
                    + actual
                    + "].");
            }
        }
    }
}
