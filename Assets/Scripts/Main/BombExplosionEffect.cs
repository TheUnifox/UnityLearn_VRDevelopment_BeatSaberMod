// Decompiled with JetBrains decompiler
// Type: BombExplosionEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class BombExplosionEffect : MonoBehaviour
{
  [SerializeField]
  protected ParticleSystem _debrisPS;
  [SerializeField]
  protected ParticleSystem _explosionPS;
  [SerializeField]
  protected int _debrisCount = 40;
  [SerializeField]
  protected int _explosionParticlesCount = 70;
  protected ParticleSystem.EmitParams _emitParams;
  protected ParticleSystem.EmitParams _explosionPSEmitParams;

  public virtual void Awake()
  {
    this._emitParams = new ParticleSystem.EmitParams();
    this._emitParams.applyShapeToPosition = true;
  }

  public virtual void SpawnExplosion(Vector3 pos)
  {
    this._emitParams.position = pos;
    this._debrisPS.Emit(this._emitParams, this._debrisCount);
    this._explosionPS.Emit(this._emitParams, this._explosionParticlesCount);
  }
}
