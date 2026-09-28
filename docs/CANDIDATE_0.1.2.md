# Candidate 0.1.2 — Atomic Reward Group + English Wording

Target: **Graveyard Keeper 1.407**

Status: **ACCEPTED in real runtime on 2026-09-28; approved for stable promotion**

## Exact identity

- candidate version: `0.1.2`
- production source SHA: `6d24553b1c23071975bb57802867ff9f8b91a855`
- installed file: `StudyRewardInsight.dll`
- DLL SHA-256: `a8497dbb7871f5167b295e8e68417a83411a4b93bb78e00154fbc883876eeac4`
- GitHub Actions run: `36467207572`
- job: `109080172996`
- workflow artifact ID: `10989888810`
- workflow artifact name: `StudyRewardInsight-0.1.2-6d24553b1c23071975bb57802867ff9f8b91a855`

Candidate 0.1.1 remains immutable and is superseded for further UX testing by this candidate.

An earlier internal 0.1.2 CI artifact at source `b3d7d74f52f9692f30ad8af715ca90cf9e53c771` was **not handed off**. Verification caught that U+2009 THIN SPACE is itself a wrap boundary in NGUI, so that artifact was superseded before runtime handoff.

## Product changes from 0.1.1

Russian:
- heading remains `Награда за исследование:`;
- scale remains `Низкая / Средняя / Высокая / Очень высокая`;
- alchemy cue is now `Исследование позволит использовать этот предмет в алхимии.`.

English is now enabled:
- heading: `Study reward:`;
- scale: `Low / Medium / High / Very High`;
- alchemy cue: `Studying unlocks an alchemy use.`.

Reward icon(s) and the complete magnitude label are formatted as one non-breaking group. This also keeps the two words in `Очень высокая` / `Very High` together with their icon group.

The heading itself may remain on the preceding centered line when the complete reward group does not fit. The alchemy sentence is intentionally allowed to wrap normally.

## Atomic wrapping mechanism

Candidate 0.1.1 runtime evidence showed that an ordinary ASCII space permits the native NGUI wrapper to split the point icon from its magnitude label.

The implementation uses U+00A0 NO-BREAK SPACE only inside SRI-owned reward groups.

Upstream NGUI source verification establishes:
- U+0020 ordinary space is a word-wrap boundary;
- U+2009 THIN SPACE is also classified as a space/wrap boundary and therefore was rejected;
- U+00A0 is not classified by `NGUIText.IsSpace` in the inspected wrapping implementation.

The exact visible advance of U+00A0 in Graveyard Keeper's active font remains a runtime presentation property. If the font gives it no width, the expected safe failure is merely a tighter icon/label gap, not a gameplay or state change.

## Preserved runtime architecture

No architecture broadening:
- one postfix on `ItemDefinition.GetTooltipData(Item,bool)`;
- only the text of the uniquely identified native incomplete-Survey row is changed;
- current native Survey output is read without calling `ResModificator.ProcessItemsListBeforeDrop`;
- accepted Circumspect handling remains;
- native decomposition metadata remains the alchemy capability source;
- unsupported dynamic R/G/B output fails safe to vanilla;
- completed Study remains native;
- languages other than Russian and English remain vanilla.

No new widget, tooltip-width override, container patch, per-frame work, cache, save mutation, reward mutation, or RNG call was added.

## CI evidence

Run `36467207572` completed successfully against exact source SHA `6d24553b1c23071975bb57802867ff9f8b91a855`.

Observed results:
- exact source identity: passed;
- production restore: passed;
- test restore: passed;
- production build: passed;
- compiler: **0 warnings, 0 errors**;
- reward-model tests: **passed**;
- development artifact preparation/upload: passed.

Build-time tests cover:
- all accepted R/G/B magnitude boundaries;
- Russian and English four-tier labels;
- magnitude-only grouping;
- split groups for different magnitudes;
- non-breaking reward-group separators;
- two-word `Очень высокая` / `Very High` group structure;
- accepted Russian and English alchemy wording.

## Remaining runtime acceptance

No new research harness is needed.

Please inspect:
1. Russian short reward case where 0.1.1 split icon and word — the icon and magnitude must now move together;
2. Russian alchemy item — wording/wrapping of `Исследование позволит использовать этот предмет в алхимии.`;
3. a `Очень высокая` reward if naturally available;
4. English mode — at least one ordinary unfinished Study and preferably one alchemy-capability item;
5. completed Study remains native if conveniently available.

The most important unknown is purely visual: whether the bundled live font gives the no-break separator a natural-looking gap.


## Runtime acceptance — 2026-09-28

The exact handed 0.1.2 binary was exercised in Graveyard Keeper 1.407 with BepInEx 5 in a normal heavily modded save.

Accepted visual evidence covered:
- Russian ordinary one-group reward rows;
- Russian multi-color grouped reward rows;
- Russian alchemy-capability wording and natural wrapping;
- English ordinary reward rows;
- English multi-color grouped reward rows;
- English alchemy-capability wording;
- the U+00A0 reward-group separator keeping point icon(s) and magnitude together.

Representative screenshots included Stone grave fence II, Maggot, Zombie juice, Green jelly, and their Russian/English equivalents. The resulting centered layout, spacing, grouping and wording were explicitly accepted by the user.

The returned BepInEx log confirms `Study Reward Insight 0.1.2` loaded successfully. A full scan of that log found no SRI warning, exception, unsupported-dynamic-output warning, or tooltip-failure signature. Other Unity/mod warnings and errors in the session are outside SRI and pre-existing/unrelated to the accepted tooltip path.

The same runtime session also exercised language switching across the game's locale set and directly observed `en`, `de`, `fr`, `pt-br`, `es`, `ru`, `it`, `pl`, `ja`, `zh_cn`, and `ko`. This is accepted evidence for the locale identifiers used by the next localization iteration.

No source rebuild is authorized under version 0.1.2. The accepted binary remains:
- source SHA: `6d24553b1c23071975bb57802867ff9f8b91a855`;
- DLL SHA-256: `a8497dbb7871f5167b295e8e68417a83411a4b93bb78e00154fbc883876eeac4`.
