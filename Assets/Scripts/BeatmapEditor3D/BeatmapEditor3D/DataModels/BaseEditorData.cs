// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BaseEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.DataModels
{
  public abstract class BaseEditorData
  {
    public readonly BeatmapEditorObjectId id;
    public readonly float beat;

    protected BaseEditorData(BeatmapEditorObjectId id, float beat)
    {
      this.id = id;
      this.beat = beat;
    }

    protected BaseEditorData(BaseEditorData other)
    {
      this.id = other.id;
      this.beat = other.beat;
    }

    public bool InstanceEquals(BaseEditorData other) => this.id == other.id;

    public bool BeatEquals(BaseEditorData other) => AudioTimeHelper.IsBeatSame(this.beat, other.beat);
  }
}
