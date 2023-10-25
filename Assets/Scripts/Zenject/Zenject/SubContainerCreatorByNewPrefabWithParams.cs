using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Zenject.Internal;

namespace Zenject
{
    // Token: 0x020002B3 RID: 691
    [NoReflectionBaking]
    public class SubContainerCreatorByNewPrefabWithParams : ISubContainerCreator
    {
        // Token: 0x06000EE9 RID: 3817 RVA: 0x00029C7C File Offset: 0x00027E7C
        public SubContainerCreatorByNewPrefabWithParams(Type installerType, DiContainer container, IPrefabProvider prefabProvider, GameObjectCreationParameters gameObjectBindInfo)
        {
            this._gameObjectBindInfo = gameObjectBindInfo;
            this._prefabProvider = prefabProvider;
            this._container = container;
            this._installerType = installerType;
        }

        // Token: 0x17000151 RID: 337
        // (get) Token: 0x06000EEA RID: 3818 RVA: 0x00029CA4 File Offset: 0x00027EA4
        protected DiContainer Container
        {
            get
            {
                return this._container;
            }
        }

        // Token: 0x06000EEB RID: 3819 RVA: 0x00029CAC File Offset: 0x00027EAC
        private DiContainer CreateTempContainer(List<TypeValuePair> args)
        {
            DiContainer diContainer = this.Container.CreateSubContainer();
            InjectTypeInfo info = TypeAnalyzer.GetInfo(this._installerType);
            using (List<TypeValuePair>.Enumerator enumerator = args.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    TypeValuePair argPair = enumerator.Current;
                    InjectableInfo injectableInfo = (from x in info.AllInjectables
                                                     where argPair.Type.DerivesFromOrEqual(x.MemberType)
                                                     orderby Zenject.Internal.ZenUtilInternal.GetInheritanceDelta(argPair.Type, x.MemberType)
                                                     select x).FirstOrDefault<InjectableInfo>();
                    ModestTree.Assert.That(injectableInfo != null, "Could not find match for argument type '{0}' when injecting into sub container installer '{1}'", argPair.Type, this._installerType);
                    diContainer.Bind(new Type[]
                    {
                        injectableInfo.MemberType
                    }).FromInstance(argPair.Value).WhenInjectedInto(new Type[]
                    {
                        this._installerType
                    });
                }
            }
            return diContainer;
        }

        // Token: 0x06000EEC RID: 3820 RVA: 0x00029DA8 File Offset: 0x00027FA8
        public DiContainer CreateSubContainer(List<TypeValuePair> args, InjectContext parentContext)
        {
            ModestTree.Assert.That(!args.IsEmpty<TypeValuePair>());
            UnityEngine.Object prefab = this._prefabProvider.GetPrefab();
            GameObjectContext component = this.CreateTempContainer(args).InstantiatePrefab(prefab, this._gameObjectBindInfo).GetComponent<GameObjectContext>();
            ModestTree.Assert.That(component != null, "Expected prefab with name '{0}' to container a component of type 'GameObjectContext'", prefab.name);
            return component.Container;
        }

        // Token: 0x040004A0 RID: 1184
        private readonly DiContainer _container;

        // Token: 0x040004A1 RID: 1185
        private readonly IPrefabProvider _prefabProvider;

        // Token: 0x040004A2 RID: 1186
        private readonly Type _installerType;

        // Token: 0x040004A3 RID: 1187
        private readonly GameObjectCreationParameters _gameObjectBindInfo;
    }
}
