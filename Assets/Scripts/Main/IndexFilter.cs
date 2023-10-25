// Decompiled with JetBrains decompiler
// Type: IndexFilter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IndexFilter : 
  IReadOnlyCollection<(int element, int durationOrder, int distributionOrder)>,
  IEnumerable<(int element, int durationOrder, int distributionOrder)>,
  IEnumerable
{
  protected readonly IndexFilter.IndexFilterRandomType _random;
  protected readonly int _seed;
  protected readonly int _groupSize;
  protected readonly int _chunkSize;
  protected readonly int _visibleCount;
  protected readonly IndexFilter.IndexFilterLimitAlsoAffectType _limitAlsoAffectType;
  protected readonly int _start;
  protected readonly int _step;
  protected readonly int _count;

  public int Count => this._count;

  public int VisibleCount => this._visibleCount;

  public IndexFilter.IndexFilterLimitAlsoAffectType limitAlsoAffectType => this._limitAlsoAffectType;

  public IndexFilter(
    int start,
    int step,
    int count,
    int groupSize,
    IndexFilter.IndexFilterRandomType random,
    int seed,
    int chunkSize,
    float limit,
    IndexFilter.IndexFilterLimitAlsoAffectType limitAlsoAffectType)
  {
    this._start = start;
    this._step = step;
    this._count = count;
    this._random = random;
    this._seed = seed;
    this._groupSize = groupSize;
    this._chunkSize = chunkSize;
    this._visibleCount = (double) limit == 0.0 || (double) limit == 1.0 ? this._count : Mathf.CeilToInt((float) this._count * limit);
    this._limitAlsoAffectType = limitAlsoAffectType;
  }

  public IndexFilter(
    int start,
    int end,
    int groupSize,
    IndexFilter.IndexFilterRandomType random,
    int seed,
    int chunkSize,
    float limit,
    IndexFilter.IndexFilterLimitAlsoAffectType limitAlsoAffectType)
    : this(start, end - start < 0 ? -1 : 1, Mathf.Abs(end - start) + 1, groupSize, random, seed, chunkSize, limit, limitAlsoAffectType)
  {
  }

  public virtual IEnumerator<(int element, int durationOrder, int distributionOrder)> GetEnumerator()
  {
    IEnumerable<int> ints1 = this.GetValues();
    if (this._random != IndexFilter.IndexFilterRandomType.NoRandom && !this._random.HasFlag((Enum) IndexFilter.IndexFilterRandomType.KeepOrder))
      ints1 = ints1.Shuffle<int>(new System.Random(this._seed));
    IEnumerable<int> ints2 = Enumerable.Range(0, this._count);
    if (this._visibleCount > 0)
      ints2 = this._random.HasFlag((Enum) IndexFilter.IndexFilterRandomType.RandomElements) ? ints2.PickRandomElementsWithTombstone<int>(this._visibleCount, this._count, new System.Random(this._seed), -1) : ints2.TakeWithTombstone<int>(this._visibleCount, -1);
    IEnumerable<(int, int)> valueTuples = ints1.ZipSkipTombstone<int, int>(ints2, -1);
    bool shouldLimitDurationOrder = this.limitAlsoAffectType.HasFlag((Enum) IndexFilter.IndexFilterLimitAlsoAffectType.Duration);
    bool shouldLimitDistributionOrder = this.limitAlsoAffectType.HasFlag((Enum) IndexFilter.IndexFilterLimitAlsoAffectType.Distribution);
    int limitedOrderIndex = 0;
    foreach ((int num1, int num2) in valueTuples)
    {
      for (int localChunkIndex = 0; localChunkIndex < this._chunkSize; ++localChunkIndex)
      {
        int num3 = num1 * this._chunkSize + localChunkIndex;
        if (num3 < this._groupSize)
        {
          int num4 = shouldLimitDurationOrder ? limitedOrderIndex : num2;
          int num5 = shouldLimitDistributionOrder ? limitedOrderIndex : num2;
          yield return (num3, num4, num5);
        }
        else
          break;
      }
      ++limitedOrderIndex;
    }
  }

  public virtual IEnumerable<int> GetValues()
  {
    int value = this._start;
    for (int i = 0; i < this._count; ++i)
    {
      yield return value;
      value += this._step;
    }
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  [Flags]
  public enum IndexFilterRandomType
  {
    NoRandom = 0,
    KeepOrder = 1,
    RandomElements = 2,
  }

  [Flags]
  public enum IndexFilterLimitAlsoAffectType
  {
    None = 0,
    Duration = 1,
    Distribution = 2,
  }
}
