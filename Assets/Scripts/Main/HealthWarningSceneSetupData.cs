// Decompiled with JetBrains decompiler
// Type: HealthWarningSceneSetupData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class HealthWarningSceneSetupData : SceneSetupData
{
  [SerializeField]
  protected ScenesTransitionSetupDataSO _nextScenesTransitionSetupData;

  public ScenesTransitionSetupDataSO nextScenesTransitionSetupData => this._nextScenesTransitionSetupData;

  public HealthWarningSceneSetupData(
    ScenesTransitionSetupDataSO nextScenesTransitionSetupData)
  {
    this._nextScenesTransitionSetupData = nextScenesTransitionSetupData;
  }
}
