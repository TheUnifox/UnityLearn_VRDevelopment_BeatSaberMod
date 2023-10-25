// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapLevelEditorNoTransitionsInstaller
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapLevelEditorNoTransitionsInstaller : NoTransitionInstaller
  {
    [SerializeField]
    private EnvironmentInfoSO _environmentInfo;
    [SerializeField]
    private BeatmapEditorLevelSceneTransitionSetupDataSO _sceneTransitionSetupData;

    public override void InstallBindings(DiContainer container)
    {
      this._sceneTransitionSetupData.Init(this._environmentInfo, (BasicSpectrogramData) null, (BeatmapDataModel) null, (IReadonlyBeatmapState) null, new BeatmapData(0), (BeatmapEditorSettingsDataModel) null);
      this._sceneTransitionSetupData.InstallBindings(container);
    }
  }
}
