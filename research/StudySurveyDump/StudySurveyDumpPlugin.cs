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
    public sealed class StudySurveyDumpPlugin : BaseUnityPlugin
    {
        public const string Guid = "nikich.graveyardkeeper.studyrewardinsight.studysurveydump";
        public const string Name = "Study Reward Insight Survey Dump";
        public const string Version = "0.1.0";
        static readonly BindingFlags Inst = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        static readonly BindingFlags Stat = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        Assembly gameAssembly;
        bool dumped;

        void Awake()
        {
            Logger.LogInfo("SRI Survey Dump 0.1.0 loaded. Read-only research probe.");
            StartCoroutine(DumpWhenReady());
        }

        IEnumerator DumpWhenReady()
        {
            while (!dumped)
            {
                if (BindGame())
                {
                    Type mainGame = GameType("MainGame");
                    Type balanceType = GameType("GameBalance");
                    if (Bool(GetStatic(mainGame, "game_started")))
                    {
                        object balance = GetStatic(balanceType, "me");
                        if (balance != null)
                        {
                            try { Dump(balance); }
                            catch (Exception ex) { Logger.LogError("SRI_SURVEY_ERROR|" + Esc(ex.ToString())); }
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
            IList items = Get(balance, "items_data") as IList;
            IList crafts = Get(balance, "craft_data") as IList;
            if (items == null || crafts == null) throw new InvalidOperationException("GameBalance items_data/craft_data not available.");

            Logger.LogInfo("SRI_SURVEY_BEGIN|probe=0.1.0|target=GraveyardKeeper-1.407|source=loaded-GameBalance");
            Logger.LogInfo("SRI_SURVEY_SOURCE|decompile_reference=Kupie/GYK_DECOMP@6abf79199d92482af1c7573870dd9a20ec2270b9");

            var mapped = new Dictionary<string,List<object>>(StringComparer.Ordinal);
            int itemMappings = 0;
            foreach (object def in items)
            {
                if (def == null) continue;
                object survey = Call0(def, "GetSurveyCraft");
                if (survey == null) continue;
                string sid = Id(survey);
                if (sid.Length == 0) continue;
                List<object> owners;
                if (!mapped.TryGetValue(sid, out owners)) mapped[sid] = owners = new List<object>();
                owners.Add(def);
                itemMappings++;

                Logger.LogInfo("SRI_ITEM_SURVEY" +
                    "|item=" + Esc(Id(def)) +
                    "|name=" + Esc(DisplayName(def)) +
                    "|type=" + Esc(Str(Get(def,"type"))) +
                    "|quality=" + Fmt(Num(Get(def,"quality"))) +
                    "|quality_type=" + Esc(Str(Get(def,"quality_type"))) +
                    "|product_tier=" + Fmt(Num(Get(def,"product_tier"))) +
                    "|alchemy_type=" + Esc(Str(Get(def,"alch_type"))) +
                    "|not_used=" + B(Get(def,"not_used")) +
                    "|survey=" + Esc(sid) +
                    "|survey_resolution=" + Resolution(def,sid));
            }

            int surveyCount=0, orphan=0, scienceDecompose=0, dynamic=0, pr=0, pg=0, pb=0, noRgb=0;
            var combos=new Dictionary<string,int>(StringComparer.Ordinal);
            var rh=new Dictionary<double,int>(); var gh=new Dictionary<double,int>(); var bh=new Dictionary<double,int>();

            foreach (object craft in crafts)
            {
                if (craft == null || Str(Get(craft,"craft_type")) != "Survey") continue;
                surveyCount++;
                string cid=Id(craft);
                IList output=Get(craft,"output") as IList, needs=Get(craft,"needs") as IList, outWgo=Get(craft,"output_to_wgo") as IList;
                double r=Reward(output,"r"), g=Reward(output,"g"), b=Reward(output,"b");
                if(r>0){pr++;Hist(rh,r);} if(g>0){pg++;Hist(gh,g);} if(b>0){pb++;Hist(bh,b);}
                string combo=Combo(r,g,b); if(combo=="none")noRgb++; Count(combos,combo);
                bool dyn=DynamicPoints(output); if(dyn)dynamic++;
                string sub=Str(Get(craft,"sub_type")); if(sub=="SurveySciencePoints")scienceDecompose++;
                List<object> owners; bool hasOwners=mapped.TryGetValue(cid,out owners)&&owners.Count>0; if(!hasOwners)orphan++;

                Logger.LogInfo("SRI_SURVEY" +
                    "|craft=" + Esc(cid) +
                    "|sub_type=" + Esc(sub) +
                    "|mapped_items=" + Esc(hasOwners?string.Join(";",owners.Select(Id).ToArray()):"") +
                    "|mapped_item_count=" + (hasOwners?owners.Count:0).ToString(CultureInfo.InvariantCulture) +
                    "|mapped_types=" + Esc(hasOwners?Distinct(owners,"type"):"") +
                    "|faith=" + Fmt(Reward(needs,"faith")) +
                    "|science=" + Fmt(Reward(needs,"science")) +
                    "|red=" + Fmt(r) + "|green=" + Fmt(g) + "|blue=" + Fmt(b) +
                    "|colors=" + combo +
                    "|dynamic_points=" + (dyn?"true":"false") +
                    "|point_details=" + Esc(PointDetails(output)) +
                    "|needs=" + Esc(ItemList(needs)) +
                    "|outputs=" + Esc(ItemList(output)) +
                    "|output_to_wgo=" + Esc(ItemList(outWgo)) +
                    "|craft_in=" + Esc(StringList(Get(craft,"craft_in") as IEnumerable)) +
                    "|energy_expr=" + Esc(Expr(Get(craft,"energy"))) +
                    "|time_expr=" + Esc(Expr(Get(craft,"craft_time"))) +
                    "|hidden=" + B(Get(craft,"hidden")) +
                    "|one_time=" + B(Get(craft,"one_time_craft")) +
                    "|is_auto=" + B(Get(craft,"is_auto")) +
                    "|needs_unlock=" + B(Get(craft,"needs_unlock")) +
                    "|can_craft_always=" + B(Get(craft,"can_craft_always")));
            }

            foreach(var p in mapped.Where(x=>x.Value.Count>1))
                Logger.LogInfo("SRI_SURVEY_SHARED|craft="+Esc(p.Key)+"|items="+Esc(string.Join(";",p.Value.Select(Id).ToArray()))+"|count="+p.Value.Count.ToString(CultureInfo.InvariantCulture));

            Logger.LogInfo("SRI_SURVEY_HIST|red="+Esc(HistString(rh))+"|green="+Esc(HistString(gh))+"|blue="+Esc(HistString(bh))+"|combos="+Esc(CountString(combos)));
            Logger.LogInfo("SRI_SURVEY_DONE" +
                "|item_mappings="+itemMappings.ToString(CultureInfo.InvariantCulture) +
                "|unique_survey_crafts="+surveyCount.ToString(CultureInfo.InvariantCulture) +
                "|orphan_survey_crafts="+orphan.ToString(CultureInfo.InvariantCulture) +
                "|science_decompose="+scienceDecompose.ToString(CultureInfo.InvariantCulture) +
                "|dynamic_point_surveys="+dynamic.ToString(CultureInfo.InvariantCulture) +
                "|positive_red="+pr.ToString(CultureInfo.InvariantCulture) +
                "|positive_green="+pg.ToString(CultureInfo.InvariantCulture) +
                "|positive_blue="+pb.ToString(CultureInfo.InvariantCulture) +
                "|no_rgb="+noRgb.ToString(CultureInfo.InvariantCulture));
        }

        static string Resolution(object def,string sid)
        {
            string id=Id(def), baseId=id; int i=id.LastIndexOf(':'); if(i>=0)baseId=id.Substring(0,i);
            if(sid=="surv:"+baseId)return "base"; if(sid=="surv:"+id)return "exact"; return "other";
        }
        static string Distinct(IEnumerable<object> xs,string member){var s=new SortedSet<string>(StringComparer.Ordinal);foreach(var x in xs){string v=Str(Get(x,member));if(v.Length>0)s.Add(v);}return string.Join(";",s.ToArray());}
        static string Combo(double r,double g,double b){string s="";if(r>0)s+="R";if(g>0)s+="G";if(b>0)s+="B";return s.Length==0?"none":s;}
        static void Count(Dictionary<string,int>d,string k){int n;d.TryGetValue(k,out n);d[k]=n+1;}
        static void Hist(Dictionary<double,int>d,double k){int n;d.TryGetValue(k,out n);d[k]=n+1;}
        static string HistString(Dictionary<double,int>d){return string.Join(",",d.OrderBy(x=>x.Key).Select(x=>Fmt(x.Key)+":"+x.Value.ToString(CultureInfo.InvariantCulture)).ToArray());}
        static string CountString(Dictionary<string,int>d){return string.Join(",",d.OrderBy(x=>x.Key,StringComparer.Ordinal).Select(x=>x.Key+":"+x.Value.ToString(CultureInfo.InvariantCulture)).ToArray());}

        static bool DynamicPoints(IList xs)
        {
            if(xs==null)return false;
            foreach(object x in xs){string id=Id(x);if(id!="r"&&id!="g"&&id!="b")continue;
                if(Int(Get(x,"chance_group"),-1)!=-1||Expr(Get(x,"self_chance")).Length>0||Expr(Get(x,"common_chance")).Length>0||Expr(Get(x,"min_value")).Length>0||Expr(Get(x,"max_value")).Length>0)return true;}
            return false;
        }
        static string PointDetails(IList xs)
        {
            if(xs==null)return "";var a=new List<string>();
            foreach(object x in xs){string id=Id(x);if(id!="r"&&id!="g"&&id!="b")continue;
                a.Add(id+":value="+Fmt(Num(Get(x,"value")))+",chance_group="+Int(Get(x,"chance_group"),-1).ToString(CultureInfo.InvariantCulture)+",self="+Expr(Get(x,"self_chance"))+",common="+Expr(Get(x,"common_chance"))+",min="+Expr(Get(x,"min_value"))+",max="+Expr(Get(x,"max_value")));}
            return string.Join(";",a.ToArray());
        }

        bool BindGame(){if(gameAssembly!=null)return true;gameAssembly=AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a=>a.GetName().Name=="Assembly-CSharp");return gameAssembly!=null;}
        Type GameType(string n){if(gameAssembly==null)return null;Type t=gameAssembly.GetType(n,false);if(t!=null)return t;try{return gameAssembly.GetTypes().FirstOrDefault(x=>x!=null&&x.Name==n);}catch(ReflectionTypeLoadException e){return e.Types.FirstOrDefault(x=>x!=null&&x.Name==n);}}
        static object Get(object o,string n){if(o==null)return null;for(Type t=o.GetType();t!=null;t=t.BaseType){FieldInfo f=t.GetField(n,Inst);if(f!=null)return f.GetValue(o);PropertyInfo p=t.GetProperty(n,Inst);if(p!=null&&p.CanRead)return p.GetValue(o,null);}return null;}
        static object GetStatic(Type t,string n){if(t==null)return null;for(;t!=null;t=t.BaseType){FieldInfo f=t.GetField(n,Stat);if(f!=null)return f.GetValue(null);PropertyInfo p=t.GetProperty(n,Stat);if(p!=null&&p.CanRead)return p.GetValue(null,null);}return null;}
        static object Call0(object o,string n){if(o==null)return null;MethodInfo m=o.GetType().GetMethods(Inst).FirstOrDefault(x=>x.Name==n&&x.GetParameters().Length==0);return m==null?null:m.Invoke(o,null);}
        static string Id(object o){return Get(o,"id") as string??"";}
        static string DisplayName(object d){try{MethodInfo m=d.GetType().GetMethods(Inst).FirstOrDefault(x=>x.Name=="GetItemName"&&x.GetParameters().Length==1&&x.GetParameters()[0].ParameterType==typeof(bool));return m==null?"":Str(m.Invoke(d,new object[]{true}));}catch{return "";}}
        static double Reward(IList xs,string id){if(xs==null)return 0;double n=0;foreach(object x in xs)if(Id(x)==id)n+=Num(Get(x,"value"));return n;}
        static string ItemList(IList xs){if(xs==null)return "";var a=new List<string>();foreach(object x in xs)if(x!=null)a.Add(Id(x)+":"+Fmt(Num(Get(x,"value"))));return string.Join(";",a.ToArray());}
        static string StringList(IEnumerable xs){if(xs==null)return "";var a=new List<string>();foreach(object x in xs)a.Add(Str(x));return string.Join(";",a.ToArray());}
        static string Expr(object o){if(o==null)return "";try{MethodInfo m=o.GetType().GetMethods(Inst).FirstOrDefault(x=>x.Name=="GetRawExpressionString"&&x.GetParameters().Length==0);return m==null?"":Str(m.Invoke(o,null));}catch{return "";}}
        static string Str(object o){return Convert.ToString(o,CultureInfo.InvariantCulture)??"";}
        static int Int(object o,int fallback){try{return o==null?fallback:Convert.ToInt32(o,CultureInfo.InvariantCulture);}catch{return fallback;}}
        static double Num(object o){try{return o==null?0:Convert.ToDouble(o,CultureInfo.InvariantCulture);}catch{return 0;}}
        static bool Bool(object o){try{return o!=null&&Convert.ToBoolean(o,CultureInfo.InvariantCulture);}catch{return false;}}
        static string B(object o){return Bool(o)?"true":"false";}
        static string Fmt(double n){return n.ToString("0.###",CultureInfo.InvariantCulture);}
        static string Esc(string s){return string.IsNullOrEmpty(s)?"":s.Replace("\\","\\\\").Replace("|","\\|").Replace("\r"," ").Replace("\n"," ");}
    }
}