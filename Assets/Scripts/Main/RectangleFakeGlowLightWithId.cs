// Decompiled with JetBrains decompiler
// Type: RectangleFakeGlowLightWithId
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class RectangleFakeGlowLightWithId : LightWithIdMonoBehaviour
{
  [SerializeField]
  protected float _minAlpha;
  [SerializeField]
  protected float _alphaMul = 1f;
  [SerializeField]
  protected RectangleFakeGlow _rectangleFakeGlow;

  public Color color => this._rectangleFakeGlow.color;

  public override void ColorWasSet(Color color)
  {
    color.a *= this._alphaMul;
    if ((double) color.a < (double) this._minAlpha)
      color.a = this._minAlpha;
    this._rectangleFakeGlow.color = color;
  }
}
