# Study Value Dump 0.2.0

Research-only, read-only incremental probe for **Study Reward Insight** on **Graveyard Keeper 1.407**.

## Research-method checkpoint

### Exact unknowns

The accepted 0.1.0 dataset already establishes every ordinary Survey row, Faith cost, and baseline R/G/B reward. The remaining product-value questions are:

1. which Study items unlock native alchemical decomposition;
2. which decomposition classes/crafts are associated with each Study item;
3. the exact Science cost carried by `CraftDefinition.needs_from_wgo`;
4. the full chance/min/max expressions of **all** Survey outputs, especially Story outputs;
5. whether ordinary Study definitions contain additional special completion fields that matter to player value.

### Why accepted evidence is not enough

Static 1.407 source proves that:
- completed Study gates `AlchemyDecompose` visibility/availability;
- `ShowSurveyCompleteWindow` reveals decomposition information after Study;
- Survey completion processes the normal `CraftDefinition.output` list through `ResModificator.ProcessItemsListBeforeDrop`;
- that processor evaluates per-item chance and min/max expressions.

The 0.1.0 runtime dump did not capture:
- `needs_from_wgo`;
- decomposition mappings;
- non-tech output expressions.

The public/community sources are useful cross-checks but do not provide a complete authoritative 1.407 mapping.

### Decision

One incremental read-only runtime probe is the lowest-assumption method. It reuses the already-proven loaded-`GameBalance` seam and records only missing fields.

## Safety

The probe:
- uses no Harmony patches;
- does not call `ResModificator.ProcessItemsListBeforeDrop`;
- does not execute Study or alchemy;
- does not mutate crafts, items, inventory, player state, save state, or completed-study state;
- calls the host's read-only item metadata accessor `GetItemDetails()`, which may populate its normal in-memory cache;
- enumerates loaded definitions once after gameplay starts, logs structured data, then becomes inert.

## Runtime handoff

1. Remove the old `StudySurveyDump-0.1.0.dll` if it is still installed.
2. Put `StudyValueDump-0.2.0.dll` in a BepInEx plugin folder.
3. Launch Graveyard Keeper and load any save until normal gameplay is visible.
4. No Study Table, inventory setup, crafting, saving, or alchemy interaction is required.
5. Exit normally and return the full `BepInEx/LogOutput.log`.

Structured records are bounded by `SRI_VALUE_BEGIN` and `SRI_VALUE_DONE`.
