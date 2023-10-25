// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EventsColorsSO
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class EventsColorsSO : PersistentScriptableObject
  {
    [SerializeField]
    private Color _defaultTrackColor;
    [SerializeField]
    private EventsColorsSO.TrackColor[] _tracksColors;
    private Dictionary<int, Color> _tracksColorsDictionary;

    public Color defaultTrackColor => this._defaultTrackColor;

    public Dictionary<int, Color> trackColors
    {
      get
      {
        if (this._tracksColorsDictionary == null)
        {
          this._tracksColorsDictionary = new Dictionary<int, Color>();
          for (int index = 0; index < this._tracksColors.Length; ++index)
            this._tracksColorsDictionary[this._tracksColors[index].value] = this._tracksColors[index].color;
        }
        return this._tracksColorsDictionary;
      }
    }

    public Color GetMarkerColor(int value) => this.trackColors.ContainsKey(value) ? this.trackColors[value] : this._defaultTrackColor;

    [Serializable]
    private struct TrackColor
    {
      [SerializeField]
      private int _value;
      [SerializeField]
      private Color _color;

      public int value => this._value;

      public Color color => this._color;
    }
  }
}
