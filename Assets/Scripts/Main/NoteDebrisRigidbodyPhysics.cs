// Decompiled with JetBrains decompiler
// Type: NoteDebrisRigidbodyPhysics
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class NoteDebrisRigidbodyPhysics : NoteDebrisPhysics
{
  [SerializeField]
  protected Rigidbody _rigidbody;
  [SerializeField]
  protected NoteDebrisSimplePhysics _simplePhysics;
  protected bool _firstUpdate;

  public override Vector3 position => this._rigidbody.position;

  public virtual void FixedUpdate()
  {
    if (this._firstUpdate)
    {
      this._firstUpdate = false;
    }
    else
    {
      this._simplePhysics.enabled = false;
      this.enabled = false;
    }
  }

  public override void Init(Vector3 force, Vector3 torque)
  {
    this._firstUpdate = true;
    this._rigidbody.velocity = force;
    this._rigidbody.angularVelocity = torque;
    this._simplePhysics.Init(force, torque);
    this._simplePhysics.enabled = true;
    this.enabled = true;
  }

  public override void AddVelocity(Vector3 force)
  {
    this._rigidbody.AddForce(force, ForceMode.VelocityChange);
    this._simplePhysics.AddVelocity(force);
  }
}
