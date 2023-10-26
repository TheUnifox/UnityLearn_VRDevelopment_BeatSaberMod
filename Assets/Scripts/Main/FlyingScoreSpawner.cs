// Decompiled with JetBrains decompiler
// Type: FlyingScoreSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class FlyingScoreSpawner : MonoBehaviour, IFlyingObjectEffectDidFinishEvent
{
  [Inject]
  protected readonly FlyingScoreEffect.Pool _flyingScoreEffectPool;
  [Inject]
  protected readonly FlyingScoreSpawner.InitData _initData;

  public virtual void SpawnFlyingScore(IReadonlyCutScoreBuffer cutScoreBuffer, Color color)
  {
    NoteCutInfo noteCutInfo = cutScoreBuffer.noteCutInfo;
    Vector3 cutPoint = noteCutInfo.cutPoint;
    FlyingScoreEffect flyingScoreEffect = this._flyingScoreEffectPool.Spawn();
    flyingScoreEffect.didFinishEvent.Add((IFlyingObjectEffectDidFinishEvent) this);
    flyingScoreEffect.transform.localPosition = cutPoint;
    Vector3 vector3 = noteCutInfo.cutPoint;
    flyingScoreEffect.transform.localPosition = vector3;
    vector3 = noteCutInfo.inverseWorldRotation * vector3;
    vector3.z = 0f;
    float y = 0.0f;
    if (this._initData.spawnPosition == FlyingScoreSpawner.SpawnPosition.Underground)
    {
      vector3.y = -0.24f;
    }
    else
    {
      vector3.y = 0.25f;
      y = -0.1f;
    }
    Vector3 targetPos = noteCutInfo.worldRotation * (vector3 + new Vector3(0.0f, y, 7.55f));
    flyingScoreEffect.InitAndPresent(cutScoreBuffer, 0.7f, targetPos, color);
  }

  public virtual void HandleFlyingObjectEffectDidFinish(FlyingObjectEffect flyingObjectEffect)
  {
    flyingObjectEffect.didFinishEvent.Remove((IFlyingObjectEffectDidFinishEvent) this);
    this._flyingScoreEffectPool.Despawn(flyingObjectEffect as FlyingScoreEffect);
  }

  public enum SpawnPosition
  {
    Underground,
    AboveGround,
  }

  public class InitData
  {
    public readonly FlyingScoreSpawner.SpawnPosition spawnPosition;

    public InitData(FlyingScoreSpawner.SpawnPosition spawnPosition) => this.spawnPosition = spawnPosition;
  }
}
