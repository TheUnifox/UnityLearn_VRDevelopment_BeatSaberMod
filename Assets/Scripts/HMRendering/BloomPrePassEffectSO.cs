// Decompiled with JetBrains decompiler
// Type: BloomPrePassEffectSO
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public abstract class BloomPrePassEffectSO : TextureEffectSO, IBloomPrePassParams
{
  [SerializeField]
  private int _textureWidth = 512;
  [SerializeField]
  private int _textureHeight = 512;
  [SerializeField]
  private Vector2 _fov = new Vector2(140f, 140f);
  [SerializeField]
  private float _linesWidth = 0.02f;

  public TextureEffectSO textureEffect => (TextureEffectSO) this;

  public int textureWidth => this._textureWidth;

  public int textureHeight => this._textureHeight;

  public Vector2 fov => this._fov;

  public float linesWidth => this._linesWidth;

  public virtual ToneMapping toneMapping => ToneMapping.None;
}
