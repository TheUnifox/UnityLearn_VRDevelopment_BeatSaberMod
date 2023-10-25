using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModestTree
{
    // Token: 0x02000015 RID: 21
    public static class TypeStringFormatter
    {
        // Token: 0x0600009A RID: 154 RVA: 0x000032B0 File Offset: 0x000014B0
        public static string PrettyName(this Type type)
        {
            string text;
            if (!TypeStringFormatter._prettyNameCache.TryGetValue(type, out text))
            {
                text = TypeStringFormatter.PrettyNameInternal(type);
                TypeStringFormatter._prettyNameCache.Add(type, text);
            }
            return text;
        }

        // Token: 0x0600009B RID: 155 RVA: 0x000032E0 File Offset: 0x000014E0
        private static string PrettyNameInternal(Type type)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (type.IsNested)
            {
                stringBuilder.Append(type.DeclaringType.PrettyName());
                stringBuilder.Append(".");
            }
            if (type.IsArray)
            {
                stringBuilder.Append(type.GetElementType().PrettyName());
                stringBuilder.Append("[]");
            }
            else
            {
                string csharpTypeName = TypeStringFormatter.GetCSharpTypeName(type.Name);
                if (type.IsGenericType())
                {
                    if (csharpTypeName.IndexOf('`') != -1)
                    {
                        stringBuilder.Append(csharpTypeName.Substring(0, csharpTypeName.IndexOf('`')));
                    }
                    else
                    {
                        stringBuilder.Append(csharpTypeName);
                    }
                    stringBuilder.Append("<");
                    if (type.IsGenericTypeDefinition())
                    {
                        int num = type.GenericArguments().Count<Type>();
                        if (num > 0)
                        {
                            stringBuilder.Append(new string(',', num - 1));
                        }
                    }
                    else
                    {
                        stringBuilder.Append(string.Join(", ", (from t in type.GenericArguments()
                                                                select t.PrettyName()).ToArray<string>()));
                    }
                    stringBuilder.Append(">");
                }
                else
                {
                    stringBuilder.Append(csharpTypeName);
                }
            }
            return stringBuilder.ToString();
        }

        // Token: 0x0600009C RID: 156 RVA: 0x0000341C File Offset: 0x0000161C
        private static string GetCSharpTypeName(string typeName)
        {
            switch (typeName)
            {
                case "String":
                case "Object":
                case "Void":
                case "Byte":
                case "Double":
                case "Decimal":
                    return typeName.ToLower();
                case "Int16":
                    return "short";
                case "Int32":
                    return "int";
                case "Int64":
                    return "long";
                case "Single":
                    return "float";
                case "Boolean":
                    return "bool";
                default:
                    return typeName;
            }
        }

        // Token: 0x04000022 RID: 34
        private static readonly Dictionary<Type, string> _prettyNameCache = new Dictionary<Type, string>();
    }
}
