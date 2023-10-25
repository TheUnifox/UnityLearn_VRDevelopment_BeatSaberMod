// Decompiled with JetBrains decompiler
// Type: UnityScenesHelper
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class UnityScenesHelper
{
  public static void SetActiveRootObjectsInScene(Scene scene, bool active)
  {
    List<GameObject> gameObjectList = new List<GameObject>(scene.rootCount);
    scene.GetRootGameObjects(gameObjectList);
    if (gameObjectList.Count == 0)
      return;
    foreach (GameObject gameObject in gameObjectList)
      gameObject.SetActive(active);
  }

  public static IEnumerable<T> FindComponentsOfTypeInScene<T>(
    Scene activeScene,
    bool includeInactive)
  {
    List<T> componentsOfTypeInScene = new List<T>();
    foreach (GameObject rootGameObject in activeScene.GetRootGameObjects())
      componentsOfTypeInScene.AddRange((IEnumerable<T>) rootGameObject.GetComponentsInChildren<T>(includeInactive));
    return (IEnumerable<T>) componentsOfTypeInScene;
  }
}
