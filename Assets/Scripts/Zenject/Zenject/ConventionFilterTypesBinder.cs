using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ModestTree;

namespace Zenject
{
    // Token: 0x02000061 RID: 97
    [NoReflectionBaking]
    public class ConventionFilterTypesBinder : ConventionAssemblySelectionBinder
    {
        // Token: 0x06000275 RID: 629 RVA: 0x00007DCC File Offset: 0x00005FCC
        public ConventionFilterTypesBinder(ConventionBindInfo bindInfo) : base(bindInfo)
        {
        }

        // Token: 0x06000276 RID: 630 RVA: 0x00007DD8 File Offset: 0x00005FD8
        public ConventionFilterTypesBinder DerivingFromOrEqual<T>()
        {
            return this.DerivingFromOrEqual(typeof(T));
        }

        // Token: 0x06000277 RID: 631 RVA: 0x00007DEC File Offset: 0x00005FEC
        public ConventionFilterTypesBinder DerivingFromOrEqual(Type parentType)
        {
            base.BindInfo.AddTypeFilter((Type type) => type.DerivesFromOrEqual(parentType));
            return this;
        }

        // Token: 0x06000278 RID: 632 RVA: 0x00007E20 File Offset: 0x00006020
        public ConventionFilterTypesBinder DerivingFrom<T>()
        {
            return this.DerivingFrom(typeof(T));
        }

        // Token: 0x06000279 RID: 633 RVA: 0x00007E34 File Offset: 0x00006034
        public ConventionFilterTypesBinder DerivingFrom(Type parentType)
        {
            base.BindInfo.AddTypeFilter((Type type) => type.DerivesFrom(parentType));
            return this;
        }

        // Token: 0x0600027A RID: 634 RVA: 0x00007E68 File Offset: 0x00006068
        public ConventionFilterTypesBinder WithAttribute<T>() where T : Attribute
        {
            return this.WithAttribute(typeof(T));
        }

        // Token: 0x0600027B RID: 635 RVA: 0x00007E7C File Offset: 0x0000607C
        public ConventionFilterTypesBinder WithAttribute(Type attribute)
        {
            ModestTree.Assert.That(attribute.DerivesFrom<Attribute>());
            base.BindInfo.AddTypeFilter((Type t) => t.HasAttribute(new Type[]
            {
                attribute
            }));
            return this;
        }

        // Token: 0x0600027C RID: 636 RVA: 0x00007EC0 File Offset: 0x000060C0
        public ConventionFilterTypesBinder WithoutAttribute<T>() where T : Attribute
        {
            return this.WithoutAttribute(typeof(T));
        }

        // Token: 0x0600027D RID: 637 RVA: 0x00007ED4 File Offset: 0x000060D4
        public ConventionFilterTypesBinder WithoutAttribute(Type attribute)
        {
            ModestTree.Assert.That(attribute.DerivesFrom<Attribute>());
            base.BindInfo.AddTypeFilter((Type t) => !t.HasAttribute(new Type[]
            {
                attribute
            }));
            return this;
        }

        // Token: 0x0600027E RID: 638 RVA: 0x00007F18 File Offset: 0x00006118
        public ConventionFilterTypesBinder WithAttributeWhere<T>(Func<T, bool> predicate) where T : Attribute
        {
            base.BindInfo.AddTypeFilter((Type t) => t.HasAttribute<T>() && t.AllAttributes<T>().All(predicate));
            return this;
        }

        // Token: 0x0600027F RID: 639 RVA: 0x00007F4C File Offset: 0x0000614C
        public ConventionFilterTypesBinder Where(Func<Type, bool> predicate)
        {
            base.BindInfo.AddTypeFilter(predicate);
            return this;
        }

        // Token: 0x06000280 RID: 640 RVA: 0x00007F5C File Offset: 0x0000615C
        public ConventionFilterTypesBinder InNamespace(string ns)
        {
            return this.InNamespaces(new string[]
            {
                ns
            });
        }

        // Token: 0x06000281 RID: 641 RVA: 0x00007F70 File Offset: 0x00006170
        public ConventionFilterTypesBinder InNamespaces(params string[] namespaces)
        {
            return this.InNamespaces(namespaces);
        }

        // Token: 0x06000282 RID: 642 RVA: 0x00007F7C File Offset: 0x0000617C
        public ConventionFilterTypesBinder InNamespaces(IEnumerable<string> namespaces)
        {
            base.BindInfo.AddTypeFilter((Type t) => namespaces.Any((string n) => ConventionFilterTypesBinder.IsInNamespace(t, n)));
            return this;
        }

        // Token: 0x06000283 RID: 643 RVA: 0x00007FB0 File Offset: 0x000061B0
        public ConventionFilterTypesBinder WithSuffix(string suffix)
        {
            base.BindInfo.AddTypeFilter((Type t) => t.Name.EndsWith(suffix));
            return this;
        }

        // Token: 0x06000284 RID: 644 RVA: 0x00007FE4 File Offset: 0x000061E4
        public ConventionFilterTypesBinder WithPrefix(string prefix)
        {
            base.BindInfo.AddTypeFilter((Type t) => t.Name.StartsWith(prefix));
            return this;
        }

        // Token: 0x06000285 RID: 645 RVA: 0x00008018 File Offset: 0x00006218
        public ConventionFilterTypesBinder MatchingRegex(string pattern)
        {
            return this.MatchingRegex(pattern, RegexOptions.None);
        }

        // Token: 0x06000286 RID: 646 RVA: 0x00008024 File Offset: 0x00006224
        public ConventionFilterTypesBinder MatchingRegex(string pattern, RegexOptions options)
        {
            return this.MatchingRegex(new Regex(pattern, options));
        }

        // Token: 0x06000287 RID: 647 RVA: 0x00008034 File Offset: 0x00006234
        public ConventionFilterTypesBinder MatchingRegex(Regex regex)
        {
            base.BindInfo.AddTypeFilter((Type t) => regex.IsMatch(t.Name));
            return this;
        }

        // Token: 0x06000288 RID: 648 RVA: 0x00008068 File Offset: 0x00006268
        private static bool IsInNamespace(Type type, string requiredNs)
        {
            string text = type.Namespace ?? "";
            return requiredNs.Length <= text.Length && text.StartsWith(requiredNs) && (text.Length == requiredNs.Length || text[requiredNs.Length] == '.');
        }
    }
}
