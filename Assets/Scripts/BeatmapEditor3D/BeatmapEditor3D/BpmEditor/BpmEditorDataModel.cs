// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.BpmEditorDataModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeatmapEditor3D.BpmEditor
{
  public class BpmEditorDataModel
  {
    private bool _isDirty;
    private bool _loaded;
    private int _startOffset;
    private int _frequency;
    private List<BpmRegion> _regions = new List<BpmRegion>();

    public IReadOnlyList<BpmRegion> regions => (IReadOnlyList<BpmRegion>) this._regions;

    public BpmData bpmData => new BpmData(this._frequency, this._startOffset, this._regions.Select<BpmRegion, BpmRegion>((Func<BpmRegion, BpmRegion>) (r => new BpmRegion(r))).ToList<BpmRegion>());

    public bool isDirty => this._isDirty;

    public bool loaded => this._loaded;

    public void UpdateWith(BpmData bpmData)
    {
      this._startOffset = bpmData.startOffset;
      this._frequency = bpmData.frequency;
      this._regions = bpmData.regions.Select<BpmRegion, BpmRegion>((Func<BpmRegion, BpmRegion>) (r => new BpmRegion(r))).ToList<BpmRegion>();
      this._loaded = true;
    }

    public void ClearDirty() => this._isDirty = false;

    public void Close()
    {
      this._isDirty = false;
      this._loaded = false;
      this._startOffset = 0;
      this._frequency = 0;
      this._regions = (List<BpmRegion>) null;
    }

    public void ReplaceRegionAtIndex(int index, BpmRegion region)
    {
      this._regions[index] = region;
      this._isDirty = true;
    }

    public void ReplaceRegionAtIndexWithSplit(int index, BpmRegion left, BpmRegion right)
    {
      this._regions[index] = left;
      this._regions.Insert(index + 1, right);
      this._isDirty = true;
    }

    public void ReplaceSplitRegionAtIndex(int index, BpmRegion region)
    {
      this._regions[index] = region;
      this._regions.RemoveAt(index + 1);
      this._isDirty = true;
    }

    public void ReplaceAllRegions(List<BpmRegion> regions)
    {
      this._regions = regions;
      this._isDirty = true;
    }
  }
}
