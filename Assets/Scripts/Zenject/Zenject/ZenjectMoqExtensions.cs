using System;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000146 RID: 326
    public static class ZenjectMoqExtensions
    {
        // Token: 0x06000711 RID: 1809 RVA: 0x00012E4C File Offset: 0x0001104C
        public static ScopeConcreteIdArgConditionCopyNonLazyBinder FromMock<TContract>(this FromBinderGeneric<TContract> binder) where TContract : MonoBehaviour
        {
            GameObject gameObject = new GameObject(string.Format("{0}", typeof(TContract)));
            gameObject.SetActive(false);
            TContract instance = gameObject.AddComponent<TContract>();
            return binder.FromInstance(instance);
        }

        // Token: 0x06000712 RID: 1810 RVA: 0x00012E88 File Offset: 0x00011088
        public static ConditionCopyNonLazyBinder FromMock<TContract>(this FactoryFromBinder<TContract> binder) where TContract : MonoBehaviour
        {
            GameObject gameObject = new GameObject(string.Format("{0}", typeof(TContract)));
            gameObject.SetActive(false);
            TContract tcontract = gameObject.AddComponent<TContract>();
            return binder.FromInstance(tcontract);
        }
    }
}
