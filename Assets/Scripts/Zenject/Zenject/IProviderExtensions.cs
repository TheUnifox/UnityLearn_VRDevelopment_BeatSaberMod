using System;
using System.Collections.Generic;
using ModestTree;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x0200025B RID: 603
    public static class IProviderExtensions
    {
        // Token: 0x06000DB7 RID: 3511 RVA: 0x0002526C File Offset: 0x0002346C
        public static void GetAllInstancesWithInjectSplit(this IProvider creator, InjectContext context, out Action injectAction, List<object> buffer)
        {
            creator.GetAllInstancesWithInjectSplit(context, IProviderExtensions.EmptyArgList, out injectAction, buffer);
        }

        // Token: 0x06000DB8 RID: 3512 RVA: 0x0002527C File Offset: 0x0002347C
        public static void GetAllInstances(this IProvider creator, InjectContext context, List<object> buffer)
        {
            creator.GetAllInstances(context, IProviderExtensions.EmptyArgList, buffer);
        }

        // Token: 0x06000DB9 RID: 3513 RVA: 0x0002528C File Offset: 0x0002348C
        public static void GetAllInstances(this IProvider creator, InjectContext context, List<TypeValuePair> args, List<object> buffer)
        {
            ModestTree.Assert.IsNotNull(context);
            Action action;
            creator.GetAllInstancesWithInjectSplit(context, args, out action, buffer);
            if (action != null)
            {
                action();
            }
        }

        // Token: 0x06000DBA RID: 3514 RVA: 0x000252B4 File Offset: 0x000234B4
        public static object TryGetInstance(this IProvider creator, InjectContext context)
        {
            return creator.TryGetInstance(context, IProviderExtensions.EmptyArgList);
        }

        // Token: 0x06000DBB RID: 3515 RVA: 0x000252C4 File Offset: 0x000234C4
        public static object TryGetInstance(this IProvider creator, InjectContext context, List<TypeValuePair> args)
        {
            List<object> list = Zenject.Internal.ZenPools.SpawnList<object>();
            object result;
            try
            {
                creator.GetAllInstances(context, args, list);
                if (list.Count == 0)
                {
                    result = null;
                }
                else
                {
                    ModestTree.Assert.That(list.Count == 1, "Provider returned multiple instances when one or zero was expected");
                    result = list[0];
                }
            }
            finally
            {
                Zenject.Internal.ZenPools.DespawnList<object>(list);
            }
            return result;
        }

        // Token: 0x06000DBC RID: 3516 RVA: 0x00025324 File Offset: 0x00023524
        public static object GetInstance(this IProvider creator, InjectContext context)
        {
            return creator.GetInstance(context, IProviderExtensions.EmptyArgList);
        }

        // Token: 0x06000DBD RID: 3517 RVA: 0x00025334 File Offset: 0x00023534
        public static object GetInstance(this IProvider creator, InjectContext context, List<TypeValuePair> args)
        {
            List<object> list = Zenject.Internal.ZenPools.SpawnList<object>();
            object result;
            try
            {
                creator.GetAllInstances(context, args, list);
                ModestTree.Assert.That(list.Count > 0, "Provider returned zero instances when one was expected when looking up type '{0}'", context.MemberType);
                ModestTree.Assert.That(list.Count == 1, "Provider returned multiple instances when only one was expected when looking up type '{0}'", context.MemberType);
                result = list[0];
            }
            finally
            {
                Zenject.Internal.ZenPools.DespawnList<object>(list);
            }
            return result;
        }

        // Token: 0x04000406 RID: 1030
        private static readonly List<TypeValuePair> EmptyArgList = new List<TypeValuePair>();
    }
}
