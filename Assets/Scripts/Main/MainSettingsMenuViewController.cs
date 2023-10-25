// Decompiled with JetBrains decompiler
// Type: MainSettingsMenuViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections.Generic;
using UnityEngine;

public class MainSettingsMenuViewController : ViewController
{
  [SerializeField]
  protected SettingsSubMenuInfo[] _settingsSubMenuInfos;
  [SerializeField]
  protected TextSegmentedControl _settingsMenuSegmentedControl;
  protected SettingsSubMenuInfo _selectedSubMenuInfo;
  protected int _selectedSubMenuInfoIdx;

  public event System.Action<SettingsSubMenuInfo, int> didSelectSettingsSubMenuEvent;

  public int numberOfSubMenus => this._settingsSubMenuInfos.Length;

  public SettingsSubMenuInfo selectedSubMenuInfo => this._selectedSubMenuInfo;

  public virtual void Init(int selectedSubMenuInfoIdx)
  {
    this._selectedSubMenuInfoIdx = selectedSubMenuInfoIdx;
    this._selectedSubMenuInfo = this._settingsSubMenuInfos[this._selectedSubMenuInfoIdx];
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this._settingsMenuSegmentedControl.didSelectCellEvent += new System.Action<SegmentedControl, int>(this.HandleSettingsMenuSegmentedControlDidSelectCell);
      List<string> texts = new List<string>(this._settingsSubMenuInfos.Length);
      foreach (SettingsSubMenuInfo settingsSubMenuInfo in this._settingsSubMenuInfos)
        texts.Add(settingsSubMenuInfo.localizedMenuName);
      this._settingsMenuSegmentedControl.SetTexts((IReadOnlyList<string>) texts);
    }
    if (!addedToHierarchy)
      return;
    this._selectedSubMenuInfo = (SettingsSubMenuInfo) null;
    this._settingsMenuSegmentedControl.SelectCellWithNumber(this._selectedSubMenuInfoIdx);
  }

  public virtual void HandleSettingsMenuSegmentedControlDidSelectCell(
    SegmentedControl segmentedControl,
    int cellIdx)
  {
    this._selectedSubMenuInfoIdx = cellIdx;
    this._selectedSubMenuInfo = this._settingsSubMenuInfos[cellIdx];
    System.Action<SettingsSubMenuInfo, int> settingsSubMenuEvent = this.didSelectSettingsSubMenuEvent;
    if (settingsSubMenuEvent == null)
      return;
    settingsSubMenuEvent(this._selectedSubMenuInfo, cellIdx);
  }
}
