// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.InteractionAndModeView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class InteractionAndModeView : BeatmapEditorView
  {
    [SerializeField]
    private TextMeshProUGUI _text;
    [Inject]
    private readonly BeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;

    protected override void DidActivate()
    {
      this._signalBus.Subscribe<InteractionModeChangedSignal>(new Action(this.HandleInteractionModeChanged));
      this._signalBus.Subscribe<BeatmapEditingModeSwitched>(new Action(this.HandleBeatmapEditingModeSwitched));
      this.SetText();
    }

    protected override void DidDeactivate()
    {
      this._signalBus.TryUnsubscribe<InteractionModeChangedSignal>(new Action(this.HandleInteractionModeChanged));
      this._signalBus.TryUnsubscribe<BeatmapEditingModeSwitched>(new Action(this.HandleBeatmapEditingModeSwitched));
    }

    private void HandleInteractionModeChanged() => this.SetText();

    private void HandleBeatmapEditingModeSwitched() => this.SetText();

    private void SetText() => this._text.text = InteractionAndModeView.GetEditingModeAsString(this._beatmapState.editingMode) + " => " + InteractionAndModeView.GetInteractionModeAsString(this._beatmapState.interactionMode);

    private static string GetEditingModeAsString(BeatmapEditingMode mode)
    {
      switch (mode)
      {
        case BeatmapEditingMode.BasicEvents:
          return "Basic Events";
        case BeatmapEditingMode.EventBoxGroups:
          return "Event Box Groups";
        case BeatmapEditingMode.EventBoxes:
          return "Event Group Boxes";
        default:
          return "Beatmap Objects";
      }
    }

    private static string GetInteractionModeAsString(InteractionMode mode)
    {
      switch (mode)
      {
        case InteractionMode.Select:
          return "Selecting";
        case InteractionMode.ClickSelect:
          return "Click-Selecting";
        case InteractionMode.Delete:
          return "Deleting";
        case InteractionMode.Modify:
          return "Modifying";
        default:
          return "Placing";
      }
    }
  }
}
