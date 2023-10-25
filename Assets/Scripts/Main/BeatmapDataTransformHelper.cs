// Decompiled with JetBrains decompiler
// Type: BeatmapDataTransformHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public abstract class BeatmapDataTransformHelper
{
  public static IReadonlyBeatmapData CreateTransformedBeatmapData(
    IReadonlyBeatmapData beatmapData,
    IPreviewBeatmapLevel beatmapLevel,
    GameplayModifiers gameplayModifiers,
    bool leftHanded,
    EnvironmentEffectsFilterPreset environmentEffectsFilterPreset,
    EnvironmentIntensityReductionOptions environmentIntensityReductionOptions,
    MainSettingsModelSO mainSettingsModel)
  {
    IReadonlyBeatmapData beatmapData1 = beatmapData;
    if (leftHanded)
      beatmapData1 = BeatmapDataMirrorTransform.CreateTransformedData(beatmapData1);
    if (gameplayModifiers.zenMode)
    {
      beatmapData1 = BeatmapDataZenModeTransform.CreateTransformedData(beatmapData1);
    }
    else
    {
      GameplayModifiers.EnabledObstacleType enabledObstacleType = gameplayModifiers.enabledObstacleType;
      if (enabledObstacleType != GameplayModifiers.EnabledObstacleType.All || gameplayModifiers.noBombs)
        beatmapData1 = BeatmapDataObstaclesAndBombsTransform.CreateTransformedData(beatmapData1, enabledObstacleType, gameplayModifiers.noBombs);
      if (gameplayModifiers.noArrows)
        beatmapData1 = BeatmapDataNoArrowsTransform.CreateTransformedData(beatmapData1);
      if (BeatmapDataTransformHelper.IsObstaclesMergingNeeded(beatmapLevel, (bool) (ObservableVariableSO<bool>) mainSettingsModel.screenDisplacementEffectsEnabled))
        beatmapData1 = BeatmapDataObstaclesMergingTransform.CreateTransformedData(beatmapData1);
    }
    if (environmentEffectsFilterPreset == EnvironmentEffectsFilterPreset.StrobeFilter)
      beatmapData1 = BeatmapDataStrobeFilterTransform.CreateTransformedData(beatmapData1, environmentIntensityReductionOptions);
    if (beatmapData1 == beatmapData)
      beatmapData1 = (IReadonlyBeatmapData) beatmapData.GetCopy();
    return beatmapData1;
  }

  private static void AddTestBurstSlider(
    float time,
    float duration,
    int headLineIndex,
    NoteLineLayer headNoteLineLayer,
    NoteCutDirection headCutDirection,
    int tailLineIndex,
    NoteLineLayer tailNoteLineLayer,
    NoteCutDirection tailCutDirection,
    int sliceCount,
    float squishAmount,
    BeatmapData beatmapData)
  {
    NoteData basicNoteData = NoteData.CreateBasicNoteData(time, headLineIndex, headNoteLineLayer, ColorType.ColorA, headCutDirection);
    basicNoteData.ChangeToBurstSliderHead();
    SliderData burstSliderData = SliderData.CreateBurstSliderData(ColorType.ColorA, time, headLineIndex, headNoteLineLayer, headNoteLineLayer, headCutDirection, time + duration, tailLineIndex, tailNoteLineLayer, tailNoteLineLayer, tailCutDirection, sliceCount, squishAmount);
    beatmapData.AddBeatmapObjectData((BeatmapObjectData) basicNoteData);
    beatmapData.AddBeatmapObjectData((BeatmapObjectData) burstSliderData);
  }

  private static void AddTestSlider(
    float time,
    float duration,
    int headLineIndex,
    NoteLineLayer headNoteLineLayer,
    NoteCutDirection headCutDirection,
    float headControlPointLength,
    int tailLineIndex,
    NoteLineLayer tailNoteLineLayer,
    NoteCutDirection tailCutDirection,
    float tailControlPointLength,
    bool hasHeadNote,
    bool hasTailNote,
    BeatmapData beatmapData)
  {
    NoteData basicNoteData1 = NoteData.CreateBasicNoteData(time, headLineIndex, headNoteLineLayer, ColorType.ColorA, headCutDirection);
    NoteData basicNoteData2 = NoteData.CreateBasicNoteData(time + duration, tailLineIndex, tailNoteLineLayer, ColorType.ColorA, tailCutDirection);
    SliderData sliderData = SliderData.CreateSliderData(ColorType.ColorA, time, headLineIndex, headNoteLineLayer, headNoteLineLayer, headControlPointLength, headCutDirection, time + duration, tailLineIndex, tailNoteLineLayer, tailNoteLineLayer, tailControlPointLength, tailCutDirection, SliderMidAnchorMode.Straight);
    sliderData.SetHasHeadNote(hasHeadNote);
    sliderData.SetHasTailNote(hasTailNote);
    if (hasHeadNote)
      beatmapData.AddBeatmapObjectData((BeatmapObjectData) basicNoteData1);
    beatmapData.AddBeatmapObjectData((BeatmapObjectData) sliderData);
    if (!hasTailNote)
      return;
    beatmapData.AddBeatmapObjectData((BeatmapObjectData) basicNoteData2);
  }

  public static bool IsObstaclesMergingNeeded(
    IPreviewBeatmapLevel beatmapLevel,
    bool screenDisplacementEffectsEnabled)
  {
    return !screenDisplacementEffectsEnabled && !beatmapLevel.levelID.StartsWith("custom_level_");
  }
}
