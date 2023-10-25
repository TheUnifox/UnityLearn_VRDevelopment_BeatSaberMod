// Decompiled with JetBrains decompiler
// Type: Billboard
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

[ExecuteInEditMode]
public class Billboard : MonoBehaviour
{
  [SerializeField]
  protected Billboard.RotationMode _rotationMode;
  [SerializeField]
  protected bool _flipDirection;
  protected Transform _transform;

  public virtual void Awake() => this._transform = this.transform;

  public virtual void OnWillRenderObject()
  {
    Vector3 position1 = Camera.current.transform.position;
    Vector3 position2 = this._transform.position;
    switch (this._rotationMode)
    {
      case Billboard.RotationMode.XAxis:
        position1.x = position2.x;
        break;
      case Billboard.RotationMode.YAxis:
        position1.y = position2.y;
        break;
      case Billboard.RotationMode.ZAxis:
        position1.z = position2.z;
        break;
    }
    if (this._flipDirection)
      this._transform.LookAt(2f * position2 - position1);
    else
      this._transform.LookAt(position1);
  }

  public enum RotationMode
  {
    AllAxis,
    XAxis,
    YAxis,
    ZAxis,
  }
}
