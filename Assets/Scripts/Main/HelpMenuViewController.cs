// Decompiled with JetBrains decompiler
// Type: HelpMenuViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuViewController : ViewController
{
  [SerializeField]
  protected TextSegmentedControl _helpMenuSegmentedControl;
  protected List<(ViewController viewController, string localizationKey)> _viewControllers;

  public event System.Action<int> didSelectHelpSubMenuEvent;

  public virtual void Init(List<(ViewController, string)> viewControllers) => this._viewControllers = viewControllers;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this._helpMenuSegmentedControl.didSelectCellEvent += new System.Action<SegmentedControl, int>(this.HandleHelpMenuSegmentedControlDidSelectCell);
      List<string> texts = new List<string>(this._viewControllers.Count);
      foreach ((ViewController viewController, string localizationKey) viewController in this._viewControllers)
        texts.Add(Localization.Get(viewController.localizationKey));
      this._helpMenuSegmentedControl.SetTexts((IReadOnlyList<string>) texts);
    }
    if (!addedToHierarchy)
      return;
    this._helpMenuSegmentedControl.SelectCellWithNumber(0);
  }

  public virtual void HandleHelpMenuSegmentedControlDidSelectCell(
    SegmentedControl segmentedControl,
    int cellIdx)
  {
    System.Action<int> helpSubMenuEvent = this.didSelectHelpSubMenuEvent;
    if (helpSubMenuEvent == null)
      return;
    helpSubMenuEvent(cellIdx);
  }
}
