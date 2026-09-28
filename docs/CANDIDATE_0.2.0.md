# Candidate 0.2.0 — Full Supported-Language Localization

Target: **Graveyard Keeper 1.407**

Status: **CI-green runtime candidate; real-runtime multilingual presentation acceptance required before stable promotion**

## Exact identity

- candidate version: `0.2.0`
- production source SHA: `61cb3dfadd91d7938a908b416b15df5769b98155`
- installed file: `StudyRewardInsight.dll`
- DLL SHA-256: `c77eb201b6293a94eea697c6f97458236c9caf54b367c4a530497f09529ee6bc`
- GitHub Actions run: `36471055299`
- job: `109093115474`
- workflow artifact ID: `10992020619`
- workflow artifact name: `StudyRewardInsight-0.2.0-61cb3dfadd91d7938a908b416b15df5769b98155`

Version 0.1.2 remains the accepted stable release. This 0.2.0 candidate is a new immutable handoff boundary once delivered.

## Change from 0.1.2

The accepted tooltip model is now enabled for every locale exposed by Graveyard Keeper 1.407:

- `en` — English;
- `de` — German;
- `fr` — French;
- `pt-br` — Brazilian Portuguese;
- `es` — Spanish;
- `ru` — Russian;
- `it` — Italian;
- `pl` — Polish;
- `ja` — Japanese;
- `zh_cn` — Simplified Chinese;
- `ko` — Korean.

The exact locale identifiers were observed in the accepted 0.1.2 runtime session while switching the game's language in the normal options UI.

Russian and English player-facing strings are unchanged from accepted 0.1.2.

## Candidate wording

| Locale | Heading | Low / Medium / High / Very High | Alchemy cue |
| --- | --- | --- | --- |
| en | Study reward: | Low / Medium / High / Very High | Studying unlocks an alchemy use. |
| de | Forschungsbelohnung: | Niedrig / Mittel / Hoch / Sehr hoch | Nach der Untersuchung kann dieser Gegenstand in der Alchemie verwendet werden. |
| fr | Récompense d'étude: | Faible / Moyenne / Élevée / Très élevée | L'étude permettra d'utiliser cet objet en alchimie. |
| pt-br | Recompensa por estudo: | Baixa / Média / Alta / Muito alta | O estudo permitirá usar este item em alquimia. |
| es | Recompensa de estudio: | Baja / Media / Alta / Muy alta | El estudio permitirá usar este objeto en alquimia. |
| ru | Награда за исследование: | Низкая / Средняя / Высокая / Очень высокая | Исследование позволит использовать этот предмет в алхимии. |
| it | Ricompensa dello studio: | Bassa / Media / Alta / Molto alta | Lo studio permetterà di usare questo oggetto in alchimia. |
| pl | Nagroda za badanie: | Niska / Średnia / Wysoka / Bardzo wysoka | Zbadanie pozwoli używać tego przedmiotu w alchemii. |
| ja | 研究報酬: | 低い / 中程度 / 高い / 非常に高い | 研究すると、このアイテムを錬金術に使用できます。 |
| zh_cn | 研究奖励: | 低 / 中 / 高 / 非常高 | 研究后可将此物品用于炼金术。 |
| ko | 연구 보상: | 낮음 / 중간 / 높음 / 매우 높음 | 연구하면 이 아이템을 연금술에 사용할 수 있습니다. |

The localization table is static and keyed by the host locale code. No external files, loader, polling or new lifecycle dependency were introduced.

## Preserved architecture and invariants

Unchanged from accepted 0.1.2:
- one postfix on `ItemDefinition.GetTooltipData(Item,bool)`;
- only the text of the uniquely identified native incomplete-Survey row is replaced;
- reward data comes from current native Survey output without an extra `ProcessItemsListBeforeDrop` call;
- accepted Circumspect handling is preserved;
- native decomposition metadata remains the alchemy-capability source;
- U+00A0 keeps each icon group attached to its complete magnitude label;
- unsupported dynamic R/G/B output fails safe to vanilla;
- completed Study remains native;
- exact point quantities and exact decomposition results remain hidden;
- no gameplay/save/RNG mutation.

## CI evidence

Run `36471055299` completed successfully against exact source SHA `61cb3dfadd91d7938a908b416b15df5769b98155`.

Observed results:
- exact source identity: passed;
- production restore: passed;
- test restore: passed;
- production build: passed;
- compiler: **0 warnings, 0 errors**;
- reward-model/localization tests: **passed**;
- candidate artifact creation/upload: passed.

Tests cover:
- all accepted R/G/B magnitude boundaries;
- all eleven host locale-code mappings;
- all four magnitude labels in all eleven languages;
- exact heading and alchemy text in all eleven languages;
- grouping/splitting invariants;
- non-breaking multi-word magnitude groups.

## Runtime acceptance requested

No new research harness is needed. Use the game's normal live language selector already exercised in the 0.1.2 session.

For the nine newly enabled locales (`de`, `fr`, `pt-br`, `es`, `it`, `pl`, `ja`, `zh_cn`, `ko`):
1. inspect one ordinary unfinished Study item;
2. check for missing glyphs, clipping, awkward spacing or broken wrapping;
3. for a representative long Latin-script locale and the CJK locales, inspect an alchemy-capability item if practical;
4. report any wording that looks visibly malformed or any locale that still shows the vanilla incomplete row.

Russian and English do not need another semantic/layout re-test unless regression is observed.
