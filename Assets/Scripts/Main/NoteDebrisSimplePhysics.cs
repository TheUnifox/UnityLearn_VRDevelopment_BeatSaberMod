// Decompiled with JetBrains decompiler
// Type: NoteDebrisSimplePhysics
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class NoteDebrisSimplePhysics : NoteDebrisPhysics
{
  protected Vector3 _currentForce;
  protected Vector3 _currentTorque;
  protected Transform _transform;
  protected Vector3 _gravity;
  protected bool _firstUpdate;

  public override Vector3 position => this._transform.position;

  public virtual void Start()
  {
    this._transform = this.transform;
    this._gravity = Physics.gravity;
  }

  public virtual void LateUpdate()
  {
    if (this._firstUpdate)
    {
      this._firstUpdate = false;
    }
    else
    {
      float deltaTime = Time.deltaTime;
      this._transform.position += this._currentForce * deltaTime;
      this._transform.rotation *= Quaternion.Euler(this._currentTorque * deltaTime);
      this._currentForce += this._gravity * deltaTime;
    }
  }

  public override void Init(Vector3 force, Vector3 torque)
  {
    this._currentForce = force;
    this._currentTorque = torque * 57.29578f;
    this._firstUpdate = true;
  }

  public override void AddVelocity(Vector3 force) => this._currentForce += force;
}
