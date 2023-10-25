// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapLevelStarterController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.SerializedData;
using BeatmapSaveDataVersion3;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapLevelStarterController
  {
    [Inject]
    private readonly MenuTransitionsHelper _menuTransitionsHelper;
    [Inject]
    private readonly IReadonlyBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly BeatmapLevelDataModelSaver _beatmapLevelDataModelSaver;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;

    public void TestBeatmap(
      Action<BeatmapEditorStandardLevelScenesTransitionSetupDataSO, LevelCompletionResults> levelFinishedCallback)
    {
      PracticeSettings practiceSettings = PracticeSettings.defaultPracticeSettings;
      practiceSettings.startSongTime = AudioTimeHelper.SamplesToSeconds(this._beatmapDataModel.bpmData.BeatToSample(this._beatmapState.beat), this._beatmapDataModel.audioClip.frequency);
      practiceSettings.startInAdvanceAndClearNotes = false;
      practiceSettings.songSpeedMul = this._beatmapEditorSettingsDataModel.playbackSpeed;
      BeatmapSaveData beatmapSaveData = this._beatmapLevelDataModelSaver.Save();
      BeatmapLevelSO instance1 = ScriptableObject.CreateInstance<BeatmapLevelSO>();
      BeatmapDataSO instance2 = ScriptableObject.CreateInstance<BeatmapDataSO>();
      instance2.SetJsonData(beatmapSaveData.SerializeToJSONString());
      IDifficultyBeatmapData difficultyBeatmap = this._beatmapDataModel.difficultyBeatmapSets[this._beatmapLevelDataModel.beatmapCharacteristic].difficultyBeatmaps[this._beatmapLevelDataModel.beatmapDifficulty];
      BeatmapLevelSO.DifficultyBeatmapSet[] difficultyBeatmapSetArray1 = new BeatmapLevelSO.DifficultyBeatmapSet[1]
      {
        new BeatmapLevelSO.DifficultyBeatmapSet(this._beatmapLevelDataModel.beatmapCharacteristic, new BeatmapLevelSO.DifficultyBeatmap[1]
        {
          new BeatmapLevelSO.DifficultyBeatmap((IBeatmapLevel) instance1, difficultyBeatmap.difficulty, difficultyBeatmap.difficultyRank, difficultyBeatmap.noteJumpMovementSpeed, difficultyBeatmap.noteJumpStartBeatOffset, instance2)
        })
      };
      Sprite imageFromTexture = BeatmapLevelStarterController.CreateCoverImageFromTexture(this._beatmapDataModel.coverImage);
      BeatmapLevelSO beatmapLevelSo = instance1;
      string songName = this._beatmapDataModel.songName;
      string songSubName = this._beatmapDataModel.songSubName;
      string songAuthorName = this._beatmapDataModel.songAuthorName;
      string levelAuthorName = this._beatmapDataModel.levelAuthorName;
      AudioClip audioClip = this._beatmapDataModel.audioClip;
      double beatsPerMinute = (double) this._beatmapDataModel.beatsPerMinute;
      double songTimeOffset = (double) this._beatmapDataModel.songTimeOffset;
      double shuffle = (double) this._beatmapDataModel.shuffle;
      double shufflePeriod = (double) this._beatmapDataModel.shufflePeriod;
      Sprite coverImage = imageFromTexture;
      BeatmapLevelSO.DifficultyBeatmapSet[] difficultyBeatmapSetArray2 = difficultyBeatmapSetArray1;
      EnvironmentInfoSO environment = this._beatmapDataModel.environment;
      EnvironmentInfoSO directionsEnvironment = this._beatmapDataModel.allDirectionsEnvironment;
      BeatmapLevelSO.DifficultyBeatmapSet[] difficultyBeatmapSets = difficultyBeatmapSetArray2;
      beatmapLevelSo.InitFull("custom_level_CustomLevel", songName, songSubName, songAuthorName, levelAuthorName, audioClip, (float) beatsPerMinute, (float) songTimeOffset, (float) shuffle, (float) shufflePeriod, 0.0f, 10f, coverImage, environment, directionsEnvironment, difficultyBeatmapSets);
      this._menuTransitionsHelper.StartBeatmapEditorStandardLevel(instance1.difficultyBeatmapSets[0].difficultyBeatmaps[0], (IPreviewBeatmapLevel) instance1, new GameplayModifiers().CopyWith(noFailOn0Energy: new bool?(true), zenMode: new bool?(this._beatmapEditorSettingsDataModel.zenMode)), new PlayerSpecificSettings().CopyWith(environmentEffectsFilterDefaultPreset: new EnvironmentEffectsFilterPreset?(EnvironmentEffectsFilterPreset.AllEffects), environmentEffectsFilterExpertPlusPreset: new EnvironmentEffectsFilterPreset?(EnvironmentEffectsFilterPreset.AllEffects)), practiceSettings, this._beatmapEditorSettingsDataModel.enableFpfc, (Action) null, levelFinishedCallback);
    }

    private static Sprite CreateCoverImageFromTexture(Texture2D texture)
    {
      if ((UnityEngine.Object) texture == (UnityEngine.Object) null)
        texture = Texture2D.whiteTexture;
      return Sprite.Create(texture, new Rect(0.0f, 0.0f, (float) texture.width, (float) texture.height), new Vector2(0.5f, 0.5f), 256f, 0U, SpriteMeshType.FullRect, Vector4.zero, false);
    }
  }
}
