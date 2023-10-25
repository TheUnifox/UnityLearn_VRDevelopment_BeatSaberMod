// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapsCollectionSignals
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using Zenject;

namespace BeatmapEditor3D.DataModels
{
  public class BeatmapsCollectionSignals
  {
    public class AddNewBeatmapSignal
    {
      public string songName { get; }

      public string customBeatmapName { get; }

      public string coverImagePath { get; }

      public string songPath { get; }

      public float bpm { get; }

      public bool shouldOpen { get; }

      public AddNewBeatmapSignal(
        string songName,
        string customBeatmapName,
        string coverImagePath,
        string songPath,
        float bpm,
        bool shouldOpen)
      {
        this.songName = songName;
        this.customBeatmapName = customBeatmapName;
        this.coverImagePath = coverImagePath;
        this.songPath = songPath;
        this.bpm = bpm;
        this.shouldOpen = shouldOpen;
      }
    }

    public class RefreshSignal
    {
    }

    public class UpdatedSignal
    {
    }

    public class AddRecentlyOpenedBeatmapSignal
    {
      public string beatmapFolderPath;
    }

    public class BeatmapAddedSignal
    {
      public IBeatmapInfoData beatmapInfoData { get; }

      public bool shouldOpen { get; }

      public BeatmapAddedSignal(IBeatmapInfoData beatmapInfoData, bool shouldOpen)
      {
        this.beatmapInfoData = beatmapInfoData;
        this.shouldOpen = shouldOpen;
      }
    }

    public class AddNewBeatmapCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapsCollectionSignals.AddNewBeatmapSignal _signal;
      [Inject]
      private readonly IBeatmapCollectionDataModel _beatmapCollectionDataModel;

      public void Execute() => this._beatmapCollectionDataModel.AddNewBeatmap(this._signal.songName, this._signal.customBeatmapName, this._signal.coverImagePath, this._signal.songPath, this._signal.bpm, this._signal.shouldOpen);
    }

    public class RefreshCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly IBeatmapCollectionDataModel _beatmapCollectionDataModel;

      public void Execute() => this._beatmapCollectionDataModel.RefreshCollection();
    }

    public class AddRecentlyOpenedBeatmapCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapsCollectionSignals.AddRecentlyOpenedBeatmapSignal _signal;
      [Inject]
      private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;

      public void Execute() => this._beatmapEditorSettingsDataModel.AddRecentlyOpenedBeatmap(this._signal.beatmapFolderPath);
    }
  }
}
