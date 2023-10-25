// Decompiled with JetBrains decompiler
// Type: UnityLightWithId
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class UnityLightWithId : LightWithIdMonoBehaviour
{
  [SerializeField]
  protected Light _light;
  [SerializeField]
  protected float _intensity = 1f;
  [SerializeField]
  protected float _minAlpha;

  public Color color => this._light.color;

  public override void ColorWasSet(Color color)
  {
    this._light.color = color;
    this._light.intensity = Mathf.Max(color.a, this._minAlpha) * this._intensity;
  }
}
