# Study Survey Dump 0.1.0

Research-only, read-only probe for **Study Reward Insight** on **Graveyard Keeper 1.407**.

## Question

Establish the complete loaded Study/Survey dataset needed to design qualitative reward bands:
- every item for which native `ItemDefinition.GetSurveyCraft()` resolves a Survey craft;
- every `CraftDefinition` whose `craft_type` is `Survey`, including orphan/special rows;
- raw red/green/blue outputs and Faith/Science needs;
- multi-color combinations and reward histograms;
- item type/quality/product-tier metadata;
- quality variants sharing a Survey craft;
- technology-point outputs using chance groups or min/max/chance expressions.

## Why runtime

Accepted research proves ownership but only has grave-only Study rows plus all-game positive-red/positive-blue subsets. Green-only and zero-red Survey rows are missing. Exact balance data is loaded from Unity `Resources/game_data`; reading the already-deserialized `GameBalance` is narrower and less assumption-heavy than offline Unity asset parsing.

## Safety

The probe uses no Harmony patches, does not mutate balance data, inventory, recipes, player state or save state, does not execute Study, and does not call `ResModificator.ProcessItemsListBeforeDrop` (so it does not advance RNG). It dumps once after gameplay starts and then becomes inert.

## Runtime handoff

1. Put `StudySurveyDump-0.1.0.dll` in a BepInEx plugin folder.
2. Launch Graveyard Keeper and load any save until normal gameplay is visible.
3. No Study Table, item setup, crafting or saving is required.
4. Exit normally and return the full `BepInEx/LogOutput.log`.

The full log also exposes the ordinary plugin-load section, which is useful for detecting possible modded Survey-data contamination. Structured records are bounded by `SRI_SURVEY_BEGIN` and `SRI_SURVEY_DONE`.
