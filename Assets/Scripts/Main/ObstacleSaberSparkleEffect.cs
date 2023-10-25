// Decompiled with JetBrains decompiler
// Type: ObstacleSaberSparkleEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class ObstacleSaberSparkleEffect : MonoBehaviour
{
  [SerializeField]
  protected ParticleSystem _sparkleParticleSystem;
  [SerializeField]
  protected ParticleSystem _burnParticleSystem;
  protected ParticleSystem.EmissionModule _sparkleParticleSystemEmissionModule;
  protected ParticleSystem.EmissionModule _burnParticleSystemEmissionModule;

  public Color color
  {
    set
    {
      this._sparkleParticleSystem.main.startColor = (ParticleSystem.MinMaxGradient) value;
      this._burnParticleSystem.main.startColor = (ParticleSystem.MinMaxGradient) value;
    }
  }

  public virtual void Awake()
  {
    this._sparkleParticleSystemEmissionModule = this._sparkleParticleSystem.emission;
    this._burnParticleSystemEmissionModule = this._burnParticleSystem.emission;
    this._sparkleParticleSystemEmissionModule.enabled = false;
    this._burnParticleSystemEmissionModule.enabled = false;
  }

  public virtual void SetPositionAndRotation(Vector3 pos, Quaternion rot) => this.transform.SetPositionAndRotation(pos, rot);

  public virtual void StartEmission()
  {
    if (this._burnParticleSystemEmissionModule.enabled)
      return;
    this._sparkleParticleSystemEmissionModule.enabled = true;
    this._burnParticleSystemEmissionModule.enabled = true;
  }

  public virtual void StopEmission()
  {
    if (!this._burnParticleSystemEmissionModule.enabled)
      return;
    this._burnParticleSystem.Clear();
    this._sparkleParticleSystemEmissionModule.enabled = false;
    this._burnParticleSystemEmissionModule.enabled = false;
  }
}
