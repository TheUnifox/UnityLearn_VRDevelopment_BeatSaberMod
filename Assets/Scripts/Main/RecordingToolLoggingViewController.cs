// Decompiled with JetBrains decompiler
// Type: RecordingToolLoggingViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RecordingToolLoggingViewController : ViewController
{
  [SerializeField]
  protected TextPageScrollView _textPageScrollView;
  [InjectOptional]
  protected readonly RecordingToolManager _recordingToolManager;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    this._textPageScrollView.SetText(this._recordingToolManager != null ? string.Join<ListLogger.LogMessage>("\n\n", (IEnumerable<ListLogger.LogMessage>) this._recordingToolManager.listLogger.messages) : "There are no logs as recording tool is not enabled.");
  }
}
