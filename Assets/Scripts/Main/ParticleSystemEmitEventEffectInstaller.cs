// Decompiled with JetBrains decompiler
// Type: ParticleSystemEmitEventEffectInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class ParticleSystemEmitEventEffectInstaller : MonoInstaller
{
  [SerializeField]
  protected ParticleSystemEventController _particleSystemEventControllerPrefab;
  [SerializeField]
  protected int _particleSystemEventControllerInitialSize;

  public override void InstallBindings() => this.Container.BindMemoryPool<ParticleSystemEventController, ParticleSystemEventController.Pool>().WithInitialSize(this._particleSystemEventControllerInitialSize).ExpandByOneAtATime().FromComponentInNewPrefab((Object) this._particleSystemEventControllerPrefab);
}
