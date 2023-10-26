// Decompiled with JetBrains decompiler
// Type: ShockwaveEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ShockwaveEffect : MonoBehaviour
{
  [SerializeField]
  protected ParticleSystem _shockwavePS;
  [SerializeField]
  protected IntSO _maxShockwaveParticles;
  protected ParticleSystem.EmitParams _shockwavePSEmitParams;
  protected float _prevShockwaveParticleSpawnTime;

  public virtual void Start()
  {
    ParticleSystem.MainModule main = this._shockwavePS.main;
    main.maxParticles = (int) (ObservableVariableSO<int>) this._maxShockwaveParticles;
    this._shockwavePSEmitParams = new ParticleSystem.EmitParams();
    this._shockwavePSEmitParams.position = Vector3.zero;
    this._shockwavePS.Emit(this._shockwavePSEmitParams, 1);
  }

  public virtual void SpawnShockwave(Vector3 pos)
  {
    if (!this.isActiveAndEnabled)
      return;
    float timeSinceLevelLoad = Time.timeSinceLevelLoad;
    if ((double) timeSinceLevelLoad - (double) this._prevShockwaveParticleSpawnTime <= 0.059999998658895493)
      return;
    this._shockwavePSEmitParams.position = pos;
    this._shockwavePS.Emit(this._shockwavePSEmitParams, 1);
    this._prevShockwaveParticleSpawnTime = timeSinceLevelLoad;
  }
}
