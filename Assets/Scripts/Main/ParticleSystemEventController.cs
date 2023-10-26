// Decompiled with JetBrains decompiler
// Type: ParticleSystemEventController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class ParticleSystemEventController : MonoBehaviour
{
  [SerializeField]
  protected ParticleSystem _particleSystem;
  [Space]
  [SerializeField]
  protected float _particleSystemFullDuration;
  [CompilerGenerated]
  protected float m_CstartTime;
  protected const float kMaxSimDelta = 0.0333333351f;

  public float startTime
  {
    get => this.m_CstartTime;
    private set => this.m_CstartTime = value;
  }

  public float endTime => this.startTime + this._particleSystemFullDuration;

  public virtual void Init(float startTime)
  {
    this.startTime = startTime;
    this._particleSystem.Simulate(0.0f, true, true);
  }

    public virtual void InitSpeed(float simulationSpeedMultiplier)
    {
        var main = this._particleSystem.main;
        main.simulationSpeed = 1f * simulationSpeedMultiplier;
    }

    public virtual void Play() => this._particleSystem.Play();

  public virtual void Pause() => this._particleSystem.Pause();

  public virtual void Stop() => this._particleSystem.Stop();

  public virtual void ManualUpdate(float time, float deltaTime)
  {
    if ((double) deltaTime == 0.0)
      return;
    if ((double) deltaTime > 0.0 && (double) deltaTime < 0.033333335071802139)
      this._particleSystem.Simulate(deltaTime, true, false);
    else
      this._particleSystem.Simulate(time - this.startTime, true, true);
  }

  public class Pool : MonoMemoryPool<ParticleSystemEventController>
  {
  }
}
