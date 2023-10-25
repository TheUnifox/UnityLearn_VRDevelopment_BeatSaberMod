// Decompiled with JetBrains decompiler
// Type: ParticleSystemEmitEventEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ParticleSystemEmitEventEffect : MonoBehaviour
{
  [SerializeField]
  protected BasicBeatmapEventType _beatmapEvent;
  [Space]
  [SerializeField]
  protected Transform _particleSystemParentTransform;
  [SerializeField]
  protected int _particleSystemMaxSpawnedSystems = 4;
  [Inject]
  protected readonly EnvironmentContext _environmentContext;
  [Inject]
  protected readonly DiContainer _diContainer;
  protected ParticleSystemEmitEventEffect.ParticleSystemEmitBehavior _particleSystemEmitBehavior;

  public virtual void Start()
  {
    switch (this._environmentContext)
    {
      case EnvironmentContext.Gameplay:
        this._particleSystemEmitBehavior = (ParticleSystemEmitEventEffect.ParticleSystemEmitBehavior) this._diContainer.Instantiate<ParticleSystemEmitEventEffect.GameplayParticleSystemEmitBehavior>((IEnumerable<object>) new object[3]
        {
          (object) this._beatmapEvent,
          (object) this._particleSystemParentTransform,
          (object) this._particleSystemMaxSpawnedSystems
        });
        break;
      case EnvironmentContext.BeatmapEditor:
        this._particleSystemEmitBehavior = (ParticleSystemEmitEventEffect.ParticleSystemEmitBehavior) this._diContainer.Instantiate<ParticleSystemEmitEventEffect.BeatmapEditorParticleSystemEmitBehavior>((IEnumerable<object>) new object[3]
        {
          (object) this._beatmapEvent,
          (object) this._particleSystemParentTransform,
          (object) this._particleSystemMaxSpawnedSystems
        });
        break;
    }
  }

  public virtual void OnDestroy() => this._particleSystemEmitBehavior?.Dispose();

  public abstract class ParticleSystemEmitBehavior : IDisposable
  {
    protected readonly IAudioTimeSource _audioTimeSource;
    private readonly BeatmapCallbacksController _beatmapCallbacksController;
    protected readonly MemoryPoolContainer<ParticleSystemEventController> _particleSystemEventControllerPoolContainer;
    private readonly Transform _particleSystemParentTransform;
    private readonly int _particleSystemMaxSpawnedSystems;
    private readonly BeatmapDataCallbackWrapper _beatmapDataCallbackWrapper;

    protected ParticleSystemEmitBehavior(
      BasicBeatmapEventType beatmapEvent,
      Transform particleSystemParentTransform,
      int particleSystemMaxSpawnedSystems,
      IAudioTimeSource audioTimeSource,
      BeatmapCallbacksController beatmapCallbacksController,
      ParticleSystemEventController.Pool particleSystemEventControllerPool)
    {
      this._audioTimeSource = audioTimeSource;
      this._beatmapCallbacksController = beatmapCallbacksController;
      this._particleSystemParentTransform = particleSystemParentTransform;
      this._particleSystemMaxSpawnedSystems = particleSystemMaxSpawnedSystems;
      this._particleSystemEventControllerPoolContainer = new MemoryPoolContainer<ParticleSystemEventController>((IMemoryPool<ParticleSystemEventController>) particleSystemEventControllerPool);
      this._beatmapDataCallbackWrapper = this._beatmapCallbacksController.AddBeatmapCallback<BasicBeatmapEventData>(new BeatmapDataCallback<BasicBeatmapEventData>(this.HandleBeatmapEvent), BasicBeatmapEventData.SubtypeIdentifier(beatmapEvent));
    }

    public virtual void Dispose()
    {
      foreach (ParticleSystemEventController activeItem in this._particleSystemEventControllerPoolContainer.activeItems)
        activeItem.Stop();
      this._beatmapCallbacksController?.RemoveBeatmapCallback(this._beatmapDataCallbackWrapper);
    }

    private void HandleBeatmapEvent(BasicBeatmapEventData basicBeatmapEventData)
    {
      if ((double) this._audioTimeSource.lastFrameDeltaSongTime < 0.0 || (double) Mathf.Abs(basicBeatmapEventData.time - this._audioTimeSource.songTime) > (double) Time.deltaTime)
        return;
      this.EmitParticles(basicBeatmapEventData.time);
    }

    protected virtual ParticleSystemEventController EmitParticles(float startTime)
    {
      if (this._particleSystemEventControllerPoolContainer.activeItems.Count == this._particleSystemMaxSpawnedSystems)
      {
        Debug.Log((object) "Hit max amount of particle systems. Ignoring next one");
        return (ParticleSystemEventController) null;
      }
      ParticleSystemEventController systemEventController = this._particleSystemEventControllerPoolContainer.Spawn();
      if ((UnityEngine.Object) systemEventController == (UnityEngine.Object) null)
        return (ParticleSystemEventController) null;
      systemEventController.transform.SetParent(this._particleSystemParentTransform, false);
      systemEventController.Init(startTime);
      return systemEventController;
    }
  }

  public class BeatmapEditorParticleSystemEmitBehavior : 
    ParticleSystemEmitEventEffect.ParticleSystemEmitBehavior,
    ITickable
  {
    protected readonly TickableManager _tickableManager;

    public BeatmapEditorParticleSystemEmitBehavior(
      BasicBeatmapEventType beatmapEvent,
      Transform particleSystemParentTransform,
      int particleSystemMaxSpawnedSystems,
      IAudioTimeSource audioTimeSource,
      BeatmapCallbacksController beatmapCallbacksController,
      ParticleSystemEventController.Pool particleSystemEventControllerPool,
      TickableManager tickableManager)
      : base(beatmapEvent, particleSystemParentTransform, particleSystemMaxSpawnedSystems, audioTimeSource, beatmapCallbacksController, particleSystemEventControllerPool)
    {
      this._tickableManager = tickableManager;
      this._tickableManager.Add((ITickable) this);
    }

    public virtual void Tick()
    {
      for (int index = this._particleSystemEventControllerPoolContainer.activeItems.Count - 1; index >= 0; --index)
      {
        ParticleSystemEventController activeItem = this._particleSystemEventControllerPoolContainer.activeItems[index];
        if ((double) activeItem.startTime - (double) this._audioTimeSource.lastFrameDeltaSongTime > (double) this._audioTimeSource.songTime || (double) activeItem.endTime < (double) this._audioTimeSource.songTime)
          this._particleSystemEventControllerPoolContainer.Despawn(activeItem);
        else
          activeItem.ManualUpdate(this._audioTimeSource.songTime, this._audioTimeSource.lastFrameDeltaSongTime);
      }
    }

    public override void Dispose()
    {
      base.Dispose();
      this._tickableManager.Remove((ITickable) this);
    }
  }

  public class GameplayParticleSystemEmitBehavior : 
    ParticleSystemEmitEventEffect.ParticleSystemEmitBehavior
  {
    protected readonly PauseController _pauseController;
    protected readonly SongSpeedData _songSpeedData;

    public GameplayParticleSystemEmitBehavior(
      BasicBeatmapEventType beatmapEvent,
      Transform particleSystemParentTransform,
      int particleSystemMaxSpawnedSystems,
      IAudioTimeSource audioTimeSource,
      BeatmapCallbacksController beatmapCallbacksController,
      ParticleSystemEventController.Pool particleSystemEventControllerPool,
      PauseController pauseController,
      SongSpeedData songSpeedData)
      : base(beatmapEvent, particleSystemParentTransform, particleSystemMaxSpawnedSystems, audioTimeSource, beatmapCallbacksController, particleSystemEventControllerPool)
    {
      this._pauseController = pauseController;
      this._songSpeedData = songSpeedData;
      this._pauseController.didPauseEvent += new System.Action(this.HandlePauseControllerDidPause);
      this._pauseController.didResumeEvent += new System.Action(this.HandlePauseControllerDidResume);
    }

    public override void Dispose()
    {
      if (!((UnityEngine.Object) this._pauseController != (UnityEngine.Object) null))
        return;
      this._pauseController.didPauseEvent -= new System.Action(this.HandlePauseControllerDidPause);
      this._pauseController.didResumeEvent -= new System.Action(this.HandlePauseControllerDidResume);
    }

    public virtual void HandlePauseControllerDidPause()
    {
      foreach (ParticleSystemEventController activeItem in this._particleSystemEventControllerPoolContainer.activeItems)
        activeItem.Pause();
    }

    public virtual void HandlePauseControllerDidResume()
    {
      foreach (ParticleSystemEventController activeItem in this._particleSystemEventControllerPoolContainer.activeItems)
        activeItem.Play();
    }

    protected override ParticleSystemEventController EmitParticles(float startTime)
    {
      ParticleSystemEventController systemEventController = base.EmitParticles(startTime);
      if ((UnityEngine.Object) systemEventController == (UnityEngine.Object) null)
        return (ParticleSystemEventController) null;
      systemEventController.InitSpeed(this._songSpeedData.speedMul);
      systemEventController.Play();
      return systemEventController;
    }
  }
}
