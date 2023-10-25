// Decompiled with JetBrains decompiler
// Type: SelectLevelCategoryViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class SelectLevelCategoryViewController : ViewController
{
  [SerializeField]
  protected SelectLevelCategoryViewController.LevelCategoryInfo[] _allLevelCategoryInfos;
  [SerializeField]
  protected IconSegmentedControl _levelFilterCategoryIconSegmentedControl;
  [Inject]
  protected readonly IAnalyticsModel _analyticsModel;
  protected SelectLevelCategoryViewController.LevelCategory _prevSelectedLevelCategory;
  protected SelectLevelCategoryViewController.LevelCategoryInfo[] _levelCategoryInfos;

  public event System.Action<SelectLevelCategoryViewController, SelectLevelCategoryViewController.LevelCategory> didSelectLevelCategoryEvent;

  public SelectLevelCategoryViewController.LevelCategory selectedLevelCategory => this._levelFilterCategoryIconSegmentedControl.selectedCellNumber >= 0 && this._levelCategoryInfos != null && this._levelFilterCategoryIconSegmentedControl.selectedCellNumber < this._levelCategoryInfos.Length ? this._levelCategoryInfos[this._levelFilterCategoryIconSegmentedControl.selectedCellNumber].levelCategory : SelectLevelCategoryViewController.LevelCategory.None;

  public virtual void Setup(
    SelectLevelCategoryViewController.LevelCategory selectedCategory,
    SelectLevelCategoryViewController.LevelCategory[] enabledLevelCategories)
  {
    this._levelCategoryInfos = ((IEnumerable<SelectLevelCategoryViewController.LevelCategoryInfo>) this._allLevelCategoryInfos).Where<SelectLevelCategoryViewController.LevelCategoryInfo>((Func<SelectLevelCategoryViewController.LevelCategoryInfo, bool>) (data => ((IEnumerable<SelectLevelCategoryViewController.LevelCategory>) enabledLevelCategories).Contains<SelectLevelCategoryViewController.LevelCategory>(data.levelCategory))).ToArray<SelectLevelCategoryViewController.LevelCategoryInfo>();
    this._levelFilterCategoryIconSegmentedControl.SetData(((IEnumerable<SelectLevelCategoryViewController.LevelCategoryInfo>) this._levelCategoryInfos).Select<SelectLevelCategoryViewController.LevelCategoryInfo, IconSegmentedControl.DataItem>((Func<SelectLevelCategoryViewController.LevelCategoryInfo, IconSegmentedControl.DataItem>) (x => new IconSegmentedControl.DataItem(x.categoryIcon, Localization.Get(x.localizedKey)))).ToArray<IconSegmentedControl.DataItem>());
    this._prevSelectedLevelCategory = SelectLevelCategoryViewController.LevelCategory.None;
    this._levelFilterCategoryIconSegmentedControl.SelectCellWithNumber(((IEnumerable<SelectLevelCategoryViewController.LevelCategoryInfo>) this._levelCategoryInfos).Select<SelectLevelCategoryViewController.LevelCategoryInfo, SelectLevelCategoryViewController.LevelCategory>((Func<SelectLevelCategoryViewController.LevelCategoryInfo, SelectLevelCategoryViewController.LevelCategory>) (x => x.levelCategory)).ToList<SelectLevelCategoryViewController.LevelCategory>().IndexOf(selectedCategory));
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (!addedToHierarchy)
      return;
    this._levelFilterCategoryIconSegmentedControl.didSelectCellEvent += new System.Action<SegmentedControl, int>(this.LevelFilterCategoryIconSegmentedControlDidSelectCell);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._levelFilterCategoryIconSegmentedControl.didSelectCellEvent -= new System.Action<SegmentedControl, int>(this.LevelFilterCategoryIconSegmentedControlDidSelectCell);
  }

  public virtual void LevelFilterCategoryIconSegmentedControlDidSelectCell(
    SegmentedControl segmentedControl,
    int index)
  {
    if (this.selectedLevelCategory != this._prevSelectedLevelCategory)
    {
      this._analyticsModel.LogClick("Level Category Button", new Dictionary<string, string>()
      {
        ["level_category_button"] = this._levelCategoryInfos[index].levelCategory.ToString()
      });
      System.Action<SelectLevelCategoryViewController, SelectLevelCategoryViewController.LevelCategory> levelCategoryEvent = this.didSelectLevelCategoryEvent;
      if (levelCategoryEvent != null)
        levelCategoryEvent(this, this._levelCategoryInfos[index].levelCategory);
    }
    this._prevSelectedLevelCategory = this.selectedLevelCategory;
  }

  public enum LevelCategory
  {
    None,
    MusicPacks,
    CustomSongs,
    Favorites,
    All,
  }

  [Serializable]
  public class LevelCategoryInfo
  {
    public SelectLevelCategoryViewController.LevelCategory levelCategory;
    public string localizedKey;
    public Sprite categoryIcon;
  }
}
