// Decompiled with JetBrains decompiler
// Type: PlayingDifficultyBeatmapRichPresenceData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System.Runtime.CompilerServices;
using System.Text;

public class PlayingDifficultyBeatmapRichPresenceData : IRichPresenceData
{
  [CompilerGenerated]
  protected string m_CapiName;
  [CompilerGenerated]
  protected string m_ClocalizedDescription;

  public string apiName
  {
    get => this.m_CapiName;
    private set => this.m_CapiName = value;
  }

  public string localizedDescription
  {
    get => this.m_ClocalizedDescription;
    private set => this.m_ClocalizedDescription = value;
  }

  public PlayingDifficultyBeatmapRichPresenceData(IDifficultyBeatmap difficultyBeatmap)
  {
    this.apiName = difficultyBeatmap.SerializedName();
    this.localizedDescription = PlayingDifficultyBeatmapRichPresenceData.GetDestinationLocalizedString(difficultyBeatmap);
  }

  public static string GetDestinationLocalizedString(IDifficultyBeatmap difficultyBeatmap)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(difficultyBeatmap.level.songName);
    if (difficultyBeatmap.parentDifficultyBeatmapSet != null && !string.IsNullOrEmpty(difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.compoundIdPartName))
    {
      stringBuilder.Append(" [");
      stringBuilder.Append(Localization.Get(difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.characteristicNameLocalizationKey));
      stringBuilder.Append("]");
    }
    stringBuilder.Append(" [");
    stringBuilder.Append(difficultyBeatmap.difficulty.Name());
    stringBuilder.Append("]");
    return stringBuilder.ToString();
  }
}
