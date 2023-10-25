// Decompiled with JetBrains decompiler
// Type: SceneLoadedCallbacksOrderDebug
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoadedCallbacksOrderDebug : MonoBehaviour
{
  public virtual void Awake() => Debug.Log((object) nameof (Awake));

  public virtual void OnEnable()
  {
    Debug.Log((object) "OnEnable called");
    SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
  }

  public virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    Debug.Log((object) ("OnSceneLoaded: " + scene.name));
    Debug.Log((object) mode);
  }

  public virtual void Start() => Debug.Log((object) nameof (Start));

  public virtual void OnDisable()
  {
    Debug.Log((object) nameof (OnDisable));
    SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
  }
}
