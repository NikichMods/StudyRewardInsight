// SPDX-License-Identifier: MPL-2.0

using System;

namespace StudyRewardInsight
{
    internal static class Program
    {
        private const string AtomicGap = "\u00A0";

        private static int Main()
        {
            TestMagnitudeBoundaries();
            TestLocaleResolution();
            TestMagnitudeLabels();
            TestLocalizedBlocks();
            TestGroupingByMagnitude();
            TestSeparateMagnitudes();
            TestAtomicVeryHighGroups();

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

        private static void TestLocaleResolution()
        {
            AssertLocale("en", TooltipLanguage.English);
            AssertLocale("de", TooltipLanguage.German);
            AssertLocale("fr", TooltipLanguage.French);
            AssertLocale("pt-br", TooltipLanguage.BrazilianPortuguese);
            AssertLocale("es", TooltipLanguage.Spanish);
            AssertLocale("ru", TooltipLanguage.Russian);
            AssertLocale("it", TooltipLanguage.Italian);
            AssertLocale("pl", TooltipLanguage.Polish);
            AssertLocale("ja", TooltipLanguage.Japanese);
            AssertLocale("zh_cn", TooltipLanguage.SimplifiedChinese);
            AssertLocale("ko", TooltipLanguage.Korean);

            AssertLocale("en-US", TooltipLanguage.English);
            AssertLocale("ru_RU", TooltipLanguage.Russian);
            AssertLocale("pt_BR", TooltipLanguage.BrazilianPortuguese);
            AssertLocale("zh-Hans", TooltipLanguage.SimplifiedChinese);

            TooltipLanguage ignored;
            AssertEqual(
                false,
                TooltipLocalization.TryResolve("xx", out ignored),
                "unsupported locale");
        }

        private static void TestMagnitudeLabels()
        {
            AssertLabels(
                TooltipLanguage.English,
                "Low", "Medium", "High", "Very High");
            AssertLabels(
                TooltipLanguage.German,
                "Niedrig", "Mittel", "Hoch", "Sehr hoch");
            AssertLabels(
                TooltipLanguage.French,
                "Faible", "Moyenne", "Élevée", "Très élevée");
            AssertLabels(
                TooltipLanguage.BrazilianPortuguese,
                "Baixa", "Média", "Alta", "Muito alta");
            AssertLabels(
                TooltipLanguage.Spanish,
                "Baja", "Media", "Alta", "Muy alta");
            AssertLabels(
                TooltipLanguage.Russian,
                "Низкая", "Средняя", "Высокая", "Очень высокая");
            AssertLabels(
                TooltipLanguage.Italian,
                "Bassa", "Media", "Alta", "Molto alta");
            AssertLabels(
                TooltipLanguage.Polish,
                "Niska", "Średnia", "Wysoka", "Bardzo wysoka");
            AssertLabels(
                TooltipLanguage.Japanese,
                "低い", "中程度", "高い", "非常に高い");
            AssertLabels(
                TooltipLanguage.SimplifiedChinese,
                "低", "中", "高", "非常高");
            AssertLabels(
                TooltipLanguage.Korean,
                "낮음", "중간", "높음", "매우 높음");
        }

        private static void TestLocalizedBlocks()
        {
            AssertLocalizedBlock(
                TooltipLanguage.English,
                "Study reward:",
                "High",
                "Studying unlocks an alchemy use.");
            AssertLocalizedBlock(
                TooltipLanguage.German,
                "Forschungsbelohnung:",
                "Hoch",
                "Nach der Untersuchung kann dieser Gegenstand in der Alchemie verwendet werden.");
            AssertLocalizedBlock(
                TooltipLanguage.French,
                "Récompense d'étude:",
                "Élevée",
                "L'étude permettra d'utiliser cet objet en alchimie.");
            AssertLocalizedBlock(
                TooltipLanguage.BrazilianPortuguese,
                "Recompensa por estudo:",
                "Alta",
                "O estudo permitirá usar este item em alquimia.");
            AssertLocalizedBlock(
                TooltipLanguage.Spanish,
                "Recompensa de estudio:",
                "Alta",
                "El estudio permitirá usar este objeto en alquimia.");
            AssertLocalizedBlock(
                TooltipLanguage.Russian,
                "Награда за исследование:",
                "Высокая",
                "Исследование позволит использовать этот предмет в алхимии.");
            AssertLocalizedBlock(
                TooltipLanguage.Italian,
                "Ricompensa dello studio:",
                "Alta",
                "Lo studio permetterà di usare questo oggetto in alchimia.");
            AssertLocalizedBlock(
                TooltipLanguage.Polish,
                "Nagroda za badanie:",
                "Wysoka",
                "Zbadanie pozwoli używać tego przedmiotu w alchemii.");
            AssertLocalizedBlock(
                TooltipLanguage.Japanese,
                "研究報酬:",
                "高い",
                "研究すると、このアイテムを錬金術に使用できます。");
            AssertLocalizedBlock(
                TooltipLanguage.SimplifiedChinese,
                "研究奖励:",
                "高",
                "研究后可将此物品用于炼金术。");
            AssertLocalizedBlock(
                TooltipLanguage.Korean,
                "연구 보상:",
                "높음",
                "연구하면 이 아이템을 연금술에 사용할 수 있습니다.");
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

            AssertEqual(
                expected,
                actual,
                "different magnitudes stay separate");
        }

        private static void TestAtomicVeryHighGroups()
        {
            RewardSnapshot snapshot = new RewardSnapshot
            {
                Blue = 66
            };

            foreach (TooltipLanguage language in
                (TooltipLanguage[])Enum.GetValues(typeof(TooltipLanguage)))
            {
                string label = RewardModel.MagnitudeLabel(
                    RewardMagnitude.VeryHigh,
                    language);
                string expected =
                    TooltipLocalization.Heading(language)
                    + " (b)"
                    + AtomicGap
                    + label.Replace(" ", AtomicGap);

                string actual = RewardModel.BuildBlock(
                    snapshot,
                    false,
                    language);

                AssertEqual(
                    expected,
                    actual,
                    language + " atomic very-high group");
            }
        }

        private static void AssertLocalizedBlock(
            TooltipLanguage language,
            string heading,
            string highLabel,
            string alchemyLine)
        {
            RewardSnapshot snapshot = new RewardSnapshot
            {
                Blue = 45
            };

            string actual = RewardModel.BuildBlock(
                snapshot,
                true,
                language);

            string expected =
                heading
                + " (b)"
                + AtomicGap
                + highLabel.Replace(" ", AtomicGap)
                + "\n"
                + alchemyLine;

            AssertEqual(
                expected,
                actual,
                language + " localized block");
        }

        private static void AssertLabels(
            TooltipLanguage language,
            string low,
            string medium,
            string high,
            string veryHigh)
        {
            AssertEqual(
                low,
                RewardModel.MagnitudeLabel(
                    RewardMagnitude.Low,
                    language),
                language + " low label");
            AssertEqual(
                medium,
                RewardModel.MagnitudeLabel(
                    RewardMagnitude.Medium,
                    language),
                language + " medium label");
            AssertEqual(
                high,
                RewardModel.MagnitudeLabel(
                    RewardMagnitude.High,
                    language),
                language + " high label");
            AssertEqual(
                veryHigh,
                RewardModel.MagnitudeLabel(
                    RewardMagnitude.VeryHigh,
                    language),
                language + " very-high label");
        }

        private static void AssertLocale(
            string locale,
            TooltipLanguage expected)
        {
            TooltipLanguage actual;
            bool supported = TooltipLocalization.TryResolve(
                locale,
                out actual);

            AssertEqual(true, supported, locale + " supported");
            AssertEqual(expected, actual, locale + " mapping");
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
