using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200015F RID: 351
    internal static class BindingUtil
    {
        // Token: 0x0600077B RID: 1915 RVA: 0x00013E18 File Offset: 0x00012018
        public static void AssertIsValidPrefab(UnityEngine.Object prefab)
        {
            ModestTree.Assert.That(!Zenject.Internal.ZenUtilInternal.IsNull(prefab), "Received null prefab during bind command");
        }

        // Token: 0x0600077C RID: 1916 RVA: 0x00013E30 File Offset: 0x00012030
        public static void AssertIsValidGameObject(GameObject gameObject)
        {
            ModestTree.Assert.That(!Zenject.Internal.ZenUtilInternal.IsNull(gameObject), "Received null game object during bind command");
        }

        // Token: 0x0600077D RID: 1917 RVA: 0x00013E48 File Offset: 0x00012048
        public static void AssertIsNotComponent(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                BindingUtil.AssertIsNotComponent(type);
            }
        }

        // Token: 0x0600077E RID: 1918 RVA: 0x00013E90 File Offset: 0x00012090
        public static void AssertIsNotComponent<T>()
        {
            BindingUtil.AssertIsNotComponent(typeof(T));
        }

        // Token: 0x0600077F RID: 1919 RVA: 0x00013EA4 File Offset: 0x000120A4
        public static void AssertIsNotComponent(Type type)
        {
            ModestTree.Assert.That(!type.DerivesFrom(typeof(Component)), "Invalid type given during bind command.  Expected type '{0}' to NOT derive from UnityEngine.Component", type);
        }

        // Token: 0x06000780 RID: 1920 RVA: 0x00013EC4 File Offset: 0x000120C4
        public static void AssertDerivesFromUnityObject(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                BindingUtil.AssertDerivesFromUnityObject(type);
            }
        }

        // Token: 0x06000781 RID: 1921 RVA: 0x00013F0C File Offset: 0x0001210C
        public static void AssertDerivesFromUnityObject<T>()
        {
            BindingUtil.AssertDerivesFromUnityObject(typeof(T));
        }

        // Token: 0x06000782 RID: 1922 RVA: 0x00013F20 File Offset: 0x00012120
        public static void AssertDerivesFromUnityObject(Type type)
        {
            ModestTree.Assert.That(type.DerivesFrom<UnityEngine.Object>(), "Invalid type given during bind command.  Expected type '{0}' to derive from UnityEngine.Object", type);
        }

        // Token: 0x06000783 RID: 1923 RVA: 0x00013F34 File Offset: 0x00012134
        public static void AssertTypesAreNotComponents(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                BindingUtil.AssertIsNotComponent(type);
            }
        }

        // Token: 0x06000784 RID: 1924 RVA: 0x00013F7C File Offset: 0x0001217C
        public static void AssertIsValidResourcePath(string resourcePath)
        {
            ModestTree.Assert.That(!string.IsNullOrEmpty(resourcePath), "Null or empty resource path provided");
        }

        // Token: 0x06000785 RID: 1925 RVA: 0x00013F94 File Offset: 0x00012194
        public static void AssertIsInterfaceOrScriptableObject(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                BindingUtil.AssertIsInterfaceOrScriptableObject(type);
            }
        }

        // Token: 0x06000786 RID: 1926 RVA: 0x00013FDC File Offset: 0x000121DC
        public static void AssertIsInterfaceOrScriptableObject<T>()
        {
            BindingUtil.AssertIsInterfaceOrScriptableObject(typeof(T));
        }

        // Token: 0x06000787 RID: 1927 RVA: 0x00013FF0 File Offset: 0x000121F0
        public static void AssertIsInterfaceOrScriptableObject(Type type)
        {
            ModestTree.Assert.That(type.DerivesFrom(typeof(ScriptableObject)) || type.IsInterface(), "Invalid type given during bind command.  Expected type '{0}' to either derive from UnityEngine.ScriptableObject or be an interface", type);
        }

        // Token: 0x06000788 RID: 1928 RVA: 0x00014018 File Offset: 0x00012218
        public static void AssertIsInterfaceOrComponent(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                BindingUtil.AssertIsInterfaceOrComponent(type);
            }
        }

        // Token: 0x06000789 RID: 1929 RVA: 0x00014060 File Offset: 0x00012260
        public static void AssertIsInterfaceOrComponent<T>()
        {
            BindingUtil.AssertIsInterfaceOrComponent(typeof(T));
        }

        // Token: 0x0600078A RID: 1930 RVA: 0x00014074 File Offset: 0x00012274
        public static void AssertIsInterfaceOrComponent(Type type)
        {
            ModestTree.Assert.That(type.DerivesFrom(typeof(Component)) || type.IsInterface(), "Invalid type given during bind command.  Expected type '{0}' to either derive from UnityEngine.Component or be an interface", type);
        }

        // Token: 0x0600078B RID: 1931 RVA: 0x0001409C File Offset: 0x0001229C
        public static void AssertIsComponent(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                BindingUtil.AssertIsComponent(type);
            }
        }

        // Token: 0x0600078C RID: 1932 RVA: 0x000140E4 File Offset: 0x000122E4
        public static void AssertIsComponent<T>()
        {
            BindingUtil.AssertIsComponent(typeof(T));
        }

        // Token: 0x0600078D RID: 1933 RVA: 0x000140F8 File Offset: 0x000122F8
        public static void AssertIsComponent(Type type)
        {
            ModestTree.Assert.That(type.DerivesFrom(typeof(Component)), "Invalid type given during bind command.  Expected type '{0}' to derive from UnityEngine.Component", type);
        }

        // Token: 0x0600078E RID: 1934 RVA: 0x00014118 File Offset: 0x00012318
        public static void AssertTypesAreNotAbstract(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                BindingUtil.AssertIsNotAbstract(type);
            }
        }

        // Token: 0x0600078F RID: 1935 RVA: 0x00014160 File Offset: 0x00012360
        public static void AssertIsNotAbstract(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                BindingUtil.AssertIsNotAbstract(type);
            }
        }

        // Token: 0x06000790 RID: 1936 RVA: 0x000141A8 File Offset: 0x000123A8
        public static void AssertIsNotAbstract<T>()
        {
            BindingUtil.AssertIsNotAbstract(typeof(T));
        }

        // Token: 0x06000791 RID: 1937 RVA: 0x000141BC File Offset: 0x000123BC
        public static void AssertIsNotAbstract(Type type)
        {
            ModestTree.Assert.That(!type.IsAbstract(), "Invalid type given during bind command.  Expected type '{0}' to not be abstract.", type);
        }

        // Token: 0x06000792 RID: 1938 RVA: 0x000141D4 File Offset: 0x000123D4
        public static void AssertIsDerivedFromType(Type concreteType, Type parentType)
        {
            ModestTree.Assert.That(parentType.IsOpenGenericType() == concreteType.IsOpenGenericType(), "Invalid type given during bind command.  Expected type '{0}' and type '{1}' to both either be open generic types or not open generic types", parentType, concreteType);
            if (parentType.IsOpenGenericType())
            {
                ModestTree.Assert.That(concreteType.IsOpenGenericType());
                ModestTree.Assert.That(ModestTree.TypeExtensions.IsAssignableToGenericType(concreteType, parentType), "Invalid type given during bind command.  Expected open generic type '{0}' to derive from open generic type '{1}'", concreteType, parentType);
                return;
            }
            ModestTree.Assert.That(concreteType.DerivesFromOrEqual(parentType), "Invalid type given during bind command.  Expected type '{0}' to derive from type '{1}'", concreteType, parentType);
        }

        // Token: 0x06000793 RID: 1939 RVA: 0x00014238 File Offset: 0x00012438
        public static void AssertConcreteTypeListIsNotEmpty(IEnumerable<Type> concreteTypes)
        {
            ModestTree.Assert.That(concreteTypes.Count<Type>() >= 1, "Must supply at least one concrete type to the current binding");
        }

        // Token: 0x06000794 RID: 1940 RVA: 0x00014250 File Offset: 0x00012450
        public static void AssertIsDerivedFromTypes(IEnumerable<Type> concreteTypes, IEnumerable<Type> parentTypes, InvalidBindResponses invalidBindResponse)
        {
            if (invalidBindResponse == InvalidBindResponses.Assert)
            {
                BindingUtil.AssertIsDerivedFromTypes(concreteTypes, parentTypes);
                return;
            }
            ModestTree.Assert.IsEqual(invalidBindResponse, InvalidBindResponses.Skip);
        }

        // Token: 0x06000795 RID: 1941 RVA: 0x00014270 File Offset: 0x00012470
        public static void AssertIsDerivedFromTypes(IEnumerable<Type> concreteTypes, IEnumerable<Type> parentTypes)
        {
            foreach (Type concreteType in concreteTypes)
            {
                BindingUtil.AssertIsDerivedFromTypes(concreteType, parentTypes);
            }
        }

        // Token: 0x06000796 RID: 1942 RVA: 0x000142B8 File Offset: 0x000124B8
        public static void AssertIsDerivedFromTypes(Type concreteType, IEnumerable<Type> parentTypes)
        {
            foreach (Type parentType in parentTypes)
            {
                BindingUtil.AssertIsDerivedFromType(concreteType, parentType);
            }
        }

        // Token: 0x06000797 RID: 1943 RVA: 0x00014300 File Offset: 0x00012500
        public static void AssertInstanceDerivesFromOrEqual(object instance, IEnumerable<Type> parentTypes)
        {
            if (!Zenject.Internal.ZenUtilInternal.IsNull(instance))
            {
                foreach (Type baseType in parentTypes)
                {
                    BindingUtil.AssertInstanceDerivesFromOrEqual(instance, baseType);
                }
            }
        }

        // Token: 0x06000798 RID: 1944 RVA: 0x00014350 File Offset: 0x00012550
        public static void AssertInstanceDerivesFromOrEqual(object instance, Type baseType)
        {
            if (!Zenject.Internal.ZenUtilInternal.IsNull(instance))
            {
                ModestTree.Assert.That(instance.GetType().DerivesFromOrEqual(baseType), "Invalid type given during bind command.  Expected type '{0}' to derive from type '{1}'", instance.GetType(), baseType);
            }
        }

        // Token: 0x06000799 RID: 1945 RVA: 0x00014378 File Offset: 0x00012578
        public static IProvider CreateCachedProvider(IProvider creator)
        {
            if (creator.TypeVariesBasedOnMemberType)
            {
                return new CachedOpenTypeProvider(creator);
            }
            return new CachedProvider(creator);
        }
    }
}
