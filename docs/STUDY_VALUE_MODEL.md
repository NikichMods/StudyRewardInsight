# Study Value Model Trade Study

Target: **Graveyard Keeper 1.407**

Status: **accepted product model; production behavior remains BLOCKED pending implementation evidence gates**

## Product outcome

Reduce blind Faith spending without turning the item tooltip into a wiki or inventing one opaque "best item" score.

Before Study, a player should be able to distinguish:

1. how large the hidden technology-point reward is;
2. whether the reward is unusually good or poor **for the Faith spent**;
3. whether Study also unlocks alchemical decomposition for that item;
4. any other materially decision-relevant Study value that can be communicated without revealing exact hidden results.

The mod should inform the decision, not make it for the player.

## Established independent value axes

### 1. Absolute technology-point yield

Accepted all-item runtime evidence establishes 223 ordinary Study crafts and separate R/G/B reward distributions.

The accepted four-band magnitude model is **Небольшая / Умеренная / Большая / Очень большая**, with the color-specific thresholds recorded in `docs/PRODUCT_CONCEPT.md` and `docs/STUDY_REWARD_DISTRIBUTION.md`.

### 2. Faith efficiency

Accepted analysis establishes a strong common host norm of roughly **10 points of a given color per 1 Faith**:

- 75.0% of positive paid red rewards are exactly 10 red/Faith;
- 93.9% of positive paid green rewards are exactly 10 green/Faith;
- only 35.6% of positive paid blue rewards are exactly 10 blue/Faith.

Blue contains the strongest meaningful exceptions, including both unusually efficient and unusually inefficient Study targets.

Faith efficiency must remain **per color**. The 12 multi-color Study rows prove that one combined efficiency score would require arbitrary exchange rates between red, green, and blue.

Example: Obsidian is simultaneously poor-to-low efficiency in red/green and exceptional in blue. A single "excellent Study" label would hide that tradeoff.

### 3. Alchemical decomposition unlock

Pinned 1.407 source establishes:

- `BaseCraftGUI.CommonOpen(... AlchemyDecompose)` filters decomposition recipes through `GameSave.IsSurveyComplete(...)`;
- `ItemDefinition.GetTooltipData` adds the native alchemy details widget only in the completed-Study branch;
- `CraftComponent.ShowSurveyCompleteWindow` reveals the item's decomposition information when Study completes.

Therefore applicable Study genuinely has a second progression value: it unlocks the item for alchemical decomposition and reveals the decomposition class.

The pre-Study mod should signal **that this utility will unlock**, but should not reveal the exact decomposition element/class unless product scope deliberately chooses to spoil the discovery.

### 4. Story / other Survey outputs

Accepted runtime data shows many Survey definitions contain non-tech outputs:
- `story:1` on 173 rows;
- `story:2` on 55;
- `story:3` on 22;
- one Keeper's-key transformation output;
- one Obsidian research output.

Pinned source proves ordinary Survey completion ultimately processes the Survey `output` list through `ResModificator.ProcessItemsListBeforeDrop`, so these outputs are on the real completion path.

However probe 0.1.0 did not capture non-tech chance/min/max expressions. Therefore Story is **not yet eligible for a player-facing value indicator**. Probe 0.2.0 exists specifically to close this uncertainty.

Quest/special Survey rows should not be folded into generic value scoring merely because their outputs live in the same data structure.

## Solution families

### A. One overall Study rating

Example:
`Study value: Excellent`

Would combine reward magnitude, Faith efficiency, alchemy utility, Story, and possibly special progression effects.

**Rejected as leading direction.**

Why:
- requires arbitrary weights between different point colors;
- requires an arbitrary monetary/value equivalent for alchemy unlock;
- hides important tradeoffs;
- becomes difficult to explain when mods alter Survey output;
- a concise label would look authoritative while encoding subjective assumptions.

### B. Full independent matrix on every item

Show:
- magnitude for every positive point color;
- Faith efficiency for every positive point color;
- alchemy unlock;
- Story chance/value;
- possibly Science cost.

**Accurate but too noisy as the default.**

Most red/green Studies sit on the host's normal 10-points-per-Faith line, so repeating "normal efficiency" everywhere adds little information.

### C. Absolute reward + exception-oriented Faith efficiency + alchemy signal

**Accepted family.**

Always:
- preserve the semantics of pending Study, but the literal vanilla incomplete-Study row may be replaced/restructured;
- show qualitative magnitude for each positive red/green/blue reward.

Only when useful:
- add a Faith-efficiency qualifier for colors materially outside the ordinary host band;
- omit an efficiency qualifier when the color is approximately normal;
- for zero-Faith Study, treat "no Faith required" as a separate fact rather than infinite efficiency.

When applicable:
- add a short pre-Study alchemy-unlock signal without revealing the exact decomposition result.

Advantages:
- directly answers the user's two separate questions: "how much?" and "is this unusually good for my Faith?";
- does not invent exchange rates between colors;
- common/boring cases remain compact;
- the interesting body-part/Obsidian/Incense exceptions become visible;
- compatible with post-Study vanilla behavior and Decomp Delight-style researched-only details.

### D. Full independent values, but only at the Study Table

Keep inventory tooltip light; show efficiency/alchemy decision support only in the Study Table UI where the Faith cost is visible.

**Potentially elegant, but broader.**

Costs:
- requires a second UI owner/seam;
- makes inventory/chest/vendor comparison less useful;
- materially larger blast radius than extending the already-proven item tooltip.

Keep as fallback if item-tooltip layout cannot carry the minimum useful information cleanly.

### E. Efficiency only

Show only Faith-normalized value and omit absolute magnitude.

**Insufficient.**

A 20-blue/1-Faith Study and 100-blue/10-Faith Study answer different needs. The user explicitly values both total payoff and resource efficiency.

## Accepted Faith-efficiency exception model

The dominant host reference is approximately 10 points of a given color per Faith. The player-facing tooltip does not mention Faith or the ratio.

Accepted boundaries:
- **< 7.5 points/Faith:** append `скупее обычного`;
- **7.5–12.5 inclusive:** no modifier;
- **> 12.5 points/Faith:** append `щедрее обычного`.

These boundaries fall in natural gaps in the observed vanilla paid-Study ratios. Zero-Faith Study receives no modifier.

The magnitude label and modifier remain separate semantics: magnitude describes absolute reward size; the modifier is only a rare relative generosity/stinginess cue.

## Multi-color implication

Efficiency qualifiers must be associated with their point color.

Examples from accepted data:
- Obsidian: red 6.25/Faith, green 3.75/Faith, blue 25/Faith;
- Ceramic funeral urn: red 2.5/Faith, blue 15/Faith;
- Incense II: red 7.5/Faith, blue 22.5/Faith;
- Black-and-gold cloak: all three colors 12.5/Faith.

This is strong evidence against a single overall "efficiency" line unless the line can encode color association unambiguously.

## Accepted UX shape

Conceptual Russian block:

`Награда за исследование:`
`[point icon(s)] Небольшая / Умеренная / Большая / Очень большая[, щедрее обычного / скупее обычного]`
`[second reward row only when another color group differs]`
`После изучения сгодится для алхимии.` — only when applicable.

Equal colors are grouped only when their complete displayed assessment matches.

## Accepted Russian wording

Magnitude:
- `Небольшая`
- `Умеренная`
- `Большая`
- `Очень большая`

Rare modifier:
- `щедрее обычного`
- `скупее обычного`

Alchemy:
- `После изучения сгодится для алхимии.`

The primary scale is intentionally maximally transparent. Character is carried by the exception/alchemy wording rather than by ambiguous magnitude adjectives.

## 0.2.0 evidence closure

StudyValueDump 0.2.0 completed successfully on Graveyard Keeper 1.407. See `docs/STUDY_VALUE_RUNTIME_0.2.0.md`.

Closed facts:
- 62 / 223 ordinary Study definitions unlock at least one native alchemical decomposition path;
- Science cost is explicit and differs from Faith on 55 / 223 rows, so Science must not be silently folded into Faith efficiency;
- 173 rows contain Story output gated by `p_naturalist`, using either independent self-chance or weighted chance groups;
- only two rows have deterministic non-Story/non-tech progression outputs: Keeper's Key and Obsidian;
- generic `end_event`, `end_script`, `craft_after_finish`, and `ach_key` fields are empty across the ordinary Study set.

Story is therefore real Study value, but conditional and not a good default tooltip axis. The core model remains magnitude + Faith return + decomposition capability.

## Quest relevance

Quest/current-task relevance was investigated and is explicitly **out of scope** for Study Reward Insight. It is a neighboring product problem and must not expand this mod's architecture.

## Remaining work

The core Russian product model is accepted. Remaining work is engineering and localization:
- prove the least-complex safe reward-read path;
- prove the narrowest incomplete-Study tooltip replacement seam;
- visually accept layout/wrapping in real runtime;
- localize the accepted semantics for supported languages.

Production remains **BLOCKED** until the applicable engineering evidence gates are READY.
