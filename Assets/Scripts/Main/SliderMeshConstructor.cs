// Decompiled with JetBrains decompiler
// Type: SliderMeshConstructor
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class SliderMeshConstructor : MonoBehaviour
{
  [SerializeField]
  private MeshFilter _meshFilter;
  protected Vector3[] reusableVerts;
  protected Vector2[] reusableUvs;
  protected Vector3[] reusableNormals;
  protected int[] reusableTriangles;
  private Mesh _mesh;

  public Mesh mesh => this._mesh;

  public void CreateMeshIfNonExisting()
  {
    if (!((Object) this._mesh == (Object) null))
      return;
    this._mesh = new Mesh();
    this._meshFilter.sharedMesh = this._mesh;
  }

  public void CreateSliderMesh(VertexPath path)
  {
    int vertexCount = this.GetVertexCount(path);
    if (this.reusableVerts == null || this.reusableVerts.Length != vertexCount)
    {
      this.reusableVerts = new Vector3[path.vertexCount * 4];
      this.reusableUvs = new Vector2[this.reusableVerts.Length];
      this.reusableNormals = new Vector3[this.reusableVerts.Length];
      this.reusableTriangles = new int[this.GetTrianglesCount(path)];
    }
    this.CreateSliderMeshInternal(path);
    this._mesh.Clear();
    this._mesh.vertices = this.reusableVerts;
    this._mesh.uv = this.reusableUvs;
    this._mesh.normals = this.reusableNormals;
    this._mesh.subMeshCount = 1;
    this._mesh.SetTriangles(this.reusableTriangles, 0);
  }

  protected abstract void CreateSliderMeshInternal(VertexPath path);

  protected abstract int GetVertexCount(VertexPath path);

  protected abstract int GetTrianglesCount(VertexPath path);
}
