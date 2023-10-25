// Decompiled with JetBrains decompiler
// Type: BeatmapCallbacksUpdater
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class BeatmapCallbacksUpdater : MonoBehaviour
{
  [Inject]
  protected readonly BeatmapCallbacksController _beatmapCallbacksController;
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;

  public virtual void LateUpdate()
  {
    if (!this._audioTimeSource.isReady)
      return;
    this._beatmapCallbacksController.ManualUpdate(this._audioTimeSource.songTime);
  }

  public virtual void Pause() => this.enabled = false;

  public virtual void Resume() => this.enabled = true;
}
