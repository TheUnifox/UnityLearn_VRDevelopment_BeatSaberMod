// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.EventObjectViewColorHelper
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D.Visuals
{
  public static class EventObjectViewColorHelper
  {
    public static Color GetLightEventObjectColor(int value, float floatValue) => value == 0 ? Color.gray : Color.Lerp(Color.gray, value < 5 ? Color.blue : (value < 9 ? Color.red : Color.white), Mathf.Lerp(0.3f, 1f, Mathf.InverseLerp(0.0f, 1f, floatValue)));
  }
}
