# Quest-Relevance Hint Research

Target: **Graveyard Keeper 1.407**

Status: **product direction accepted for investigation; mechanism BLOCKED pending owner/data-path proof**

## Product need

Some Study targets have progression value that is not visible from their Study reward.

Example class:
- the player has an already-known quest/task requiring an item;
- that item is produced by an already-known craft/alchemy recipe;
- one recipe input can ultimately be obtained by decomposing an item;
- decomposition of that source item is gated by Study.

The useful information is not the exact hidden decomposition result. It is that **this unstudied item is relevant to something the player already knows they need**.

The desired UX is a wink, not a walkthrough.

## Spoiler boundary

Hard requirement:

> Never use unrevealed future quests or hidden future recipes to create a quest-relevance hint.

A hint is eligible only from information the player's current save has already exposed.

This avoids a static `item -> future quest` spoiler table.

### Safe information boundary

Candidate inputs:
1. a task currently stored as Visible in `KnownNPC.tasks`;
2. a concrete item requirement recovered from that task's authored completion path;
3. craft paths that are currently visible/unlocked under the game's own visibility/lock state;
4. an uncompleted Study item's loaded native decomposition path.

Candidate output:
- a generic signal such as `May help with a current task`;
- no NPC name;
- no quest name;
- no target recipe;
- no decomposition element;
- no exact ingredient;
- no future-task information.

## Existing host evidence

Accepted shared research proves:
- task visibility is stored through `KnownNPC.TaskState`;
- visible task state alone is not enough to infer exact actionability;
- task completion is authored in FlowCanvas through `Flow_SetTaskState`;
- Day Wheel Quest Markers research can recover task-owner completion routes and game-owned `SmartRes` price/lock requirements from those authored paths;
- direct SmartRes sufficiency should be delegated to the host rather than reimplemented.

Pinned 1.407 source also proves:
- `GameSave.IsCraftVisible(craft)` uses the host's current hidden/lock/one-time state;
- `CraftDefinition.IsLocked()` respects `needs_unlock`, `unlocked_crafts`, and `locked_crafts`.

Therefore a save-sensitive, no-future-spoiler approach is technically plausible.

## Solution families

### A. Static list of quest-important Study items

Example:
`cabbage -> quest useful`

Rejected.

Problems:
- hardcoded content table;
- future-quest spoilers;
- fragile under mods;
- cannot distinguish a task the player knows from one they have never encountered.

### B. Any item upstream of any quest recipe in the full game database

Rejected.

Even if the tooltip only says "quest relevant", the marker itself leaks future progression knowledge.

### C. Current visible tasks + all game recipes

Better, but still too permissive.

A visible task could cause a hint through a recipe the player has not learned yet, leaking the hidden route to the objective.

### D. Current visible task + current-save-known recipe graph + hidden Study decomposition endpoint

**Leading family.**

Process:
1. enumerate current Visible tasks;
2. recover the concrete item requirement from the authored completion route;
3. walk backward only through recipes the host currently considers visible/unlocked;
4. match those known recipe inputs against outputs of native `AlchemyDecompose` crafts;
5. if the source item's Study is incomplete, expose only a generic quest-relevance hint.

The only hidden edge traversed is the Study-gated decomposition itself, and the UI does not reveal what that edge produces. That is the intentional "wink".

## Why this is distinct from the alchemy hint

These are separate claims:

- `Study unlocks alchemical decomposition` = capability.
- `May help with a current task` = current-save relevance.

An item may be decomposable but irrelevant to any current task.
An item can therefore carry the first signal without the second.

Do not merge them into an overall value rating.

## Remaining unknowns

### 1. Cheapest owner for current task item requirements

Day Wheel Quest Markers proves the general FlowCanvas path is recoverable, but its production cache is specialized around weekday-NPC interaction prediction.

Study Reward Insight must not copy a large graph compiler unless necessary.

Research order:
1. inspect whether current task localization exposes a stable machine-readable item reference;
2. inspect whether a narrower native task/completion data owner exposes direct SmartRes requirements;
3. only if neither exists, consider a narrow FlowCanvas extraction for currently Visible tasks;
4. fail closed for unsupported task shapes.

### 2. Definition of "known recipe"

Candidate canonical check:
`MainGame.me.save.IsCraftVisible(craft)`

Need verify this is sufficient for all relevant alchemy recipe families, especially MixedCraft recipes and recipe-discovery state.

### 3. Backward-graph depth

Prefer the minimum useful depth.

Initial candidate:
- current quest-required item;
- already-known recipe producing it;
- one upstream alchemy product;
- one Study-gated decomposition source.

Do not recursively traverse arbitrary production chains until evidence shows that users need it.

## Evidence gate

**State: BLOCKED**

Observable property:
- an unstudied item receives a generic current-task relevance cue only when its hidden decomposition output feeds an already-known production route to an already-visible task requirement.

Canonical owner/data path:
- task owner: not yet narrow enough;
- recipe visibility: host `GameSave.IsCraftVisible` candidate;
- decomposition source: loaded `AlchemyDecompose` definitions + incomplete Survey state.

Preserved invariants:
- no future quest disclosure;
- no hidden recipe disclosure;
- no exact decomposition result before Study;
- no quest/progression mutation;
- no hardcoded item/quest whitelist.

Acceptance evidence required:
- exact task-requirement owner;
- exact known-recipe visibility rule;
- at least one real positive path (Merchant/Spices class is a useful representative);
- at least one negative path proving future/unknown routes remain unmarked.
