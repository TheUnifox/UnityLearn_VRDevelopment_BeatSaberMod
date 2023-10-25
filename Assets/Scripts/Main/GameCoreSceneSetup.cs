// Decompiled with JetBrains decompiler
// Type: GameCoreSceneSetup
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class GameCoreSceneSetup : MonoInstaller
{
  [SerializeField]
  protected ScreenCaptureAfterDelay _screenCaptureAfterDelayPrefab;
  [SerializeField]
  protected MainSettingsModelSO _mainSettingsModel;
  [SerializeField]
  protected BloomFogSO _bloomFog;
  protected const float kPauseButtonPressDurationMultiplier = 0.75f;

  public override void InstallBindings()
  {
    if ((int) (ObservableVariableSO<int>) this._mainSettingsModel.pauseButtonPressDurationLevel == 0)
    {
      this.Container.Bind(typeof (ITickable), typeof (IMenuButtonTrigger)).To<InstantMenuButtonTrigger>().AsSingle();
    }
    else
    {
      this.Container.Bind<float>().FromInstance((float) (int) (ObservableVariableSO<int>) this._mainSettingsModel.pauseButtonPressDurationLevel * 0.75f).WhenInjectedInto<DelayedMenuButtonTrigger>();
      this.Container.Bind(typeof (ITickable), typeof (IMenuButtonTrigger)).To<DelayedMenuButtonTrigger>().AsSingle();
    }
    this.Container.Bind<BloomFogSO>().FromScriptableObject((ScriptableObject) this._bloomFog).AsSingle();
    this.Container.Bind<NoteCutter>().AsSingle();
    if (!this._mainSettingsModel.createScreenshotDuringTheGame)
      return;
    this.Container.Bind<ScreenCaptureAfterDelay.InitData>().FromInstance(new ScreenCaptureAfterDelay.InitData(ScreenCaptureCache.ScreenshotType.Game, 5f, 1920, 1080));
    this.Container.Bind<ScreenCaptureAfterDelay>().FromComponentInNewPrefab((Object) this._screenCaptureAfterDelayPrefab).AsSingle().NonLazy();
  }
}
