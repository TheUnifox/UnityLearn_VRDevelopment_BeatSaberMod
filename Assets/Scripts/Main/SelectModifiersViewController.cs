// Decompiled with JetBrains decompiler
// Type: SelectModifiersViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;

public class SelectModifiersViewController : ViewController
{
  [SerializeField]
  protected GameplayModifiersPanelController _gameplayModifiersPanelController;

  public GameplayModifiers gameplayModifiers => this._gameplayModifiersPanelController.gameplayModifiers;

  public virtual void Setup(GameplayModifiers gameplayModifiers) => this._gameplayModifiersPanelController.SetData(gameplayModifiers);

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    ((IRefreshable) this._gameplayModifiersPanelController).Refresh();
  }
}
