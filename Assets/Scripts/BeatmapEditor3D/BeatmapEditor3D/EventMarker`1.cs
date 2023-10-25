// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventMarker`1
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

namespace BeatmapEditor3D
{
  public class EventMarker<T> where T : class
  {
    private T _data;

    public T data => this._data;

    public void Init(T data) => this._data = data;

    protected void Reset() => this._data = default (T);
  }
}
