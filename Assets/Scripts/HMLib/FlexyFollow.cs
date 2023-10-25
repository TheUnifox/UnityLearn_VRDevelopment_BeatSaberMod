// Decompiled with JetBrains decompiler
// Type: FlexyFollow
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class FlexyFollow : MonoBehaviour
{
  public GameObject _followObject;
  public float _followSpeed = 2f;
  public Vector3 _offset;
  public bool _fixedXOffset;
  public bool _fixedYOffset;
  public bool _fixedZOffset;
  public bool _useLocalPosition;
  protected Transform _followTransform;
  protected Transform _transform;

  public virtual void Start()
  {
    this._transform = this.transform;
    this._followTransform = this._followObject.transform;
    if (this._useLocalPosition)
      this._transform.position = this._followTransform.position + this._offset;
    else
      this._transform.localPosition = this._followTransform.position + this._offset;
  }

  public virtual void LateUpdate()
  {
    Vector3 a = this._useLocalPosition ? this._transform.localPosition : this._transform.position;
    Vector3 vector3_1 = this._followTransform.position + this._offset;
    Vector3 b = vector3_1;
    double t = (double) Time.deltaTime * (double) this._followSpeed;
    Vector3 vector3_2 = Vector3.Lerp(a, b, (float) t);
    if (this._fixedXOffset)
      vector3_2.x = vector3_1.x;
    if (this._fixedYOffset)
      vector3_2.y = vector3_1.y;
    if (this._fixedZOffset)
      vector3_2.z = vector3_1.z;
    if (this._useLocalPosition)
      this._transform.localPosition = vector3_2;
    else
      this._transform.position = vector3_2;
  }
}
