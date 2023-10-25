// Decompiled with JetBrains decompiler
// Type: TestFlowCoordinator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;

public class TestFlowCoordinator : FlowCoordinator
{
  [SerializeField]
  [NullAllowed]
  protected ViewController _viewController;
  [SerializeField]
  [NullAllowed]
  protected ViewController _leftViewController;
  [SerializeField]
  [NullAllowed]
  protected ViewController _rightViewController;
  [SerializeField]
  [NullAllowed]
  protected ViewController _bottomScreenViewController;
  [SerializeField]
  [NullAllowed]
  protected ViewController _topScreenViewController;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!addedToHierarchy)
      return;
    this.ProvideInitialViewControllers(this._viewController, this._leftViewController, this._rightViewController, this._bottomScreenViewController, this._topScreenViewController);
  }
}
