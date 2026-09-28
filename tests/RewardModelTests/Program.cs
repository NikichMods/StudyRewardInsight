// SPDX-License-Identifier: MPL-2.0

using System;

namespace StudyRewardInsight
{
    internal static class Program
    {
        private const string AtomicGap = "\u2009\u2009";

        private static int Main()
        {
            TestMagnitudeBoundaries();
            TestMagnitudeLabels();
            TestGroupingByMagnitude();
            TestSeparateMagnitudes();
            TestAtomicVeryHighGroups();
            TestRussianAlchemyLine();
            TestEnglishBlock();

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
            AssertEqual("Низкая", RewardModel.MagnitudeLabel(
                RewardMagnitude.Low, TooltipLanguage.Russian), "Russian low label");
            AssertEqual("Средняя", RewardModel.MagnitudeLabel(
                RewardMagnitude.Medium, TooltipLanguage.Russian), "Russian medium label");
            AssertEqual("Высокая", RewardModel.MagnitudeLabel(
                RewardMagnitude.High, TooltipLanguage.Russian), "Russian high label");
            AssertEqual("Очень высокая", RewardModel.MagnitudeLabel(
                RewardMagnitude.VeryHigh, TooltipLanguage.Russian), "Russian very high label");

            AssertEqual("Low", RewardModel.MagnitudeLabel(
                RewardMagnitude.Low, TooltipLanguage.English), "English low label");
            AssertEqual("Medium", RewardModel.MagnitudeLabel(
                RewardMagnitude.Medium, TooltipLanguage.English), "English medium label");
            AssertEqual("High", RewardModel.MagnitudeLabel(
                RewardMagnitude.High, TooltipLanguage.English), "English high label");
            AssertEqual("Very High", RewardModel.MagnitudeLabel(
                RewardMagnitude.VeryHigh, TooltipLanguage.English), "English very high label");
        }

        private static void TestGroupingByMagnitude()
        {
            RewardSnapshot snapshot = new RewardSnapshot
            {
                Red = 10,
                Green = 5
            };

            string actual = RewardModel.BuildBlock(
                snapshot,
                false,
                TooltipLanguage.Russian);

            string expected =
                "Награда за исследование: (r)(g)"
                + AtomicGap
                + "Низкая";

            AssertEqual(expected, actual, "same magnitude groups");
        }

        private static void TestSeparateMagnitudes()
        {
            RewardSnapshot snapshot = new RewardSnapshot
            {
                Red = 10,
                Green = 13
            };

            string actual = RewardModel.BuildBlock(
                snapshot,
                false,
                TooltipLanguage.Russian);

            string expected =
                "Награда за исследование: (r)"
                + AtomicGap
                + "Низкая\n"
                + "(g)"
                + AtomicGap
                + "Средняя";

            AssertEqual(expected, actual, "different magnitudes stay separate");
        }

        private static void TestAtomicVeryHighGroups()
        {
            RewardSnapshot snapshot = new RewardSnapshot
            {
                Blue = 66
            };

            string russian = RewardModel.BuildBlock(
                snapshot,
                false,
                TooltipLanguage.Russian);

            string english = RewardModel.BuildBlock(
                snapshot,
                false,
                TooltipLanguage.English);

            AssertEqual(
                "Награда за исследование: (b)"
                + AtomicGap
                + "Очень"
                + AtomicGap
                + "высокая",
                russian,
                "Russian icon plus two-word label is atomic");

            AssertEqual(
                "Study reward: (b)"
                + AtomicGap
                + "Very"
                + AtomicGap
                + "High",
                english,
                "English icon plus two-word label is atomic");
        }

        private static void TestRussianAlchemyLine()
        {
            RewardSnapshot snapshot = new RewardSnapshot
            {
                Blue = 45
            };

            string actual = RewardModel.BuildBlock(
                snapshot,
                true,
                TooltipLanguage.Russian);

            string expected =
                "Награда за исследование: (b)"
                + AtomicGap
                + "Высокая\n"
                + "Исследование позволит использовать этот предмет в алхимии.";

            AssertEqual(expected, actual, "Russian alchemy capability line");
        }

        private static void TestEnglishBlock()
        {
            RewardSnapshot snapshot = new RewardSnapshot
            {
                Blue = 45
            };

            string actual = RewardModel.BuildBlock(
                snapshot,
                true,
                TooltipLanguage.English);

            string expected =
                "Study reward: (b)"
                + AtomicGap
                + "High\n"
                + "Studying unlocks an alchemy use.";

            AssertEqual(expected, actual, "English block");
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
