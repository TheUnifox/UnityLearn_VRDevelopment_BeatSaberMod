// Decompiled with JetBrains decompiler
// Type: BloomPrePassBackgroundColor
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class BloomPrePassBackgroundColor : BloomPrePassNonLightPass
{
  [SerializeField]
  protected float _intensity = 1f;
  [SerializeField]
  protected float _minAlpha;
  [SerializeField]
  protected float _grayscaleFactor = 0.7f;
  [Space]
  [SerializeField]
  protected Shader _shader;
  protected Color _color;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _colorID = Shader.PropertyToID("_Color");
  [DoesNotRequireDomainReloadInit]
  protected static Material _material;
  [DoesNotRequireDomainReloadInit]
  protected static bool _initialized;

  public Color color
  {
    get => this._color;
    set => this._color = value;
  }

  private Color bgColor
  {
    get
    {
      Color a = this._color;
      a.a *= this._intensity;
      if ((double) a.a < (double) this._minAlpha)
        a.a = this._minAlpha;
      Color b = new Color();
      float grayscale = a.grayscale;
      b.r = grayscale;
      b.g = grayscale;
      b.b = grayscale;
      b.a = a.a;
      a = Color.Lerp(a, b, this._grayscaleFactor);
      return a;
    }
  }

  public virtual void InitIfNeeded()
  {
    if (BloomPrePassBackgroundColor._initialized)
      return;
    BloomPrePassBackgroundColor._initialized = true;
    if ((Object) BloomPrePassBackgroundColor._material == (Object) null)
    {
      BloomPrePassBackgroundColor._material = new Material(this._shader);
      BloomPrePassBackgroundColor._material.hideFlags = HideFlags.HideAndDontSave;
    }
    if (!((Object) BloomPrePassBackgroundColor._material == (Object) null))
      return;
    BloomPrePassBackgroundColor._initialized = false;
  }

  public override void Render(RenderTexture dest, Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix)
  {
    this.InitIfNeeded();
    BloomPrePassBackgroundColor._material.SetColor(BloomPrePassBackgroundColor._colorID, this.bgColor);
    Graphics.Blit((Texture) null, dest, BloomPrePassBackgroundColor._material);
  }
}
