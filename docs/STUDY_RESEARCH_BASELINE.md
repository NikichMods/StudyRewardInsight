# Study / Survey Research Baseline

Target: **Graveyard Keeper 1.407**

Status: **accepted prior evidence recovered; Study Reward Insight production behavior remains BLOCKED**

This document is the project-local starting point for Study Reward Insight. It records only previously established facts and clearly separates them from historical product candidates. Reusable host/runtime facts remain canonical in `NikichMods/GraveyardKeeperResearch`.

## Evidence sources reviewed

Canonical shared source:
- `NikichMods/GraveyardKeeperResearch/docs/RESEARCH_INDEX.md`
- `docs/CRAFTING_INVENTORY_AND_TRADING.md`
- `docs/GAME_INTERNALS.md`

Historical project source:
- `NikichMods/BetterGraveCraftingRewards`
- branch `research/study-tooltip-ux`
- `docs/STUDY_TOOLTIP_UX.md`
- `docs/VANILLA_BALANCE_RESEARCH.md`
- `docs/PRODUCTION_EVIDENCE_GATE.md`

The Better Grave Crafting Rewards material is historical evidence for hypotheses and already-tested host behavior. Its product scope, G2 balance model, grave-item target set, reward bands, and UX choices are **not** automatically requirements for Study Reward Insight.

## Accepted host facts

### Survey reward ownership

Shared Graveyard Keeper 1.407 research establishes:

- `GameBalance.craft_data` contains native craft definitions.
- Technology-point rewards are represented as ordinary `CraftDefinition.output` items.
- `TechDefinition.TECH_POINTS` classifies point IDs including `r`, `g`, and `b`.
- `ItemDefinition.GetSurveyCraft()` resolves an item's native Survey craft.
- Study/Survey technology-point rewards use the same `CraftDefinition.output` data model.
- The inspected normal Survey/craft completion path consumes the native output through the host-owned craft/output machinery.

Implication: Study Reward Insight should treat the resolved native Survey craft/output as the starting source of truth rather than maintain an independent item-to-reward table.

### Native unstudied tooltip semantics

Pinned 1.407 inspection in shared research establishes that `ItemDefinition.GetTooltipData(Item,bool)` owns the standard item-tooltip Survey hint.

For an incomplete Survey:
- the host resolves the Survey craft;
- it iterates the processed Survey output;
- it keeps technology-point entries;
- it renders the point **types/icons** but deliberately omits numeric `Item.value` quantities.

For a completed Survey:
- the same native owner renders the ordinary completed state.

Implication: the game already exposes reward type while intentionally preserving quantity uncertainty. A qualitative magnitude cue can preserve that information model.

### Effective/processed output evidence

Historical BGCR inspection of the same 1.407 path records that the incomplete-Survey tooltip processes Survey output through `ResModificator.ProcessItemsListBeforeDrop(...)` during gameplay before collecting technology-point types.

This supports, but does not yet by itself finalize, the Study Reward Insight compatibility requirement that a cue should follow the current effective Survey output rather than a copied vanilla table.

Before production implementation, re-confirm the exact processed-output value available at the chosen patch/data boundary and its applicability to all intended Survey items.

### Standard item-tooltip UI family

Shared UI research establishes for the inspected standard item-tooltip path:

- `ItemDefinition.GetTooltipData(Item,bool)` returns the native `List<BubbleWidgetData>`;
- text rows are rendered by `BubbleWidgetText`;
- `WidgetsBubbleGUI` sizes the enclosing bubble from current child-widget geometry;
- child text alignment and child placement inside the centered bubble are distinct concerns.

Applicability limit: Technology-tooltip width findings are a different UI family and must not be generalized to the standard item tooltip without evidence.

## Previously established Study data relevant to scope

BGCR's accepted runtime research was grave-balance-specific, not a complete Study Reward Insight dataset.

It nevertheless establishes useful boundaries:

- its grave dataset contained 24 Survey/Study definitions;
- grave Study blue rewards covered a broad range and rose substantially with tier;
- later BGCR whole-game red-economy research found **69 positive-red Survey rows** in the loaded dataset and a raw positive-red Survey pool of 2810 red;
- representative Survey rewards include substantial red values, so Study is demonstrably not a blue-only mechanism.

These facts support designing Study Reward Insight for multiple technology-point colors. They do **not** establish the full native Study population or final R/G/B qualitative thresholds.

## Historical BGCR UX candidate — not accepted here

BGCR explored a grave-only cue with these candidate properties:

- preserve the native Study row;
- append a short second qualitative line;
- show no exact quantity;
- hide the cue after Study completion;
- derive the cue from effective live Survey output;
- use four blue bands: 1–40 / 41–80 / 81–120 / 121+;
- scope the cue only to 23 active G2 grave Study targets;
- likely extend the existing Survey text row rather than replace the tooltip.

For Study Reward Insight:
- the historical **native-row + short qualitative line** layout is no longer a requirement;
- the native incomplete-Study row may be replaced/restructured when the accepted Study Reward Insight block preserves its semantics more clearly;
- the **effective-output** principle remains desirable for compatibility, but must not force a broader or state-mutating mechanism when a simpler safe reward source satisfies the accepted product envelope;
- the historical BGCR blue thresholds are rejected;
- grave-only filtering is explicitly rejected as the default product scope;
- blue-only semantics are explicitly insufficient;
- the exact Harmony/row-identification mechanism remains unselected until its evidence gate is READY.

## What prior evidence already closes

No new probe is needed merely to prove that:

1. native Study rewards are represented in Survey `CraftDefinition.output`;
2. the standard item tooltip has an event-driven `ItemDefinition.GetTooltipData` seam;
3. vanilla already distinguishes incomplete and completed Study;
4. vanilla intentionally shows point colors without exact quantities;
5. Study can award more than blue points;
6. a qualitative cue can conceptually be presentation-only without changing Study economics.

## Full-dataset research completed — 2026-09-28

The all-item Study/Survey distribution is now established from the accepted `StudySurveyDump 0.1.0` runtime evidence and normalized in:

- `docs/STUDY_REWARD_DISTRIBUTION.md`
- `research/data/study-survey-1.407-normalized.csv`

Established:
- 396 item -> Survey mappings;
- 234 unique Survey crafts;
- 11 `SurveySciencePoints` decomposition rows;
- 223 ordinary one-time Study rows;
- baseline positive reward coverage of 69 red / 108 green / 61 blue;
- 12 genuinely multi-color baseline Study rows;
- all 173 dynamic technology-point rows use the same conditional `buff_survay` +1 blue mechanism;
- natural distribution gaps support color-specific threshold candidates rather than one shared numerical scale.

The probe session's reward values are usable as vanilla 1.407 evidence: its dump completed before Better Grave Crafting Rewards applied its 23 G2 Study mutations, and Queue Everything! reported zero research-output halving.

## What remains open

The product semantics are now accepted. Before production behavior can become READY:

1. prove the narrowest robust presentation seam that can restructure the incomplete-Study block while preserving unrelated tooltip rows and the native completed state;
2. choose and prove the least-complex safe reward read model, including the known vanilla Circumspect +1-blue case, without consuming meaningful RNG or changing game state;
3. establish real-runtime layout/wrapping acceptance for the Russian block;
4. establish localization behavior for the supported language set.

Exact Science cost per Study row was not captured by probe 0.1.0 because it lives on the workstation-resource path (`needs_from_wgo`). This does not block the current absolute-yield cue. It becomes relevant only if the product changes to a Faith/Science-efficiency ranking.

## Current production gate

**BLOCKED**

Reason: product vocabulary, magnitude thresholds, exception thresholds, grouping, and alchemy wording are accepted; the final reward-read and presentation boundaries are not yet proved.

Allowed now:
- static inspection and narrowly justified presentation/reward-read research;
- implementation planning behind an explicit evidence gate.

Not allowed yet:
- production tooltip mutation;
- item whitelist/category table;
- grave-only or blue-only production behavior inherited from BGCR.
