// Decompiled with JetBrains decompiler
// Type: NoteCutParticlesEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class NoteCutParticlesEffect : MonoBehaviour
{
  [SerializeField]
  protected ParticleSystem _sparklesPS;
  [SerializeField]
  protected ParticleSystem _explosionPS;
  [SerializeField]
  protected ParticleSystem _explosionCorePS;
  [SerializeField]
  protected ParticleSystem _explosionPrePassBloomPS;
  protected ParticleSystem.EmitParams _sparklesPSEmitParams;
  protected ParticleSystem.MainModule _sparklesPSMainModule;
  protected ParticleSystem.ShapeModule _sparklesPSShapeModule;
  protected ParticleSystem.MinMaxCurve _sparklesLifetimeMinMaxCurve;
  protected ParticleSystem.EmitParams _explosionPSEmitParams;
  protected ParticleSystem.EmitParams _explosionCorePSEmitParams;
  protected ParticleSystem.MainModule _explosionCorePSMainModule;
  protected ParticleSystem.ShapeModule _explosionCorePSShapeModule;
  protected ParticleSystem.ShapeModule _explosionPrePassBloomPSShapeModule;

  public virtual void Awake()
  {
    this._sparklesPSEmitParams = new ParticleSystem.EmitParams()
    {
      applyShapeToPosition = true
    };
    this._sparklesPSMainModule = this._sparklesPS.main;
    this._sparklesPSShapeModule = this._sparklesPS.shape;
    this._sparklesLifetimeMinMaxCurve = new ParticleSystem.MinMaxCurve(0.6f, 1f);
    this._explosionPSEmitParams = new ParticleSystem.EmitParams()
    {
      applyShapeToPosition = true
    };
    this._explosionCorePSEmitParams = new ParticleSystem.EmitParams()
    {
      applyShapeToPosition = true
    };
    this._explosionCorePSMainModule = this._explosionCorePS.main;
    this._explosionCorePSShapeModule = this._explosionCorePS.shape;
    this._explosionPrePassBloomPSShapeModule = this._explosionPrePassBloomPS.shape;
  }

  public virtual void SpawnParticles(
    Vector3 cutPoint,
    Vector3 cutNormal,
    Vector3 saberDir,
    float saberSpeed,
    Vector3 noteMovementVec,
    Color32 color,
    int sparkleParticlesCount,
    int explosionParticlesCount,
    float lifetimeMultiplier)
  {
    Quaternion quaternion = new Quaternion();
    quaternion.SetLookRotation(cutNormal, saberDir);
    this._sparklesPSEmitParams.startColor = color;
    this._sparklesPSEmitParams.position = cutPoint - saberDir * 0.2f;
    this._sparklesPSShapeModule.rotation = (quaternion * Quaternion.Euler(new Vector3(0.0f, 0.0f, 40f))).eulerAngles;
    this._sparklesLifetimeMinMaxCurve.constantMin = 0.8f * lifetimeMultiplier;
    this._sparklesLifetimeMinMaxCurve.constantMax = 1.2f * lifetimeMultiplier;
    this._sparklesPSMainModule.startLifetime = this._sparklesLifetimeMinMaxCurve;
    this._sparklesPS.Emit(this._sparklesPSEmitParams, sparkleParticlesCount);
    this._explosionPSEmitParams.startColor = color;
    this._explosionPSEmitParams.position = cutPoint;
    this._explosionPS.Emit(this._explosionPSEmitParams, explosionParticlesCount);
    Vector3 eulerAngles = (quaternion * Quaternion.Euler(new Vector3(-90f, 0.0f, 0.0f))).eulerAngles;
    this._explosionCorePSEmitParams.startColor = color;
    this._explosionCorePSEmitParams.position = cutPoint - saberDir * 0.75f;
    this._explosionCorePSEmitParams.rotation3D = eulerAngles;
    this._explosionCorePSShapeModule.rotation = eulerAngles;
    this._explosionCorePSMainModule.startSpeed = (ParticleSystem.MinMaxCurve) saberSpeed;
    this._explosionCorePS.Emit(this._explosionCorePSEmitParams, 1);
    this._explosionPrePassBloomPSShapeModule.rotation = eulerAngles;
    this._explosionPrePassBloomPS.Emit(this._explosionCorePSEmitParams, 1);
  }
}
