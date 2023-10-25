// Decompiled with JetBrains decompiler
// Type: PlayAudioOnGameEventController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class PlayAudioOnGameEventController : MonoBehaviour
{
  [SerializeField]
  protected AudioClipQueue _audioClipQueue;
  [SerializeField]
  protected PlayAudioOnGameEventController.EventAudioBinding[] _eventAudioBindings;

  public virtual void Awake()
  {
    foreach (PlayAudioOnGameEventController.EventAudioBinding eventAudioBinding in this._eventAudioBindings)
      eventAudioBinding.Init(this._audioClipQueue);
  }

  public virtual void OnDestroy()
  {
    foreach (PlayAudioOnGameEventController.EventAudioBinding eventAudioBinding in this._eventAudioBindings)
      eventAudioBinding.Deinit();
  }

  [Serializable]
  public class EventAudioBinding
  {
    [Header("==================")]
    [SerializeField]
    protected Signal _signal;
    [SerializeField]
    protected float _delay;
    [SerializeField]
    protected LocalizedAudioClipSO[] _localizedAudioClips;
    protected AudioClipQueue _audioClipQueue;
    protected RandomObjectPicker<LocalizedAudioClipSO> _randomObjectPicker;

    public virtual void Init(AudioClipQueue audioClipQueue)
    {
      this._audioClipQueue = audioClipQueue;
      this._randomObjectPicker = new RandomObjectPicker<LocalizedAudioClipSO>(this._localizedAudioClips, 0.2f);
      this._signal.Subscribe(new System.Action(this.HandleGameEvent));
    }

    public virtual void Deinit() => this._signal.Unsubscribe(new System.Action(this.HandleGameEvent));

    public virtual void HandleGameEvent()
    {
      LocalizedAudioClipSO localizedAudioClipSo = this._randomObjectPicker.PickRandomObject();
      if (!(bool) (UnityEngine.Object) localizedAudioClipSo)
        return;
      this._audioClipQueue.PlayAudioClipWithDelay(localizedAudioClipSo.localizedAudioClip, this._delay);
    }
  }
}
