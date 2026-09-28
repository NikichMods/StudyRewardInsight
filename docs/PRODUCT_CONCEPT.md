# Product Concept — Study Reward Insight

Target: **Graveyard Keeper 1.407**

Status: **working product concept after Study-value research; production remains BLOCKED pending final UX wording/layout and implementation evidence gate**

## Product problem

Vanilla Study asks the player to commit scarce resources, especially Faith, while exposing only part of the decision-relevant information.

Before Study, vanilla already communicates:
- that the item can be studied;
- the technology-point color(s);
- the Study costs through the native Study UI.

It intentionally does **not** communicate the point quantity.

Player-facing uncertainty is therefore not just "what color do I get?". The important unanswered questions are:

1. roughly how large is the hidden technology-point reward?
2. is that reward unusually good or poor for the Faith being spent?
3. does Study unlock an additional alchemical use for this item?

The mod should answer those questions without turning Study into a wiki lookup or an automatic recommendation engine.

## Core product promise

> **Make Study an informed choice without spoiling the discovery.**

Study Reward Insight should reduce blind Faith spending while preserving Graveyard Keeper's sense of experimentation.

It should describe **properties of the Study result**, not decide whether the player "should" study the item.

## Information architecture

### 1. Qualitative technology-point magnitude — always

For every positive R/G/B Study reward, show a qualitative magnitude associated with that color.

Leading model: four tiers with color-specific numerical thresholds.

Current working vocabulary:
- Low;
- Moderate;
- High;
- Very high.

Atmospheric wording remains open; the semantics are more important than the labels.

Why always:
- this is the most direct missing information in vanilla;
- it answers "how much?" without revealing exact numbers;
- it remains useful even when Faith is plentiful;
- it works for zero-Faith Study;
- it preserves multi-color tradeoffs instead of inventing a combined score.

### 2. Faith return — exception-oriented

Faith efficiency is a separate property from absolute reward magnitude.

Do **not** combine colors into one efficiency score.

Leading behavior:
- omit the efficiency qualifier when a rewarded color is close to the host's ordinary ~10-points-per-Faith norm;
- show a qualifier only for materially unusual low/high returns;
- associate the qualifier with the specific technology-point color.

Working normalized bands for research:
- low return: < 0.75× host norm;
- normal: 0.75×–1.25×;
- high: > 1.25× and < 2×;
- exceptional: >= 2×.

At these working boundaries:
- 30 of 178 paid ordinary Study definitions have at least one unusual color return;
- only ~17% of paid Studies therefore receive any efficiency exception marker.

This keeps the signal scarce enough to remain meaningful.

Zero-Faith Study should not be labeled "infinitely efficient". The native zero cost already carries the important information unless UX testing proves an explicit cue is needed.

### 3. Alchemical decomposition unlock — when applicable

Accepted runtime evidence establishes that 62 / 223 ordinary Study definitions unlock at least one native alchemical decomposition path.

For those unstudied items, show a short capability cue:
- Study will unlock alchemical decomposition.

Do **not** reveal before Study:
- the exact alchemical element;
- the exact decomposition workstation;
- the exact output.

After Study, preserve vanilla completed behavior and allow post-research mods such as Decomp Delight to show the discovered decomposition information.

This makes Study Reward Insight complementary to research-gated alchemy-information mods rather than duplicative.

## Explicit non-goals

### No overall "Study value" score

Do not produce:
- "worth studying";
- "excellent Study";
- stars/tiers combining all value;
- a ranked priority list.

Why:
- red/green/blue have no canonical exchange rate;
- absolute reward and Faith efficiency differ;
- alchemical utility is qualitatively different from technology points;
- any combined score would encode subjective hidden weights.

### No quest-relevance engine

Do not analyze active/future quests, production chains, or current-save objective relevance.

That is a neighboring but distinct product:
- broader runtime state;
- different owner/data paths;
- materially larger architecture;
- potential spoiler concerns.

The research concept is preserved separately in `docs/QUEST_RELEVANCE_RESEARCH.md` but is **out of scope for Study Reward Insight** unless the product is explicitly redefined later.

### No Story prediction by default

Story outputs are real but conditional on `p_naturalist` and chance logic.

They are not part of the core Study decision problem and would add noise. Keep them out unless later user evidence establishes a specific unmet need.

### No exact hidden quantities

Do not show:
- exact technology-point counts;
- exact ranges;
- repeated point icons as a numerical scale;
- stars/grave-quality symbols as surrogate numbers.

The goal is informed uncertainty, not removal of uncertainty.

### No tutorial replacement

Do not turn the mod into a Study-table onboarding guide for:
- how to generate Science;
- where Faith comes from;
- where the Study Table is;
- how technology trees work.

Those are real player questions but a different onboarding problem.

## External player-signal audit — 2026-09-28

Representative community questions repeatedly cluster around:
- "what does researching an item do?";
- "what should I prioritize studying?";
- "I only have limited Faith and do not want to waste it randomly";
- "which items are good sources of blue points?";
- "why can/can't I decompose this item for alchemy?";
- spoiler avoidance while still wanting enough guidance to proceed.

Representative sources reviewed:
- Reddit: `/r/GraveyardKeeper/comments/1waidta/` — non-spoiler early-game player explicitly asks what researching an item does while having only 5 Faith;
- Reddit: `/r/GraveyardKeeper/comments/1ioxo81/` — player asks what to prioritize and explicitly does not want to waste Faith by random Study;
- Reddit: `/r/GraveyardKeeper/comments/1h38fd3/` — Study-table confusion and reminder that researched items expose decomposition information;
- Reddit: `/r/GraveyardKeeper/comments/lblrz7/` — discussion distinguishes point-farming value from alchemy-unlock value;
- Nexus Mods: Decomp Delight — deliberately reveals exact decomposition element only after vanilla research;
- Nexus Mods: Alchemy Research Redux — contextual live preview at the alchemy-mixing decision point.

The useful design pattern is contextual decision support rather than a global answer sheet.

## Neighboring-mod boundary

### Decomp Delight

Problem:
- after research, the player forgets what an ingredient decomposes into.

Solution:
- show the exact element on researched items only.

Study Reward Insight should stop one step earlier:
- before Study, say only that decomposition capability will unlock;
- after Study, disappear and leave the discovered result to vanilla / Decomp Delight.

### Alchemy Research Redux

Problem:
- while experimenting at the mixer, the player cannot see what the current combination will produce.

Solution:
- live result preview at the point of alchemy composition.

Relevant principle:
- expose information at the moment it supports a concrete decision;
- do not broaden the feature into general game-solving.

## Leading product shape

For an ordinary single-color Study:
- preserve native Study information;
- add qualitative reward magnitude.

For an unusually efficient/inefficient paid Study:
- add a compact Faith-return qualifier associated with that color.

For a Study-gated alchemy item:
- add one short "unlocks alchemical decomposition" signal.

For multi-color Study:
- preserve color association for both magnitude and any efficiency exception;
- do not collapse the colors into one score.

After completed Study:
- remove all predictive cues;
- preserve vanilla completed state.

## Questions the mod should answer

1. **What kind of technology points will I get?**
   - Vanilla already answers the color; preserve it.

2. **Roughly how much will I get?**
   - Study Reward Insight: qualitative magnitude.

3. **Is this an unusual use of my Faith for that color?**
   - Study Reward Insight: exception-only Faith return.

4. **Will studying this item unlock an alchemical use?**
   - Study Reward Insight: yes/no capability only.

## Questions the mod should deliberately not answer

- Should I study this item first?
- Which item is globally "best"?
- What exact number of points will I receive?
- What exact alchemical element will this item produce?
- What future quest will this help with?
- What hidden recipe chain uses this item?
- What Story will I receive?

## Remaining open product questions

1. final four magnitude labels, including whether a more Graveyard-Keeper-like vocabulary is preferable to Low/Moderate/High/Very high;
2. final Faith-return vocabulary and exact exception thresholds;
3. compact representation for multi-color Study without making the tooltip tall/noisy;
4. whether low-return exceptions should be shown as prominently as high/exceptional returns;
5. exact wording/iconography for the alchemy-unlock cue;
6. localization behavior and layout across supported languages.

## Remaining engineering questions

Before production source mutation:
1. prove the narrowest safe way to observe effective processed Survey R/G/B output at tooltip time without altering RNG/game state;
2. prove the narrowest native-tooltip insertion point and row identity;
3. establish final writer/consumer, blast radius, invariants, and acceptance evidence;
4. mark each materially independent behavior change READY/BLOCKED.

Until then production behavior remains **BLOCKED**.
