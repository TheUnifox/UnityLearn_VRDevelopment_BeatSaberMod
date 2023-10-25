// Decompiled with JetBrains decompiler
// Type: BeatmapSelectionView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;

public class BeatmapSelectionView : MonoBehaviour
{
  [SerializeField]
  protected LevelBar _levelBar;
  [SerializeField]
  protected TextMeshProUGUI _noLevelText;

  public virtual void SetBeatmap(PreviewDifficultyBeatmap beatmapLevel)
  {
    this._noLevelText.enabled = beatmapLevel == (PreviewDifficultyBeatmap) null;
    this._levelBar.hide = beatmapLevel == (PreviewDifficultyBeatmap) null;
    if (!(beatmapLevel != (PreviewDifficultyBeatmap) null))
      return;
    this._levelBar.Setup(beatmapLevel.beatmapLevel, beatmapLevel.beatmapCharacteristic, beatmapLevel.beatmapDifficulty);
  }
}
