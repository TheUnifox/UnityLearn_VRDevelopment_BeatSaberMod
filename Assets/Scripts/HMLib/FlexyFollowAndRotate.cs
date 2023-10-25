// Decompiled with JetBrains decompiler
// Type: FlexyFollowAndRotate
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class FlexyFollowAndRotate : MonoBehaviour
{
  [SerializeField]
  protected Transform _target;
  [SerializeField]
  protected float _smooth = 4f;

  public virtual void Update()
  {
    Vector3 position = this.transform.position;
    Quaternion rotation = this.transform.rotation;
    this.transform.position = Vector3.Slerp(position, this._target.position, Time.deltaTime * this._smooth);
    this.transform.rotation = Quaternion.Slerp(rotation, this._target.rotation, Time.deltaTime * this._smooth);
  }
}
