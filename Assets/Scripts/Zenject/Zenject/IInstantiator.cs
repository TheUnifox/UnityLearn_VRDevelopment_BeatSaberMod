using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zenject
{
    // Token: 0x02000236 RID: 566
    public interface IInstantiator
    {
        // Token: 0x06000CFC RID: 3324
        T Instantiate<T>();

        // Token: 0x06000CFD RID: 3325
        T Instantiate<T>(IEnumerable<object> extraArgs);

        // Token: 0x06000CFE RID: 3326
        object Instantiate(Type concreteType);

        // Token: 0x06000CFF RID: 3327
        object Instantiate(Type concreteType, IEnumerable<object> extraArgs);

        // Token: 0x06000D00 RID: 3328
        TContract InstantiateComponent<TContract>(GameObject gameObject) where TContract : Component;

        // Token: 0x06000D01 RID: 3329
        TContract InstantiateComponent<TContract>(GameObject gameObject, IEnumerable<object> extraArgs) where TContract : Component;

        // Token: 0x06000D02 RID: 3330
        Component InstantiateComponent(Type componentType, GameObject gameObject);

        // Token: 0x06000D03 RID: 3331
        Component InstantiateComponent(Type componentType, GameObject gameObject, IEnumerable<object> extraArgs);

        // Token: 0x06000D04 RID: 3332
        T InstantiateComponentOnNewGameObject<T>() where T : Component;

        // Token: 0x06000D05 RID: 3333
        T InstantiateComponentOnNewGameObject<T>(string gameObjectName) where T : Component;

        // Token: 0x06000D06 RID: 3334
        T InstantiateComponentOnNewGameObject<T>(IEnumerable<object> extraArgs) where T : Component;

        // Token: 0x06000D07 RID: 3335
        T InstantiateComponentOnNewGameObject<T>(string gameObjectName, IEnumerable<object> extraArgs) where T : Component;

        // Token: 0x06000D08 RID: 3336
        GameObject InstantiatePrefab(UnityEngine.Object prefab);

        // Token: 0x06000D09 RID: 3337
        GameObject InstantiatePrefab(UnityEngine.Object prefab, Transform parentTransform);

        // Token: 0x06000D0A RID: 3338
        GameObject InstantiatePrefab(UnityEngine.Object prefab, Vector3 position, Quaternion rotation, Transform parentTransform);

        // Token: 0x06000D0B RID: 3339
        GameObject InstantiatePrefabResource(string resourcePath);

        // Token: 0x06000D0C RID: 3340
        GameObject InstantiatePrefabResource(string resourcePath, Transform parentTransform);

        // Token: 0x06000D0D RID: 3341
        GameObject InstantiatePrefabResource(string resourcePath, Vector3 position, Quaternion rotation, Transform parentTransform);

        // Token: 0x06000D0E RID: 3342
        T InstantiatePrefabForComponent<T>(UnityEngine.Object prefab);

        // Token: 0x06000D0F RID: 3343
        T InstantiatePrefabForComponent<T>(UnityEngine.Object prefab, IEnumerable<object> extraArgs);

        // Token: 0x06000D10 RID: 3344
        T InstantiatePrefabForComponent<T>(UnityEngine.Object prefab, Transform parentTransform);

        // Token: 0x06000D11 RID: 3345
        T InstantiatePrefabForComponent<T>(UnityEngine.Object prefab, Transform parentTransform, IEnumerable<object> extraArgs);

        // Token: 0x06000D12 RID: 3346
        T InstantiatePrefabForComponent<T>(UnityEngine.Object prefab, Vector3 position, Quaternion rotation, Transform parentTransform);

        // Token: 0x06000D13 RID: 3347
        T InstantiatePrefabForComponent<T>(UnityEngine.Object prefab, Vector3 position, Quaternion rotation, Transform parentTransform, IEnumerable<object> extraArgs);

        // Token: 0x06000D14 RID: 3348
        object InstantiatePrefabForComponent(Type concreteType, UnityEngine.Object prefab, Transform parentTransform, IEnumerable<object> extraArgs);

        // Token: 0x06000D15 RID: 3349
        T InstantiatePrefabResourceForComponent<T>(string resourcePath);

        // Token: 0x06000D16 RID: 3350
        T InstantiatePrefabResourceForComponent<T>(string resourcePath, IEnumerable<object> extraArgs);

        // Token: 0x06000D17 RID: 3351
        T InstantiatePrefabResourceForComponent<T>(string resourcePath, Transform parentTransform);

        // Token: 0x06000D18 RID: 3352
        T InstantiatePrefabResourceForComponent<T>(string resourcePath, Transform parentTransform, IEnumerable<object> extraArgs);

        // Token: 0x06000D19 RID: 3353
        T InstantiatePrefabResourceForComponent<T>(string resourcePath, Vector3 position, Quaternion rotation, Transform parentTransform);

        // Token: 0x06000D1A RID: 3354
        T InstantiatePrefabResourceForComponent<T>(string resourcePath, Vector3 position, Quaternion rotation, Transform parentTransform, IEnumerable<object> extraArgs);

        // Token: 0x06000D1B RID: 3355
        object InstantiatePrefabResourceForComponent(Type concreteType, string resourcePath, Transform parentTransform, IEnumerable<object> extraArgs);

        // Token: 0x06000D1C RID: 3356
        T InstantiateScriptableObjectResource<T>(string resourcePath) where T : ScriptableObject;

        // Token: 0x06000D1D RID: 3357
        T InstantiateScriptableObjectResource<T>(string resourcePath, IEnumerable<object> extraArgs) where T : ScriptableObject;

        // Token: 0x06000D1E RID: 3358
        object InstantiateScriptableObjectResource(Type scriptableObjectType, string resourcePath);

        // Token: 0x06000D1F RID: 3359
        object InstantiateScriptableObjectResource(Type scriptableObjectType, string resourcePath, IEnumerable<object> extraArgs);

        // Token: 0x06000D20 RID: 3360
        GameObject CreateEmptyGameObject(string name);
    }
}