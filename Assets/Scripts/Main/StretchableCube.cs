// Decompiled with JetBrains decompiler
// Type: StretchableCube
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[RequireComponent(typeof (MeshRenderer), typeof (MeshFilter))]
public class StretchableCube : MonoBehaviour
{
  protected const float kLength = 1f;
  protected const float kWidth = 1f;
  protected const float kHeight = 1f;
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3[] vertices = new Vector3[24]
  {
    StretchableCube.p0,
    StretchableCube.p1,
    StretchableCube.p2,
    StretchableCube.p3,
    StretchableCube.p7,
    StretchableCube.p4,
    StretchableCube.p0,
    StretchableCube.p3,
    StretchableCube.p4,
    StretchableCube.p5,
    StretchableCube.p1,
    StretchableCube.p0,
    StretchableCube.p6,
    StretchableCube.p7,
    StretchableCube.p3,
    StretchableCube.p2,
    StretchableCube.p5,
    StretchableCube.p6,
    StretchableCube.p2,
    StretchableCube.p1,
    StretchableCube.p7,
    StretchableCube.p6,
    StretchableCube.p5,
    StretchableCube.p4
  };
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 up = Vector3.up;
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 down = Vector3.down;
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 front = Vector3.forward;
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 back = Vector3.back;
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 left = Vector3.left;
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3 right = Vector3.right;
  [DoesNotRequireDomainReloadInit]
  protected static readonly Vector3[] normals = new Vector3[24]
  {
    StretchableCube.down,
    StretchableCube.down,
    StretchableCube.down,
    StretchableCube.down,
    StretchableCube.left,
    StretchableCube.left,
    StretchableCube.left,
    StretchableCube.left,
    StretchableCube.front,
    StretchableCube.front,
    StretchableCube.front,
    StretchableCube.front,
    StretchableCube.back,
    StretchableCube.back,
    StretchableCube.back,
    StretchableCube.back,
    StretchableCube.right,
    StretchableCube.right,
    StretchableCube.right,
    StretchableCube.right,
    StretchableCube.up,
    StretchableCube.up,
    StretchableCube.up,
    StretchableCube.up
  };
  [DoesNotRequireDomainReloadInit]
  protected static readonly int[] triangles = new int[36]
  {
    3,
    1,
    0,
    3,
    2,
    1,
    7,
    5,
    4,
    7,
    6,
    5,
    11,
    9,
    8,
    11,
    10,
    9,
    15,
    13,
    12,
    15,
    14,
    13,
    19,
    17,
    16,
    19,
    18,
    17,
    23,
    21,
    20,
    23,
    22,
    21
  };
  protected Vector2[] _uvs;
  protected Mesh _mesh;

  public virtual void Awake()
  {
    MeshFilter component = this.GetComponent<MeshFilter>();
    this._mesh = this.CreateBox();
    Mesh mesh = this._mesh;
    component.mesh = mesh;
  }

  public virtual void OnDestroy() => EssentialHelpers.SafeDestroy((Object) this._mesh);

  public virtual Mesh CreateBox()
  {
    Mesh box = new Mesh();
    box.name = nameof (StretchableCube);
    this._uvs = new Vector2[24];
    this.RecalculateUVs(this._uvs);
    box.vertices = StretchableCube.vertices;
    box.normals = StretchableCube.normals;
    box.uv = this._uvs;
    box.triangles = StretchableCube.triangles;
    box.RecalculateBounds();
    return box;
  }

  public virtual void RecalculateUVs(Vector2[] uvs)
  {
    Vector3 lossyScale = this.transform.lossyScale;
    Vector2[] vector2Array = new Vector2[6]
    {
      new Vector2(lossyScale.x, lossyScale.z),
      new Vector2(lossyScale.z, lossyScale.y),
      new Vector2(lossyScale.x, lossyScale.y),
      new Vector2(lossyScale.x, lossyScale.y),
      new Vector2(lossyScale.z, lossyScale.y),
      new Vector2(lossyScale.x, lossyScale.z)
    };
    for (int index = 0; index < vector2Array.Length; ++index)
    {
      uvs[index * 4] = vector2Array[index];
      uvs[index * 4 + 1] = new Vector2(0.0f, vector2Array[index].y);
      uvs[index * 4 + 2] = Vector2.zero;
      uvs[index * 4 + 3] = new Vector2(vector2Array[index].x, 0.0f);
    }
  }

  public virtual void RefreshUVs()
  {
    if ((Object) this._mesh == (Object) null)
      return;
    this.RecalculateUVs(this._uvs);
    this._mesh.uv = this._uvs;
  }
}
