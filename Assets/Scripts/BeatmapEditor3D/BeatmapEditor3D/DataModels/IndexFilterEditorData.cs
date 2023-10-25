// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.IndexFilterEditorData
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D.DataModels
{
  public class IndexFilterEditorData
  {
    public readonly IndexFilterEditorData.IndexFilterType type;
    public readonly int param0;
    public readonly int param1;
    public readonly bool reversed;
    public readonly int chunks;
    public readonly IndexFilter.IndexFilterRandomType randomType;
    public readonly int seed;
    public readonly float limit;
    public readonly IndexFilter.IndexFilterLimitAlsoAffectType limitAlsoAffectType;

    public int numberOfSections => this.param0;

    public int sectionId => this.param1;

    public int offset => this.param0;

    public int step => this.param1;

    private IndexFilterEditorData(
      IndexFilterEditorData.IndexFilterType type,
      int param0,
      int param1,
      bool reversed,
      int chunks,
      IndexFilter.IndexFilterRandomType randomType,
      int seed,
      float limit,
      IndexFilter.IndexFilterLimitAlsoAffectType limitAlsoAffectType)
    {
      this.type = type;
      this.param0 = param0;
      this.param1 = param1;
      this.reversed = reversed;
      this.chunks = chunks;
      this.randomType = randomType;
      this.seed = seed;
      this.limit = limit;
      this.limitAlsoAffectType = limitAlsoAffectType;
    }

    public static IndexFilterEditorData Copy(IndexFilterEditorData other) => IndexFilterEditorData.CreateNew(other.type, other.param0, other.param1, other.reversed, other.chunks, other.randomType, other.seed, other.limit, other.limitAlsoAffectType);

    public static IndexFilterEditorData CreateNew(
      IndexFilterEditorData.IndexFilterType type,
      int param0,
      int param1,
      bool reversed,
      int chunks,
      IndexFilter.IndexFilterRandomType randomType,
      int seed,
      float limit,
      IndexFilter.IndexFilterLimitAlsoAffectType limitAlsoAffectType)
    {
      return new IndexFilterEditorData(type, param0, param1, reversed, chunks, randomType, seed, limit, limitAlsoAffectType);
    }

    public static IndexFilterEditorData CreateDivisionIndexFilter(
      int numberOfSections,
      int sectionIdx,
      bool reversed)
    {
      return IndexFilterEditorData.CreateNew(IndexFilterEditorData.IndexFilterType.Division, numberOfSections, sectionIdx, reversed, 0, IndexFilter.IndexFilterRandomType.NoRandom, 0, 0.0f, IndexFilter.IndexFilterLimitAlsoAffectType.None);
    }

    public static IndexFilterEditorData CreateStepIndexFilter(int offset, int step, bool reversed) => IndexFilterEditorData.CreateNew(IndexFilterEditorData.IndexFilterType.StepAndOffset, offset, step, reversed, 0, IndexFilter.IndexFilterRandomType.NoRandom, 0, 0.0f, IndexFilter.IndexFilterLimitAlsoAffectType.None);

    public static IndexFilterEditorData CreateExtensionIndexFilter() => IndexFilterEditorData.CreateNew(IndexFilterEditorData.IndexFilterType.Division, 1, 0, false, 0, IndexFilter.IndexFilterRandomType.NoRandom, 0, 0.0f, IndexFilter.IndexFilterLimitAlsoAffectType.None);

    public enum IndexFilterType
    {
      Division = 1,
      StepAndOffset = 2,
    }
  }
}
