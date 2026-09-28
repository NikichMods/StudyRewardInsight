# Product Concept — Study Reward Insight

Target: **Graveyard Keeper 1.407**

Status: **0.1.2 is stable; 0.2.0 full-language localization is a CI-green runtime candidate**

## Product problem

Vanilla Study asks the player to commit scarce resources, especially Faith, while exposing only part of the decision-relevant information.

Before Study, vanilla already communicates:
- that the item can be studied;
- the technology-point color(s);
- the Study costs through the native Study UI.

It intentionally does **not** communicate the point quantity.

Player-facing uncertainty is therefore not just "what color do I get?". The important unanswered questions are:

1. roughly how large is the hidden technology-point reward?
2. does Study unlock an additional alchemical use for this item?

The mod should answer those questions without turning Study into a wiki lookup or an automatic recommendation engine.

## Core product promise

> **Make Study an informed choice without spoiling the discovery.**

Study Reward Insight should reduce blind Faith spending while preserving Graveyard Keeper's sense of experimentation.

It should describe **properties of the Study result**, not decide whether the player "should" study the item.

## Information architecture

### 1. Qualitative technology-point magnitude — always

For every positive R/G/B Study reward, show a qualitative magnitude associated with that color.

Accepted four-tier player-facing vocabulary:
- Russian: **Низкая / Средняя / Высокая / Очень высокая**;
- English: **Low / Medium / High / Very High**.

Accepted color-specific boundaries:
- red: 1–12 / 13–40 / 41–75 / 76+;
- green: 1–12 / 13–25 / 26–45 / 46+;
- blue: 1–12 / 13–37 / 38–65 / 66+.

The boundaries sit in empty gaps in the observed vanilla 1.407 reward distributions. Every ordinary Study receives one of these qualitative magnitude labels for each rewarded color; there is no unlabeled "ordinary reward" class.

When multiple colors have the same complete displayed assessment, collapse them into one row with multiple native point icons. Split into separate rows only when their displayed assessments differ.

Why always:
- this is the most direct missing information in vanilla;
- it answers "how much?" without revealing exact numbers;
- it remains useful even when Faith is plentiful;
- it works for zero-Faith Study;
- it preserves multi-color tradeoffs instead of inventing a combined score.

### 2. Relative Faith-efficiency modifier — researched, not player-facing

The Study/Faith efficiency analysis remains valid research and is preserved in `docs/STUDY_FAITH_EFFICIENCY.md` and related model history.

Runtime UX review showed that the exception wording `щедрее обычного / скупее обычного` reads as an authorial aside rather than a neutral game-system label and makes otherwise compact reward rows visually unstable.

Accepted product decision:
- do **not** show a relative generosity/stinginess modifier;
- do **not** compute Faith efficiency in production merely for an invisible cue;
- keep the research data as historical evidence for future product work;
- group reward colors by displayed magnitude only.

### 3. Alchemical decomposition unlock — when applicable

Accepted runtime evidence establishes that 62 / 223 ordinary Study definitions unlock at least one native alchemical decomposition path.

For those unstudied items, show a direct capability cue:
- Russian: **Исследование позволит использовать этот предмет в алхимии.**
- English: **Studying unlocks an alchemy use.**

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

## Accepted product shape

Before an incomplete Study, the predictive block is conceptually:

Russian:
`Награда за исследование: [point icon(s)] [magnitude]`
`[second reward row only if another color group has a different magnitude]`
`Исследование позволит использовать этот предмет в алхимии.` — only when applicable.

English:
`Study reward: [point icon(s)] [magnitude]`
`[second reward row only if another color group has a different magnitude]`
`Studying unlocks an alchemy use.` — only when applicable.

Rules:
- every ordinary Study gets a magnitude assessment using the accepted four-tier scale for the active supported language;
- the first reward group stays on the same line as the Study-reward heading when the native tooltip width permits;
- each complete reward group — its point icon(s) plus its qualitative label, including two-word labels such as **Очень высокая / Very High** — must wrap as one visual unit;
- if the first group does not fit after the heading, the whole group may move to the next centered line; the icon must not remain separated from its label;
- equal magnitudes across colors are collapsed into one group;
- different magnitudes remain separate rows;
- no relative generosity/stinginess modifier is shown;
- the alchemy cue is a separate explanatory sentence and may wrap naturally;
- the native incomplete-Study row may be replaced/restructured rather than preserved literally, provided the resulting block still communicates pending Study and the reward color(s) clearly;
- exact point quantities, exact alchemy results, and Faith-efficiency formulas remain hidden.

After completed Study:
- remove all predictive cues;
- preserve vanilla completed behavior.

The current vanilla dataset yields:
- 217 / 223 Study definitions (97.3%) required only one reward row under the original stricter grouping model; magnitude-only grouping can only reduce, never increase, the number of split reward rows;
- 62 / 223 (27.8%) receive the additional alchemy sentence.

## Questions the mod should answer

1. **What kind of technology points will I get?**
   - Vanilla already answers the color; preserve it.

2. **Roughly how much will I get?**
   - Study Reward Insight: qualitative magnitude.

3. **Will studying this item unlock an alchemical use?**
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

Accepted for candidate 0.1.2:
- Russian scale: **Низкая / Средняя / Высокая / Очень высокая**;
- English scale: **Low / Medium / High / Very High**;
- Russian heading: **Награда за исследование:**;
- English heading: **Study reward:**;
- Russian alchemy cue: **Исследование позволит использовать этот предмет в алхимии.**;
- English alchemy cue: **Studying unlocks an alchemy use.**;
- reward icons and their magnitude label wrap as one visual unit;
- no player-facing relative generosity/stinginess modifier;
- color grouping by magnitude.

Candidate 0.2.0 extends the same model to all eleven game locales. Remaining acceptance is multilingual runtime typography/wrapping and final wording review for the nine newly enabled locales.

Accepted in 0.1.2 real runtime:
- atomic reward-group wrapping;
- Russian alchemy wording and layout;
- English heading, magnitude labels, alchemy wording and layout.

## Remaining engineering questions

The production owner/data-path questions are closed for the current Russian candidate. The selected implementation remains the narrow postfix/read-only native-data path documented in `docs/PRODUCTION_EVIDENCE_GATE.md`.

No new broad host/runtime probe is required. Candidate 0.2.0 uses the same accepted owner/writer path and requires only multilingual runtime presentation acceptance before stable promotion.
