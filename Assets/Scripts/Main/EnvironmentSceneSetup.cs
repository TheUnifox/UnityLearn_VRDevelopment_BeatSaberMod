// Decompiled with JetBrains decompiler
// Type: EnvironmentSceneSetup
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class EnvironmentSceneSetup : MonoInstaller
{
  [Inject]
  protected readonly EnvironmentSceneSetupData _sceneSetupData;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int trackLaneYPositionPropertyId = Shader.PropertyToID("_TrackLaneYPosition");

  public override void InstallBindings()
  {
    this.Container.Bind<EnvironmentBrandingManager.InitData>().FromInstance(new EnvironmentBrandingManager.InitData(this._sceneSetupData.hideBranding));
    switch (this._sceneSetupData.environmentInfo.environmentSizeData.trackLaneType)
    {
      case EnvironmentSizeData.TrackLaneType.None:
        Shader.SetGlobalFloat(EnvironmentSceneSetup.trackLaneYPositionPropertyId, -100f);
        break;
      case EnvironmentSizeData.TrackLaneType.Normal:
        Shader.SetGlobalFloat(EnvironmentSceneSetup.trackLaneYPositionPropertyId, 0.0f);
        break;
    }
  }
}
