// Decompiled with JetBrains decompiler
// Type: EnableEmmisionOnVisible
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class EnableEmmisionOnVisible : MonoBehaviour
{
  [SerializeField]
  protected ParticleSystem[] _particleSystems;
  protected ParticleSystem.EmissionModule[] _emmisionModules;

  public virtual void Awake()
  {
    this._emmisionModules = new ParticleSystem.EmissionModule[this._particleSystems.Length];
    for (int index = 0; index < this._particleSystems.Length; ++index)
    {
      this._emmisionModules[index] = this._particleSystems[index].emission;
      this._emmisionModules[index].enabled = false;
    }
  }

  public virtual void OnBecameVisible()
  {
    for (int index = 0; index < this._particleSystems.Length; ++index)
      this._emmisionModules[index].enabled = true;
  }

  public virtual void OnBecameInvisible()
  {
    for (int index = 0; index < this._particleSystems.Length; ++index)
      this._emmisionModules[index].enabled = false;
  }
}
