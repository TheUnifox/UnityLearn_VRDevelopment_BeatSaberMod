// Decompiled with JetBrains decompiler
// Type: FindUnityObjectsHelper
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class FindUnityObjectsHelper
{
  public static List<MonoBehaviour> GetMonoBehavioursInLoadedScenes() => FindUnityObjectsHelper.GetComponentsInGameObjects<MonoBehaviour>((IReadOnlyList<GameObject>) ((IEnumerable<GameObject>) FindUnityObjectsHelper.GetAllGameObjectsInLoadedScenes()).ToArray<GameObject>());

  public static List<GameObject> GetAllRootGameObjectsInLoadedScenes()
  {
    List<GameObject> objectsInLoadedScenes = new List<GameObject>();
    for (int index = 0; index < SceneManager.sceneCount; ++index)
    {
      Scene sceneAt = SceneManager.GetSceneAt(index);
      objectsInLoadedScenes.AddRange((IEnumerable<GameObject>) sceneAt.GetRootGameObjects());
    }
    return objectsInLoadedScenes;
  }

  public static List<GameObject> GetAllGameObjectsInGameObject(GameObject go)
  {
    List<GameObject> objectsInGameObject = new List<GameObject>();
    objectsInGameObject.Add(go);
    int index1 = 0;
    do
    {
      Transform transform = objectsInGameObject[index1].transform;
      int childCount = transform.childCount;
      for (int index2 = 0; index2 < childCount; ++index2)
        objectsInGameObject.Add(transform.GetChild(index2).gameObject);
      ++index1;
    }
    while (index1 < objectsInGameObject.Count);
    return objectsInGameObject;
  }

  public static GameObject[] GetAllGameObjectsInLoadedScenes()
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    for (int index = 0; index < SceneManager.sceneCount; ++index)
    {
      Scene sceneAt = SceneManager.GetSceneAt(index);
      gameObjectList.AddRange((IEnumerable<GameObject>) sceneAt.GetRootGameObjects());
    }
    if (gameObjectList.Count == 0)
      return gameObjectList.ToArray();
    int index1 = 0;
    do
    {
      Transform transform = gameObjectList[index1].transform;
      int childCount = transform.childCount;
      for (int index2 = 0; index2 < childCount; ++index2)
        gameObjectList.Add(transform.GetChild(index2).gameObject);
      ++index1;
    }
    while (index1 < gameObjectList.Count);
    return gameObjectList.ToArray();
  }

  public static List<T> GetComponentsInGameObjects<T>(IReadOnlyList<GameObject> gameObjects) where T : Behaviour
  {
    List<T> componentsInGameObjects = new List<T>();
    foreach (GameObject gameObject in (IEnumerable<GameObject>) gameObjects)
    {
      foreach (T component in gameObject.GetComponents<T>())
        componentsInGameObjects.Add(component);
    }
    return componentsInGameObjects;
  }

  public static List<T> GetComponentsInScene<T>(Scene scene, bool includeInactive = false)
  {
    List<T> componentsInScene = new List<T>();
    foreach (GameObject rootGameObject in scene.GetRootGameObjects())
      componentsInScene.AddRange((IEnumerable<T>) rootGameObject.GetComponentsInChildren<T>(includeInactive));
    return componentsInScene;
  }

  public static T GetComponentInScene<T>(Scene scene, bool includeInactive = false) where T : class
  {
    foreach (GameObject rootGameObject in scene.GetRootGameObjects())
    {
      T componentInChildren = rootGameObject.GetComponentInChildren<T>(includeInactive);
      if ((object) componentInChildren != null)
        return componentInChildren;
    }
    return default (T);
  }
}
