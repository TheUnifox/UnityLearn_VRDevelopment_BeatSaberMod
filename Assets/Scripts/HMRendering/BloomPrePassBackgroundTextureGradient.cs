// Decompiled with JetBrains decompiler
// Type: BloomPrePassBackgroundTextureGradient
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using Unity.Collections;
using UnityEngine;

[ExecuteAlways]
public abstract class BloomPrePassBackgroundTextureGradient : BloomPrePassNonLightPass
{
  [SerializeField]
  private Color _tintColor = Color.white;
  [DoesNotRequireDomainReloadInit]
  private static readonly int _gradientTexID = Shader.PropertyToID("_GradientTex");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _inverseProjectionMatrixID = Shader.PropertyToID("_InverseProjectionMatrix");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _cameraToWorldMatrixID = Shader.PropertyToID("_CameraToWorldMatrix");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _colorID = Shader.PropertyToID("_Color");
  private const string kUseToneMappingKeyword = "USE_TONE_MAPPING";
  private const string kSkyGradientShaderName = "Hidden/SkyGradient";
  private const int kTextureWidth = 128;
  private Texture2D _texture;
  private Material _material;

  public Color tintColor
  {
    get => this._tintColor;
    set => this._tintColor = value;
  }

  private void InitIfNeeded()
  {
    if ((Object) this._material != (Object) null && (Object) this._texture != (Object) null)
      return;
    EssentialHelpers.SafeDestroy((Object) this._texture);
    EssentialHelpers.SafeDestroy((Object) this._material);
    Texture2D texture2D = new Texture2D(128, 1, TextureFormat.RGBA32, false, false);
    texture2D.name = "SkyGradient";
    texture2D.filterMode = FilterMode.Bilinear;
    texture2D.wrapMode = TextureWrapMode.Clamp;
    this._texture = texture2D;
    this._material = new Material(Shader.Find("Hidden/SkyGradient"));
    this._material.SetTexture(BloomPrePassBackgroundTextureGradient._gradientTexID, (Texture) this._texture);
    if (this.executionTimeType == BloomPrePassNonLightPass.ExecutionTimeType.AfterBlur)
      this._material.EnableKeyword("USE_TONE_MAPPING");
    else
      this._material.DisableKeyword("USE_TONE_MAPPING");
  }

  protected void Start() => this.UpdateGradientTexture();

  protected void OnDestroy()
  {
    EssentialHelpers.SafeDestroy((Object) this._texture);
    EssentialHelpers.SafeDestroy((Object) this._material);
  }

  protected abstract void UpdatePixels(NativeArray<Color32> pixels, int numberOfPixels);

  protected override void OnValidate()
  {
    base.OnValidate();
    this.UpdateGradientTexture();
  }

  public void UpdateGradientTexture()
  {
    this.InitIfNeeded();
    this.UpdatePixels(this._texture.GetRawTextureData<Color32>(), 128);
    this._texture.Apply();
  }

  public override void Render(RenderTexture dest, Matrix4x4 viewMatrix, Matrix4x4 projectionMatrix)
  {
    this.InitIfNeeded();
    this._material.SetMatrix(BloomPrePassBackgroundTextureGradient._inverseProjectionMatrixID, projectionMatrix.inverse);
    this._material.SetMatrix(BloomPrePassBackgroundTextureGradient._cameraToWorldMatrixID, viewMatrix.inverse);
    this._material.SetColor(BloomPrePassBackgroundTextureGradient._colorID, this._tintColor);
    Graphics.Blit((Texture) null, dest, this._material);
  }
}
