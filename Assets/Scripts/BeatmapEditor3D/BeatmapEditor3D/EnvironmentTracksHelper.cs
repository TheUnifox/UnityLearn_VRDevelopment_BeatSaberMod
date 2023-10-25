// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EnvironmentTracksHelper
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;

namespace BeatmapEditor3D
{
  public static class EnvironmentTracksHelper
  {
    public static LightAxis GetLightAxis(
      EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo trackInfo,
      EventBoxGroupEditorData.EventBoxGroupType eventBoxGroupType)
    {
      switch (eventBoxGroupType)
      {
        case EventBoxGroupEditorData.EventBoxGroupType.Rotation:
          if (trackInfo.overrideDefaultRotationAxis != EnvironmentTracksDefinitionSO.OverrideDefaultLightAxis.NoOverride)
            return (LightAxis) (trackInfo.overrideDefaultRotationAxis - 1);
          if (trackInfo.showRotationXTrack)
            return LightAxis.X;
          if (trackInfo.showRotationYTrack)
            return LightAxis.Y;
          if (trackInfo.showRotationZTrack)
            return LightAxis.Z;
          break;
        case EventBoxGroupEditorData.EventBoxGroupType.Translation:
          if (trackInfo.overrideDefaultTranslationAxis != EnvironmentTracksDefinitionSO.OverrideDefaultLightAxis.NoOverride)
            return (LightAxis) (trackInfo.overrideDefaultTranslationAxis - 1);
          if (trackInfo.showTranslationXTrack)
            return LightAxis.X;
          if (trackInfo.showTranslationYTrack)
            return LightAxis.Y;
          if (trackInfo.showTranslationZTrack)
            return LightAxis.Z;
          break;
      }
      return LightAxis.X;
    }
  }
}
