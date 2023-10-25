// Decompiled with JetBrains decompiler
// Type: BloomPrePassBackgroundColorsGradient
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using Unity.Collections;
using UnityEngine;

public class BloomPrePassBackgroundColorsGradient : BloomPrePassBackgroundTextureGradient
{
  [SerializeField]
  protected BloomPrePassBackgroundColorsGradient.Element[] _elements;

  public BloomPrePassBackgroundColorsGradient.Element[] elements => this._elements;

  protected override void UpdatePixels(NativeArray<Color32> pixels, int numberOfPixels)
  {
    for (int index = 0; index < numberOfPixels; ++index)
      pixels[index] = (Color32) this.EvaluateColor((float) index / (float) (numberOfPixels - 1));
  }

  public virtual Color EvaluateColor(float t)
  {
    for (int index = this._elements.Length - 2; index >= 0; --index)
    {
      BloomPrePassBackgroundColorsGradient.Element element1 = this._elements[index];
      if ((double) t >= (double) element1.startT)
      {
        BloomPrePassBackgroundColorsGradient.Element element2 = this._elements[index + 1];
        return Color.LerpUnclamped(element1.color, element2.color, Mathf.Pow((float) (((double) t - (double) element1.startT) / ((double) element2.startT - (double) element1.startT)), element1.exp));
      }
    }
    return this._elements[this._elements.Length - 1].color;
  }

  [Serializable]
  public class Element
  {
    public Color color;
    public float startT;
    public float exp;
  }
}
