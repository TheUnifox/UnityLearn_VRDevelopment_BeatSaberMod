// Decompiled with JetBrains decompiler
// Type: AlwaysVisibleQuad
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using UnityEngine;

[ExecuteInEditMode]
public class AlwaysVisibleQuad : MonoBehaviour
{
  private UnityEngine.Mesh _mesh;

  protected void OnEnable()
  {
    Vector3[] vector3Array = new Vector3[4]
    {
      new Vector3(-1f, -1f, 0.0f),
      new Vector3(1f, -1f, 0.0f),
      new Vector3(1f, 1f, 0.0f),
      new Vector3(-1f, 1f, 0.0f)
    };
    int[] numArray = new int[6]{ 0, 1, 2, 2, 3, 0 };
    this._mesh = new UnityEngine.Mesh();
    this._mesh.hideFlags = HideFlags.HideAndDontSave;
    this.GetComponent<MeshFilter>().mesh = this._mesh;
    this._mesh.vertices = vector3Array;
    this._mesh.triangles = numArray;
    this._mesh.bounds = new Bounds(Vector3.zero, new Vector3(1E+08f, 1E+08f, 1E+08f));
  }

  protected void OnDisable() => EssentialHelpers.SafeDestroy((Object) this._mesh);
}
