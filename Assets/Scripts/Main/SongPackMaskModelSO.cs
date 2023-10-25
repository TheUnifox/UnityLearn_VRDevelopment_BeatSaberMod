// Decompiled with JetBrains decompiler
// Type: SongPackMaskModelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SongPackMaskModelSO : PersistentScriptableObject
{
  [SerializeField]
  protected List<string> _defaultSongPackMaskItems;
  [SerializeField]
  protected SongPackMaskModelSO.SongPackMaskItem[] _customSongPackMaskItems;
  [Space]
  [SerializeField]
  protected BeatmapLevelPackCollectionSO _ostAndExtrasCollection;
  [SerializeField]
  protected BeatmapLevelPackCollectionSO _dlcCollection;
  protected Language _currentLocalizedLanguage;
  protected Dictionary<string, (string name, bool plural)> _songPackSerializedNameToLocalizedNameDict;
  protected Dictionary<string, SongPackMask> _songPackSerializedNameToMaskDict;
  protected Dictionary<SongPackMask, string> _songPackMaskToSerializedNameDict;

  public List<string> defaultSongPackMaskItems => this._defaultSongPackMaskItems;

  public SongPackMaskModelSO.SongPackMaskItem[] customSongPackMaskItems => this._customSongPackMaskItems;

  public BeatmapLevelPackCollectionSO ostAndExtrasCollection => this._ostAndExtrasCollection;

  public BeatmapLevelPackCollectionSO dlcCollection => this._dlcCollection;

  public virtual string ToLocalizedName(string serializedName) => this.ToLocalizedName(serializedName, out bool _);

  public virtual string ToLocalizedName(string serializedName, out bool plural)
  {
    this.LazyInit();
    plural = false;
    (string name, bool plural) tuple;
    if (!this._songPackSerializedNameToLocalizedNameDict.TryGetValue(serializedName, out tuple))
      return string.Empty;
    plural = tuple.plural;
    return tuple.name;
  }

  public virtual bool ToSongPackMask(string serializedName, out SongPackMask songPackMask)
  {
    this.LazyInit();
    return this._songPackSerializedNameToMaskDict.TryGetValue(serializedName, out songPackMask);
  }

  public virtual SongPackMask ToSongPackMask(string serializedName)
  {
    SongPackMask songPackMask;
    return !this.ToSongPackMask(serializedName, out songPackMask) ? SongPackMask.all : songPackMask;
  }

  public virtual bool ToSerializedName(SongPackMask songPackMask, out string serializedName)
  {
    this.LazyInit();
    return this._songPackMaskToSerializedNameDict.TryGetValue(songPackMask, out serializedName);
  }

  public virtual string ToSerializedName(SongPackMask songPackMask)
  {
    string serializedName;
    return !this.ToSerializedName(songPackMask, out serializedName) ? "" : serializedName;
  }

  public virtual void LazyInit()
  {
    if (this._songPackMaskToSerializedNameDict != null && this._songPackSerializedNameToMaskDict != null && this._songPackSerializedNameToLocalizedNameDict != null && this._currentLocalizedLanguage == Localization.Instance.SelectedLanguage)
      return;
    this._currentLocalizedLanguage = Localization.Instance.SelectedLanguage;
    this._songPackSerializedNameToLocalizedNameDict = new Dictionary<string, (string, bool)>(this._customSongPackMaskItems.Length + this._dlcCollection.beatmapLevelPacks.Length);
    this._songPackSerializedNameToMaskDict = new Dictionary<string, SongPackMask>(this._customSongPackMaskItems.Length + this._dlcCollection.beatmapLevelPacks.Length);
    this._songPackMaskToSerializedNameDict = new Dictionary<SongPackMask, string>(this._customSongPackMaskItems.Length + this._dlcCollection.beatmapLevelPacks.Length);
    foreach (SongPackMaskModelSO.SongPackMaskItem songPackMaskItem in this._customSongPackMaskItems)
    {
      string serializedName = songPackMaskItem.serializedName;
      SongPackMask songPackMask = songPackMaskItem.songPackMask;
      this._songPackSerializedNameToLocalizedNameDict.Add(serializedName, (Localization.Get(serializedName), songPackMaskItem.containsMultiplePacks));
      this._songPackSerializedNameToMaskDict.Add(serializedName, songPackMask);
      this._songPackMaskToSerializedNameDict.Add(songPackMask, serializedName);
    }
    foreach (IBeatmapLevelPack beatmapLevelPack in this._ostAndExtrasCollection.beatmapLevelPacks)
    {
      string packId = beatmapLevelPack.packID;
      SongPackMask key = new SongPackMask(beatmapLevelPack.packID);
      this._songPackSerializedNameToLocalizedNameDict.Add(packId, (beatmapLevelPack.packName, false));
      this._songPackSerializedNameToMaskDict.Add(packId, key);
      this._songPackMaskToSerializedNameDict.Add(key, packId);
    }
    foreach (IBeatmapLevelPack beatmapLevelPack in this._dlcCollection.beatmapLevelPacks)
    {
      string packId = beatmapLevelPack.packID;
      SongPackMask key = new SongPackMask(beatmapLevelPack.packID);
      this._songPackSerializedNameToLocalizedNameDict.Add(packId, (beatmapLevelPack.packName, false));
      this._songPackSerializedNameToMaskDict.Add(packId, key);
      this._songPackMaskToSerializedNameDict.Add(key, packId);
    }
  }

  public enum SongPackDataType
  {
    SingleBeatmapLevelPack,
    MultipleBeatmapLevelPacks,
    SinglePreviewBeatmapLevelPack,
    MultiplePreviewBeatmapLevelPacks,
    SingleBeatmapLevelPackCollection,
    MultipleBeatmapLevelPackCollections,
  }

  [Serializable]
  public class SongPackMaskItem
  {
    public SongPackMaskModelSO.SongPackDataType _type;
    [NullAllowed]
    public BeatmapLevelPackSO _beatmapLevelPack;
    [NullAllowed]
    public PreviewBeatmapLevelPackSO _previewBeatmapLevelPack;
    public string _serializedName;
    [NullAllowed]
    public BeatmapLevelPackCollectionSO _levelPackCollection;
    public List<BeatmapLevelPackSO> _beatmapLevelPacks;
    public List<PreviewBeatmapLevelPackSO> _previewBeatmapLevelPacks;
    public List<BeatmapLevelPackCollectionSO> _levelPackCollections;

    public string serializedName
    {
      get
      {
        switch (this._type)
        {
          case SongPackMaskModelSO.SongPackDataType.SingleBeatmapLevelPack:
            return this._beatmapLevelPack.packID;
          case SongPackMaskModelSO.SongPackDataType.SinglePreviewBeatmapLevelPack:
            return this._previewBeatmapLevelPack.packID;
          default:
            return this._serializedName;
        }
      }
    }

    public SongPackMask songPackMask
    {
      get
      {
        switch (this._type)
        {
          case SongPackMaskModelSO.SongPackDataType.SingleBeatmapLevelPack:
            return new SongPackMask(this._beatmapLevelPack.packID);
          case SongPackMaskModelSO.SongPackDataType.MultipleBeatmapLevelPacks:
            return new SongPackMask(this._beatmapLevelPacks.Select<BeatmapLevelPackSO, string>((Func<BeatmapLevelPackSO, string>) (pack => pack.packID)));
          case SongPackMaskModelSO.SongPackDataType.SinglePreviewBeatmapLevelPack:
            return new SongPackMask(this._previewBeatmapLevelPack.packID);
          case SongPackMaskModelSO.SongPackDataType.MultiplePreviewBeatmapLevelPacks:
            return new SongPackMask(this._previewBeatmapLevelPacks.Select<PreviewBeatmapLevelPackSO, string>((Func<PreviewBeatmapLevelPackSO, string>) (pack => pack.packID)));
          case SongPackMaskModelSO.SongPackDataType.SingleBeatmapLevelPackCollection:
            return new SongPackMask(((IEnumerable<IBeatmapLevelPack>) this._levelPackCollection.beatmapLevelPacks).Select<IBeatmapLevelPack, string>((Func<IBeatmapLevelPack, string>) (pack => pack.packID)));
          case SongPackMaskModelSO.SongPackDataType.MultipleBeatmapLevelPackCollections:
            return new SongPackMask(this._levelPackCollections.SelectMany<BeatmapLevelPackCollectionSO, string>((Func<BeatmapLevelPackCollectionSO, IEnumerable<string>>) (collection => ((IEnumerable<IBeatmapLevelPack>) collection.beatmapLevelPacks).Select<IBeatmapLevelPack, string>((Func<IBeatmapLevelPack, string>) (pack => pack.packID)))));
          default:
            return SongPackMask.all;
        }
      }
    }

    public bool containsMultiplePacks
    {
      get
      {
        switch (this._type)
        {
          case SongPackMaskModelSO.SongPackDataType.SingleBeatmapLevelPack:
          case SongPackMaskModelSO.SongPackDataType.SinglePreviewBeatmapLevelPack:
            return false;
          default:
            return true;
        }
      }
    }
  }
}
