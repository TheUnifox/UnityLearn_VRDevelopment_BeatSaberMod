// Decompiled with JetBrains decompiler
// Type: CountdownController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CountdownController : MonoBehaviour
{
  [SerializeField]
  protected AudioSource _audioSource;
  [SerializeField]
  protected CountdownElementController[] _countdownElementControllers;
  [Inject]
  protected readonly ITimeProvider _timeProvider;
  protected const float kGongTime = 5f;
  protected float _countdownEndTime;
  protected int _currentRemainingSecond = -1;
  protected bool _gongSounded;
  protected bool _countdownRunning;
  protected readonly Queue<CountdownElementController> _countdownElementControllerQueue = new Queue<CountdownElementController>();

  public virtual void Awake()
  {
    if (this._countdownRunning)
      return;
    this.enabled = false;
  }

  public virtual void Update()
  {
    float f = this._countdownEndTime - this._timeProvider.time;
    int num = Mathf.CeilToInt(f);
    if (!this._gongSounded && (double) f <= 5.0)
    {
      this._audioSource.Play();
      this._gongSounded = true;
    }
    if (num < 1 || num == this._currentRemainingSecond)
      return;
    CountdownElementController elementController = this._countdownElementControllerQueue.Dequeue();
    elementController.SetTextAndRunAnimation(num.ToString());
    this._countdownElementControllerQueue.Enqueue(elementController);
    this._currentRemainingSecond = num;
  }

  public virtual void StartCountdown(float countdownEndTime)
  {
    this._countdownElementControllerQueue.Clear();
    foreach (CountdownElementController elementController in this._countdownElementControllers)
    {
      elementController.StopAndHide();
      this._countdownElementControllerQueue.Enqueue(elementController);
    }
    this._countdownEndTime = countdownEndTime;
    this.enabled = true;
    this._gongSounded = false;
    this._countdownRunning = true;
  }

  public virtual void UpdateCountdown(float countdownEndTime)
  {
    this._countdownEndTime = countdownEndTime;
    foreach (CountdownElementController elementController in this._countdownElementControllers)
      elementController.StopAndHide();
    this._gongSounded = false;
  }

  public virtual void StopCountdown()
  {
    this.enabled = false;
    this._currentRemainingSecond = -1;
    foreach (CountdownElementController elementController in this._countdownElementControllers)
      elementController.StopAndHide();
    this._gongSounded = false;
    this._countdownRunning = false;
  }
}
