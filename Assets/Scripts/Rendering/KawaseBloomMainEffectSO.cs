// Decompiled with JetBrains decompiler
// Type: KawaseBloomMainEffectSO
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using UnityEngine;

public class KawaseBloomMainEffectSO : MainEffectSO
{
  [SerializeField]
  private KawaseBlurRendererSO _kawaseBlurRenderer;
  [SerializeField]
  private Shader _fadeShader;
  [SerializeField]
  private Shader _mainEffectShader;
  [Space]
  [SerializeField]
  [Range(0.0f, 5f)]
  private float _bloomIntensity = 1f;
  [SerializeField]
  private int _bloomIterations = 4;
  [SerializeField]
  private float _bloomBoost;
  [SerializeField]
  private float _bloomAlphaWeights = 1f;
  [Space]
  [SerializeField]
  private int _bloomTextureWidth = 512;
  [Space]
  [SerializeField]
  [Range(0.0f, 3f)]
  private float _baseColorBoost = 1f;
  [SerializeField]
  private float _baseColorBoostThreshold;
  [DoesNotRequireDomainReloadInit]
  private static readonly int _bloomTexID = Shader.PropertyToID("_BloomTex");
  [DoesNotRequireDomainReloadInit]
  private static readonly int _bloomIntensityID = Shader.PropertyToID("_BloomIntensity");
  private Material _fadeMaterial;
  private Material _mainEffectMaterial;

  public override bool hasPostProcessEffect => true;

  protected override void OnEnable()
  {
    base.OnEnable();
    this.LazyInitializeMaterials();
  }

  protected void OnDisable()
  {
    EssentialHelpers.SafeDestroy((Object) this._fadeMaterial);
    this._fadeMaterial = (Material) null;
    EssentialHelpers.SafeDestroy((Object) this._mainEffectMaterial);
    this._mainEffectMaterial = (Material) null;
  }

  private void LazyInitializeMaterials()
  {
    if ((Object) this._fadeMaterial == (Object) null)
    {
      this._fadeMaterial = new Material(this._fadeShader);
      this._fadeMaterial.hideFlags = HideFlags.HideAndDontSave;
    }
    if (!((Object) this._mainEffectMaterial == (Object) null))
      return;
    this._mainEffectMaterial = new Material(this._mainEffectShader);
    this._mainEffectMaterial.hideFlags = HideFlags.HideAndDontSave;
  }

  public override void PreRender() => MainEffectCore.SetGlobalShaderValues(this._baseColorBoost, this._baseColorBoostThreshold);

  public override void Render(RenderTexture src, RenderTexture dest, float fade)
  {
    this.LazyInitializeMaterials();
    RenderTexture temporary = RenderTexture.GetTemporary(this._bloomTextureWidth * (src.vrUsage == VRTextureUsage.TwoEyes ? 2 : 1), (int) ((double) (this._bloomTextureWidth * src.height) / (double) src.width) * (src.vrUsage == VRTextureUsage.TwoEyes ? 2 : 1), 0, RenderTextureFormat.RGB111110Float, RenderTextureReadWrite.Linear);
    this._kawaseBlurRenderer.Bloom(src, temporary, 0, this._bloomIterations, this._bloomBoost, this._bloomAlphaWeights, KawaseBlurRendererSO.WeightsType.AlphaWeights, (float[]) null);
    this._mainEffectMaterial.SetFloat(KawaseBloomMainEffectSO._bloomIntensityID, this._bloomIntensity);
    this._mainEffectMaterial.SetTexture(KawaseBloomMainEffectSO._bloomTexID, (Texture) temporary);
    Graphics.Blit((Texture) src, dest, this._mainEffectMaterial, 0);
    RenderTexture.ReleaseTemporary(temporary);
    if ((double) fade >= 0.99000000953674316)
      return;
    this._fadeMaterial.color = new Color(0.0f, 0.0f, 0.0f, 1f - fade);
    Graphics.Blit((Texture) null, dest, this._fadeMaterial);
  }
}
