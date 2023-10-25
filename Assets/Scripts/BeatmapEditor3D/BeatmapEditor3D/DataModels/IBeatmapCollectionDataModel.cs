// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.IBeatmapCollectionDataModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.DataModels
{
  public interface IBeatmapCollectionDataModel : IReadonlyBeatmapCollectionDataModel
  {
    void RefreshCollection();

    void AddNewBeatmap(
      string songName,
      string customBeatmap,
      string coverImagePath,
      string songPath,
      float bpm,
      bool shouldOpen);
  }
}
