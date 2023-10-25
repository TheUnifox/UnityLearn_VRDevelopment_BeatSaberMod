// Decompiled with JetBrains decompiler
// Type: MarkSceneAsPersistent
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;
using Zenject;

public class MarkSceneAsPersistent : MonoBehaviour
{
  [Inject]
  protected GameScenesManager _gameScenesManager;

  public virtual void Awake() => this._gameScenesManager.MarkSceneAsPersistent(this.gameObject.scene.name);
}
