// SPDX-License-Identifier: MPL-2.0

using System;

namespace StudyRewardInsight
{
    internal enum TooltipLanguage
    {
        English,
        German,
        French,
        BrazilianPortuguese,
        Spanish,
        Russian,
        Italian,
        Polish,
        Japanese,
        SimplifiedChinese,
        Korean
    }

    internal static class TooltipLocalization
    {
        internal static bool TryResolve(
            string locale,
            out TooltipLanguage language)
        {
            string current = (locale ?? string.Empty)
                .Trim()
                .ToLowerInvariant()
                .Replace('-', '_');

            if (Matches(current, "en", "english"))
            {
                language = TooltipLanguage.English;
                return true;
            }

            if (Matches(current, "de", "german"))
            {
                language = TooltipLanguage.German;
                return true;
            }

            if (Matches(current, "fr", "french"))
            {
                language = TooltipLanguage.French;
                return true;
            }

            if (current == "pt_br"
                || current == "ptbr"
                || current == "brazilian_portuguese"
                || current == "portuguese_brazil")
            {
                language = TooltipLanguage.BrazilianPortuguese;
                return true;
            }

            if (Matches(current, "es", "spanish"))
            {
                language = TooltipLanguage.Spanish;
                return true;
            }

            if (Matches(current, "ru", "russian"))
            {
                language = TooltipLanguage.Russian;
                return true;
            }

            if (Matches(current, "it", "italian"))
            {
                language = TooltipLanguage.Italian;
                return true;
            }

            if (Matches(current, "pl", "polish"))
            {
                language = TooltipLanguage.Polish;
                return true;
            }

            if (Matches(current, "ja", "japanese"))
            {
                language = TooltipLanguage.Japanese;
                return true;
            }

            if (current == "zh_cn"
                || current == "zh_hans"
                || current == "simplified_chinese"
                || current == "chinese_simplified")
            {
                language = TooltipLanguage.SimplifiedChinese;
                return true;
            }

            if (Matches(current, "ko", "korean"))
            {
                language = TooltipLanguage.Korean;
                return true;
            }

            language = TooltipLanguage.English;
            return false;
        }

        internal static string Heading(TooltipLanguage language)
        {
            switch (language)
            {
                case TooltipLanguage.English:
                    return "Study reward:";
                case TooltipLanguage.German:
                    return "Forschungsbelohnung:";
                case TooltipLanguage.French:
                    return "Récompense d'étude:";
                case TooltipLanguage.BrazilianPortuguese:
                    return "Recompensa por estudo:";
                case TooltipLanguage.Spanish:
                    return "Recompensa de estudio:";
                case TooltipLanguage.Russian:
                    return "Награда за исследование:";
                case TooltipLanguage.Italian:
                    return "Ricompensa dello studio:";
                case TooltipLanguage.Polish:
                    return "Nagroda za badanie:";
                case TooltipLanguage.Japanese:
                    return "研究報酬:";
                case TooltipLanguage.SimplifiedChinese:
                    return "研究奖励:";
                case TooltipLanguage.Korean:
                    return "연구 보상:";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language));
            }
        }

        internal static string MagnitudeLabel(
            RewardMagnitude magnitude,
            TooltipLanguage language)
        {
            switch (language)
            {
                case TooltipLanguage.English:
                    return EnglishMagnitude(magnitude);
                case TooltipLanguage.German:
                    return GermanMagnitude(magnitude);
                case TooltipLanguage.French:
                    return FrenchMagnitude(magnitude);
                case TooltipLanguage.BrazilianPortuguese:
                    return BrazilianPortugueseMagnitude(magnitude);
                case TooltipLanguage.Spanish:
                    return SpanishMagnitude(magnitude);
                case TooltipLanguage.Russian:
                    return RussianMagnitude(magnitude);
                case TooltipLanguage.Italian:
                    return ItalianMagnitude(magnitude);
                case TooltipLanguage.Polish:
                    return PolishMagnitude(magnitude);
                case TooltipLanguage.Japanese:
                    return JapaneseMagnitude(magnitude);
                case TooltipLanguage.SimplifiedChinese:
                    return SimplifiedChineseMagnitude(magnitude);
                case TooltipLanguage.Korean:
                    return KoreanMagnitude(magnitude);
                default:
                    throw new ArgumentOutOfRangeException(nameof(language));
            }
        }

        internal static string AlchemyLine(TooltipLanguage language)
        {
            switch (language)
            {
                case TooltipLanguage.English:
                    return "Studying unlocks an alchemy use.";
                case TooltipLanguage.German:
                    return "Nach der Untersuchung kann dieser Gegenstand in der Alchemie verwendet werden.";
                case TooltipLanguage.French:
                    return "L'étude permettra d'utiliser cet objet en alchimie.";
                case TooltipLanguage.BrazilianPortuguese:
                    return "O estudo permitirá usar este item em alquimia.";
                case TooltipLanguage.Spanish:
                    return "El estudio permitirá usar este objeto en alquimia.";
                case TooltipLanguage.Russian:
                    return "Исследование позволит использовать этот предмет в алхимии.";
                case TooltipLanguage.Italian:
                    return "Lo studio permetterà di usare questo oggetto in alchimia.";
                case TooltipLanguage.Polish:
                    return "Zbadanie pozwoli używać tego przedmiotu w alchemii.";
                case TooltipLanguage.Japanese:
                    return "研究すると、このアイテムを錬金術に使用できます。";
                case TooltipLanguage.SimplifiedChinese:
                    return "研究后可将此物品用于炼金术。";
                case TooltipLanguage.Korean:
                    return "연구하면 이 아이템을 연금술에 사용할 수 있습니다.";
                default:
                    throw new ArgumentOutOfRangeException(nameof(language));
            }
        }

        private static bool Matches(
            string current,
            string code,
            string name)
        {
            return current == code
                || current == name
                || current.StartsWith(
                    code + "_",
                    StringComparison.Ordinal);
        }

        private static string EnglishMagnitude(RewardMagnitude magnitude)
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

        private static string GermanMagnitude(RewardMagnitude magnitude)
        {
            switch (magnitude)
            {
                case RewardMagnitude.Low: return "Niedrig";
                case RewardMagnitude.Medium: return "Mittel";
                case RewardMagnitude.High: return "Hoch";
                case RewardMagnitude.VeryHigh: return "Sehr hoch";
                default: throw new ArgumentOutOfRangeException(nameof(magnitude));
            }
        }

        private static string FrenchMagnitude(RewardMagnitude magnitude)
        {
            switch (magnitude)
            {
                case RewardMagnitude.Low: return "Faible";
                case RewardMagnitude.Medium: return "Moyenne";
                case RewardMagnitude.High: return "Élevée";
                case RewardMagnitude.VeryHigh: return "Très élevée";
                default: throw new ArgumentOutOfRangeException(nameof(magnitude));
            }
        }

        private static string BrazilianPortugueseMagnitude(
            RewardMagnitude magnitude)
        {
            switch (magnitude)
            {
                case RewardMagnitude.Low: return "Baixa";
                case RewardMagnitude.Medium: return "Média";
                case RewardMagnitude.High: return "Alta";
                case RewardMagnitude.VeryHigh: return "Muito alta";
                default: throw new ArgumentOutOfRangeException(nameof(magnitude));
            }
        }

        private static string SpanishMagnitude(RewardMagnitude magnitude)
        {
            switch (magnitude)
            {
                case RewardMagnitude.Low: return "Baja";
                case RewardMagnitude.Medium: return "Media";
                case RewardMagnitude.High: return "Alta";
                case RewardMagnitude.VeryHigh: return "Muy alta";
                default: throw new ArgumentOutOfRangeException(nameof(magnitude));
            }
        }

        private static string RussianMagnitude(RewardMagnitude magnitude)
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

        private static string ItalianMagnitude(RewardMagnitude magnitude)
        {
            switch (magnitude)
            {
                case RewardMagnitude.Low: return "Bassa";
                case RewardMagnitude.Medium: return "Media";
                case RewardMagnitude.High: return "Alta";
                case RewardMagnitude.VeryHigh: return "Molto alta";
                default: throw new ArgumentOutOfRangeException(nameof(magnitude));
            }
        }

        private static string PolishMagnitude(RewardMagnitude magnitude)
        {
            switch (magnitude)
            {
                case RewardMagnitude.Low: return "Niska";
                case RewardMagnitude.Medium: return "Średnia";
                case RewardMagnitude.High: return "Wysoka";
                case RewardMagnitude.VeryHigh: return "Bardzo wysoka";
                default: throw new ArgumentOutOfRangeException(nameof(magnitude));
            }
        }

        private static string JapaneseMagnitude(RewardMagnitude magnitude)
        {
            switch (magnitude)
            {
                case RewardMagnitude.Low: return "低い";
                case RewardMagnitude.Medium: return "中程度";
                case RewardMagnitude.High: return "高い";
                case RewardMagnitude.VeryHigh: return "非常に高い";
                default: throw new ArgumentOutOfRangeException(nameof(magnitude));
            }
        }

        private static string SimplifiedChineseMagnitude(
            RewardMagnitude magnitude)
        {
            switch (magnitude)
            {
                case RewardMagnitude.Low: return "低";
                case RewardMagnitude.Medium: return "中";
                case RewardMagnitude.High: return "高";
                case RewardMagnitude.VeryHigh: return "非常高";
                default: throw new ArgumentOutOfRangeException(nameof(magnitude));
            }
        }

        private static string KoreanMagnitude(RewardMagnitude magnitude)
        {
            switch (magnitude)
            {
                case RewardMagnitude.Low: return "낮음";
                case RewardMagnitude.Medium: return "중간";
                case RewardMagnitude.High: return "높음";
                case RewardMagnitude.VeryHigh: return "매우 높음";
                default: throw new ArgumentOutOfRangeException(nameof(magnitude));
            }
        }
    }
}
