# Candidate 0.1.0 — Russian Study tooltip preview

Status: **production candidate built; real-runtime visual acceptance pending; do not promote to `main` yet**

## Exact identity

- candidate version: `0.1.0`
- production source SHA: `20656d4c2a260a7716ad6c87a550806426412e90`
- branch: `feature/study-tooltip-preview`
- GitHub Actions run: `36444957560`
- workflow artifact ID: `10980401162`
- artifact name: `StudyRewardInsight-0.1.0-20656d4c2a260a7716ad6c87a550806426412e90`
- installed file: `StudyRewardInsight.dll`
- DLL SHA-256: `89c98a2dbeff7650be0433131e6007b16e5d9d6208ebc19c81d5e4f42353b5fe`
- target: Graveyard Keeper 1.407 / BepInEx 5

The earlier CI artifact from source `8ebb405c48adc259d2d60144a8465d4dfd96a16b` was superseded before handoff by the hardened localization-binding source above.

## Implemented behavior

For Russian-language, full-detail, ordinary incomplete Study tooltips:

- locate the already-created native incomplete Survey text row;
- read the current Survey `CraftDefinition.output` directly;
- read fixed R/G/B rewards without re-running `ResModificator.ProcessItemsListBeforeDrop`;
- handle the single accepted vanilla dynamic R/G/B shape, Circumspect `+1 blue`, from the current `buff_survay` player parameter;
- read current fixed Faith need only for the internal optional relative modifier;
- apply the accepted red / green / blue magnitude thresholds;
- group colors only when magnitude and optional modifier are identical;
- append the alchemy sentence only when native `ItemDefinition.GetItemDetails().alchemy.decomposes` contains at least one real decomposition type;
- replace only the text of the existing native incomplete Survey `BubbleWidgetTextData`.

The candidate does not create a tooltip, widget, renderer, cache, per-frame path, or additional reward-processing call.

If SRI sees an unsupported dynamic R/G/B output shape, it leaves that tooltip's vanilla Study row unchanged and logs one warning for that Survey craft.

## Preserved behavior

- completed Study stays native;
- `SurveySciencePoints` stays native;
- non-Russian languages stay native in this candidate;
- Story output is not predicted;
- exact technology-point quantities/ranges are not shown;
- exact alchemy output/workstation is not shown;
- Study rewards, Faith/Science costs, recipes, technologies, progression and save state are not changed.

## Build/test evidence

GitHub Actions run `36444957560`:

- exact checkout SHA verified as `20656d4c2a260a7716ad6c87a550806426412e90`;
- production restore succeeded;
- production build succeeded;
- **0 warnings / 0 errors**;
- reward-model tests passed;
- development artifact uploaded with exact source identity;
- downloaded DLL hash matches the workflow identity file.

Reward-model checks cover:
- every accepted red threshold boundary;
- every accepted green threshold boundary;
- every accepted blue threshold boundary;
- modifier boundaries below 7.5, exactly 7.5, exactly 12.5, and above 12.5;
- zero-Faith modifier suppression;
- grouping only for identical complete displayed assessments;
- alchemy-line formatting.

## Remaining acceptance

No new research probe is required.

The remaining blocker is real runtime/perceptual acceptance of the production candidate:

1. run the game in Russian with this exact DLL;
2. inspect ordinary **unfinished** Study item tooltips available on the normal save;
3. confirm the block renders cleanly and the point icons remain native-looking;
4. where naturally accessible, inspect a multi-color Study and an item that shows the alchemy sentence;
5. inspect any convenient already-studied item and confirm its completed state is unchanged;
6. retain the normal BepInEx log for checking initialization or fail-safe warnings.

The user does not need to obtain arbitrary items or synthesize a test state. Visual screenshots plus the normal log are sufficient evidence for this gate.
