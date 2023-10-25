// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapProjectManager
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor;
using BeatmapEditor3D.Logging;
using BeatmapEditor3D.SerializedData;
using BeatmapSaveDataVersion3;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.DataModels
{
  public class BeatmapProjectManager
  {
    [Inject]
    private readonly BeatmapEditorLogger _logger;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly IBeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly IBeatmapEventsDataModel _beatmapEventsDataModel;
    [Inject]
    private readonly BpmEditorDataModel _bpmEditorDataModel;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly BeatmapEditorSettingsLoader _beatmapEditorSettingsLoader;
    [Inject]
    private readonly BeatmapEditorSettingsSaver _beatmapEditorSettingsSaver;
    [Inject]
    private readonly BeatmapLevelDataModelVersionedLoader _beatmapLevelDataModelLoader;
    [Inject]
    private readonly BeatmapLevelDataModelSaver _beatmapLevelDataModelSaver;
    [Inject]
    private readonly BeatmapDataModelVersionedLoader _beatmapDataModelLoader;
    [Inject]
    private readonly BeatmapDataModelSaver _beatmapDataModelSaver;
    [Inject]
    private readonly BeatmapBpmDataVersionedLoader _beatmapBpmDataLoader;
    [Inject]
    private readonly BeatmapBpmDataSaver _beatmapBpmDataSaver;
    [Inject]
    private readonly BeatmapState _beatmapState;
    [Inject]
    private readonly IBasicEventsState _basicEventsState;
    private bool _projectOpened;
    private string _originalBeatmapProject;
    private string _workingBeatmapProject;
    private string _lastBackup;
    private Queue<string> _projectBackups = new Queue<string>();

    public string workingBeatmapProject => this._workingBeatmapProject;

    public void LoadBeatmapEditorSettings() => this._beatmapEditorSettingsLoader.Load(BeatmapFileUtils.beatmapEditorSettingsFilePath);

    public void SaveBeatmapEditorSettings() => BeatmapProjectFileHelper.SaveSettings(this._beatmapEditorSettingsSaver.Save());

    public bool ProjectTempExists(string projectPath) => BeatmapProjectFileHelper.ProjectTempExists(projectPath);

    public void LoadBeatmapProject(string projectPath, bool openTempPath)
    {
      if (this._projectOpened)
      {
        this._logger.Log(UnityEngine.LogType.Warning, (object) ("Project " + projectPath + " already loaded"));
      }
      else
      {
        this._originalBeatmapProject = projectPath;
        if (!openTempPath)
          BeatmapProjectFileHelper.CopyProjectToTemp(this._originalBeatmapProject, out this._workingBeatmapProject);
        else
          this._workingBeatmapProject = BeatmapFileUtils.GetTempBeatmapDirectoryPath(this._originalBeatmapProject);
        BeatmapProjectFileHelper.LoadProjectBackups(this._originalBeatmapProject, ref this._projectBackups, ref this._lastBackup);
        this._beatmapBpmDataLoader.Load(this._workingBeatmapProject);
        this._beatmapDataModelLoader.Load(this._workingBeatmapProject);
        this._projectOpened = true;
      }
    }

    public void LoadBeatmapProjectFromLastSave()
    {
      if (!this._projectOpened)
      {
        this._logger.Log(UnityEngine.LogType.Warning, (object) ("Project " + this._workingBeatmapProject + " not loaded"));
      }
      else
      {
        StandardLevelInfoSaveData levelInfoSaveData = BeatmapProjectFileHelper.LoadProjectInfo(this._originalBeatmapProject);
        string filename1 = BeatmapProjectFileHelper.GetCorrectedPathAndFilename(this._workingBeatmapProject, levelInfoSaveData.songFilename).filename;
        string filename2 = BeatmapProjectFileHelper.GetCorrectedPathAndFilename(this._workingBeatmapProject, levelInfoSaveData.coverImageFilename).filename;
        if (BeatmapProjectFileHelper.FileExists(this._workingBeatmapProject, this._beatmapDataModel.songFilename))
          BeatmapProjectFileHelper.DeleteBeatmapFile(this._workingBeatmapProject, this._beatmapDataModel.songFilename);
        if (BeatmapProjectFileHelper.FileExists(this._workingBeatmapProject, this._beatmapDataModel.coverImageFilename))
          BeatmapProjectFileHelper.DeleteBeatmapFile(this._workingBeatmapProject, this._beatmapDataModel.coverImageFilename);
        BeatmapProjectFileHelper.CopyProjectFile(this._originalBeatmapProject, this._workingBeatmapProject);
        BeatmapProjectFileHelper.CopyBeatmapFile(this._originalBeatmapProject, this._workingBeatmapProject, filename1);
        if (!string.IsNullOrEmpty(filename2))
          BeatmapProjectFileHelper.CopyBeatmapFile(this._originalBeatmapProject, this._workingBeatmapProject, filename2);
        this._beatmapDataModelLoader.Load(this._workingBeatmapProject);
      }
    }

    public void SaveBeatmapProject(bool clearDirty)
    {
      if (!this._projectOpened)
      {
        this._logger.Log(UnityEngine.LogType.Warning, (object) "Project not loaded");
      }
      else
      {
        BeatmapProjectFileHelper.SaveProjectInfo(this._workingBeatmapProject, this._beatmapDataModelSaver.Save());
        BeatmapProjectFileHelper.SaveBpmInfo(this._workingBeatmapProject, this._beatmapBpmDataSaver.SaveFromBeatmapDataModel());
        this._logger.Log((object) "BeatmapProjectManager - SaveBeatmapProject");
        if (!clearDirty)
          return;
        this.BackupProject();
        this.SaveTempProject();
      }
    }

    public void SaveBeatmapSong(string newSongFilePath, AudioClip audioClip)
    {
      (string songFilePath, string songFilename) = BeatmapProjectFileHelper.SaveBeatmapFile(this._beatmapDataModel.songFilePath, this._workingBeatmapProject, newSongFilePath);
      if (songFilePath == null || songFilename == null)
        return;
      this._beatmapDataModel.UpdateSong(songFilePath, songFilename, audioClip);
      this._logger.Log((object) "BeatmapProjectManager - SaveBeatmapSong");
    }

    public void SaveBeatmapCoverImage(string newCoverFilePath, Texture2D coverImage)
    {
      (string coverImageFilename, string coverImageFilePath) = BeatmapProjectFileHelper.SaveBeatmapFile(this._beatmapDataModel.coverImageFilePath, this._workingBeatmapProject, newCoverFilePath);
      if (coverImageFilename == null || coverImageFilePath == null)
        return;
      this._beatmapDataModel.UpdateCoverImage(coverImageFilePath, coverImageFilename, coverImage);
      this._logger.Log((object) "BeatmapProjectManager - SaveBeatmapCoverImage");
    }

    public void CloseBeatmapProject()
    {
      BeatmapProjectFileHelper.DeleteTempDirectory(this._workingBeatmapProject);
      this._projectOpened = false;
      this._projectBackups.Clear();
      this._originalBeatmapProject = (string) null;
      this._workingBeatmapProject = (string) null;
      this._beatmapDataModel.Close();
    }

    public void SaveBpmInfo(bool clearDirty)
    {
      if (!this._projectOpened)
      {
        this._logger.Log(UnityEngine.LogType.Warning, (object) "Project not loaded");
      }
      else
      {
        BeatmapProjectFileHelper.SaveBpmInfo(this._workingBeatmapProject, this._beatmapBpmDataSaver.SaveFromBpmDataModel());
        this._logger.Log((object) "BeatmapProjectManager - SaveBpmInfo");
        if (!clearDirty)
          return;
        this._bpmEditorDataModel.ClearDirty();
        this.BackupProject();
        this.SaveTempProject();
      }
    }

    public bool LoadBeatmapLevel(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      IDifficultyBeatmapSetData difficultyBeatmapSetData;
      IDifficultyBeatmapData difficultyBeatmapData;
      if (!this._projectOpened || !this._beatmapDataModel.difficultyBeatmapSets.TryGetValue(beatmapCharacteristic, out difficultyBeatmapSetData) || !difficultyBeatmapSetData.difficultyBeatmaps.TryGetValue(beatmapDifficulty, out difficultyBeatmapData))
        return false;
      this._beatmapLevelDataModelLoader.LoadToDataModel(beatmapCharacteristic, beatmapDifficulty, this._workingBeatmapProject, difficultyBeatmapData.beatmapFilename);
      return true;
    }

    public void SaveBeatmapLevel(bool clearDirty)
    {
      IDifficultyBeatmapSetData difficultyBeatmapSetData;
      IDifficultyBeatmapData difficultyBeatmapData;
      if (!this._projectOpened || !this._beatmapDataModel.difficultyBeatmapSets.TryGetValue(this._beatmapLevelDataModel.beatmapCharacteristic, out difficultyBeatmapSetData) || !difficultyBeatmapSetData.difficultyBeatmaps.TryGetValue(this._beatmapLevelDataModel.beatmapDifficulty, out difficultyBeatmapData) || !this._beatmapLevelDataModelSaver.NeedsSaving())
        return;
      BeatmapSaveData beatmapSaveData = this._beatmapLevelDataModelSaver.Save();
      BeatmapProjectFileHelper.SaveBeatmapLevel(this._workingBeatmapProject, difficultyBeatmapData.beatmapFilename, beatmapSaveData);
      if (!clearDirty)
        return;
      this._beatmapLevelDataModel.ClearDirty();
      this._beatmapEventsDataModel.ClearDirty();
      this._beatmapEventBoxGroupsDataModel.ClearDirty();
      this.BackupProject();
      this.SaveTempProject();
    }

    public void SaveEmptyBeatmapLevel(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      IDifficultyBeatmapSetData difficultyBeatmapSetData;
      IDifficultyBeatmapData difficultyBeatmapData;
      if (!this._projectOpened || !this._beatmapDataModel.difficultyBeatmapSets.TryGetValue(beatmapCharacteristic, out difficultyBeatmapSetData) || !difficultyBeatmapSetData.difficultyBeatmaps.TryGetValue(beatmapDifficulty, out difficultyBeatmapData))
        return;
      BeatmapProjectFileHelper.SaveBeatmapLevel(this._workingBeatmapProject, difficultyBeatmapData.beatmapFilename, new BeatmapSaveData(new List<BeatmapSaveData.BpmChangeEventData>(), new List<BeatmapSaveData.RotationEventData>(), new List<BeatmapSaveData.ColorNoteData>(), new List<BeatmapSaveData.BombNoteData>(), new List<BeatmapSaveData.ObstacleData>(), new List<BeatmapSaveData.SliderData>(), new List<BeatmapSaveData.BurstSliderData>(), new List<BeatmapSaveData.WaypointData>(), new List<BeatmapSaveData.BasicEventData>(), new List<BeatmapSaveData.ColorBoostEventData>(), new List<BeatmapSaveData.LightColorEventBoxGroup>(), new List<BeatmapSaveData.LightRotationEventBoxGroup>(), new List<BeatmapSaveData.LightTranslationEventBoxGroup>(), new BeatmapSaveData.BasicEventTypesWithKeywords(new List<BeatmapSaveData.BasicEventTypesWithKeywords.BasicEventTypesForKeyword>()), false));
    }

    public void RemoveBeatmapLevel(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      IDifficultyBeatmapSetData difficultyBeatmapSetData;
      IDifficultyBeatmapData difficultyBeatmapData;
      if (!this._projectOpened || !this._beatmapDataModel.difficultyBeatmapSets.TryGetValue(beatmapCharacteristic, out difficultyBeatmapSetData) || !difficultyBeatmapSetData.difficultyBeatmaps.TryGetValue(beatmapDifficulty, out difficultyBeatmapData))
        return;
      BeatmapProjectFileHelper.DeleteBeatmapLevel(this._workingBeatmapProject, difficultyBeatmapData.beatmapFilename);
    }

    public void ReplaceBeatmapLevelFromLatestSave(
      BeatmapCharacteristicSO beatmapCharacteristic,
      BeatmapDifficulty beatmapDifficulty)
    {
      IDifficultyBeatmapSetData difficultyBeatmapSetData;
      IDifficultyBeatmapData difficultyBeatmapData;
      if (!this._projectOpened || !this._beatmapDataModel.difficultyBeatmapSets.TryGetValue(beatmapCharacteristic, out difficultyBeatmapSetData) || !difficultyBeatmapSetData.difficultyBeatmaps.TryGetValue(beatmapDifficulty, out difficultyBeatmapData))
        return;
      BeatmapProjectFileHelper.CopyBeatmapLevel(this._originalBeatmapProject, difficultyBeatmapData.beatmapFilename, this._workingBeatmapProject, difficultyBeatmapData.beatmapFilename);
    }

    public void CloseBeatmapLevel()
    {
      if (!this._projectOpened)
        return;
      this._beatmapState.beat = 0.0f;
      this._beatmapState.rotation = 0;
      this._basicEventsState.activeEventTypes = this._basicEventsState.allEventTypes;
      this._basicEventsState.currentHoverBeat = 0.0f;
      this._beatmapLevelDataModel.Clear();
      this._beatmapEventsDataModel.Clear();
      this._beatmapEventBoxGroupsDataModel.Clear();
    }

    private void BackupProject()
    {
      if (this._projectBackups.Count == 10)
        BeatmapProjectFileHelper.DeleteBackup(this._projectBackups.Dequeue());
      BeatmapProjectFileHelper.BackupProject(this._originalBeatmapProject, ref this._projectBackups, out this._lastBackup);
    }

    private void SaveTempProject() => BeatmapProjectFileHelper.CopyTempToProject(this._workingBeatmapProject, this._originalBeatmapProject);
  }
}
