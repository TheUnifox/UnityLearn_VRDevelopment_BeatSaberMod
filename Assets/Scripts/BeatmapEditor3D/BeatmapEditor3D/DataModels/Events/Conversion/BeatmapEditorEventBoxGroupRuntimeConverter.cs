// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.Events.Conversion.BeatmapEditorEventBoxGroupRuntimeConverter
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.SerializedData;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BeatmapEditor3D.DataModels.Events.Conversion
{
  public class BeatmapEditorEventBoxGroupRuntimeConverter
  {
    public static BeatmapEventDataBoxGroup ConvertBoxGroup(
      EventBoxGroupEditorData eventBoxGroup,
      List<EventBoxEditorData> eventBoxes,
      List<List<BaseEditorData>> eventBoxesLists,
      int groupSize)
    {
      try
      {
        List<BeatmapEventDataBox> beatmapEventDataBoxList = new List<BeatmapEventDataBox>(eventBoxes.Count);
        for (int index = 0; index < eventBoxes.Count; ++index)
        {
          EventBoxEditorData eventBox = eventBoxes[index];
          List<BaseEditorData> eventBoxesList = eventBoxesLists[index];
          IndexFilter indexFilter = BeatmapEditorEventBoxGroupRuntimeConverter.ConvertIndexFilter(eventBox.indexFilter, groupSize);
          beatmapEventDataBoxList.Add(BeatmapEditorEventBoxGroupRuntimeConverter.ConvertEventBox(eventBox, indexFilter, eventBoxesList));
        }
        return new BeatmapEventDataBoxGroup(eventBoxGroup.beat, (IReadOnlyCollection<BeatmapEventDataBox>) beatmapEventDataBoxList);
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) string.Format("Unable to convert index filter on beat: {0}", (object) eventBoxGroup.beat));
      }
      return (BeatmapEventDataBoxGroup) null;
    }

    private static BeatmapEventDataBox ConvertEventBox(
      EventBoxEditorData eventBox,
      IndexFilter indexFilter,
      List<BaseEditorData> baseList)
    {
      switch (eventBox)
      {
        case LightColorEventBoxEditorData eventBoxEditorData1:
          return (BeatmapEventDataBox) new LightColorBeatmapEventDataBox(indexFilter, eventBoxEditorData1.beatDistributionParam, eventBoxEditorData1.beatDistributionParamType, eventBoxEditorData1.brightnessDistributionParam, eventBoxEditorData1.brightnessDistributionParamType, eventBoxEditorData1.brightnessDistributionShouldAffectFirstBaseEvent, eventBoxEditorData1.brightnessDistributionEaseType, (IReadOnlyList<LightColorBaseData>) baseList.Select<BaseEditorData, LightColorBaseData>((Func<BaseEditorData, LightColorBaseData>) (d => BeatmapEditorEventBoxGroupRuntimeConverter.ConvertLightColorBaseData((LightColorBaseEditorData) d))).ToList<LightColorBaseData>());
        case LightRotationEventBoxEditorData eventBoxEditorData2:
          return (BeatmapEventDataBox) new LightRotationBeatmapEventDataBox(indexFilter, eventBoxEditorData2.beatDistributionParam, eventBoxEditorData2.beatDistributionParamType, eventBoxEditorData2.axis, eventBoxEditorData2.flipRotation, eventBoxEditorData2.rotationDistributionParam, eventBoxEditorData2.rotationDistributionParamType, eventBoxEditorData2.rotationDistributionShouldAffectFirstBaseEvent, eventBoxEditorData2.rotationDistributionEaseType, (IReadOnlyList<LightRotationBaseData>) baseList.Select<BaseEditorData, LightRotationBaseData>((Func<BaseEditorData, LightRotationBaseData>) (d => BeatmapEditorEventBoxGroupRuntimeConverter.ConvertLightRotationBaseData((LightRotationBaseEditorData) d))).ToList<LightRotationBaseData>());
        case LightTranslationEventBoxEditorData eventBoxEditorData3:
          return (BeatmapEventDataBox) new LightTranslationBeatmapEventDataBox(indexFilter, eventBoxEditorData3.beatDistributionParam, eventBoxEditorData3.beatDistributionParamType, eventBoxEditorData3.axis, eventBoxEditorData3.flipTranslation, eventBoxEditorData3.gapDistributionParam, eventBoxEditorData3.gapDistributionParamType, eventBoxEditorData3.gapDistributionShouldAffectFirstBaseEvent, eventBoxEditorData3.gapDistributionEaseType, (IReadOnlyList<LightTranslationBaseData>) baseList.Select<BaseEditorData, LightTranslationBaseData>((Func<BaseEditorData, LightTranslationBaseData>) (d => BeatmapEditorEventBoxGroupRuntimeConverter.ConvertLightTranslationBaseData((LightTranslationBaseEditorData) d))).ToList<LightTranslationBaseData>());
        default:
          return (BeatmapEventDataBox) null;
      }
    }

    private static IndexFilter ConvertIndexFilter(IndexFilterEditorData indexFilter, int groupSize) => BeatmapDataLoader.IndexFilterConvertor.Convert(BeatmapLevelDataModelSaver.CreateIndexFilter(indexFilter), groupSize);

    private static LightColorBaseData ConvertLightColorBaseData(
      LightColorBaseEditorData lightColorBaseEditorData)
    {
      return new LightColorBaseData(lightColorBaseEditorData.beat, (BeatmapEventTransitionType) lightColorBaseEditorData.transitionType, (global::EnvironmentColorType) lightColorBaseEditorData.colorType, lightColorBaseEditorData.brightness, lightColorBaseEditorData.strobeBeatFrequency);
    }

    private static LightRotationBaseData ConvertLightRotationBaseData(
      LightRotationBaseEditorData lightColorBaseEditorData)
    {
      double beat = (double) lightColorBaseEditorData.beat;
      EaseType easeType = lightColorBaseEditorData.easeType;
      float rotation1 = lightColorBaseEditorData.rotation;
      int loopsCount1 = lightColorBaseEditorData.loopsCount;
      LightRotationDirection rotationDirection = lightColorBaseEditorData.rotationDirection;
      int num1 = lightColorBaseEditorData.usePreviousEventRotationValue ? 1 : 0;
      int num2 = (int) easeType;
      double rotation2 = (double) rotation1;
      int loopsCount2 = loopsCount1;
      int num3 = (int) rotationDirection;
      return new LightRotationBaseData((float) beat, num1 != 0, (EaseType) num2, (float) rotation2, loopsCount2, (LightRotationDirection) num3);
    }

    private static LightTranslationBaseData ConvertLightTranslationBaseData(
      LightTranslationBaseEditorData lightTranslationBaseEditorData)
    {
      double beat = (double) lightTranslationBaseEditorData.beat;
      EaseType easeType = lightTranslationBaseEditorData.easeType;
      float translation1 = lightTranslationBaseEditorData.translation;
      int num1 = lightTranslationBaseEditorData.usePreviousEventTranslationValue ? 1 : 0;
      int num2 = (int) easeType;
      double translation2 = (double) translation1;
      return new LightTranslationBaseData((float) beat, num1 != 0, (EaseType) num2, (float) translation2);
    }
  }
}
