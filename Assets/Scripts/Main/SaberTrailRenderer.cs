// Decompiled with JetBrains decompiler
// Type: SaberTrailRenderer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class SaberTrailRenderer : MonoBehaviour
{
  [SerializeField]
  protected MeshRenderer _meshRenderer;
  [SerializeField]
  protected MeshFilter _meshFilter;
  [DoesNotRequireDomainReloadInit]
  protected static readonly Bounds _bounds = new Bounds(Vector3.zero, new Vector3(100f, 100f, 100f));
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _saberTravelledDistanceId = Shader.PropertyToID("_TravelledDistance");
  protected const float kMinMotionBlurSpeed = 2.5f;
  protected const float kMotionBlurStrength = 0.8f;
  protected Mesh _mesh;
  protected Vector3[] _vertices;
  protected int[] _indices;
  protected Vector2[] _uvs;
  protected Color[] _colors;
  protected float _trailWidth;
  protected float _trailDuration;
  protected float _segmentDuration;
  protected int _granularity;
  protected float _whiteSectionMaxDuration;

  public virtual void Init(
    float trailWidth,
    float trailDuration,
    int granularity,
    float whiteSectionMaxDuration)
  {
    this._trailWidth = trailWidth;
    this._trailDuration = trailDuration;
    this._segmentDuration = trailDuration / (float) granularity;
    this._granularity = granularity;
    this._whiteSectionMaxDuration = whiteSectionMaxDuration;
    this._mesh = new Mesh();
    this._meshFilter.mesh = this._mesh;
    this._mesh.MarkDynamic();
    int length1 = this._granularity * 3;
    int length2 = (this._granularity - 1) * 12;
    this._vertices = new Vector3[length1];
    this._uvs = new Vector2[length1];
    this._colors = new Color[length1];
    this._indices = new int[length2];
    this.UpdateIndices();
  }

  public virtual void OnDestroy() => EssentialHelpers.SafeDestroy((Object) this._mesh);

  public virtual void OnValidate()
  {
    if ((Object) this._meshFilter == (Object) null)
      this._meshFilter = this.GetComponent<MeshFilter>();
    if (!((Object) this._meshRenderer == (Object) null))
      return;
    this._meshRenderer = this.GetComponent<MeshRenderer>();
  }

  public virtual void OnEnable()
  {
    if (!(bool) (Object) this._meshRenderer)
      return;
    this._meshRenderer.enabled = true;
  }

  public virtual void OnDisable()
  {
    if (!(bool) (Object) this._meshRenderer)
      return;
    this._meshRenderer.enabled = false;
  }

  public virtual void SetTrailWidth(float width) => this._trailWidth = width;

  public virtual void UpdateMesh(TrailElementCollection trailElementCollection, Color color)
  {
    this.UpdateVertices(trailElementCollection, color);
    this._mesh.vertices = this._vertices;
    this._mesh.uv = this._uvs;
    this._mesh.colors = this._colors;
    this._mesh.triangles = this._indices;
    this._mesh.bounds = SaberTrailRenderer._bounds;
  }

  protected virtual void UpdateVertices(TrailElementCollection trailElementCollection, Color color)
  {
    TrailElementCollection.InterpolationState lerpState = new TrailElementCollection.InterpolationState();
    Vector3 position1;
    Vector3 normal;
    float time;
    trailElementCollection.Interpolate(0.0f, ref lerpState, out position1, out normal, out time);
    for (int index1 = 0; index1 < this._granularity; ++index1)
    {
      int index2 = index1 * 3;
      float num1 = TimeHelper.time - time;
      float num2 = num1 / this._trailDuration;
      Vector2 zero = Vector2.zero;
      Vector3 vector3_1 = position1 + normal * (this._trailWidth * 0.5f);
      Vector3 vector3_2 = position1 - normal * (this._trailWidth * 0.5f);
      Vector3 position2;
      trailElementCollection.Interpolate(((float) index1 + 1f) / (float) this._granularity, ref lerpState, out position2, out normal, out time);
      Color color1;
      if ((double) num1 < (double) this._whiteSectionMaxDuration)
      {
        float num3 = (float) (1.0 - (double) num1 / (double) this._whiteSectionMaxDuration);
        float num4 = (float) ((double) (2.5f / Mathf.Max((position2 - position1).magnitude / this._segmentDuration, 2.5f)) * 0.800000011920929 + 1.0 - 0.800000011920929);
        color1 = Color.LerpUnclamped(color, Color.white, num3 * num4);
      }
      else
        color1 = color;
      this._vertices[index2] = vector3_1;
      this._colors[index2] = color1;
      zero.x = 0.0f;
      zero.y = num2;
      this._uvs[index2] = zero;
      this._vertices[index2 + 1] = position1;
      this._colors[index2 + 1] = color1;
      zero.x = 0.5f;
      zero.y = num2;
      this._uvs[index2 + 1] = zero;
      this._vertices[index2 + 2] = vector3_2;
      this._colors[index2 + 2] = color1;
      zero.x = 1f;
      zero.y = num2;
      this._uvs[index2 + 2] = zero;
      position1 = position2;
    }
  }

  public virtual void UpdateIndices()
  {
    for (int index1 = 0; index1 < this._granularity - 1; ++index1)
    {
      int num1 = index1 * 3;
      int num2 = (index1 + 1) * 3;
      int index2 = index1 * 12;
      this._indices[index2] = num2;
      this._indices[index2 + 1] = num2 + 1;
      this._indices[index2 + 2] = num1;
      this._indices[index2 + 3] = num2 + 1;
      this._indices[index2 + 4] = num1 + 1;
      this._indices[index2 + 5] = num1;
      this._indices[index2 + 6] = num2 + 1;
      this._indices[index2 + 7] = num2 + 2;
      this._indices[index2 + 8] = num1 + 1;
      this._indices[index2 + 9] = num2 + 2;
      this._indices[index2 + 10] = num1 + 2;
      this._indices[index2 + 11] = num1 + 1;
    }
  }
}
