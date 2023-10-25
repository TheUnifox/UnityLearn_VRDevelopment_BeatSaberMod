// Decompiled with JetBrains decompiler
// Type: MipMapBiasSpriteSetter
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class MipMapBiasSpriteSetter : MonoBehaviour
{
  [SerializeField]
  protected Sprite[] _sprites;
  [SerializeField]
  protected float _mipMapBias = -1.68f;

  public virtual void Start()
  {
    foreach (Sprite sprite in this._sprites)
      sprite.texture.mipMapBias = this._mipMapBias;
  }
}
