// Decompiled with JetBrains decompiler
// Type: AudioLatencyViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AudioLatencyViewController : ViewController
{
  [SerializeField]
  protected FloatSO _audioLatency;
  [SerializeField]
  protected BoolSO _overrideAudioLatency;
  [Space]
  [SerializeField]
  protected CanvasGroup _setupCanvasGroup;
  [SerializeField]
  protected Toggle _overrideAudioLatencyToggle;
  [SerializeField]
  protected RangeValuesTextSlider _slider;
  [SerializeField]
  protected VisualMetronome _visualMetronome;
  [Space]
  [SerializeField]
  protected float _disabledAlpha = 0.2f;
  [Inject]
  protected readonly SongPreviewPlayer _songPreviewPlayer;
  protected ToggleBinder _toggleBinder;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this._setupCanvasGroup.interactable = false;
      this._slider.valueDidChangeEvent += new System.Action<RangeValuesTextSlider, float>(this.SliderValueDidChange);
      this._toggleBinder = new ToggleBinder();
      this._toggleBinder.AddBinding(this._overrideAudioLatencyToggle, new System.Action<bool>(this.HandleOverrideAudioLatencyToggleValueChanged));
    }
    if (addedToHierarchy)
      this._songPreviewPlayer.FadeOut(1f);
    this._visualMetronome.zeroOffset = (float) (ObservableVariableSO<float>) this._audioLatency;
    this._overrideAudioLatencyToggle.isOn = (bool) (ObservableVariableSO<bool>) this._overrideAudioLatency;
    this._slider.value = (bool) (ObservableVariableSO<bool>) this._overrideAudioLatency ? (float) (ObservableVariableSO<float>) this._audioLatency : 0.0f;
    this.RefreshVisuals((bool) (ObservableVariableSO<bool>) this._overrideAudioLatency);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    this._songPreviewPlayer.CrossfadeToDefault();
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) this._slider != (UnityEngine.Object) null)
      this._slider.valueDidChangeEvent -= new System.Action<RangeValuesTextSlider, float>(this.SliderValueDidChange);
    this._toggleBinder.ClearBindings();
  }

  public virtual void SliderValueDidChange(RangeValuesTextSlider slider, float value)
  {
    this._audioLatency.value = value;
    this._visualMetronome.zeroOffset = value;
  }

  public virtual void HandleOverrideAudioLatencyToggleValueChanged(bool isOn)
  {
    this._overrideAudioLatency.value = isOn;
    this.RefreshVisuals(isOn);
  }

  public virtual void RefreshVisuals(bool overrideAudioLatencyIsEnabled)
  {
    if (overrideAudioLatencyIsEnabled)
    {
      this._setupCanvasGroup.interactable = true;
      this._setupCanvasGroup.alpha = 1f;
      this._visualMetronome.enabled = true;
      this._slider.value = (float) (ObservableVariableSO<float>) this._audioLatency;
    }
    else
    {
      this._setupCanvasGroup.interactable = false;
      this._setupCanvasGroup.alpha = this._disabledAlpha;
      this._visualMetronome.enabled = false;
      this._slider.value = 0.0f;
    }
  }
}
