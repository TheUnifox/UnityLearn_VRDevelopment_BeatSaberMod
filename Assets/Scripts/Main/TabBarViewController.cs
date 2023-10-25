// Decompiled with JetBrains decompiler
// Type: TabBarViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabBarViewController : ViewController
{
  [SerializeField]
  protected TextSegmentedControl _segmentedControll;
  [SerializeField]
  protected ContentSizeFitter _contentSizeFilter;
  protected string[] _labels;
  protected TabBarViewController.TabBarItem[] _items;
  protected bool _shouldReloadData;

  public bool sizeToFit
  {
    set => this._contentSizeFilter.enabled = value;
    get => this._contentSizeFilter.enabled;
  }

  public int selectedCellNumber => this._segmentedControll.selectedCellNumber;

  public virtual void Setup(TabBarViewController.TabBarItem[] items)
  {
    this._items = items;
    List<string> stringList = new List<string>(this._items.Length);
    foreach (TabBarViewController.TabBarItem tabBarItem in this._items)
      stringList.Add(tabBarItem.title);
    this._labels = stringList.ToArray();
    if (this.isActiveAndEnabled)
    {
      this._segmentedControll.SetTexts((IReadOnlyList<string>) this._labels);
      this._shouldReloadData = false;
    }
    else
      this._shouldReloadData = true;
  }

  public virtual void SelectItem(int index) => this._segmentedControll.SelectCellWithNumber(index);

  public virtual void Clear() => this.Setup(new TabBarViewController.TabBarItem[0]);

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
      this._segmentedControll.didSelectCellEvent += new System.Action<SegmentedControl, int>(this.HandleDidSelectCell);
    if (this._labels == null || !this._shouldReloadData)
      return;
    this._shouldReloadData = false;
    this._segmentedControll.SetTexts((IReadOnlyList<string>) this._labels);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!(bool) (UnityEngine.Object) this._segmentedControll)
      return;
    this._segmentedControll.didSelectCellEvent -= new System.Action<SegmentedControl, int>(this.HandleDidSelectCell);
  }

  public virtual void HandleDidSelectCell(SegmentedControl segmentedControl, int cellNumber)
  {
    System.Action action = this._items[cellNumber].action;
    if (action == null)
      return;
    action();
  }

  public class TabBarItem
  {
    public readonly string title;
    public readonly System.Action action;

    public TabBarItem(string title, System.Action action)
    {
      this.title = title;
      this.action = action;
    }
  }
}
