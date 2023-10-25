// Decompiled with JetBrains decompiler
// Type: EffectPoolsManualInstaller
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class EffectPoolsManualInstaller : MonoBehaviour
{
  [SerializeField]
  protected FlyingTextEffect _flyingTextEffectPrefab;
  [SerializeField]
  protected FlyingScoreEffect _flyingScoreEffectPrefab;
  [Space]
  [SerializeField]
  protected BeatEffect _beatEffectPrefab;
  [SerializeField]
  protected BeatEffect _shortBeatEffectPrefab;
  [Space]
  [SerializeField]
  protected NoteCutSoundEffect _noteCutSoundEffectPrefab;
  [SerializeField]
  protected BombCutSoundEffect _bombCutSoundEffectPrefab;
  [SerializeField]
  protected FlyingSpriteEffect _flyingSpriteEffectPrefab;

  public virtual void ManualInstallBindings(DiContainer container, bool shortBeatEffect)
  {
    container.BindMemoryPool<FlyingTextEffect, FlyingTextEffect.Pool>().WithInitialSize(20).FromComponentInNewPrefab((Object) this._flyingTextEffectPrefab);
    container.BindMemoryPool<FlyingScoreEffect, FlyingScoreEffect.Pool>().WithInitialSize(20).FromComponentInNewPrefab((Object) this._flyingScoreEffectPrefab);
    container.BindMemoryPool<FlyingSpriteEffect, FlyingSpriteEffect.Pool>().WithInitialSize(20).FromComponentInNewPrefab((Object) this._flyingSpriteEffectPrefab);
    container.BindMemoryPool<BeatEffect, BeatEffect.Pool>().WithInitialSize(20).FromComponentInNewPrefab(shortBeatEffect ? (Object) this._shortBeatEffectPrefab : (Object) this._beatEffectPrefab);
    container.BindMemoryPool<NoteCutSoundEffect, NoteCutSoundEffect.Pool>().WithInitialSize(80).FromComponentInNewPrefab((Object) this._noteCutSoundEffectPrefab);
    container.BindMemoryPool<BombCutSoundEffect, BombCutSoundEffect.Pool>().WithInitialSize(20).FromComponentInNewPrefab((Object) this._bombCutSoundEffectPrefab);
  }
}
