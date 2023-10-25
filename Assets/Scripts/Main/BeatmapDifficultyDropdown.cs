// Decompiled with JetBrains decompiler
// Type: BeatmapDifficultyDropdown
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

public class BeatmapDifficultyDropdown : MonoBehaviour
{
  [SerializeField]
  protected SimpleTextDropdown _simpleTextDropdown;
  protected IReadOnlyList<Tuple<BeatmapDifficultyMask, string>> _beatmapDifficultyData;
  [CompilerGenerated]
  protected bool m_CincludeAllDifficulties;

  public event System.Action<int> didSelectCellWithIdxEvent;

  private IReadOnlyList<Tuple<BeatmapDifficultyMask, string>> beatmapDifficultyData
  {
    get
    {
      if (this._beatmapDifficultyData == null)
      {
        List<Tuple<BeatmapDifficultyMask, string>> list = new List<Tuple<BeatmapDifficultyMask, string>>();
        list.Add<BeatmapDifficultyMask, string>(BeatmapDifficultyMask.Easy, BeatmapDifficulty.Easy.Name());
        list.Add<BeatmapDifficultyMask, string>(BeatmapDifficultyMask.Normal, BeatmapDifficulty.Normal.Name());
        list.Add<BeatmapDifficultyMask, string>(BeatmapDifficultyMask.Hard, BeatmapDifficulty.Hard.Name());
        list.Add<BeatmapDifficultyMask, string>(BeatmapDifficultyMask.Expert, BeatmapDifficulty.Expert.Name());
        list.Add<BeatmapDifficultyMask, string>(BeatmapDifficultyMask.ExpertPlus, BeatmapDifficulty.ExpertPlus.Name());
        List<Tuple<BeatmapDifficultyMask, string>> tupleList = list;
        if (this.includeAllDifficulties)
          tupleList.Insert(0, new Tuple<BeatmapDifficultyMask, string>(BeatmapDifficultyMask.All, Localization.Get("BEATMAP_DIFFICULTY_ALL")));
        this._beatmapDifficultyData = (IReadOnlyList<Tuple<BeatmapDifficultyMask, string>>) tupleList;
      }
      return this._beatmapDifficultyData;
    }
  }

  public bool includeAllDifficulties
  {
    get => this.m_CincludeAllDifficulties;
    set => this.m_CincludeAllDifficulties = value;
  }

  public virtual void Start()
  {
    this._simpleTextDropdown.didSelectCellWithIdxEvent += new System.Action<DropdownWithTableView, int>(this.HandleSimpleTextDropdownDidSelectCellWithIdx);
    this._simpleTextDropdown.SetTexts((IReadOnlyList<string>) this.beatmapDifficultyData.Select<Tuple<BeatmapDifficultyMask, string>, string>((Func<Tuple<BeatmapDifficultyMask, string>, string>) (x => x.Item2)).ToArray<string>());
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._simpleTextDropdown == (UnityEngine.Object) null)
      return;
    this._simpleTextDropdown.didSelectCellWithIdxEvent -= new System.Action<DropdownWithTableView, int>(this.HandleSimpleTextDropdownDidSelectCellWithIdx);
  }

  public virtual BeatmapDifficultyMask GetSelectedBeatmapDifficultyMask() => this.beatmapDifficultyData[this._simpleTextDropdown.selectedIndex].Item1;

  public virtual void SelectCellWithBeatmapDifficultyMask(
    BeatmapDifficultyMask beatmapDifficultyMask)
  {
    this._simpleTextDropdown.SelectCellWithIdx(this.GetIdxForBeatmapDifficultyMask(beatmapDifficultyMask));
  }

  public virtual int GetIdxForBeatmapDifficultyMask(BeatmapDifficultyMask beatmapDifficultyMask)
  {
    int num = this.includeAllDifficulties ? 1 : 0;
    int beatmapDifficultyMask1 = 0;
    if (beatmapDifficultyMask.Contains(BeatmapDifficulty.Easy))
      beatmapDifficultyMask1 = num;
    else if (beatmapDifficultyMask.Contains(BeatmapDifficulty.Normal))
      beatmapDifficultyMask1 = 1 + num;
    else if (beatmapDifficultyMask.Contains(BeatmapDifficulty.Hard))
      beatmapDifficultyMask1 = 2 + num;
    else if (beatmapDifficultyMask.Contains(BeatmapDifficulty.Expert))
      beatmapDifficultyMask1 = 3 + num;
    else if (beatmapDifficultyMask.Contains(BeatmapDifficulty.ExpertPlus))
      beatmapDifficultyMask1 = 4 + num;
    return beatmapDifficultyMask1;
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
}
