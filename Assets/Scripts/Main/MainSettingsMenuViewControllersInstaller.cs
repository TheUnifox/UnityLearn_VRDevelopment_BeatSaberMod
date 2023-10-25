// Decompiled with JetBrains decompiler
// Type: MainSettingsMenuViewControllersInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MainSettingsMenuViewControllersInstaller : MonoInstaller
{
  [SerializeField]
  protected MainSettingsMenuViewController _defaultSettingsMenuViewController;
  [SerializeField]
  protected MainSettingsMenuViewController _oculusPCSettingsMenuViewController;
  [SerializeField]
  protected MainSettingsMenuViewController _questSettingsMenuViewController;
  [SerializeField]
  protected MainSettingsMenuViewController _psvrSettingsMenuViewController;
  [SerializeField]
  protected TabBarViewController _tabBarViewControllerPrefab;

  public override void InstallBindings()
  {
    this.Container.Bind<MainSettingsMenuViewController>().FromInstance(this._oculusPCSettingsMenuViewController);
    this.Container.Bind<TabBarViewController>().FromComponentInNewPrefab((Object) this._tabBarViewControllerPrefab).AsTransient();
  }
}
