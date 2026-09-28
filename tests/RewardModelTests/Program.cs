// SPDX-License-Identifier: MPL-2.0

using System;

namespace StudyRewardInsight
{
    internal static class Program
    {
        private static int Main()
        {
            TestMagnitudeBoundaries();
            TestMagnitudeLabels();
            TestGroupingByMagnitude();
            TestSeparateMagnitudes();
            TestAlchemyLine();

            Console.WriteLine("RewardModel tests passed.");
            return 0;
        }

        private static void TestMagnitudeBoundaries()
        {
            AssertMagnitude("r", 1, RewardMagnitude.Low);
            AssertMagnitude("r", 12, RewardMagnitude.Low);
            AssertMagnitude("r", 13, RewardMagnitude.Medium);
            AssertMagnitude("r", 40, RewardMagnitude.Medium);
            AssertMagnitude("r", 41, RewardMagnitude.High);
            AssertMagnitude("r", 75, RewardMagnitude.High);
            AssertMagnitude("r", 76, RewardMagnitude.VeryHigh);

            AssertMagnitude("g", 12, RewardMagnitude.Low);
            AssertMagnitude("g", 13, RewardMagnitude.Medium);
            AssertMagnitude("g", 25, RewardMagnitude.Medium);
            AssertMagnitude("g", 26, RewardMagnitude.High);
            AssertMagnitude("g", 45, RewardMagnitude.High);
            AssertMagnitude("g", 46, RewardMagnitude.VeryHigh);

            AssertMagnitude("b", 12, RewardMagnitude.Low);
            AssertMagnitude("b", 13, RewardMagnitude.Medium);
            AssertMagnitude("b", 37, RewardMagnitude.Medium);
            AssertMagnitude("b", 38, RewardMagnitude.High);
            AssertMagnitude("b", 65, RewardMagnitude.High);
            AssertMagnitude("b", 66, RewardMagnitude.VeryHigh);
        }

        private static void TestMagnitudeLabels()
        {
            AssertEqual(
                "Низкая",
                RewardModel.MagnitudeLabel(RewardMagnitude.Low),
                "low label");
            AssertEqual(
                "Средняя",
                RewardModel.MagnitudeLabel(RewardMagnitude.Medium),
                "medium label");
            AssertEqual(
                "Высокая",
                RewardModel.MagnitudeLabel(RewardMagnitude.High),
                "high label");
            AssertEqual(
                "Очень высокая",
                RewardModel.MagnitudeLabel(RewardMagnitude.VeryHigh),
                "very high label");
        }

        private static void TestGroupingByMagnitude()
        {
            RewardSnapshot snapshot = new RewardSnapshot
            {
                Red = 10,
                Green = 5
            };

            string actual = RewardModel.BuildRussianBlock(
                snapshot,
                false);

            string expected =
                "Награда за исследование: (r)(g) Низкая";

            AssertEqual(
                expected,
                actual,
                "same magnitude groups regardless of former efficiency");
        }

        private static void TestSeparateMagnitudes()
        {
            RewardSnapshot snapshot = new RewardSnapshot
            {
                Red = 10,
                Green = 13
            };

            string actual = RewardModel.BuildRussianBlock(
                snapshot,
                false);

            string expected =
                "Награда за исследование: (r) Низкая\n"
                + "(g) Средняя";

            AssertEqual(
                expected,
                actual,
                "different magnitudes stay separate");
        }

        private static void TestAlchemyLine()
        {
            RewardSnapshot snapshot = new RewardSnapshot
            {
                Blue = 45
            };

            string actual = RewardModel.BuildRussianBlock(
                snapshot,
                true);

            string expected =
                "Награда за исследование: (b) Высокая\n"
                + "Открывает алхимическое разложение.";

            AssertEqual(expected, actual, "alchemy capability line");
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
