// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ArcTubePhysicsMeshConstructor
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public class ArcTubePhysicsMeshConstructor : MonoBehaviour
  {
    [SerializeField]
    private MeshCollider _meshCollider;
    [SerializeField]
    private SliderMeshController _sliderMeshController;
    [Space]
    [SerializeField]
    private float _radius;
    private Mesh _mesh;
    [DoesNotRequireDomainReloadInit]
    private static readonly int[] _triangleMap = new int[24]
    {
      1,
      4,
      0,
      1,
      5,
      4,
      2,
      5,
      1,
      2,
      6,
      5,
      3,
      6,
      2,
      3,
      7,
      6,
      0,
      7,
      3,
      0,
      4,
      7
    };
    [DoesNotRequireDomainReloadInit]
    private static readonly (int x, int y)[] _directions = new (int, int)[4]
    {
      (-1, -1),
      (-1, 1),
      (1, 1),
      (1, -1)
    };
    private Vector3[] _verts;
    private int[] _triangles;

    public void CreatePhysicsMeshFromController()
    {
      VertexPath vertexPath = this._sliderMeshController.pathsHolder.vertexPath;
      int length = vertexPath.vertexCount * 4;
      if (this._verts == null || this._verts.Length != length)
      {
        this._verts = new Vector3[length];
        this._triangles = new int[(4 + 8 * (vertexPath.vertexCount - 1)) * 3];
      }
      this._meshCollider.sharedMesh = (Mesh) null;
      if ((Object) this._mesh == (Object) null)
        this._mesh = new Mesh();
      Vector3 position;
      Vector3 tangent;
      Vector3 normal;
      vertexPath.GetVertex(0, out position, out tangent, out normal);
      Vector3 playerArea1 = this.SoftClampToPlayerArea(position);
      Vector3 vector3_1 = Vector3.Cross(normal, tangent);
      for (int index = 0; index < ArcTubePhysicsMeshConstructor._directions.Length; ++index)
        this._verts[index] = playerArea1 + normal * ((float) ArcTubePhysicsMeshConstructor._directions[index].x * this._radius) + vector3_1 * ((float) ArcTubePhysicsMeshConstructor._directions[index].y * this._radius);
      this._triangles[0] = 0;
      this._triangles[1] = 2;
      this._triangles[2] = 1;
      this._triangles[3] = 0;
      this._triangles[4] = 3;
      this._triangles[5] = 2;
      int num1 = 4;
      int index1 = 6;
      int num2 = 0;
      for (int index2 = 1; index2 < vertexPath.vertexCount; ++index2)
      {
        vertexPath.GetVertex(index2, out position, out tangent, out normal);
        Vector3 playerArea2 = this.SoftClampToPlayerArea(vertexPath.GetPoint(index2));
        Vector3 vector3_2 = Vector3.Cross(normal, tangent);
        for (int index3 = 0; index3 < ArcTubePhysicsMeshConstructor._directions.Length; ++index3)
          this._verts[num1 + index3] = playerArea2 + normal * ((float) ArcTubePhysicsMeshConstructor._directions[index3].x * this._radius) + vector3_2 * ((float) ArcTubePhysicsMeshConstructor._directions[index3].y * this._radius);
        for (int index4 = 0; index4 < ArcTubePhysicsMeshConstructor._triangleMap.Length; ++index4)
          this._triangles[index1 + index4] = num2 + ArcTubePhysicsMeshConstructor._triangleMap[index4];
        num1 += 4;
        num2 += 4;
        index1 += ArcTubePhysicsMeshConstructor._triangleMap.Length;
      }
      int num3 = num1 - 4;
      this._triangles[index1] = num3;
      this._triangles[index1 + 1] = num3 + 2;
      this._triangles[index1 + 2] = num3 + 1;
      this._triangles[index1 + 3] = num3;
      this._triangles[index1 + 4] = num3 + 3;
      this._triangles[index1 + 5] = num3 + 2;
      this._mesh.Clear();
      this._mesh.vertices = this._verts;
      this._mesh.subMeshCount = 1;
      this._mesh.SetTriangles(this._triangles, 0);
      this._meshCollider.sharedMesh = this._mesh;
    }

    private Vector3 SoftClampToPlayerArea(Vector3 point)
    {
      point.x = ArcTubePhysicsMeshConstructor.SoftClamp(point.x, -2.5f, 2.5f);
      point.y = ArcTubePhysicsMeshConstructor.SoftClamp(point.y, this._radius, 3f);
      return point;
    }

    private static float SoftClamp(float x, float a, float b) => Mathf.SmoothStep(0.0f, 1f, (float) (0.66666668653488159 * ((double) x - (double) a) / ((double) b - (double) a) + 0.1666666716337204)) * (b - a) + a;
  }
}
