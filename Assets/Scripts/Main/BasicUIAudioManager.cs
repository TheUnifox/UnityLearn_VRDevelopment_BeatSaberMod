// Decompiled with JetBrains decompiler
// Type: BasicUIAudioManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class BasicUIAudioManager : MonoBehaviour
{
  [SerializeField]
  protected Signal[] _buttonClickEvents;
  [SerializeField]
  protected AudioSource _audioSource;
  [SerializeField]
  protected AudioClip[] _clickSounds;
  [SerializeField]
  protected float _minPitch = 1f;
  [SerializeField]
  protected float _maxPitch = 1f;
  protected RandomObjectPicker<AudioClip> _randomSoundPicker;

  public virtual void Start()
  {
    this._audioSource.ignoreListenerPause = true;
    this._randomSoundPicker = new RandomObjectPicker<AudioClip>(this._clickSounds, 0.07f);
  }

  public virtual void OnEnable()
  {
    for (int index = 0; index < this._buttonClickEvents.Length; ++index)
      this._buttonClickEvents[index].Subscribe(new System.Action(this.HandleButtonClickEvent));
  }

  public virtual void OnDisable()
  {
    for (int index = 0; index < this._buttonClickEvents.Length; ++index)
      this._buttonClickEvents[index].Unsubscribe(new System.Action(this.HandleButtonClickEvent));
  }

  public virtual void HandleButtonClickEvent()
  {
    AudioClip clip = this._randomSoundPicker.PickRandomObject();
    if (!(bool) (UnityEngine.Object) clip)
      return;
    this._audioSource.pitch = UnityEngine.Random.Range(this._minPitch, this._maxPitch);
    this._audioSource.PlayOneShot(clip, 1f);
  }
}
