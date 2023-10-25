// Decompiled with JetBrains decompiler
// Type: DefaultSceneStart
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections;
using UnityEngine;
using Zenject;

public class DefaultSceneStart : MonoBehaviour
{
  [SerializeField]
  protected FlowCoordinator _flowCoordinator;
  [Inject]
  protected readonly GameScenesManager _gameScenesManager;
  [Inject]
  protected readonly HierarchyManager _hierarchyManager;

  public virtual IEnumerator Start()
  {
    yield return (object) this._gameScenesManager.waitUntilSceneTransitionFinish;
    this._hierarchyManager.StartWithFlowCoordinator(this._flowCoordinator);
  }
}
