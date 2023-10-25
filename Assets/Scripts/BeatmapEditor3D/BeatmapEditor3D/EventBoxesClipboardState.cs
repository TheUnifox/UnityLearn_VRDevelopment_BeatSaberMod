// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventBoxesClipboardState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;

namespace BeatmapEditor3D
{
  public class EventBoxesClipboardState
  {
    private readonly Dictionary<int, List<BaseEditorData>> _baseEditorDataLists = new Dictionary<int, List<BaseEditorData>>();

    public float startBeat { get; set; }

    public int startEventBoxIndex { get; set; }

    public EventBoxGroupEditorData.EventBoxGroupType eventBoxGroupType { get; set; }

    public IReadOnlyDictionary<int, List<BaseEditorData>> baseEditorDataList => (IReadOnlyDictionary<int, List<BaseEditorData>>) this._baseEditorDataLists;

    public void Add(int index, BaseEditorData d)
    {
      if (!this._baseEditorDataLists.ContainsKey(index))
        this._baseEditorDataLists[index] = new List<BaseEditorData>();
      this._baseEditorDataLists[index].Add(d);
    }

    public void Clear() => this._baseEditorDataLists.Clear();
  }
}
