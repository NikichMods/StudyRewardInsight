# Study / Survey Reward Distribution — Graveyard Keeper 1.407

Status: **accepted research dataset for product-band design; production behavior remains BLOCKED**

This document records the full Study/Survey reward distribution recovered for Study Reward Insight from the research-only `StudySurveyDump 0.1.0` runtime probe.

## Evidence identity

- Target: Graveyard Keeper **1.407**
- Probe source branch: `research/full-study-dataset`
- Probe source SHA: `790fe496384cc578081b55245e47e6a0ab8e2b7f`
- Runtime log SHA-256: `ea2179fca79c8277ccb7bdfcf95787f3d79327234a1d12b98f658452e6fbff89`
- Canonical host reference: `Kupie/GYK_DECOMP@6abf79199d92482af1c7573870dd9a20ec2270b9`
- Normalized machine-readable dataset: `research/data/study-survey-1.407-normalized.csv`

The probe read the already-deserialized `GameBalance` once after normal gameplay became active. It applied no Harmony patches and did not execute Study or call `ResModificator.ProcessItemsListBeforeDrop`, so the research pass did not advance the output-processing RNG.

## Environment / contamination check

The runtime contained 37 BepInEx plugins, including Better Grave Crafting Rewards (BGCR), Queue Everything!, Alchemy Research Redux, and the research probe.

### Better Grave Crafting Rewards

The dump completed before BGCR applied its G2 Study mutations:

1. `SRI_SURVEY_DONE`
2. vanilla `OnGameStartedPlaying`
3. `Better Grave Crafting Rewards] G2 active ... studies=23`

This ordering is also consistent with the captured grave Study rows: for example the wooden grave marker was logged with raw blue `11`, represented as the ordinary fixed `10` blue plus the conditional Circumspect placeholder described below. BGCR's accepted G2 code expects that vanilla raw value before changing the fixed reward.

**Conclusion:** the captured Survey reward values precede BGCR's 23 Study blue changes and are usable as vanilla 1.407 reward evidence.

### Queue Everything!

The runtime configuration had:
- `Auto-Craft = False`
- `Half Research Point Outputs = False`

Its mutation summary reported `converted=0` and `halved=0`.

Current inspected Queue Everything! source also gates its R/G/B output halving behind that disabled setting.

**Conclusion:** Queue Everything! did not change the captured R/G/B Survey values.

### Other loaded mods

No evidence was found that another loaded plugin mutated native Survey R/G/B definitions before the dump. Alchemy Research Redux's inspected behavior concerns alchemy-recipe knowledge/preview/refill rather than Survey reward values.

## Coverage

The loaded dataset contains:

- **396 item -> Survey mappings**
- **234 unique Survey crafts**
- **0 orphan Survey crafts**
- **11 `SurveySciencePoints` decomposition crafts**
- **223 ordinary one-time Study crafts**

Every ordinary Study craft has at least one positive baseline red, green, or blue technology-point reward.

`ItemDefinition.GetSurveyCraft()` sharing matters: quality/body-part variants can map to one common Survey craft. Therefore item-row count must not be mistaken for unique research opportunities.

## Science-decomposition rows are a separate behavior

The 11 `SurveySciencePoints` rows are:

| Survey item family | Science output |
|---|---:|
| Clean Paper | 2 |
| Notes I / II / III | 4 / 5 / 7 |
| Chapter I / II / III | 15 / 20 / 25 |
| Book I / II / III | 30 / 50 / 75 |
| Lens | 20 |

These rows do not award ordinary R/G/B Study points and should not receive the proposed qualitative R/G/B reward cue.

## Conditional Circumspect blue bonus

The raw dump marked 173 Survey rows as `dynamic_points=true`.

Inspection of every dynamic point entry shows exactly one common shape:

`b:value=1, chance_group=-1, min=1*Ppar("buff_survay")`

There are no other chance-group, random-range, or min/max dynamic technology-point outputs in the Survey dataset.

This is the game's temporary Circumspect Study bonus. Its raw serialized placeholder value must **not** be treated as unconditional blue reward.

For distribution analysis this document therefore defines:

- **baseline reward** = fixed R/G/B Survey reward with the conditional `buff_survay` entry excluded;
- **effective runtime reward** = host-processed Survey output at tooltip time, which may include the +1 blue Circumspect bonus when active.

This distinction fixes the misleading raw histogram where 194 Survey rows appeared to have positive blue output.

## Vanilla baseline color coverage

Across the 223 ordinary Study crafts:

- positive red: **69**
- positive green: **108**
- positive blue: **61**

Baseline color combinations:

| Combination | Study crafts |
|---|---:|
| Green only | 101 |
| Red only | 58 |
| Blue only | 52 |
| Red + Blue | 5 |
| Red + Green | 3 |
| Red + Green + Blue | 3 |
| Green + Blue | 1 |

Multi-color baseline rewards are uncommon but real: **12 / 223 (5.4%)**. The product therefore must support independent qualitative treatment of every positive color in the effective output instead of assuming one reward color per item.

## Reward distributions

### Red

69 positive-red Study crafts.

| Red reward | Count |
|---:|---:|
| 1 | 5 |
| 5 | 3 |
| 10 | 18 |
| 15 | 1 |
| 20 | 6 |
| 25 | 1 |
| 30 | 10 |
| 50 | 11 |
| 100 | 10 |
| 150 | 4 |

Median: **30**. Mean: **40.72**. Range: **1–150**.

### Green

108 positive-green Study crafts.

| Green reward | Count |
|---:|---:|
| 1 | 10 |
| 5 | 16 |
| 10 | 34 |
| 15 | 1 |
| 20 | 18 |
| 30 | 15 |
| 40 | 4 |
| 50 | 9 |
| 70 | 1 |

Median: **10**. Mean: **17.92**. Range: **1–70**.

### Blue

61 positive-blue Study crafts.

| Blue reward | Count |
|---:|---:|
| 1 | 7 |
| 5 | 10 |
| 10 | 3 |
| 15 | 1 |
| 20 | 9 |
| 30 | 7 |
| 45 | 1 |
| 50 | 7 |
| 80 | 2 |
| 90 | 3 |
| 100 | 9 |
| 120 | 2 |

Median: **30**. Mean: **40.28**. Range: **1–120**.

## Distribution interpretation

The colors do **not** share one useful numerical scale.

The observed values naturally leave different gaps:

- red: `10 -> 15`, `30 -> 50`, `50 -> 100`
- green: `10 -> 15`, `20 -> 30`, `40 -> 50`
- blue: `10 -> 15`, `30 -> 45`, `50 -> 80`

Blue especially has four well-separated practical groups:
- 1 / 5 / 10
- 15 / 20 / 30
- 45 / 50
- 80 / 90 / 100 / 120

Collapsing blue to three bands would merge either the 15–30 group with 45–50, or the 45–50 group with 80+, losing a real distinction present in the vanilla data.

## Accepted four-band product model

**Status: accepted product behavior for Study Reward Insight.**

A four-tier vocabulary can be shared across colors while thresholds remain color-specific.

| Color | Небольшая | Умеренная | Большая | Очень большая | Population |
|---|---:|---:|---:|---:|---|
| Red | 1–12 | 13–40 | 41–75 | 76+ | 26 / 18 / 11 / 14 |
| Green | 1–12 | 13–25 | 26–45 | 46+ | 60 / 19 / 19 / 10 |
| Blue | 1–12 | 13–37 | 38–65 | 66+ | 20 / 17 / 8 / 16 |

The boundaries sit inside empty gaps of the observed vanilla distributions rather than on populated values.

This also makes the vanilla Circumspect +1 comparatively stable: fixed vanilla blue values are not immediately adjacent to the proposed boundaries. A modded Survey value near a boundary may legitimately move bands after effective-output processing.

The accepted Russian labels are `Небольшая / Умеренная / Большая / Очень большая`. Other-language localization remains a later localization task.

## Runtime source requirement

The normalized CSV is **research evidence only**. It must not become a production lookup table.

Production should derive the cue from the currently resolved Survey craft and the host's effective/processed output at tooltip time so that:

- vanilla Circumspect behavior is respected;
- mods changing Study rewards are reflected automatically;
- multi-color rewards are handled from the current output;
- no item-ID whitelist is needed;
- completed Study remains vanilla-owned.

The next implementation evidence gate must prove the least-invasive way to obtain those effective values without consuming meaningful RNG or otherwise changing game state.

## Science-cost limitation

Probe 0.1.0 logged `CraftDefinition.needs` but not `needs_from_wgo`.

Static 1.407 inspection shows Study Science cost is carried through `needs_from_wgo` / the workstation resource path, so the normalized dataset does **not** establish exact Science cost per Study craft.

This does not block the current product question because the intended tooltip cue represents **absolute Study yield**, not exact Faith/Science efficiency. If the product scope changes to an efficiency ranking, exact Science cost becomes a new evidence requirement.

## Gate consequence

The reward-distribution question needed before band design is now **READY**:

- complete ordinary Study population identified;
- R/G/B baseline distributions established;
- multi-color cases established;
- the only dynamic technology-point modifier characterized;
- mod contamination relevant to reward values excluded.

Production behavior remains **BLOCKED** until the implementation gate proves the reward read boundary and the minimal native-tooltip insertion/restructuring mechanism.
