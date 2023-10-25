// Decompiled with JetBrains decompiler
// Type: BloomPrePassBackgroundColorsGradientElementWithLightId
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using UnityEngine;

public class BloomPrePassBackgroundColorsGradientElementWithLightId : LightWithIdMonoBehaviour
{
  [SerializeField]
  protected BloomPrePassBackgroundColorsGradient _bloomPrePassBackgroundColorsGradient;
  [SerializeField]
  protected BloomPrePassBackgroundColorsGradientElementWithLightId.Elements[] _elements;

  public override void ColorWasSet(Color color)
  {
    foreach (BloomPrePassBackgroundColorsGradientElementWithLightId.Elements element in this._elements)
      this._bloomPrePassBackgroundColorsGradient.elements[element.elementNumber].color = color * Mathf.Max(color.a * element.intensity, element.minIntensity);
    this._bloomPrePassBackgroundColorsGradient.UpdateGradientTexture();
  }

  [Serializable]
  public class Elements
  {
    public int elementNumber;
    public float intensity = 1f;
    public float minIntensity;
  }
}
