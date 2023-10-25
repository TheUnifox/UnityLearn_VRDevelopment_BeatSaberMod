// Decompiled with JetBrains decompiler
// Type: CustomBoundingBox
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class CustomBoundingBox : MonoBehaviour
{
  [SerializeField]
  protected MeshFilter _meshFilter;
  [SerializeField]
  protected Vector3 _boundingBoxCenter;
  [SerializeField]
  protected Vector3 _boundingBoxSize = Vector3.one;
  [SerializeField]
  protected MeshRenderer _meshRenderer;

  public virtual void Awake() => this._meshFilter.mesh.bounds = new Bounds(this._boundingBoxCenter, this._boundingBoxSize * 2f);
}
