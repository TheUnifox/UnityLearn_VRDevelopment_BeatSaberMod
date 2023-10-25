// Decompiled with JetBrains decompiler
// Type: TubeBloomPrePassLightWithId
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class TubeBloomPrePassLightWithId : LightWithIdMonoBehaviour
{
  [SerializeField]
  protected TubeBloomPrePassLight _tubeBloomPrePassLight;
  [SerializeField]
  protected bool _setOnlyOnce;
  [SerializeField]
  protected bool _setColorOnly;

  public Color color => this._tubeBloomPrePassLight.color;

  public override void ColorWasSet(Color color)
  {
    if (this._setColorOnly)
      color.a = this._tubeBloomPrePassLight.color.a;
    this._tubeBloomPrePassLight.color = color;
    if (!this._setOnlyOnce)
      return;
    this.enabled = false;
  }
}
