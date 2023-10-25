// Decompiled with JetBrains decompiler
// Type: Parametric3SliceSpriteController
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

[RequireComponent(typeof (MeshRenderer))]
[RequireComponent(typeof (MeshFilter))]
[ExecuteInEditMode]
public class Parametric3SliceSpriteController : MonoBehaviour
{
  [SerializeField]
  protected float _widthMultiplier = 1f;
  public float width = 0.5f;
  public float length = 1f;
  public float center = 0.5f;
  public Color color;
  public float alphaMultiplier = 1f;
  public float minAlpha;
  [Space]
  [Min(0.0f)]
  public float alphaStart = 1f;
  [Min(0.0f)]
  public float alphaEnd = 1f;
  [Min(0.0f)]
  public float widthStart = 1f;
  [Min(0.0f)]
  public float widthEnd = 1f;
  protected MeshRenderer _meshRenderer;
  protected MeshFilter _meshFilter;
  protected bool _isInitialized;
  protected const float kMaxWidth = 10f;
  protected const float kMaxLength = 2500f;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _colorID = Shader.PropertyToID("_Color");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _sizeParamsID = Shader.PropertyToID("_SizeParams");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _alphaStartID = Shader.PropertyToID("_AlphaStart");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _alphaEndID = Shader.PropertyToID("_AlphaEnd");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _widthStartID = Shader.PropertyToID("_StartWidth");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _widthEndID = Shader.PropertyToID("_EndWidth");
  protected static MaterialPropertyBlock _materialPropertyBlock;
  protected static Mesh _mesh;
  protected static int _instanceCount = 0;

  public virtual void Awake()
  {
    this.Init();
    this._meshRenderer.enabled = false;
    if ((Object) Parametric3SliceSpriteController._mesh == (Object) null)
    {
      Parametric3SliceSpriteController._mesh = this._meshFilter.sharedMesh;
      if ((Object) Parametric3SliceSpriteController._mesh == (Object) null)
        Parametric3SliceSpriteController._mesh = this.CreateMesh();
    }
    ++Parametric3SliceSpriteController._instanceCount;
  }

  public virtual void Start() => this._meshFilter.sharedMesh = Parametric3SliceSpriteController._mesh;

  public virtual void OnEnable()
  {
    this._meshRenderer.enabled = true;
    this.Refresh();
  }

  public virtual void OnDisable() => this._meshRenderer.enabled = false;

  public virtual void OnDestroy()
  {
    --Parametric3SliceSpriteController._instanceCount;
    if (Parametric3SliceSpriteController._instanceCount < 0)
      Parametric3SliceSpriteController._instanceCount = 0;
    if (Parametric3SliceSpriteController._instanceCount > 0)
      return;
    EssentialHelpers.SafeDestroy((Object) Parametric3SliceSpriteController._mesh);
  }

  public virtual void Init()
  {
    if (this._isInitialized)
      return;
    this._isInitialized = true;
    this._meshFilter = this.GetComponent<MeshFilter>();
    this._meshRenderer = this.GetComponent<MeshRenderer>();
  }

  public virtual Mesh CreateMesh()
  {
    Mesh mesh = new Mesh();
    mesh.name = "Dynamic3SliceSprite";
    Vector3[] vector3Array = new Vector3[8]
    {
      new Vector3(-1f, 0.0f, 0.0f),
      new Vector3(1f, 0.0f, 0.0f),
      new Vector3(-1f, 0.0f, 0.0f),
      new Vector3(1f, 0.0f, 0.0f),
      new Vector3(-1f, 1f, 0.0f),
      new Vector3(1f, 1f, 0.0f),
      new Vector3(-1f, 1f, 0.0f),
      new Vector3(1f, 1f, 0.0f)
    };
    int[] numArray = new int[18]
    {
      0,
      2,
      1,
      1,
      2,
      3,
      2,
      4,
      3,
      3,
      4,
      5,
      4,
      6,
      5,
      5,
      6,
      7
    };
    Vector2[] vector2Array = new Vector2[8]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 0.25f),
      new Vector2(1f, 0.25f),
      new Vector2(0.0f, 0.75f),
      new Vector2(1f, 0.75f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 1f)
    };
    mesh.vertices = vector3Array;
    mesh.triangles = numArray;
    mesh.uv = vector2Array;
    mesh.bounds = new Bounds(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(10f, 5000f, 0.1f));
    mesh.hideFlags = HideFlags.DontSaveInEditor | HideFlags.DontUnloadUnusedAsset;
    return mesh;
  }

  public virtual void Refresh()
  {
    if ((Object) this._meshRenderer == (Object) null)
      return;
    if (Parametric3SliceSpriteController._materialPropertyBlock == null)
      Parametric3SliceSpriteController._materialPropertyBlock = new MaterialPropertyBlock();
    Color color = this.color;
    color.a *= this.alphaMultiplier;
    if ((double) color.a < (double) this.minAlpha)
      color.a = this.minAlpha;
    Parametric3SliceSpriteController._materialPropertyBlock.SetColor(Parametric3SliceSpriteController._colorID, color);
    Parametric3SliceSpriteController._materialPropertyBlock.SetFloat(Parametric3SliceSpriteController._alphaStartID, this.alphaStart);
    Parametric3SliceSpriteController._materialPropertyBlock.SetFloat(Parametric3SliceSpriteController._alphaEndID, this.alphaEnd);
    Parametric3SliceSpriteController._materialPropertyBlock.SetFloat(Parametric3SliceSpriteController._widthStartID, this.widthStart);
    Parametric3SliceSpriteController._materialPropertyBlock.SetFloat(Parametric3SliceSpriteController._widthEndID, this.widthEnd);
    Parametric3SliceSpriteController._materialPropertyBlock.SetVector(Parametric3SliceSpriteController._sizeParamsID, new Vector4(this.width * this._widthMultiplier, this.length, this.center, this.width * 2f * this._widthMultiplier));
    this._meshRenderer.SetPropertyBlock(Parametric3SliceSpriteController._materialPropertyBlock);
  }
}
