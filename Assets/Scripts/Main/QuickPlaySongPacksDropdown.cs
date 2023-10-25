// Decompiled with JetBrains decompiler
// Type: QuickPlaySongPacksDropdown
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class QuickPlaySongPacksDropdown : MonoBehaviour
{
  [SerializeField]
  protected SimpleTextDropdown _simpleTextDropdown;
  [Space]
  [SerializeField]
  protected SongPackMaskModelSO _songPackMaskModel;
  protected bool _initialized;
  protected QuickPlaySetupData.QuickPlaySongPacksOverride _quickPlaySongPacksOverride;
  protected List<QuickPlaySongPacksDropdown.SongPackMaskItem> _data;

  public event System.Action<int> didSelectCellWithIdxEvent;

  public virtual void Start() => this._simpleTextDropdown.didSelectCellWithIdxEvent += new System.Action<DropdownWithTableView, int>(this.HandleSimpleTextDropdownDidSelectCellWithIdx);

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._simpleTextDropdown != (UnityEngine.Object) null))
      return;
    this._simpleTextDropdown.didSelectCellWithIdxEvent -= new System.Action<DropdownWithTableView, int>(this.HandleSimpleTextDropdownDidSelectCellWithIdx);
  }

  public virtual void SetOverrideSongPacks(
    QuickPlaySetupData.QuickPlaySongPacksOverride quickPlaySongPacksOverride)
  {
    this._quickPlaySongPacksOverride = quickPlaySongPacksOverride;
  }

  public virtual string GetSelectedSerializedName()
  {
    this.LazyInit();
    return this._data[this._simpleTextDropdown.selectedIndex].serializedName;
  }

  public virtual void SelectCellWithSerializedName(string serializedName)
  {
    this.LazyInit();
    this._simpleTextDropdown.SelectCellWithIdx(Mathf.Clamp(this._data.IndexOf(this._data.FirstOrDefault<QuickPlaySongPacksDropdown.SongPackMaskItem>((Func<QuickPlaySongPacksDropdown.SongPackMaskItem, bool>) (item => item.serializedName == serializedName))), 0, this._data.Count - 1));
  }

  public virtual void HandleSimpleTextDropdownDidSelectCellWithIdx(
    DropdownWithTableView dropdownWithTableView,
    int idx)
  {
    System.Action<int> cellWithIdxEvent = this.didSelectCellWithIdxEvent;
    if (cellWithIdxEvent == null)
      return;
    cellWithIdxEvent(idx);
  }

  public virtual void LazyInit()
  {
    if (this._initialized)
      return;
    this._initialized = true;
    if (this._quickPlaySongPacksOverride == null || this._quickPlaySongPacksOverride.predefinedPackIds.Count == 0)
    {
      this._data = this._songPackMaskModel.defaultSongPackMaskItems.Select<string, QuickPlaySongPacksDropdown.SongPackMaskItem>((Func<string, QuickPlaySongPacksDropdown.SongPackMaskItem>) (serializedName => new QuickPlaySongPacksDropdown.SongPackMaskItem()
      {
        serializedName = serializedName,
        localizedName = Localization.Get(serializedName),
        songPackMask = this._songPackMaskModel.ToSongPackMask(serializedName)
      })).ToList<QuickPlaySongPacksDropdown.SongPackMaskItem>();
    }
    else
    {
      this._data = new List<QuickPlaySongPacksDropdown.SongPackMaskItem>();
      this._data.AddRange(this._quickPlaySongPacksOverride.predefinedPackIds.Select<QuickPlaySetupData.QuickPlaySongPacksOverride.PredefinedPack, QuickPlaySongPacksDropdown.SongPackMaskItem>((Func<QuickPlaySetupData.QuickPlaySongPacksOverride.PredefinedPack, QuickPlaySongPacksDropdown.SongPackMaskItem>) (pack => new QuickPlaySongPacksDropdown.SongPackMaskItem()
      {
        serializedName = pack.packId,
        localizedName = Localization.Get(pack.packId),
        order = pack.order,
        songPackMask = this._songPackMaskModel.ToSongPackMask(pack.packId)
      })));
      this._data.AddRange(this._quickPlaySongPacksOverride.localizedCustomPacks.Select<QuickPlaySetupData.QuickPlaySongPacksOverride.LocalizedCustomPack, QuickPlaySongPacksDropdown.SongPackMaskItem>((Func<QuickPlaySetupData.QuickPlaySongPacksOverride.LocalizedCustomPack, QuickPlaySongPacksDropdown.SongPackMaskItem>) (localizedPack => new QuickPlaySongPacksDropdown.SongPackMaskItem()
      {
        serializedName = localizedPack.serializedName,
        localizedName = ((IEnumerable<QuickPlaySetupData.QuickPlaySongPacksOverride.LocalizedCustomPackName>) localizedPack.localizedNames).FirstOrDefault<QuickPlaySetupData.QuickPlaySongPacksOverride.LocalizedCustomPackName>((Func<QuickPlaySetupData.QuickPlaySongPacksOverride.LocalizedCustomPackName, bool>) (packName => packName.language.ToLanguage() == Localization.Instance.SelectedLanguage))?.packName ?? "",
        order = localizedPack.order,
        songPackMask = new SongPackMask((IEnumerable<string>) localizedPack.packIds)
      })));
    }
    this._simpleTextDropdown.SetTexts((IReadOnlyList<string>) this._data.Select<QuickPlaySongPacksDropdown.SongPackMaskItem, string>((Func<QuickPlaySongPacksDropdown.SongPackMaskItem, string>) (item => item.localizedName)).ToArray<string>());
  }

  [CompilerGenerated]
  public virtual QuickPlaySongPacksDropdown.SongPackMaskItem m_CLazyInitm_Eb__15_0(
    string serializedName)
  {
    return new QuickPlaySongPacksDropdown.SongPackMaskItem()
    {
      serializedName = serializedName,
      localizedName = Localization.Get(serializedName),
      songPackMask = this._songPackMaskModel.ToSongPackMask(serializedName)
    };
  }

  [CompilerGenerated]
  public virtual QuickPlaySongPacksDropdown.SongPackMaskItem m_CLazyInitm_Eb__15_1(
    QuickPlaySetupData.QuickPlaySongPacksOverride.PredefinedPack pack)
  {
    return new QuickPlaySongPacksDropdown.SongPackMaskItem()
    {
      serializedName = pack.packId,
      localizedName = Localization.Get(pack.packId),
      order = pack.order,
      songPackMask = this._songPackMaskModel.ToSongPackMask(pack.packId)
    };
  }

  public class SongPackMaskItem
  {
    public string serializedName;
    public string localizedName;
    public int order;
    public SongPackMask songPackMask;
  }
}
