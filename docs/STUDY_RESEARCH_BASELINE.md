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
- the **native-row + short qualitative line** family remains a preferred direction;
- the **effective-output** principle remains desirable for compatibility;
- the four historical blue thresholds are **not accepted**;
- grave-only filtering is explicitly rejected as the default product scope;
- blue-only semantics are explicitly insufficient unless later evidence justifies them;
- the exact Harmony/row-identification mechanism remains unselected until its evidence gate is READY.

## What prior evidence already closes

No new probe is needed merely to prove that:

1. native Study rewards are represented in Survey `CraftDefinition.output`;
2. the standard item tooltip has an event-driven `ItemDefinition.GetTooltipData` seam;
3. vanilla already distinguishes incomplete and completed Study;
4. vanilla intentionally shows point colors without exact quantities;
5. Study can award more than blue points;
6. a qualitative cue can conceptually be presentation-only without changing Study economics.

## What remains open

The next research stage must establish, from authoritative 1.407 evidence:

1. the **full native Study/Survey item set**, not just grave decorations;
2. actual reward distributions for red, green, and blue across that set;
3. multi-color Survey combinations and frequency;
4. early/mid-game-important groups and the practical meaning of their reward magnitudes;
5. whether tiny values such as 1 point should receive a qualitative label or no additional cue;
6. natural threshold candidates per color, without assuming equal intervals or equal R/G/B scales;
7. the narrowest robust presentation seam for adding the cue while preserving all native tooltip rows and completed-state behavior;
8. exact localization/layout implications for the supported language set.

## Current production gate

**BLOCKED**

Reason: the product-level reward-band model and full all-item Study dataset are not yet established, and the final presentation/processed-value boundary has not yet been proved for the broader Study Reward Insight scope.

Allowed now:
- static/data research;
- analysis of already accepted runtime evidence;
- a narrowly justified read-only probe only if existing evidence/direct inspection cannot produce the complete Study dataset;
- UX trade study.

Not allowed yet:
- production tooltip mutation;
- fixed reward thresholds presented as final;
- item whitelist/category table;
- grave-only or blue-only production behavior inherited from BGCR.
