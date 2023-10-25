// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.ChangeSubdivisionSignal
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.LevelEditor
{
  public class ChangeSubdivisionSignal
  {
    public readonly ChangeSubdivisionSignal.Type type;
    public readonly float delta;

    public ChangeSubdivisionSignal(ChangeSubdivisionSignal.Type type, float delta)
    {
      this.type = type;
      this.delta = delta;
    }

    public enum Type
    {
      Subdivision,
      Multiplication,
    }
  }
}
