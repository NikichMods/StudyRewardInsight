# Study Faith Efficiency — Graveyard Keeper 1.407

Status: **accepted research analysis; product semantics remain open**

This analysis uses the accepted normalized Study dataset in `research/data/study-survey-1.407-normalized.csv`.

Machine-readable per-Study calculations are stored in:
- `research/data/study-faith-efficiency-1.407.csv`

## Definition

For an ordinary Study craft with positive technology-point output and Faith cost > 0:

- red efficiency = baseline red / Faith;
- green efficiency = baseline green / Faith;
- blue efficiency = baseline blue / Faith.

Colors are kept separate. Red, green, and blue are different progression currencies and are **not** summed into a synthetic "total points per Faith" score.

The probe's `faith` field is a direct read of the native `faith` entry in `CraftDefinition.needs`. Re-parsing the serialized `needs` field reproduces every captured Faith value with zero mismatches.

The 11 `SurveySciencePoints` decomposition crafts are excluded.

## Faith-cost distribution

Across 223 ordinary one-time Study crafts:

| Faith cost | Study crafts |
|---:|---:|
| 0 | 45 |
| 1 | 54 |
| 2 | 32 |
| 3 | 35 |
| 4 | 7 |
| 5 | 24 |
| 7 | 4 |
| 8 | 3 |
| 10 | 19 |

The 45 zero-Faith rows are mostly low-value alchemical powders, solutions, extracts and goo variants. Their points-per-Faith ratio is undefined; operationally they are free with respect to Faith and should be treated separately from paid Study.

## Reward and efficiency by Faith cost

Each reward cell below is `median [min–max]` among Studies at that Faith cost that actually award that color. Efficiency is the median reward per Faith for those positive-color rows.

| Faith | Studies | Red reward | R/Faith | Green reward | G/Faith | Blue reward | B/Faith |
|---:|---:|---:|---:|---:|---:|---:|---:|
| 0 | 45 | 1 [1–1] | free | 5 [1–5] | free | 5 [1–5] | free |
| 1 | 54 | 10 [5–10] | 10 | 10 [10–10] | 10 | 20 [5–20] | 20 |
| 2 | 32 | 20 [5–20] | 10 | 20 [10–20] | 10 | 20 [10–45] | 10 |
| 3 | 35 | 30 [30–50] | 10 | 30 [10–30] | 10 | 30 [10–50] | 10 |
| 4 | 7 | 25 [10–50] | 6.25 | 40 [15–50] | 10 | 50 [50–100] | 12.5 |
| 5 | 24 | 50 [50–100] | 10 | 50 [50–50] | 10 | 75 [30–100] | 15 |
| 7 | 4 | 100 [100–100] | 14.29 | 70 [70–70] | 10 | 80 [80–80] | 11.43 |
| 8 | 3 | — | — | — | — | 90 [90–90] | 11.25 |
| 10 | 19 | 100 [100–150] | 10 | — | — | 100 [100–120] | 10 |

## Strong host pattern: approximately 10 points per Faith

For positive paid Study rewards:

- red: 64 rows; **48 / 64 (75.0%)** are exactly 10 red per Faith;
- green: 82 rows; **77 / 82 (93.9%)** are exactly 10 green per Faith;
- blue: 45 rows; only **16 / 45 (35.6%)** are exactly 10 blue per Faith.

Green is therefore almost a direct `10 × Faith` scale. Red follows the same pattern strongly but has more exceptions. Blue is deliberately much less uniform and contains several substantial efficiency premiums.

## Blue-efficiency examples

Representative high blue-per-Faith rows:

| Item | Faith | Blue | Blue/Faith |
|---|---:|---:|---:|
| Obsidian | 4 | 100 | 25 |
| Incense II | 2 | 45 | 22.5 |
| Dark heart / intestine / brain | 5 | 100 | 20 |
| Blood / skull / skin / fat / flesh / bone | 1 | 20 | 20 |
| Heart / brain / intestine | 3 | 50 | 16.67 |
| Ceramic funeral urn | 2 | 30 | 15 |

Representative low blue-per-Faith rows:

| Item | Faith | Blue | Blue/Faith |
|---|---:|---:|---:|
| Zombie extract | 3 | 10 | 3.33 |
| Keeper's key | 2 | 10 | 5 |
| Electric powder | 1 | 5 | 5 |
| Stone stele | 5 | 30 | 6 |
| Many ordinary grave items | varies | varies | about 10 |

These examples are efficiency comparisons only. They do not account for item availability, progression stage, acquisition cost, Science cost, or whether that point color is currently useful to the player.

## Product implication

Absolute reward magnitude and Faith efficiency answer different player questions.

Example:
- a 100-blue Study costing 10 Faith is **Very high** in absolute yield but only 10 blue/Faith;
- a 20-blue Study costing 1 Faith is only a modest absolute yield but gives 20 blue/Faith.

Therefore a qualitative cue derived from **absolute reward** should not be described as "efficiency", "value for Faith", "best research", or similar prioritization language.

If the product goal remains:
> tell the player roughly how large the hidden Study reward will be,

then the current absolute-yield bands remain coherent.

If the product goal changes to:
> help the player decide what to spend scarce Faith on first,

then Faith-normalized efficiency becomes the more relevant quantity and requires a separate product model. It should not silently replace absolute yield.

## Current conclusion

Faith cost is strongly correlated with reward size, especially for green and red, so the game already embeds a rough cost scaling. It is **not** strong enough to make absolute yield and efficiency interchangeable, especially for blue.

The current four absolute-yield bands should therefore remain conceptually separate from any future Faith-efficiency indicator.
