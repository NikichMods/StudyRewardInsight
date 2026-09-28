// SPDX-License-Identifier: MPL-2.0
using BepInEx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace NikichMods.StudyRewardInsight.Research
{
    [BepInPlugin(Guid, Name, Version)]
    public sealed class StudyValueDumpPlugin : BaseUnityPlugin
    {
        public const string Guid = "nikich.graveyardkeeper.studyrewardinsight.studyvaluedump";
        public const string Name = "Study Reward Insight Value Dump";
        public const string Version = "0.2.0";

        static readonly BindingFlags Inst = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        static readonly BindingFlags Stat = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        Assembly gameAssembly;
        bool dumped;

        void Awake()
        {
            Logger.LogInfo("SRI Value Dump 0.2.0 loaded. Read-only research probe.");
            StartCoroutine(DumpWhenReady());
        }

        IEnumerator DumpWhenReady()
        {
            while (!dumped)
            {
                if (BindGame())
                {
                    var mainGame = GameType("MainGame");
                    var balanceType = GameType("GameBalance");
                    if (Bool(GetStatic(mainGame, "game_started")))
                    {
                        var balance = GetStatic(balanceType, "me");
                        if (balance != null)
                        {
                            try { Dump(balance); }
                            catch (Exception ex) { Logger.LogError("SRI_VALUE_ERROR|" + Esc(ex.ToString())); }
                            dumped = true;
                            yield break;
                        }
                    }
                }
                yield return null;
            }
        }

        void Dump(object balance)
        {
            var items = Get(balance, "items_data") as IList;
            var crafts = Get(balance, "craft_data") as IList;
            if (items == null || crafts == null)
                throw new InvalidOperationException("GameBalance items_data/craft_data not available.");

            Logger.LogInfo("SRI_VALUE_BEGIN|probe=0.2.0|target=GraveyardKeeper-1.407|source=loaded-GameBalance");
            Logger.LogInfo("SRI_VALUE_SOURCE|decompile_reference=Kupie/GYK_DECOMP@6abf79199d92482af1c7573870dd9a20ec2270b9");

            var owners = new Dictionary<string, List<object>>(StringComparer.Ordinal);
            foreach (object def in items)
            {
                if (def == null) continue;
                var survey = Call0(def, "GetSurveyCraft");
                if (survey == null) continue;
                if (Str(Get(survey, "craft_type")) != "Survey") continue;
                if (Str(Get(survey, "sub_type")) == "SurveySciencePoints") continue;

                var sid = Id(survey);
                if (sid.Length == 0) continue;
                List<object> list;
                if (!owners.TryGetValue(sid, out list))
                    owners[sid] = list = new List<object>();
                list.Add(def);
            }

            int ordinary = 0, decomposable = 0, storyBearing = 0, specialNonTech = 0;
            var techIds = new HashSet<string>(new[] { "r", "g", "b", "v", "gratitude_points" }, StringComparer.Ordinal);

            foreach (object craft in crafts)
            {
                if (craft == null || Str(Get(craft, "craft_type")) != "Survey") continue;
                if (Str(Get(craft, "sub_type")) == "SurveySciencePoints") continue;

                ordinary++;
                var cid = Id(craft);
                List<object> mapped;
                owners.TryGetValue(cid, out mapped);
                mapped = mapped ?? new List<object>();

                var output = Get(craft, "output") as IList;
                var needs = Get(craft, "needs") as IList;
                var needsFromWgo = Get(craft, "needs_from_wgo") as IList;
                var outputToWgo = Get(craft, "output_to_wgo") as IList;

                var decompTypes = new SortedSet<string>(StringComparer.Ordinal);
                foreach (var def in mapped)
                    ReadAlchemyTypes(def, decompTypes);

                var decompCrafts = FindDecomposeCrafts(crafts, mapped);
                if (decompTypes.Count > 0 || decompCrafts.Count > 0) decomposable++;

                bool hasStory = false;
                bool hasSpecial = false;
                if (output != null)
                {
                    foreach (object item in output)
                    {
                        var id = Id(item);
                        if (id.StartsWith("story:", StringComparison.Ordinal)) hasStory = true;
                        else if (!techIds.Contains(id) && id.Length > 0) hasSpecial = true;
                    }
                }
                if (hasStory) storyBearing++;
                if (hasSpecial) specialNonTech++;

                Logger.LogInfo(
                    "SRI_VALUE_SURVEY" +
                    "|craft=" + Esc(cid) +
                    "|items=" + Esc(string.Join(";", mapped.Select(Id).ToArray())) +
                    "|names=" + Esc(string.Join(";", mapped.Select(DisplayName).Distinct().ToArray())) +
                    "|faith=" + Fmt(Reward(needs, "faith")) +
                    "|science=" + Fmt(Reward(needsFromWgo, "science")) +
                    "|needs=" + Esc(ItemList(needs)) +
                    "|needs_from_wgo=" + Esc(ItemList(needsFromWgo)) +
                    "|outputs=" + Esc(FullItemDetails(output)) +
                    "|output_to_wgo=" + Esc(FullItemDetails(outputToWgo)) +
                    "|alchemy_decomposable=" + ((decompTypes.Count > 0 || decompCrafts.Count > 0) ? "true" : "false") +
                    "|alchemy_types=" + Esc(string.Join(";", decompTypes.ToArray())) +
                    "|alchemy_crafts=" + Esc(string.Join(";", decompCrafts.ToArray())) +
                    "|end_event=" + Esc(Str(Get(craft, "end_event"))) +
                    "|end_script=" + Esc(Str(Get(craft, "end_script"))) +
                    "|craft_after_finish=" + Esc(Str(Get(craft, "craft_after_finish"))) +
                    "|ach_key=" + Esc(Str(Get(craft, "ach_key"))) +
                    "|flag=" + Int(Get(craft, "flag"), 0).ToString(CultureInfo.InvariantCulture));
            }

            Logger.LogInfo(
                "SRI_VALUE_DONE" +
                "|ordinary_surveys=" + ordinary.ToString(CultureInfo.InvariantCulture) +
                "|decomposable_surveys=" + decomposable.ToString(CultureInfo.InvariantCulture) +
                "|story_output_surveys=" + storyBearing.ToString(CultureInfo.InvariantCulture) +
                "|special_nontech_surveys=" + specialNonTech.ToString(CultureInfo.InvariantCulture));
        }

        void ReadAlchemyTypes(object def, SortedSet<string> result)
        {
            try
            {
                var details = Call0(def, "GetItemDetails");
                var alchemy = Get(details, "alchemy");
                if (alchemy == null) return;

                var decomposes = Get(alchemy, "decomposes") as IEnumerable;
                if (decomposes == null) return;
                foreach (var x in decomposes)
                    result.Add(Str(x));
            }
            catch (Exception ex)
            {
                Logger.LogWarning("SRI_VALUE_ALCHEMY_META_FAIL|item=" + Esc(Id(def)) + "|error=" + Esc(ex.GetType().Name));
            }
        }

        List<string> FindDecomposeCrafts(IList crafts, List<object> mapped)
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var def in mapped)
            {
                var id = Id(def);
                if (id.Length == 0) continue;
                ids.Add(id);
                ids.Add(BaseId(id));
            }

            var result = new List<string>();
            foreach (object craft in crafts)
            {
                if (craft == null || Str(Get(craft, "craft_type")) != "AlchemyDecompose") continue;
                var needs = Get(craft, "needs") as IList;
                if (needs == null || needs.Count == 0) continue;
                var needId = Id(needs[0]);
                if (!ids.Contains(needId)) continue;

                result.Add(Id(craft) + "=>" + ShortOutputs(Get(craft, "output") as IList));
            }
            result.Sort(StringComparer.Ordinal);
            return result;
        }

        static string FullItemDetails(IList list)
        {
            if (list == null) return "";
            var parts = new List<string>();
            foreach (object item in list)
            {
                if (item == null) continue;
                parts.Add(
                    Id(item) +
                    ":value=" + Fmt(Num(Get(item, "value"))) +
                    ",chance_group=" + Int(Get(item, "chance_group"), -1).ToString(CultureInfo.InvariantCulture) +
                    ",self=" + Expr(Get(item, "self_chance")) +
                    ",common=" + Expr(Get(item, "common_chance")) +
                    ",min=" + Expr(Get(item, "min_value")) +
                    ",max=" + Expr(Get(item, "max_value")));
            }
            return string.Join(";", parts.ToArray());
        }

        static string ShortOutputs(IList list)
        {
            if (list == null) return "";
            var parts = new List<string>();
            foreach (object item in list)
            {
                if (item == null) continue;
                parts.Add(Id(item) + ":" + Fmt(Num(Get(item, "value"))));
            }
            return string.Join(",", parts.ToArray());
        }

        static string ItemList(IList list)
        {
            if (list == null) return "";
            var parts = new List<string>();
            foreach (object item in list)
            {
                if (item == null) continue;
                parts.Add(Id(item) + ":" + Fmt(Num(Get(item, "value"))));
            }
            return string.Join(";", parts.ToArray());
        }

        static string BaseId(string id)
        {
            int i = id.LastIndexOf(':');
            return i >= 0 ? id.Substring(0, i) : id;
        }

        bool BindGame()
        {
            if (gameAssembly != null) return true;
            gameAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "Assembly-CSharp");
            return gameAssembly != null;
        }

        Type GameType(string name)
        {
            if (gameAssembly == null) return null;
            var direct = gameAssembly.GetType(name, false);
            if (direct != null) return direct;
            try { return gameAssembly.GetTypes().FirstOrDefault(t => t != null && t.Name == name); }
            catch (ReflectionTypeLoadException ex) { return ex.Types.FirstOrDefault(t => t != null && t.Name == name); }
        }

        static object Get(object obj, string name)
        {
            if (obj == null) return null;
            for (var t = obj.GetType(); t != null; t = t.BaseType)
            {
                var f = t.GetField(name, Inst);
                if (f != null) return f.GetValue(obj);
                var p = t.GetProperty(name, Inst);
                if (p != null && p.CanRead) return p.GetValue(obj, null);
            }
            return null;
        }

        static object GetStatic(Type type, string name)
        {
            if (type == null) return null;
            for (var t = type; t != null; t = t.BaseType)
            {
                var f = t.GetField(name, Stat);
                if (f != null) return f.GetValue(null);
                var p = t.GetProperty(name, Stat);
                if (p != null && p.CanRead) return p.GetValue(null, null);
            }
            return null;
        }

        static object Call0(object obj, string name)
        {
            if (obj == null) return null;
            var m = obj.GetType().GetMethods(Inst)
                .FirstOrDefault(x => x.Name == name && x.GetParameters().Length == 0);
            return m == null ? null : m.Invoke(obj, null);
        }

        static string Id(object obj) { return Get(obj, "id") as string ?? ""; }

        static string DisplayName(object def)
        {
            try
            {
                var m = def.GetType().GetMethods(Inst)
                    .FirstOrDefault(x => x.Name == "GetItemName" &&
                                         x.GetParameters().Length == 1 &&
                                         x.GetParameters()[0].ParameterType == typeof(bool));
                return m == null ? Id(def) : Str(m.Invoke(def, new object[] { true }));
            }
            catch { return Id(def); }
        }

        static double Reward(IList list, string id)
        {
            if (list == null) return 0d;
            double sum = 0d;
            foreach (object item in list)
                if (Id(item) == id) sum += Num(Get(item, "value"));
            return sum;
        }

        static string Expr(object obj)
        {
            if (obj == null) return "";
            try
            {
                var m = obj.GetType().GetMethods(Inst)
                    .FirstOrDefault(x => x.Name == "GetRawExpressionString" && x.GetParameters().Length == 0);
                return m == null ? "" : Str(m.Invoke(obj, null));
            }
            catch { return ""; }
        }

        static string Str(object obj) { return Convert.ToString(obj, CultureInfo.InvariantCulture) ?? ""; }
        static int Int(object obj, int fallback) { try { return obj == null ? fallback : Convert.ToInt32(obj, CultureInfo.InvariantCulture); } catch { return fallback; } }
        static double Num(object obj) { try { return obj == null ? 0d : Convert.ToDouble(obj, CultureInfo.InvariantCulture); } catch { return 0d; } }
        static bool Bool(object obj) { try { return obj != null && Convert.ToBoolean(obj, CultureInfo.InvariantCulture); } catch { return false; } }
        static string Fmt(double value) { return value.ToString("0.###", CultureInfo.InvariantCulture); }
        static string Esc(string s) { return string.IsNullOrEmpty(s) ? "" : s.Replace("\\", "\\\\").Replace("|", "\\|").Replace("\r", " ").Replace("\n", " "); }
    }
}
