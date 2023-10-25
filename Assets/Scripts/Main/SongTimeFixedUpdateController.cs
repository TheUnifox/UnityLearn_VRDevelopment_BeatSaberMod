// Decompiled with JetBrains decompiler
// Type: SongTimeFixedUpdateController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class SongTimeFixedUpdateController : MonoBehaviour
{
  [Inject]
  protected readonly IAudioTimeSource _audioTimeSource;
  protected float _accumulator;
  protected float _interpolationFactor;
  protected const float kFixedDeltaTime = 0.0166666675f;

  public event System.Action<float> songControllerFixedTimeDidUpdateEvent;

  public event System.Action songControllerTimeDidUpdateEvent;

  public float fixedDeltaTime => 0.0166666675f;

  public float interpolationFactor => this._interpolationFactor;

  public virtual void Update()
  {
    for (this._accumulator += this._audioTimeSource.lastFrameDeltaSongTime; (double) this._accumulator > 0.01666666753590107; this._accumulator -= 0.0166666675f)
    {
      System.Action<float> timeDidUpdateEvent = this.songControllerFixedTimeDidUpdateEvent;
      if (timeDidUpdateEvent != null)
        timeDidUpdateEvent(0.0166666675f);
    }
    this._interpolationFactor = this._accumulator / 0.0166666675f;
    System.Action timeDidUpdateEvent1 = this.songControllerTimeDidUpdateEvent;
    if (timeDidUpdateEvent1 == null)
      return;
    timeDidUpdateEvent1();
  }
}
