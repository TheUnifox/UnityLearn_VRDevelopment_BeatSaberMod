// Decompiled with JetBrains decompiler
// Type: SliderMeshConstructorCrossedStrips
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class SliderMeshConstructorCrossedStrips : SliderMeshConstructor
{
  protected readonly int[] _triangleMap = new int[6]
  {
    0,
    4,
    1,
    1,
    4,
    5
  };

  protected override void CreateSliderMeshInternal(VertexPath path)
  {
    int index1 = 0;
    int num1 = 0;
    int num2 = 0;
    for (int vertexCount = path.vertexCount; num2 < vertexCount; ++num2)
    {
      Vector3 position;
      Vector3 tangent;
      Vector3 normal;
      path.GetVertex(num2, out position, out tangent, out normal);
      Vector3 vector3 = Vector3.Cross(normal, tangent);
      this.reusableVerts[index1] = position;
      this.reusableVerts[index1 + 1] = position;
      this.reusableVerts[index1 + 2] = position;
      this.reusableVerts[index1 + 3] = position;
      float y = path.TimeAtPoint(num2);
      this.reusableUvs[index1] = new Vector2(0.0f, y);
      this.reusableUvs[index1 + 1] = new Vector2(1f, y);
      this.reusableUvs[index1 + 2] = new Vector2(0.0f, y);
      this.reusableUvs[index1 + 3] = new Vector2(1f, y);
      this.reusableNormals[index1] = normal;
      this.reusableNormals[index1 + 1] = normal;
      this.reusableNormals[index1 + 2] = vector3;
      this.reusableNormals[index1 + 3] = vector3;
      if (num2 < vertexCount - 1)
      {
        for (int index2 = 0; index2 < this._triangleMap.Length; ++index2)
        {
          this.reusableTriangles[num1 + index2] = (index1 + this._triangleMap[index2]) % this.reusableVerts.Length;
          this.reusableTriangles[num1 + this._triangleMap.Length + index2] = (index1 + this._triangleMap[index2] + 2) % this.reusableVerts.Length;
        }
      }
      index1 += 4;
      num1 += this._triangleMap.Length * 2;
    }
  }

  protected override int GetVertexCount(VertexPath path) => path.vertexCount * 4;

  protected override int GetTrianglesCount(VertexPath path) => 12 * (path.vertexCount - 1);
}
