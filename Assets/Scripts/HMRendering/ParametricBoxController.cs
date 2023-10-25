// Decompiled with JetBrains decompiler
// Type: ParametricBoxController
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

[RequireComponent(typeof (MeshRenderer))]
[RequireComponent(typeof (MeshFilter))]
[ExecuteInEditMode]
public class ParametricBoxController : MonoBehaviour
{
  public float width = 1f;
  public float height = 1f;
  public float length = 1f;
  [Range(0.0f, 1f)]
  public float heightCenter = 0.5f;
  public Color color;
  public float alphaMultiplier = 1f;
  public float minAlpha;
  [Space]
  [Range(0.0f, 1f)]
  public float alphaStart = 1f;
  [Range(0.0f, 1f)]
  public float alphaEnd = 1f;
  public float widthStart = 1f;
  public float widthEnd = 1f;
  [Space]
  [SerializeField]
  protected MeshRenderer _meshRenderer;
  protected static MaterialPropertyBlock _materialPropertyBlock;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _colorID = Shader.PropertyToID("_Color");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _alphaStartID = Shader.PropertyToID("_AlphaStart");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _alphaEndID = Shader.PropertyToID("_AlphaEnd");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _widthStartID = Shader.PropertyToID("_StartWidth");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _widthEndID = Shader.PropertyToID("_EndWidth");

  public virtual void Awake() => this._meshRenderer.enabled = false;

  public virtual void OnEnable()
  {
    this.Refresh();
    this._meshRenderer.enabled = true;
  }

  public virtual void OnDisable() => this._meshRenderer.enabled = false;

  public virtual void Refresh()
  {
    if ((Object) this._meshRenderer == (Object) null)
      return;
    this.transform.localScale = new Vector3(this.width * 0.5f, this.height * 0.5f, this.length * 0.5f);
    this.transform.localPosition = new Vector3(0.0f, (0.5f - this.heightCenter) * this.height, 0.0f);
    if (ParametricBoxController._materialPropertyBlock == null)
      ParametricBoxController._materialPropertyBlock = new MaterialPropertyBlock();
    Color color = this.color;
    color.a *= this.alphaMultiplier;
    if ((double) color.a < (double) this.minAlpha)
      color.a = this.minAlpha;
    ParametricBoxController._materialPropertyBlock.SetColor(ParametricBoxController._colorID, color);
    ParametricBoxController._materialPropertyBlock.SetFloat(ParametricBoxController._alphaStartID, this.alphaStart);
    ParametricBoxController._materialPropertyBlock.SetFloat(ParametricBoxController._alphaEndID, this.alphaEnd);
    ParametricBoxController._materialPropertyBlock.SetFloat(ParametricBoxController._widthStartID, this.widthStart);
    ParametricBoxController._materialPropertyBlock.SetFloat(ParametricBoxController._widthEndID, this.widthEnd);
    this._meshRenderer.SetPropertyBlock(ParametricBoxController._materialPropertyBlock);
  }
}
