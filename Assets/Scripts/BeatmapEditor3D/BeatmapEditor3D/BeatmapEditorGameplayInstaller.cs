// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorGameplayInstaller
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorGameplayInstaller : MonoInstaller
  {
    [SerializeField]
    private BeatmapEditorGameplayLevelRestartController _levelRestartController;
    [SerializeField]
    private BeatmapEditorGameplayReturnToMenuController _returnToMenuController;
    [Inject]
    private readonly BeatmapEditorGameplaySceneSetupData _beatmapEditorGameplaySceneSetupData;

    public override void InstallBindings()
    {
      this.Container.Bind<BeatmapEditorGameplayFpfcInit.Init>().FromInstance(new BeatmapEditorGameplayFpfcInit.Init(this._beatmapEditorGameplaySceneSetupData.useFirstPersonFlyingController)).AsSingle();
      this.Container.Rebind<ILevelRestartController>().FromInstance((ILevelRestartController) this._levelRestartController);
      this.Container.Rebind<IReturnToMenuController>().FromInstance((IReturnToMenuController) this._returnToMenuController);
    }
  }
}
