// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalActivePlayerInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class MultiplayerLocalActivePlayerInstaller : MonoInstaller
{
  [Inject]
  protected readonly GameplayCoreSceneSetupData _sceneSetupData;

  public override void InstallBindings()
  {
    this.Container.BindInterfacesAndSelfTo<NoPauseGamePause>().AsSingle();
    this.Container.Bind<MultiplayerLocalActivePlayerGameplayManager.InitData>().FromInstance(new MultiplayerLocalActivePlayerGameplayManager.InitData(!this._sceneSetupData.gameplayModifiers.noFailOn0Energy)).AsSingle();
  }
}
