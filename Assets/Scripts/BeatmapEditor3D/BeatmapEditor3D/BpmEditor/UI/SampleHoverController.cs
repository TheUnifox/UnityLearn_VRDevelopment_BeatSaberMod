// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.UI.SampleHoverController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.Commands.Tools;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor.UI
{
  public class SampleHoverController : MonoBehaviour
  {
    [SerializeField]
    private BpmRegionsInputController _inputController;
    [Inject]
    private readonly SignalBus _signalBus;

    protected void Start() => this._inputController.mouseOverEvent += new Action<int>(this.HandleInputControllerMouseOver);

    protected void OnDestroy() => this._inputController.mouseOverEvent -= new Action<int>(this.HandleInputControllerMouseOver);

    private void HandleInputControllerMouseOver(int sample) => this._signalBus.Fire<SetHoverSampleSignal>(new SetHoverSampleSignal(sample));
  }
}
