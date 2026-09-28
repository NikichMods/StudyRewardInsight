# Production Evidence Gate — Study Tooltip Preview

Target: **Graveyard Keeper 1.407**

Branch: `feature/study-tooltip-preview`

Status: **candidate 0.1.2 atomic reward-group wrapping and English localization READY for implementation; remaining-language localization and final real-runtime visual acceptance stay separate release work**

This record closes the implementation evidence gates required before the first production-source mutation for the accepted Study Reward Insight tooltip model.

## Evidence reviewed

Project evidence:
- `docs/PRODUCT_CONCEPT.md`
- `docs/STUDY_RESEARCH_BASELINE.md`
- `docs/STUDY_REWARD_DISTRIBUTION.md`
- `docs/STUDY_FAITH_EFFICIENCY.md`
- `docs/STUDY_VALUE_MODEL.md`
- `docs/STUDY_VALUE_RUNTIME_0.2.0.md`
- `research/data/study-survey-1.407-normalized.csv`

Shared accepted evidence:
- `NikichMods/GraveyardKeeperResearch/docs/CRAFTING_INVENTORY_AND_TRADING.md`
- `NikichMods/GraveyardKeeperResearch/docs/GAME_INTERNALS.md`
- `NikichMods/GraveyardKeeperResearch/docs/UI_INPUT_TIME_AND_ENVIRONMENT.md`

Pinned host inspection:
- `Kupie/GYK_DECOMP@6abf79199d92482af1c7573870dd9a20ec2270b9`
- `ItemDefinition.GetTooltipData(Item,bool)`
- `ItemDefinition.GetSurveyCraft()`
- `ItemDefinition.GetItemDetails()`
- `ResModificator.ProcessItemsListBeforeDrop(...)`
- `SmartExpression.EvaluateChance/EvaluateFloat/GetRawExpressionString`
- `BaseItemCellGUI`
- `BaseCraftGUI`
- `GameSave.IsSurveyComplete(...)`
- `BubbleWidgetAlchemyItem`

## Solution-family checkpoint — safe reward read

Goal: derive the current incomplete-Study R/G/B qualitative preview without changing Study output, save state, or RNG and without duplicating a vanilla item-to-reward table.

### A. Call `ResModificator.ProcessItemsListBeforeDrop` again

Rejected.

It reproduces the host's processed-output semantics, but it calls Unity RNG through `EvaluateChance`, chance-group selection, and dynamic min/max handling. A tooltip preview must not advance reward-processing RNG merely to inspect a value.

### B. Patch/capture the host's existing `ProcessItemsListBeforeDrop` result

Not selected.

This could capture the exact list produced by vanilla without a second processor call, but it adds a second global Harmony hook plus call-context/reentrancy state to associate a broad shared processor invocation with one `GetTooltipData` call. That is a larger blast radius and introduces ordering/lifecycle assumptions not required by the vanilla 1.407 acceptance envelope.

### C. Transpile `ItemDefinition.GetTooltipData` around the processed-output local

Not selected.

This can reuse the exact native list with one patched method, but it couples the mod to the method's IL/local structure and is materially more brittle than a postfix that operates on the returned native `BubbleWidgetData` list.

### D. Read current `CraftDefinition.output` deterministically and handle the one established vanilla dynamic point case

**Selected.**

Accepted all-item evidence proves that ordinary vanilla 1.407 Survey R/G/B output has only these shapes:
- fixed red entries;
- fixed green entries;
- fixed blue entries;
- one conditional Circumspect blue entry: `b:value=1, chance_group=-1, min=1*Ppar("buff_survay")`.

There are no other vanilla Survey technology-point chance groups, self/common chance expressions, or dynamic min/max shapes.

Implementation policy:
- read the currently resolved Survey craft, not a copied reward table;
- read fixed R/G/B values directly from the current `output`;
- recognize only the exact accepted Circumspect dynamic shape and read the current player `buff_survay` parameter directly;
- never invoke `ResModificator.ProcessItemsListBeforeDrop` from the mod;
- if another dynamic R/G/B shape is encountered, fail safe by leaving the vanilla incomplete-Study row unchanged rather than guessing;
- fixed reward mutations made through native `CraftDefinition.output` remain visible automatically.

This is the least-complex mechanism that covers vanilla 1.407, Better Grave Crafting Rewards/native fixed-output mutations, multi-color Study, and the only established vanilla dynamic point modifier while preserving RNG/state.

## Gate A — qualitative reward snapshot

**READY**

**Observable property**

For a Russian-language, full-detail, ordinary incomplete Study tooltip, derive the positive R/G/B reward assessment used by the accepted magnitude bands and rare generosity/stinginess modifier. Do not expose exact point quantities.

**Canonical owner / data path**

- item -> `ItemDefinition.GetSurveyCraft()`;
- current Study point data -> resolved `CraftDefinition.output`;
- Faith cost used only internally for the optional modifier -> current Survey `CraftDefinition.needs`;
- Circumspect current state -> player parameter `buff_survay`;
- accepted thresholds and grouping -> SRI product model.

**Final writer / consumer**

SRI consumes the read-only snapshot only while transforming the returned `ItemDefinition.GetTooltipData(...)` result. The mod does not write back to the craft, item output, player parameter, save, or RNG.

**Blast radius**

Only ordinary incomplete Survey items on the selected full-detail tooltip path. Unsupported dynamic R/G/B output disables SRI transformation for that tooltip instance and preserves vanilla.

**Preserved invariants**

- no reward/cost/progression/recipe/technology mutation;
- no additional reward-processor invocation;
- no Unity RNG consumption by SRI;
- no Story/special-output prediction;
- `SurveySciencePoints` rows unchanged;
- completed Study unchanged;
- exact hidden point quantities remain hidden.

**Acceptance evidence**

- accepted 223-Study dataset and normalized dynamic-shape audit;
- pinned static host inspection of the native processor and SmartExpression behavior;
- CI compilation;
- deterministic formatter/model tests or equivalent build-time checks for threshold boundaries, zero-Faith behavior, modifiers, and color grouping;
- later real-runtime spot check exercises the real tooltip path; no new research probe is required for the reward owner/read mechanism.

## Gate B — incomplete-Study row restructuring

**READY**

**Observable property**

Replace only the native full-detail incomplete-Study row with the accepted Russian block while preserving every unrelated tooltip row and preserving native completed behavior.

**Canonical owner / data path**

`ItemDefinition.GetTooltipData(Item,bool)` creates the standard item-tooltip `List<BubbleWidgetData>`. Its incomplete Survey branch appends one `BubbleWidgetTextData` containing localized `survey_not_complete` plus point icons.

**Final writer / consumer / commit point**

The postfix sees the method's final returned list. `BaseItemCellGUI` then passes that list to the native tooltip via `SetData`; `WidgetsBubbleGUI` / native bubble widgets render and size it.

**Blast radius**

The Harmony patch is on `ItemDefinition.GetTooltipData`, but mutation is gated to:
- `full_detail == true`;
- non-null item;
- Russian current language;
- ordinary Survey craft;
- Survey not completed;
- a uniquely identified native incomplete Survey text row;
- a supported reward snapshot.

`TechUnlock` calls `GetTooltipData(..., false)` and is therefore outside the mutation path.

**Preserved invariants**

- title, description, durability, body-part effects, sermon data, crafted-at rows and other native rows keep their original objects/order;
- the existing Survey separator is retained;
- no renderer/container replacement;
- no per-frame work;
- completed-state row and native post-Study alchemy widget stay vanilla-owned;
- if row identity is ambiguous or missing, leave the list unchanged.

**Acceptance evidence**

- pinned 1.407 source establishes the exact native incomplete branch and return-list lifecycle;
- shared item-tooltip research establishes the `BubbleWidgetData -> WidgetsBubbleGUI` native rendering family;
- CI compilation plus deterministic list-transformation tests/equivalent checks;
- real-runtime visual acceptance is still required for wrapping/spacing, but that is a presentation acceptance gate, not an unresolved owner/final-writer question.

## Gate C — pre-Study alchemy capability cue

**READY**

**Observable property**

Append `После изучения сгодится для алхимии.` only when the item has at least one real native decomposition type that becomes usable after the item's Survey is completed.

**Canonical owner / data path**

- `ItemDefinition.GetItemDetails()` builds native alchemy decomposition metadata from loaded `AlchemyDecompose` crafts;
- meaningful decomposition capability is `itemDetails.alchemy.decomposes.Count > 0`;
- `BaseCraftGUI` exposes an `AlchemyDecompose` craft only when `GameSave.IsSurveyComplete(..., craft.needs[0].id)` is true;
- the native completed item tooltip renders `BubbleWidgetAlchemyItemData`, whose `BubbleWidgetAlchemyItem.DrawDecomposeInfo` consumes the same `details.alchemy.decomposes` list.

**Final writer / consumer**

SRI reads only the native capability metadata and appends the sentence inside the already-identified native incomplete Survey text row. It does not create a parallel widget and does not reveal decomposition type IDs or outputs.

**Blast radius**

Only already-supported ordinary incomplete Study tooltips. No alchemy craft, visibility flag, item definition, or save state is changed.

**Preserved invariants**

- exact element/output/workstation remains undisclosed before Study;
- Study completion remains the host gate;
- completed tooltip remains native;
- Story/quest/progression outputs remain out of scope.

**Acceptance evidence**

- accepted runtime population: 62 / 223 ordinary Study definitions have native decomposition capability;
- pinned static host inspection proves the completion gate and native metadata/render consumer;
- later runtime spot check only needs to confirm the resulting line/layout, not rediscover capability ownership.

## Gate D — localization expansion

**READY for English; BLOCKED for languages other than Russian and English**

Russian and English wording is now accepted. Other supported-language wording remains unaccepted.

Current-language ownership is already accepted through `GameSettings.GetCurrentLanguage()`. Stable PrayerClarity localization code and accepted runtime language switching use the game's `en` / `ru` locale codes, with normalized regional variants handled by prefix.

Candidate 0.1.2 may therefore transform the row for Russian and English only. Every other language remains vanilla until its wording is accepted and separately gated.

## Gate E — real-runtime visual acceptance

**BLOCKED for promotion to stable; not blocking implementation**

Static evidence proves the semantic owner, returned-list insertion boundary, and native renderer family. It does not prove the final visual quality of the new multi-line Russian content in the live parchment.

Required later acceptance:
- one ordinary one-row Study preview;
- one two-row/multi-color preview if naturally accessible;
- one alchemy-capability preview if naturally accessible;
- verify wrapping, spacing, icon grouping, and that completed Study remains vanilla when an accessible completed item is available.

Do not build a new runtime probe merely for this visual property. Human observation of the real production candidate is the stronger evidence.


## Gate F — compact neutral runtime-UX iteration

**READY**

This gate was recorded after review of the first real-runtime candidate and before the follow-up production-source mutation.

**Observable property**

For Russian incomplete Study tooltips:
- use **Низкая / Средняя / Высокая / Очень высокая** for the existing color-specific magnitude bands;
- keep the first reward group on the same line as `Награда за исследование:` instead of forcing a newline;
- keep later reward groups on separate lines only when their magnitudes differ;
- remove `щедрее обычного / скупее обычного` entirely from player-facing output;
- stop reading/calculating Faith efficiency in production when it no longer affects visible behavior;
- use the neutral alchemy capability wording `Открывает алхимическое разложение.`.

**Canonical owner / data path**

No host owner changes:
- magnitude still derives from current Survey `CraftDefinition.output`;
- the same accepted color-specific thresholds apply;
- alchemy capability still derives from native `ItemDefinition.GetItemDetails().alchemy.decomposes`;
- the same existing `BubbleWidgetTextData.text` field remains the sole mutation point.

The removed relative modifier formerly read `CraftDefinition.needs` only to derive an additional display property. Because that display property is no longer part of the product, the Faith-read path should be deleted rather than retained as hidden computation.

**Final writer / consumer / commit point**

Unchanged: SRI replaces only the text of the uniquely identified native incomplete-Survey row returned from `ItemDefinition.GetTooltipData(Item,bool)`; the native tooltip then renders and sizes that row.

**Blast radius**

Formatting/model simplification only on the already-gated Russian full-detail incomplete-Study path. No new hook, widget, cache, lifecycle assumption, or host mutation is introduced. Removing the Faith-derived modifier reduces the production data surface.

**Preserved invariants**

- exact R/G/B quantities remain hidden;
- reward thresholds and underlying Survey rewards remain unchanged;
- no Faith/Science cost, reward, recipe, technology, progression, save, or RNG mutation;
- completed Study remains native;
- unsupported dynamic R/G/B output still fails safe to vanilla;
- Story and quest relevance remain out of scope;
- non-Russian tooltips remain vanilla.

**Acceptance evidence**

- real-runtime screenshots from candidate 0.1.0 showed that forced heading/reward line separation and long relative-modifier text made short rows visually unstable in the centered native tooltip;
- the accepted follow-up product decision removes the relative modifier and adopts neutral low/medium/high labels;
- build-time tests can prove exact strings, threshold preservation, magnitude-only grouping, and one-line-first-group formatting;
- no new host/runtime research is required because the owner, writer, renderer, and data paths are unchanged or reduced;
- final acceptance remains a direct human runtime visual check of the new candidate.


## Gate G — atomic reward icon + magnitude wrapping

**READY**

Candidate 0.1.1 runtime screenshots showed the point icon can remain at the end of one centered line while the magnitude moves to the next.

**Observable property**

Each complete reward group — point icon(s) plus its full magnitude label — must wrap as one visual unit. The heading may stay on the previous line. The alchemy sentence may wrap normally.

**Canonical owner / data path**

SRI owns the formatted string. Native NGUI owns final wrapping/rendering. The existing `BubbleWidgetTextData.text` row remains the only modified host field.

**Final writer / consumer / commit point**

Unchanged: SRI changes the returned incomplete-Survey row text; native `BubbleWidgetText -> UILabel/NGUIText` performs final wrapping.

**Blast radius**

Only spacing inside SRI-owned reward groups changes. No tooltip width, alignment, widget hierarchy, renderer, hook, or lifecycle behavior changes.

**Preserved invariants**

- centered native alignment stays unchanged;
- the heading and first reward group may still separate when the group does not fit;
- icons remain native inline point symbols;
- the alchemy sentence keeps ordinary wrapping;
- rewards, costs, progression, save state and RNG remain unchanged.

**Acceptance evidence**

NGUI source inspection shows ordinary ASCII spaces act as word-wrap boundaries while the bitmap-font path explicitly renders U+2009 THIN SPACE via the normal space glyph at half advance. Candidate 0.1.2 will use two thin spaces for approximately normal visual spacing inside the atomic reward group, including inside `Очень высокая` / `Very High`. Formatter tests can prove the string structure; the user's real-runtime view remains the final visual acceptance. No dedicated harness is justified.

## Gate H — English player-facing wording

**READY**

**Observable property**

For English game language, show:
- `Study reward: [icons] Low / Medium / High / Very High`;
- `Studying unlocks an alchemy use.` only when native decomposition capability is present.

For Russian, use:
- `Награда за исследование: ...`;
- `Исследование позволит использовать этот предмет в алхимии.`.

**Canonical owner / data path**

Reward and alchemy ownership is unchanged. Language selection uses accepted `GameSettings.GetCurrentLanguage()`; existing accepted project-family localization evidence uses `en` and `ru`.

**Final writer / consumer / commit point**

The same uniquely identified native incomplete-Survey text row is replaced before the native item-tooltip renderer consumes it.

**Blast radius**

The existing Russian transformation expands to English only. Other languages remain vanilla.

**Preserved invariants**

Magnitude thresholds are identical across locales; exact quantities and exact alchemy output remain hidden; no new UI objects or language polling; completed Study remains native.

**Acceptance evidence**

Russian and English wording is accepted for this candidate. Build-time tests verify both language blocks and labels. English runtime visual acceptance is still required before stable promotion.
