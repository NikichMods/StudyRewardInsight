# Study Reward Insight — Working Contract

This repository follows the canonical global development rules in `NikichMods/DevRules`.

Read before substantive technical work:
- `ENGINEERING_RULES.md`
- `CI_POLICY.md`
- `GIT_WORKFLOW.md`
- `PROJECT_BOOTSTRAP.md`
- `LICENSE_POLICY.md`
- `RUNTIME_TEST_HARNESS.md` when installed-runtime evidence is relevant

This file contains only project-specific additions and constraints.

## Project identity

- Project: **Study Reward Insight**
- Repository: `NikichMods/StudyRewardInsight`
- Target: **Graveyard Keeper 1.407**
- Runtime: **BepInEx 5**
- Planned installed DLL: `StudyRewardInsight.dll`
- Purpose: improve Study/Survey information UX by giving a qualitative preview of pending technology-point rewards without revealing exact quantities or changing Study economics or progression.

## Scope

This is a general information mod for native Study/Survey items, not a grave-decoration-only or blue-point-only feature.

The mod must not change:
- Study rewards;
- Faith/Science costs;
- technologies;
- recipes;
- progression order;
- unrelated economy.

Before Study, preserve the **semantics** of the native incomplete-Study state, but the literal native Study row may be replaced or restructured when the accepted Study Reward Insight block communicates that information more clearly. After completed Study, preserve the native completed state without a forecast.

Do not hardcode an item-type whitelist or an `item ID -> category` table unless evidence shows it is necessary and preferable.

The accepted reward scales are Russian **Низкая / Средняя / Высокая / Очень высокая** and English **Low / Medium / High / Very High**, with the same color-specific thresholds documented in `docs/PRODUCT_CONCEPT.md`. The player-facing tooltip does **not** show the former relative `щедрее обычного / скупее обычного` modifier; its underlying research remains historical evidence only. The reward icon group and its qualitative label must wrap as one visual unit. Treat future changes to those semantics as product changes, not implementation details.

## Mandatory project-specific start-of-work checks

Before substantive implementation:
1. inspect current branches, commits, PRs, source, docs, CI/build evidence and release state;
2. read this `AGENTS.md` and task-relevant project docs;
3. check `NikichMods/GraveyardKeeperResearch/docs/RESEARCH_INDEX.md` before fresh host/runtime research;
4. use `NikichMods/BetterGraveCraftingRewards` only as historical project-specific evidence where relevant;
5. distinguish accepted host facts from project hypotheses, product candidates, and mutable implementation state.

Repository and accepted evidence outrank chat memory and old handoffs.

## Shared host/runtime research

Canonical reusable Graveyard Keeper 1.407 research: `NikichMods/GraveyardKeeperResearch`.

Reusable host/runtime facts belong there once accepted. Study Reward Insight-specific UX, reward-band decisions, implementation gates, candidate state and release evidence belong in this repository.

Lookup order before a new probe:
`project docs -> shared research index/docs -> accepted history/evidence -> direct inspection -> narrow probe only if still needed`.

Do not repeat a runtime experiment when accepted evidence already proves the same owner/data path and applicability envelope.

## Product and architecture constraints

Prefer host-native-first and the narrowest verified semantic seam.

The intended compatibility envelope is:
- vanilla Graveyard Keeper;
- Better Grave Crafting Rewards;
- where practical, other mods that alter Study rewards through compatible native paths.

Therefore prefer deriving the qualitative cue from the current effective Survey output used by the host rather than duplicating vanilla reward tables.

Do not:
- show exact reward numbers or exact ranges before Study;
- use repeated point icons as a numeric scale;
- use stars or grave-quality glyphs as reward bands;
- use color intensity as the only category carrier;
- replace the whole tooltip/UI when the native item-tooltip pipeline can be safely extended;
- add broad/per-frame hooks without evidence that a narrower event-driven seam is insufficient;
- reconstruct vanilla Study wording from memory when native localization already owns it.

When several point colors are present, the design should be capable of describing each significant reward without assuming identical thresholds across colors.

Before changing a shared UI helper or formatter, inspect its callers/consumers and final owner.

## Production evidence gate

Before the first production-source mutation for each materially independent behavior change, make the DevRules gate reviewable as **READY** or **BLOCKED**, covering:
- observable property;
- canonical owner/data path;
- final writer/consumer/commit point where applicable;
- blast radius;
- preserved invariants;
- acceptance evidence.

There is no exception for a small, obvious, UI-only or follow-up change.

**BLOCKED means research/probe only.**

Keep gate granularity separate from candidate/build granularity. Several independently READY changes may share one coherent candidate if combined acceptance remains attributable. Do not bundle BLOCKED or independently unverified mechanisms to reduce test cycles.

## Runtime testing constraints

The user's normal save has no assumed dev console/cheats and cannot be expected to provide arbitrary items or instant progression.

Before creating a new probe/harness:
1. state the exact unknown fact;
2. check whether accepted evidence, direct inspection, an existing exact artifact, or a short direct runtime action can answer it;
3. if a probe is still needed, explain why it is simpler or more reliable.

A harness must not synthesize the very result it is supposed to verify. Prefer real runtime observation for visual/perceptual UX acceptance.

## Git / version / acceptance

- `main` is the stable line.
- Use `research/<topic>` for unresolved research/probes.
- Use `dev/<version>`, `feature/<topic>`, or `fix/<topic>` for production development as appropriate.
- Numbered handed artifacts are immutable.
- Research-only work does not consume a release/handoff version.
- Runtime behavior reaches `main` only after the applicable acceptance gate is satisfied.
- Stable installed filename: `StudyRewardInsight.dll`.
- GitHub Releases is the default stable binary surface once releases exist.

## Licensing

Original project software source uses MPL-2.0 under `NikichMods/DevRules/LICENSE_POLICY.md`. Game-owned assemblies, decompiled source, data, assets and localization remain research inputs and are not relicensed by this project. See `LICENSING.md`.

## Long-lived sources of truth

- `AGENTS.md`
- `docs/CHATGPT_PROJECT_INSTRUCTIONS.md`
- `docs/STUDY_RESEARCH_BASELINE.md` once established
- future test/build/release docs when needed
- `README.md`

Mutable candidate versions, SHAs, bugs, hypotheses, test artifact IDs and unaccepted reward bands belong in repository docs/issues/branches, not ChatGPT Project Instructions.
