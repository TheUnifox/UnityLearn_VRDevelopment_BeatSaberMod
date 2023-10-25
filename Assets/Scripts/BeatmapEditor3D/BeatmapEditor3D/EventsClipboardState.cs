// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventsClipboardState
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;

namespace BeatmapEditor3D
{
  public class EventsClipboardState : IReadonlyEventsClipboardState
  {
    private readonly BeatmapEditorObjectsCollection<BasicEventEditorData> _events = new BeatmapEditorObjectsCollection<BasicEventEditorData>();

    public float startBeat { get; set; }

    public bool allEventsSame { get; set; }

    public IReadOnlyList<BasicEventEditorData> events => this._events.items;

    public BeatmapEditorObjectsCollection<BasicEventEditorData> collection => this._events;

    public void Add(BasicEventEditorData item) => this._events.Add(item);

    public void AddRange(IEnumerable<BasicEventEditorData> items) => this._events.AddRange(items);

    public void Remove(BasicEventEditorData item) => this._events.Remove(item);

    public void Clear() => this._events.Clear();

    public void Copy(
      BeatmapEditorObjectsCollection<BasicEventEditorData> other)
    {
      this._events.CopyFrom(other);
    }
  }
}
