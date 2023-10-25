// Decompiled with JetBrains decompiler
// Type: BeatmapDifficultySegmentedControlController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeatmapDifficultySegmentedControlController : MonoBehaviour
{
  [SerializeField]
  protected TextSegmentedControl _difficultySegmentedControl;
  protected readonly List<BeatmapDifficulty> _difficulties = new List<BeatmapDifficulty>(5);
  protected BeatmapDifficulty _selectedDifficulty;

  public event System.Action<BeatmapDifficultySegmentedControlController, BeatmapDifficulty> didSelectDifficultyEvent;

  public BeatmapDifficulty selectedDifficulty => this._selectedDifficulty;

  public virtual void Awake() => this._difficultySegmentedControl.didSelectCellEvent += new System.Action<SegmentedControl, int>(this.HandleDifficultySegmentedControlDidSelectCell);

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._difficultySegmentedControl != (UnityEngine.Object) null))
      return;
    this._difficultySegmentedControl.didSelectCellEvent -= new System.Action<SegmentedControl, int>(this.HandleDifficultySegmentedControlDidSelectCell);
  }

  public virtual int GetClosestDifficultyIndex(BeatmapDifficulty searchDifficulty)
  {
    int closestDifficultyIndex = -1;
    foreach (BeatmapDifficulty difficulty in this._difficulties)
    {
      if (searchDifficulty >= difficulty)
        ++closestDifficultyIndex;
      else
        break;
    }
    if (closestDifficultyIndex == -1)
      closestDifficultyIndex = 0;
    return closestDifficultyIndex;
  }

  public virtual void HandleDifficultySegmentedControlDidSelectCell(
    SegmentedControl segmentedControl,
    int cellIdx)
  {
    this._selectedDifficulty = this._difficulties[cellIdx];
    System.Action<BeatmapDifficultySegmentedControlController, BeatmapDifficulty> selectDifficultyEvent = this.didSelectDifficultyEvent;
    if (selectDifficultyEvent == null)
      return;
    selectDifficultyEvent(this, this._selectedDifficulty);
  }

  public virtual void SetData(
    IReadOnlyList<IDifficultyBeatmap> difficultyBeatmaps,
    BeatmapDifficulty selectedDifficulty)
  {
    this._difficulties.Clear();
    List<string> stringList = new List<string>(difficultyBeatmaps.Count<IDifficultyBeatmap>());
    foreach (IDifficultyBeatmap difficultyBeatmap in (IEnumerable<IDifficultyBeatmap>) difficultyBeatmaps)
    {
      stringList.Add(difficultyBeatmap.difficulty.Name());
      this._difficulties.Add(difficultyBeatmap.difficulty);
    }
    this._difficultySegmentedControl.SetTexts((IReadOnlyList<string>) stringList.ToArray());
    int closestDifficultyIndex = this.GetClosestDifficultyIndex(selectedDifficulty);
    if (closestDifficultyIndex != -1)
      this._difficultySegmentedControl.SelectCellWithNumber(closestDifficultyIndex);
    this._selectedDifficulty = this._difficulties[this._difficultySegmentedControl.selectedCellNumber];
  }
}
