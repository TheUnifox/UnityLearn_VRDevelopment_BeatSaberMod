// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorNoTransitionInstaller
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorNoTransitionInstaller : NoTransitionInstaller
  {
    [SerializeField]
    private string _projectPath;
    [SerializeField]
    private bool _ignoreTemp;
    [SerializeField]
    private BeatmapCharacteristicSO _beatmapCharacteristic;
    [SerializeField]
    private BeatmapDifficulty _beatmapDifficulty;

    public override void InstallBindings(DiContainer container)
    {
      if (!this.enabled)
        return;
      container.Bind<OpenBeatmapLevelDestination>().FromInstance(new OpenBeatmapLevelDestination(this._projectPath, this._ignoreTemp, this._beatmapCharacteristic, this._beatmapDifficulty)).AsSingle();
    }
  }
}
