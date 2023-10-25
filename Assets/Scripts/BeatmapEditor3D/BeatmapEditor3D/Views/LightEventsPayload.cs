// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.LightEventsPayload
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;

namespace BeatmapEditor3D.Views
{
  public class LightEventsPayload
  {
    public LightColor color;
    public LightEventType type;
    public float intensity;

    public LightEventsPayload()
    {
      this.color = LightColor.Blue;
      this.type = LightEventType.On;
      this.intensity = 1f;
    }

    public LightEventsPayload(int value)
      : this()
    {
      if (value >= 9)
      {
        this.color = LightColor.White;
        this.type = (LightEventType) (value - 9);
      }
      else if (value >= 5)
      {
        this.color = LightColor.Red;
        this.type = (LightEventType) (value - 5);
      }
      else
      {
        this.color = LightColor.Blue;
        this.type = (LightEventType) (value - 1);
      }
    }

    public LightEventsPayload(int value, float floatValue)
      : this(value)
    {
      this.intensity = floatValue;
    }

    public int ToValue() => this.type == LightEventType.Off ? 0 : (int) (this.color + (int) this.type);

    public float ToAltValue() => Mathf.Clamp(this.intensity, 0.0f, 1.2f);

    public override string ToString() => string.Format("<color=#{0}>{1} : {2}</color>", (object) this.color.ToColorHex(), (object) this.type.ToName(), (object) Mathf.RoundToInt(this.ToAltValue() * 100f));
  }
}
