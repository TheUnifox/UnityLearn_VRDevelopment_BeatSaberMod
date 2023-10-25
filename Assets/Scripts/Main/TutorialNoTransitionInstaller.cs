// Decompiled with JetBrains decompiler
// Type: TutorialNoTransitionInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class TutorialNoTransitionInstaller : NoTransitionInstaller
{
  [SerializeField]
  protected TutorialScenesTransitionSetupDataSO _scenesTransitionSetupData;
  [SerializeField]
  protected PlayerSpecificSettings _playerSpecificSettings;

  public override void InstallBindings(DiContainer container)
  {
    this._scenesTransitionSetupData.Init(this._playerSpecificSettings);
    this._scenesTransitionSetupData.InstallBindings(container);
  }
}
