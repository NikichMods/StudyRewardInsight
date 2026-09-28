# Study Value Model Trade Study

Target: **Graveyard Keeper 1.407**

Status: **research / product design; production behavior remains BLOCKED**

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

A four-band magnitude model remains the leading candidate because the vanilla distributions contain natural gaps and the colors require different numerical thresholds.

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

Leading family.

Always:
- preserve vanilla Study line;
- add qualitative magnitude for each positive technology-point color.

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

## Working Faith-efficiency model

Because the dominant host norm is shared across colors, a **common normalized efficiency scale** is more defensible than separate R/G/B thresholds.

Working candidate, not accepted behavior:

- **Low return:** < 0.75× the 10-points/Faith host norm;
- **Normal return:** 0.75×–1.25×;
- **High return:** >1.25× and <2×;
- **Exceptional return:** ≥2×.

This places observed values approximately as:

- low: 2.5 / 3.33 / 3.75 / 5 / 6 / 6.25 points per Faith;
- normal: 7.5 through 12.5;
- high: 14.29 / 15 / 16.67;
- exceptional: 20 / 22.5 / 25.

Player-facing UI would use words only, not these numeric thresholds.

A stricter exception-oriented presentation could omit the **Normal** label entirely and show only low/high/exceptional deviations.

## Multi-color implication

Efficiency qualifiers must be associated with their point color.

Examples from accepted data:
- Obsidian: red 6.25/Faith, green 3.75/Faith, blue 25/Faith;
- Ceramic funeral urn: red 2.5/Faith, blue 15/Faith;
- Incense II: red 7.5/Faith, blue 22.5/Faith;
- Black-and-gold cloak: all three colors 12.5/Faith.

This is strong evidence against a single overall "efficiency" line unless the line can encode color association unambiguously.

## Working UX shape

Not accepted wording; conceptual example only.

Single-color ordinary item:
- native Study line;
- `Blue reward: Moderate`

Single-color unusually efficient item:
- native Study line;
- `Blue reward: Moderate · Faith return: Exceptional`

Multi-color item:
- native Study line;
- one compact entry per rewarded color when needed, e.g. red and blue magnitude + return.

Alchemy-capable item:
- separate short signal such as `Study unlocks alchemical decomposition`.

The alchemy signal should describe a **verified capability**, not a vague "interesting for alchemy" recommendation.

## Wording direction

Absolute magnitude:
- clear baseline: `Low / Moderate / High / Very high`;
- more atmospheric candidate: `Minor / Moderate / Significant / Exceptional`.

Avoid language such as "valuable" or "worthwhile" on the magnitude line because those words imply total utility rather than only point yield.

Faith-normalized dimension:
- prefer a semantic noun such as `Faith return` / `Return`;
- candidate levels: `Low / Normal / High / Exceptional`.

Final localization wording requires UI/layout testing and all supported game languages.

## 0.2.0 evidence closure

StudyValueDump 0.2.0 completed successfully on Graveyard Keeper 1.407. See `docs/STUDY_VALUE_RUNTIME_0.2.0.md`.

Closed facts:
- 62 / 223 ordinary Study definitions unlock at least one native alchemical decomposition path;
- Science cost is explicit and differs from Faith on 55 / 223 rows, so Science must not be silently folded into Faith efficiency;
- 173 rows contain Story output gated by `p_naturalist`, using either independent self-chance or weighted chance groups;
- only two rows have deterministic non-Story/non-tech progression outputs: Keeper's Key and Obsidian;
- generic `end_event`, `end_script`, `craft_after_finish`, and `ach_key` fields are empty across the ordinary Study set.

Story is therefore real Study value, but conditional and not a good default tooltip axis. The core model remains magnitude + Faith return + decomposition capability.

## Quest relevance as a separate signal

A further player-value axis is now in research: whether an unstudied item's decomposition route can help satisfy an **already-visible current task** through recipes the player already knows.

This is deliberately not part of the Study reward score. It is a current-save relevance signal.

Leading no-spoiler rule:
- current Visible task only;
- task requirement recovered from authored current-task data;
- traverse only recipes the current save already exposes;
- allow the final hidden edge to be the uncompleted Study-gated decomposition;
- display only a generic hint, not the quest, recipe, ingredient, or decomposition result.

See `docs/QUEST_RELEVANCE_RESEARCH.md`.

## Remaining product decisions

Production remains **BLOCKED** pending presentation decisions and the quest-relevance owner proof.

Still open:
- whether Faith return should be displayed always or only for meaningful deviations from the host norm;
- exact wording and compact multi-color presentation;
- whether the alchemy capability cue should be text, icon+text, or folded into a compact secondary line;
- whether Story should remain intentionally omitted;
- whether a Study Table-specific augmentation is necessary after tooltip layout testing;
- exact current-task requirement owner/data path for the no-spoiler quest-relevance signal.
