// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.EventBoxGroupsState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.DataModels
{
  public class EventBoxGroupsState
  {
    public float scrollPrecision = 1f;

    public int currentPage { get; set; }

    public float currentHoverBeat { get; set; }

    public int currentHoverGroupId { get; set; }

    public EventBoxGroupEditorData.EventBoxGroupType currentHoverGroupType { get; set; }

    public bool eventBoxGroupExtension { get; set; }

    public EventBoxGroupEditorData eventBoxGroupContext { get; set; }

    public BeatmapEditorObjectId currentHoverEventBoxId { get; set; } = BeatmapEditorObjectId.invalid;

    public BeatmapEditorObjectId currentHoverBaseEventId { get; set; } = BeatmapEditorObjectId.invalid;

    public LightColorBaseEditorData.EnvironmentColorType lightColorType { get; set; }

    public LightColorBaseEditorData.TransitionType lightColorTransitionType { get; set; }

    public float lightColorBrightness { get; set; } = 1f;

    public int lightStrobeBeatFrequency { get; set; }

    public EaseType lightRotationEaseType { get; set; } = EaseType.Linear;

    public int lightRotationLoopCount { get; set; }

    public float lightRotation { get; set; }

    public LightRotationDirection lightRotationDirection { get; set; }

    public EaseType lightTranslationEaseType { get; set; } = EaseType.Linear;

    public float lightTranslation { get; set; }

    public void Reset()
    {
      this.currentPage = 0;
      this.currentHoverBeat = 0.0f;
      this.currentHoverGroupId = 0;
      this.currentHoverGroupType = EventBoxGroupEditorData.EventBoxGroupType.Color;
      this.eventBoxGroupExtension = false;
      this.eventBoxGroupContext = (EventBoxGroupEditorData) null;
      this.currentHoverEventBoxId = BeatmapEditorObjectId.invalid;
      this.currentHoverBaseEventId = BeatmapEditorObjectId.invalid;
      this.lightColorType = LightColorBaseEditorData.EnvironmentColorType.Color0;
      this.lightColorTransitionType = LightColorBaseEditorData.TransitionType.Instant;
      this.lightColorBrightness = 1f;
      this.lightRotationEaseType = EaseType.Linear;
      this.lightRotationLoopCount = 0;
      this.lightRotation = 0.0f;
      this.lightTranslationEaseType = EaseType.Linear;
      this.lightTranslation = 0.0f;
      this.scrollPrecision = 1f;
    }
  }
}
