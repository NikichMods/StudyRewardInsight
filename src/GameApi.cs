// SPDX-License-Identifier: MPL-2.0

using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace StudyRewardInsight
{
    internal static class GameApi
    {
        internal const BindingFlags Inst =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        internal const BindingFlags Stat =
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        internal static Type ItemDefinitionType { get; private set; }
        internal static Type MainGameType { get; private set; }
        internal static Type GameSettingsType { get; private set; }

        internal static MethodInfo GetTooltipDataMethod { get; private set; }

        private static Assembly _gameAssembly;
        private static MethodInfo _getSurveyCraft;
        private static MethodInfo _getItemDetails;
        private static MethodInfo _getCurrentLanguage;
        private static MethodInfo _localize;

        internal static void Bind()
        {
            _gameAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a =>
                    string.Equals(
                        a.GetName().Name,
                        "Assembly-CSharp",
                        StringComparison.Ordinal));

            if (_gameAssembly == null)
                throw new InvalidOperationException("Assembly-CSharp was not found.");

            ItemDefinitionType = RequireType("ItemDefinition");
            MainGameType = RequireType("MainGame");
            GameSettingsType = RequireType("GameSettings");

            GetTooltipDataMethod = RequireMethod(
                ItemDefinitionType,
                "GetTooltipData",
                2,
                false);

            _getSurveyCraft = RequireMethod(
                ItemDefinitionType,
                "GetSurveyCraft",
                0,
                false);

            _getItemDetails = RequireMethod(
                ItemDefinitionType,
                "GetItemDetails",
                0,
                false);

            _getCurrentLanguage = RequireMethod(
                GameSettingsType,
                "GetCurrentLanguage",
                0,
                true);

            Type gjl = RequireTypeAcrossLoadedAssemblies("GJL");
            _localize = gjl.GetMethods(Stat)
                .FirstOrDefault(m =>
                {
                    if (m.Name != "L")
                        return false;

                    ParameterInfo[] parameters = m.GetParameters();
                    return parameters.Length == 1
                        && parameters[0].ParameterType == typeof(string)
                        && m.ReturnType == typeof(string);
                });

            if (_localize == null)
                throw new MissingMethodException("GJL", "L(string)");
        }

        internal static object GetSurveyCraft(object itemDefinition)
        {
            return itemDefinition == null
                ? null
                : _getSurveyCraft.Invoke(itemDefinition, null);
        }

        internal static string CurrentLanguage()
        {
            return _getCurrentLanguage.Invoke(null, null) as string
                ?? string.Empty;
        }

        internal static string Localize(string key)
        {
            return _localize.Invoke(null, new object[] { key }) as string
                ?? string.Empty;
        }

        internal static int GetAlchemyDecompositionCount(object itemDefinition)
        {
            if (itemDefinition == null)
                return 0;

            object details = _getItemDetails.Invoke(itemDefinition, null);
            object alchemy = Get(details, "alchemy");
            object decomposes = Get(alchemy, "decomposes");

            ICollection collection = decomposes as ICollection;
            if (collection != null)
                return collection.Count;

            IEnumerable enumerable = decomposes as IEnumerable;
            if (enumerable == null)
                return 0;

            int count = 0;
            foreach (object ignored in enumerable)
                count++;

            return count;
        }

        internal static bool TryGetPlayerParam(
            string name,
            out float value)
        {
            value = 0f;

            object mainGame = GetStatic(MainGameType, "me");
            object player = Get(mainGame, "player");
            if (player == null)
                return false;

            MethodInfo method = player.GetType().GetMethod(
                "GetParam",
                Inst,
                null,
                new[] { typeof(string), typeof(float) },
                null);

            if (method == null)
                return false;

            value = Convert.ToSingle(
                method.Invoke(
                    player,
                    new object[] { name, 0f }));

            return true;
        }

        internal static string RawExpression(object expression)
        {
            if (expression == null)
                return string.Empty;

            MethodInfo method = expression.GetType().GetMethod(
                "GetRawExpressionString",
                Inst,
                null,
                Type.EmptyTypes,
                null);

            if (method == null)
                return string.Empty;

            return method.Invoke(expression, null) as string
                ?? string.Empty;
        }

        internal static object Get(object obj, string name)
        {
            if (obj == null)
                return null;

            for (Type type = obj.GetType(); type != null; type = type.BaseType)
            {
                FieldInfo field = type.GetField(name, Inst);
                if (field != null)
                    return field.GetValue(obj);

                PropertyInfo property = type.GetProperty(name, Inst);
                if (property != null && property.CanRead)
                    return property.GetValue(obj, null);
            }

            return null;
        }

        internal static void Set(object obj, string name, object value)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            for (Type type = obj.GetType(); type != null; type = type.BaseType)
            {
                FieldInfo field = type.GetField(name, Inst);
                if (field != null)
                {
                    field.SetValue(obj, value);
                    return;
                }

                PropertyInfo property = type.GetProperty(name, Inst);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(obj, value, null);
                    return;
                }
            }

            throw new MissingMemberException(
                obj.GetType().FullName,
                name);
        }

        internal static object GetStatic(Type type, string name)
        {
            if (type == null)
                return null;

            for (Type current = type;
                current != null;
                current = current.BaseType)
            {
                FieldInfo field = current.GetField(name, Stat);
                if (field != null)
                    return field.GetValue(null);

                PropertyInfo property = current.GetProperty(name, Stat);
                if (property != null && property.CanRead)
                    return property.GetValue(null, null);
            }

            return null;
        }

        internal static int IntValue(
            object obj,
            string name,
            int fallback)
        {
            object value = Get(obj, name);
            if (value == null)
                return fallback;

            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return fallback;
            }
        }

        internal static string Id(object obj)
        {
            return Get(obj, "id") as string
                ?? string.Empty;
        }

        internal static string EnumName(object obj, string name)
        {
            object value = Get(obj, name);
            return value == null
                ? string.Empty
                : value.ToString();
        }

        internal static IList List(object obj, string name)
        {
            return Get(obj, name) as IList;
        }

        private static Type RequireType(string name)
        {
            Type direct = _gameAssembly.GetType(name, false);
            if (direct != null)
                return direct;

            Type found;
            try
            {
                found = _gameAssembly.GetTypes()
                    .FirstOrDefault(t =>
                        t != null
                        && t.Name == name);
            }
            catch (ReflectionTypeLoadException ex)
            {
                found = ex.Types
                    .FirstOrDefault(t =>
                        t != null
                        && t.Name == name);
            }

            if (found == null)
                throw new TypeLoadException(name);

            return found;
        }

        private static Type RequireTypeAcrossLoadedAssemblies(string name)
        {
            Type gameType = _gameAssembly.GetType(name, false);
            if (gameType != null)
                return gameType;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type direct = assembly.GetType(name, false);
                if (direct != null)
                    return direct;

                try
                {
                    Type found = assembly.GetTypes()
                        .FirstOrDefault(t =>
                            t != null
                            && t.Name == name);

                    if (found != null)
                        return found;
                }
                catch (ReflectionTypeLoadException ex)
                {
                    Type found = ex.Types
                        .FirstOrDefault(t =>
                            t != null
                            && t.Name == name);

                    if (found != null)
                        return found;
                }
            }

            throw new TypeLoadException(name);
        }

        private static MethodInfo RequireMethod(
            Type type,
            string name,
            int parameterCount,
            bool isStatic)
        {
            BindingFlags flags = isStatic ? Stat : Inst;
            MethodInfo method = type.GetMethods(flags)
                .FirstOrDefault(m =>
                    m.Name == name
                    && m.GetParameters().Length == parameterCount);

            if (method == null)
                throw new MissingMethodException(
                    type.FullName,
                    name);

            return method;
        }
    }
}
