// Decompiled with JetBrains decompiler
// Type: RecordingToolSettingsViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RecordingToolSettingsViewController : ViewController
{
  [SerializeField]
  protected Button _continueButton;
  [SerializeField]
  protected TextPageScrollView _textPageScrollView;
  [InjectOptional]
  protected readonly RecordingToolManager _recordingToolManager;

  public event System.Action didFinishEvent;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this._textPageScrollView.SetText(this._recordingToolManager == null || !this._recordingToolManager.recordingToolEnabled ? "Recording tool settings is null as recording tool is not enabled." : (this._recordingToolManager.recordingToolSettings != null ? this._recordingToolManager.recordingToolSettings.ToString() : "Recording tool settings is null as recording tool fails to read or parse configuration file."));
    if (!firstActivation)
      return;
    this.buttonBinder.AddBinding(this._continueButton, (System.Action) (() =>
    {
      System.Action didFinishEvent = this.didFinishEvent;
      if (didFinishEvent == null)
        return;
      didFinishEvent();
    }));
  }

  [CompilerGenerated]
  public virtual void m_CDidActivatem_Eb__6_0()
  {
    System.Action didFinishEvent = this.didFinishEvent;
    if (didFinishEvent == null)
      return;
    didFinishEvent();
  }
}
