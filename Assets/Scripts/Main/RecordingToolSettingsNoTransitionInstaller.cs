// Decompiled with JetBrains decompiler
// Type: RecordingToolSettingsNoTransitionInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class RecordingToolSettingsNoTransitionInstaller : NoTransitionInstaller
{
  [SerializeField]
  protected RecordingToolSceneSetupData _recordingToolSceneSetupData;
  [SerializeField]
  protected RecordingToolScenesTransitionSetupDataSO _scenesTransitionSetupData;

  public override void InstallBindings(DiContainer container)
  {
    this._scenesTransitionSetupData.Init(this._recordingToolSceneSetupData);
    this._scenesTransitionSetupData.InstallBindings(container);
  }
}
