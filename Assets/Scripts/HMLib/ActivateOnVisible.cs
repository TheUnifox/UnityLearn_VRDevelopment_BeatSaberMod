// Decompiled with JetBrains decompiler
// Type: ActivateOnVisible
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class ActivateOnVisible : MonoBehaviour
{
  public GameObject[] _gameObjects;

  public virtual void Awake()
  {
    for (int index = 0; index < this._gameObjects.Length; ++index)
      this._gameObjects[index].SetActive(false);
  }

  public virtual void OnBecameVisible()
  {
    for (int index = 0; index < this._gameObjects.Length; ++index)
      this._gameObjects[index].SetActive(true);
  }

  public virtual void OnBecameInvisible()
  {
    for (int index = 0; index < this._gameObjects.Length; ++index)
      this._gameObjects[index].SetActive(false);
  }
}
