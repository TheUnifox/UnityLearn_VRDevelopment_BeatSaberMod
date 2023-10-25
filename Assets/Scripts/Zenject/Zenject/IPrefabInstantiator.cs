using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000272 RID: 626
    public interface IPrefabInstantiator
    {
        // Token: 0x1700013D RID: 317
        // (get) Token: 0x06000E25 RID: 3621
        Type ArgumentTarget { get; }

        // Token: 0x1700013E RID: 318
        // (get) Token: 0x06000E26 RID: 3622
        List<TypeValuePair> ExtraArguments { get; }

        // Token: 0x1700013F RID: 319
        // (get) Token: 0x06000E27 RID: 3623
        GameObjectCreationParameters GameObjectCreationParameters { get; }

        // Token: 0x06000E28 RID: 3624
        GameObject Instantiate(InjectContext context, List<TypeValuePair> args, out Action injectAction);

        // Token: 0x06000E29 RID: 3625
        UnityEngine.Object GetPrefab();
    }
}
