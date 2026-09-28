# Study Reward Insight

Information-only mod for **Graveyard Keeper 1.407** using **BepInEx 5**.

Study Reward Insight makes pending Study rewards easier to evaluate without revealing exact technology-point quantities or changing Study economics, rewards, technologies, recipes, or progression.

## Current stable behavior

Version **0.1.2** adds a qualitative preview to unfinished Study tooltips in **Russian and English**:

- technology-point reward magnitude: Low / Medium / High / Very High (localized in Russian);
- multiple point colors are grouped when they share the same magnitude;
- point icon(s) and their magnitude label wrap as one visual unit;
- items whose Study unlocks a native alchemical use say so without revealing the exact decomposition result;
- completed Study tooltips remain native.

Other game languages currently keep vanilla Study tooltip behavior.

Installed DLL: `StudyRewardInsight.dll`.

## Scope

The mod is informational only. It does not modify:
- Study rewards;
- Faith or Science costs;
- technologies;
- recipes;
- progression;
- unrelated economy.

Exact technology-point quantities, exact alchemy decomposition results, Story prediction, and quest relevance remain intentionally hidden/out of scope.

## Development

The next planned iteration expands the same accepted presentation model to the remaining Graveyard Keeper languages.

Source is licensed under MPL-2.0. See `LICENSE` and `LICENSING.md`.
