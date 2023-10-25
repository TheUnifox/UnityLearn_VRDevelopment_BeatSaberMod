// Decompiled with JetBrains decompiler
// Type: PhysicsFollow
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]
public class PhysicsFollow : MonoBehaviour
{
  public Transform _targetTransform;
  public Vector3 _offset;
  public float _friction = 0.9f;
  public float _elasticity = 10f;
  protected Rigidbody2D _rigidBody2D;

  public virtual void Start()
  {
    this._rigidBody2D = this.GetComponent<Rigidbody2D>();
    this.transform.position = this._targetTransform.position + this._offset;
  }

  public virtual void FixedUpdate() => this._rigidBody2D.velocity = (Vector2) (((Vector3) this._rigidBody2D.velocity + (this._targetTransform.position + this._offset - (Vector3) this._rigidBody2D.position) * this._elasticity * Time.fixedDeltaTime) * this._friction);
}
