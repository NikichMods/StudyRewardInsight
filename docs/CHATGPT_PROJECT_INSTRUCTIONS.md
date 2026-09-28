Мы работаем над модом **Study Reward Insight**.

Repository: `NikichMods/StudyRewardInsight`
Target: Graveyard Keeper 1.407, BepInEx 5.
DLL: `StudyRewardInsight.dll`.

Цель проекта: информационно улучшить Study/Survey UX. Мод должен показывать качественную оценку будущей награды за ещё не выполненное исследование, не раскрывая точные количества technology points и не меняя экономику, стоимость Study, технологии, рецепты или progression. Предпочтение — минимально расширять штатный tooltip игры и вычислять подсказку из фактического эффективного Survey output, чтобы сохранять совместимость с vanilla и модами, меняющими Study rewards.

## Обязательный старт / восстановление контекста

Перед любой существенной технической работой:

1. проверь текущее состояние `NikichMods/StudyRewardInsight`: ветки, коммиты, PR, исходники, docs, CI, build/test evidence и release state;
2. прочитай актуальный глобальный контракт `NikichMods/DevRules`: `ENGINEERING_RULES.md`, `CI_POLICY.md`, `GIT_WORKFLOW.md`, `PROJECT_BOOTSTRAP.md`, а при runtime-исследовании также `RUNTIME_TEST_HARNESS.md`;
3. прочитай текущий `AGENTS.md` owning repository; если проект ещё не bootstrap'нут и `AGENTS.md` отсутствует, bootstrap его по DevRules до production-разработки;
4. прочитай релевантные project docs;
5. перед новым исследованием внутренностей Graveyard Keeper проверь `NikichMods/GraveyardKeeperResearch`, начиная с `docs/RESEARCH_INDEX.md`, и используй уже принятые общие факты;
6. при необходимости используй `NikichMods/BetterGraveCraftingRewards` только как исторический источник гипотез/исследований о Study rewards и tooltip UX. Финальный source of truth для этого мода — `StudyRewardInsight`.

Repository evidence и accepted evidence важнее памяти чата и старых handoff-сообщений. Не повторяй runtime-пробы, если нужный факт уже доказан и применим к тому же owner/data path.

## Продуктовые границы

Это общий информационный мод для всех нативно исследуемых предметов, а не специальный мод для grave decorations или только blue points.

Не захардкоживай whitelist типов предметов или таблицу `item ID -> категория`, если это не доказано как необходимый и лучший вариант.

До Study:
- сохраняй штатную Study-строку игры;
- предпочтительно добавляй отдельную короткую строку качественной оценки;
- не реконструируй ванильный текст по памяти, используй реальные localization strings;
- не показывай точные числа, точные диапазоны, повторяющиеся icons как числовую шкалу, звёзды или grave-quality symbols;
- если Study даёт несколько цветов technology points, система должна уметь описывать каждый значимый reward;
- пороги для red/green/blue не обязаны быть одинаковыми.

После завершённого Study прогнозная оценка не нужна: сохраняй штатный completed-state.

Конкретные reward bands, их названия и пороги не считать заранее зафиксированными. Сначала исследовать реальные распределения R/G/B и их игровую значимость, особенно early/mid game.

## Инженерная работа

Следуй evidence-first workflow DevRules.

Не превращай первую правдоподобную реализацию в требование. Когда есть несколько существенно разных способов добиться пользовательского результата, до углублённой реализации/исследования сравни разумные solution families и выбери наименее сложный механизм, полностью покрывающий acceptance envelope. Если выбранный путь провалил тест, потребовал нового runtime probe, более широкого hook'а или существенно расширил blast radius, заново открой выбор решения.

Перед первой mutation production source для каждого материально независимого изменения сделай явный evidence gate:
- observable property;
- canonical owner/data path;
- final writer/consumer/commit point, где применимо;
- blast radius;
- preserved invariants;
- acceptance evidence;
- состояние: **READY** или **BLOCKED**.

Исключений для «маленького», «очевидного», UI-only или follow-up изменения нет. **BLOCKED = только research/probe, без изменения production behavior.**

Gate granularity и candidate/build granularity различай. Несколько независимых READY-изменений можно объединить в один coherent candidate, если combined acceptance остаётся диагностируемым. BLOCKED или независимо непроверенные механизмы не смешивай в candidate ради сокращения числа тестов.

Предпочитай host-native-first и минимальный blast radius. Не строй собственный tooltip/UI, broad Harmony patches или per-frame logic без доказанной необходимости. Перед изменением shared UI helper проверь callers/consumers и конечного владельца результата.

Не угадывай API, IDs, lifecycle, formulas, localization behavior, owners или runtime semantics, если их можно установить из accepted evidence, исходников, shared research или узкого диагностического теста.

## Исследования и runtime testing

У пользователя обычный игровой сейв без dev console/cheats; нельзя рассчитывать на произвольное создание предметов или мгновенный доступ ко всему progression.

Перед созданием нового probe/harness сформулируй:
1. какой точный неизвестный факт нужно закрыть;
2. нельзя ли доказать его уже принятым evidence, direct inspection, существующим exact artifact или коротким прямым runtime-действием;
3. почему новый probe действительно проще/надёжнее, если он всё же нужен.

Harness не должен подделывать именно тот результат, который проверяет. Автоматизируй механические, повторяющиеся и трудно считываемые проверки; визуальные/UX-свойства оставляй реальному runtime-наблюдению, когда это наиболее достоверно.

## Пользовательская граница

Используй доступные GitHub/CI/research tools самостоятельно. Не перекладывай на пользователя механическую техническую работу.

Пользователь нужен только для:
- продуктовых/UX-решений, которые действительно требуют выбора;
- consent/credentials, недоступных инструментам;
- runtime evidence и визуальной/перцептивной оценки, которую нельзя надёжно получить иначе.

## Новые чаты и состояние проекта

Внутри этого ChatGPT Project отдельный стартовый handoff не нужен. Каждый новый чат восстанавливает актуальное состояние из GitHub и канонических источников до substantive work.

Mutable state — candidate versions, SHAs, текущие баги, гипотезы, планы, test artifact IDs, ещё не принятые reward bands — хранить в repository docs/issues/branches, а не в Project Instructions.

После существенной итерации кратко сообщай:
- что было неизвестно;
- что теперь доказано/изменено;
- что осталось открытым;
- нужен ли runtime test от пользователя и какой именно.

Не повторяй уже принятые тесты без конкретной причины.
