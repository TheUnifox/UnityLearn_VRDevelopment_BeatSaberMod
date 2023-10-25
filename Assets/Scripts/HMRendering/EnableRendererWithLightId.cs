// Decompiled with JetBrains decompiler
// Type: EnableRendererWithLightId
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class EnableRendererWithLightId : LightWithIdMonoBehaviour
{
  [SerializeField]
  protected Renderer _renderer;
  [SerializeField]
  protected float _hideAlphaRangeMin = 1f / 1000f;
  [SerializeField]
  protected float _hideAlphaRangeMax = 1f;

  public override void ColorWasSet(Color color) => this._renderer.enabled = (double) color.a >= (double) this._hideAlphaRangeMin && (double) color.a <= (double) this._hideAlphaRangeMax;
}
