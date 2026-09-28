# Candidate 0.1.1 — Compact Neutral Study Tooltip

Target: **Graveyard Keeper 1.407**

Status: **CI-green runtime candidate; real-runtime visual acceptance still required before stable promotion**

## Exact identity

- candidate version: `0.1.1`
- production source SHA: `344c3d69f740d2804d2c7147b6298d98579ad410`
- installed file: `StudyRewardInsight.dll`
- DLL SHA-256: `b2285209de7c3981a96db0681c926885ff4a2c83cb207904a956269cecf7a58a`
- GitHub Actions run: `36459348314`
- job: `109053691895`
- workflow artifact ID: `10986414011`
- workflow artifact name: `StudyRewardInsight-0.1.1-344c3d69f740d2804d2c7147b6298d98579ad410`

Candidate 0.1.0 remains immutable and is superseded for further UX testing by this candidate.

## Product changes from 0.1.0

- reward labels are now `Низкая / Средняя / Высокая / Очень высокая`;
- `щедрее обычного / скупее обычного` is removed completely from player-facing output;
- Faith is no longer read or evaluated by production code for a hidden relative modifier;
- the first reward group is kept on the same line as `Награда за исследование:`;
- only additional magnitude groups create additional reward lines;
- the alchemy cue is shortened to the neutral `Открывает алхимическое разложение.`;
- native centered alignment is retained.

The color-specific magnitude thresholds are unchanged.

## Preserved runtime architecture

The candidate keeps the 0.1.0 mechanism:
- one postfix on `ItemDefinition.GetTooltipData(Item,bool)`;
- mutate only the text of the uniquely identified native incomplete-Survey row;
- read current native Survey output without calling `ResModificator.ProcessItemsListBeforeDrop`;
- preserve the accepted Circumspect dynamic blue-point handling;
- use native decomposition metadata for the pre-Study alchemy capability cue;
- fail safe to vanilla for unsupported dynamic R/G/B output;
- Russian only; other languages remain vanilla.

No new hook, widget, cache, per-frame work, save mutation, reward mutation, or RNG call was added.

## CI evidence

Run `36459348314` completed successfully against exact source SHA `344c3d69f740d2804d2c7147b6298d98579ad410`.

Observed CI results:
- source identity check: passed;
- production restore: passed;
- test restore: passed;
- production build: passed;
- compiler result: **0 warnings, 0 errors**;
- reward-model tests: **passed**;
- development artifact preparation/upload: passed.

Build-time tests cover:
- all accepted red/green/blue magnitude boundaries;
- all four new Russian magnitude labels;
- magnitude-only color grouping;
- separation of colors with different magnitudes;
- first reward group inline with the Study heading;
- neutral alchemy cue wording.

## Remaining acceptance

No new research probe is needed.

The remaining gate is perceptual/runtime:
1. inspect an ordinary one-group unfinished Study tooltip;
2. inspect an alchemy-capability item if naturally available;
3. inspect a split multi-magnitude item if naturally available;
4. confirm wrapping, visual balance, icon grouping and centered alignment;
5. confirm a completed Study item remains native if conveniently available.

Return screenshots and the normal BepInEx log only if something behaves unexpectedly or for final acceptance evidence.
