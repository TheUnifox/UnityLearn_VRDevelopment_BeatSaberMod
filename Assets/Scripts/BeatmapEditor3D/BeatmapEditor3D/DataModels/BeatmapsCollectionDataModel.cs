// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapsCollectionDataModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D.DataModels
{
  public class BeatmapsCollectionDataModel : 
    IBeatmapCollectionDataModel,
    IReadonlyBeatmapCollectionDataModel
  {
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject(Id = "Normal")]
    private readonly EnvironmentTypeSO _normalEnvironmentType;
    [Inject(Id = "Circle")]
    private readonly EnvironmentTypeSO _circleEnvironmentType;
    [Inject]
    private readonly EnvironmentsListSO _environmentsList;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    private List<BeatmapsCollectionDataModel.BeatmapInfoData> _beatmapInfos = new List<BeatmapsCollectionDataModel.BeatmapInfoData>();

    public IReadOnlyList<IBeatmapInfoData> beatmapInfos => (IReadOnlyList<IBeatmapInfoData>) this._beatmapInfos;

    public void RefreshCollection()
    {
      if (!Directory.Exists(this._beatmapEditorSettingsDataModel.customLevelsFolder))
        return;
      this._beatmapInfos = ((IEnumerable<string>) BeatmapProjectFileHelper.GetProjectDirectories(this._beatmapEditorSettingsDataModel.customLevelsFolder)).Select<string, BeatmapsCollectionDataModel.BeatmapInfoData>((Func<string, BeatmapsCollectionDataModel.BeatmapInfoData>) (projectDirectoryPath => new BeatmapsCollectionDataModel.BeatmapInfoData(FileHelpers.LoadFromJSONFile<StandardLevelInfoSaveData>(Path.Combine(projectDirectoryPath, "Info.dat")), projectDirectoryPath, this.GenerateRelativePath(projectDirectoryPath), this.GetLastModifiedDateTime(Path.Combine(projectDirectoryPath, "Info.dat"))))).ToList<BeatmapsCollectionDataModel.BeatmapInfoData>();
      this.SortBeatmaps();
      this._signalBus.Fire<BeatmapsCollectionSignals.UpdatedSignal>();
    }

    public void AddNewBeatmap(
      string songName,
      string customBeatmap,
      string coverImagePath,
      string songPath,
      float bpm,
      bool shouldOpen)
    {
      string safeDirectoryPath = BeatmapProjectFileHelper.CreateSafeDirectoryPath(this._beatmapEditorSettingsDataModel.customLevelsFolder, string.IsNullOrEmpty(customBeatmap) ? songName : customBeatmap);
      if (!File.Exists(songPath))
        return;
      Directory.CreateDirectory(safeDirectoryPath);
      string songFilename = BeatmapProjectFileHelper.SaveBeatmapFile((string) null, safeDirectoryPath, songPath).Item1;
      string coverImageFilename = BeatmapProjectFileHelper.SaveBeatmapFile((string) null, safeDirectoryPath, coverImagePath).Item1;
      StandardLevelInfoSaveData info = new StandardLevelInfoSaveData(songName, "", "", "", bpm, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, songFilename, coverImageFilename, this._environmentsList.GetLastEnvironmentInfoWithType(this._normalEnvironmentType)?.serializedName ?? "", this._environmentsList.GetLastEnvironmentInfoWithType(this._circleEnvironmentType)?.serializedName ?? "", (StandardLevelInfoSaveData.DifficultyBeatmapSet[]) null);
      BeatmapProjectFileHelper.SaveProjectInfo(safeDirectoryPath, info);
      BeatmapsCollectionDataModel.BeatmapInfoData beatmapInfoData = new BeatmapsCollectionDataModel.BeatmapInfoData(info, safeDirectoryPath, this.GenerateRelativePath(safeDirectoryPath), DateTime.Now);
      this._beatmapInfos.Add(beatmapInfoData);
      this.SortBeatmaps();
      this._signalBus.Fire<BeatmapsCollectionSignals.BeatmapAddedSignal>(new BeatmapsCollectionSignals.BeatmapAddedSignal((IBeatmapInfoData) beatmapInfoData, shouldOpen));
    }

    private DateTime GetLastModifiedDateTime(string path)
    {
      try
      {
        return Directory.GetLastWriteTime(path);
      }
      catch (Exception)
      {
        return new DateTime();
      }
    }

    private void SortBeatmaps() => this._beatmapInfos.Sort((Comparison<BeatmapsCollectionDataModel.BeatmapInfoData>) ((dataA, dataB) => dataB.lastModifiedTimestamp.CompareTo(dataA.lastModifiedTimestamp)));

    private string GenerateRelativePath(string projectDirectoryPath)
    {
      if (this._beatmapEditorSettingsDataModel.customLevelsFolder.Equals(projectDirectoryPath))
        return string.Empty;
      int startIndex = this._beatmapEditorSettingsDataModel.customLevelsFolder.Length + 1;
      int length1 = Path.GetFileNameWithoutExtension(projectDirectoryPath).Length;
      int length2 = projectDirectoryPath.Length;
      return projectDirectoryPath.Substring(startIndex, length2 - length1 - startIndex);
    }

    private class BeatmapInfoData : IBeatmapInfoData
    {
      public string songName { get; }

      public string songSubName { get; }

      public string songAuthorName { get; }

      public string levelAuthorName { get; }

      public string coverImagePath { get; }

      public string beatmapFolderPath { get; }

      public string relativeFolderPath { get; }

      public DateTime lastModifiedTimestamp { get; }

      public BeatmapInfoData(
        StandardLevelInfoSaveData info,
        string folderPath,
        string infoFilePath,
        DateTime modifiedTimestamp)
      {
        this.songName = info.songName;
        this.songSubName = info.songSubName;
        this.songAuthorName = info.songAuthorName;
        this.levelAuthorName = info.levelAuthorName;
        this.coverImagePath = info.coverImageFilename;
        this.beatmapFolderPath = folderPath;
        this.relativeFolderPath = Path.Combine(infoFilePath, this.songName);
        this.lastModifiedTimestamp = modifiedTimestamp;
      }
    }
  }
}
