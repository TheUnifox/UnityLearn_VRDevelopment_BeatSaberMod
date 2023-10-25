// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BaseBeatmapObjectEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.DataModels
{
  public abstract class BaseBeatmapObjectEditorData : BaseEditorData
  {
    public int column { get; }

    public int row { get; }

    protected BaseBeatmapObjectEditorData(
      BeatmapEditorObjectId id,
      float beat,
      int column,
      int row)
      : base(id, beat)
    {
      this.column = column;
      this.row = row;
    }

    protected BaseBeatmapObjectEditorData(BaseBeatmapObjectEditorData other)
      : base((BaseEditorData) other)
    {
      this.column = other.column;
      this.row = other.row;
    }

    public bool PositionEquals(BaseBeatmapObjectEditorData other) => this.BeatEquals((BaseEditorData) other) && this.column == other.column && this.row == other.row;
  }
}
