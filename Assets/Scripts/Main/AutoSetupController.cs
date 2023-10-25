// Decompiled with JetBrains decompiler
// Type: AutoSetupController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class AutoSetupController : MonoBehaviour
{
  [SerializeField]
  protected Button _closeButton;
  [Header("Toggles")]
  [SerializeField]
  protected Toggle _selectBasedOnSuggestionsToggle;
  [SerializeField]
  protected Toggle _autoStartWhenAllReadyToggle;
  [SerializeField]
  protected Toggle _forceAutoStartAfterSongSelectionToggle;
  [SerializeField]
  protected Toggle _randomSongIfNoneSuggestedToggle;
  protected readonly ButtonBinder _buttonBinder = new ButtonBinder();
  protected readonly ToggleBinder _toggleBinder = new ToggleBinder();
  protected AutoSetupData _autoSetupData = new AutoSetupData();

  public event System.Action<AutoSetupData> didFinishEvent;

  public virtual void Setup(AutoSetupData autoSetupData) => this._autoSetupData = autoSetupData;

  public virtual void OnEnable()
  {
    this._buttonBinder.AddBinding(this._closeButton, (System.Action) (() =>
    {
      System.Action<AutoSetupData> didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent(this._autoSetupData);
    }));
    this._toggleBinder.AddBinding(this._selectBasedOnSuggestionsToggle, (System.Action<bool>) (isOn => this._autoSetupData.selectBasedOnSuggestions = isOn));
    this._toggleBinder.AddBinding(this._autoStartWhenAllReadyToggle, (System.Action<bool>) (isOn => this._autoSetupData.autoStartWhenAllReady = isOn));
    this._toggleBinder.AddBinding(this._forceAutoStartAfterSongSelectionToggle, (System.Action<bool>) (isOn => this._autoSetupData.forceAutoStartAfterSongSelection = isOn));
    this._toggleBinder.AddBinding(this._randomSongIfNoneSuggestedToggle, (System.Action<bool>) (isOn => this._autoSetupData.randomSongIfNoneSuggested = isOn));
  }

  public virtual void OnDisable() => this._buttonBinder.ClearBindings();

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__12_0()
  {
    System.Action<AutoSetupData> didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent(this._autoSetupData);
  }

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__12_1(bool isOn) => this._autoSetupData.selectBasedOnSuggestions = isOn;

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__12_2(bool isOn) => this._autoSetupData.autoStartWhenAllReady = isOn;

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__12_3(bool isOn) => this._autoSetupData.forceAutoStartAfterSongSelection = isOn;

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__12_4(bool isOn) => this._autoSetupData.randomSongIfNoneSuggested = isOn;
}
