# Study Value Runtime Evidence 0.2.0

Target: **Graveyard Keeper 1.407**

Status: **accepted project research evidence; production behavior remains BLOCKED**

## Evidence identity

- Probe: `StudyValueDump 0.2.0`
- Probe source branch: `research/study-value-model`
- Build source SHA: `2256dd86d0f7f5c1e5f7b57887472f70af3cfc42`
- Probe DLL SHA-256: `62fc4314593f72c60871a7f83ec188c7b62c0d1c1ef9b1278cf01e5070dc2004`
- Returned runtime log SHA-256: `57ec5f9a563b67ce6c3d8000f30cbf672f7157171c9a599d3bccf55a8b69070c`
- Host reference: `Kupie/GYK_DECOMP@6abf79199d92482af1c7573870dd9a20ec2270b9`
- Runtime: Steam, Graveyard Keeper 1.407, BepInEx 5.4.23.5.

The probe was read-only. It did not execute Study, invoke `ResModificator.ProcessItemsListBeforeDrop`, change craft definitions, mutate save state, or call RNG-bearing reward processing.

## Completion

The log contains:
- `SRI_VALUE_BEGIN`;
- 223 `SRI_VALUE_SURVEY` records;
- `SRI_VALUE_DONE|ordinary_surveys=223|decomposable_surveys=62|story_output_surveys=173|special_nontech_surveys=2`;
- no `SRI_VALUE_ERROR`.

Therefore the 0.2.0 dump completed successfully and covers the same 223 ordinary one-time Study definitions established by the 0.1.0 dataset.

## Alchemical decomposition value

Exactly **62 / 223** ordinary Study definitions map to at least one native `AlchemyDecompose` path.

Observed decomposition-type combinations:
- type 1 only: 21;
- type 2 only: 6;
- type 3 only: 4;
- types 1+2: 5;
- types 1+3: 5;
- types 2+3: 9;
- types 1+2+3: 8;
- special type 9: 4 goo-recovery rows.

This closes the existence/population question. A pre-Study hint can be derived dynamically from loaded host data rather than an item whitelist.

The player-facing hint should disclose only the capability unless a later product decision explicitly allows more:
- safe: `Study unlocks alchemical decomposition`;
- avoid before Study: exact element, exact workstation, exact output.

## Science cost

Every ordinary Study row has an explicit `science` cost in `needs_from_wgo`.

Distribution:
- Science 1: 102 rows;
- Science 2: 31;
- Science 3: 36;
- Science 4: 5;
- Science 5: 23;
- Science 7: 4;
- Science 8: 3;
- Science 10: 19.

Science and Faith are **not interchangeable**: 55 / 223 rows have different Science and Faith costs.

Product consequence:
- Faith-normalized reward efficiency remains a legitimate independent signal because that is the intended scarcity question;
- Science must not be silently folded into the same efficiency denominator;
- if Science becomes player-facing, it needs a separate semantic treatment rather than an invented combined cost score.

## Story outputs

173 Study definitions contain Story outputs.

All observed Story outputs are gated by:
`min = 1 * Ppar("p_naturalist")`

Two host shapes exist.

### Independent self-chance

Lower-tier rows commonly have one `story:1` output with:
- `self=0.1`, `0.2`, or `0.3`;
- `chance_group=-1`.

These are ordinary independent chance checks.

### Weighted chance groups

Higher-tier rows use `chance_group=1` and multiple Story qualities, for example:
- 0.3 / 0.1;
- 0.3 / 0.2;
- 0.4 / 0.3;
- 0.4 / 0.3 / 0.3.

Pinned `ResModificator.ProcessItemsListBeforeDrop` source selects one member from a positive-weight chance group by drawing across the sum of the weights. Therefore these weights are **relative selection weights**, not independent absolute drop percentages.

Product consequence:
- Story is conditional/perk-sensitive Study value;
- it should not be merged into a generic Study-value rating;
- displaying Story prediction is not currently justified by the core product goal and would add substantial tooltip noise.

## Special non-tech Study outputs

Only two ordinary Study definitions have non-Story, non-tech outputs:

1. Keeper's Key Study:
   - deterministic `ques_key_cultist`;
   - blue reward also present.
2. Obsidian Study:
   - deterministic `obsidian_research`;
   - red/green/blue rewards also present.

All surveyed generic completion fields are empty across the 223 rows:
- `end_event`;
- `end_script`;
- `craft_after_finish`;
- `ach_key`.

These two rows are special authored progression cases and must not define generic tooltip scoring semantics.

## Accepted implication for the value model

The Study value model now has three evidence-backed generic axes:

1. absolute technology-point magnitude by color;
2. technology-point return per Faith by color;
3. whether Study unlocks alchemical decomposition.

Story is real but conditional and non-core.
Special quest/progression outputs are real but exceptional.

The remaining product question is presentation, not whether these mechanisms exist.
