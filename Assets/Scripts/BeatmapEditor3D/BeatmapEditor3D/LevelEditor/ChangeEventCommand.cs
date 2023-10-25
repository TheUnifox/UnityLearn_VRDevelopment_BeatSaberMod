// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.LevelEditor.ChangeEventCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using Zenject;

namespace BeatmapEditor3D.LevelEditor
{
  public class ChangeEventCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly ChangeEventSignal _changeEventSignal;
    [Inject]
    private readonly BeatmapState _beatmapState;
    [Inject]
    private readonly BasicEventsState _basicBasicEventsState;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      this._basicBasicEventsState.SetSelectedBeatmapTypeValue(this._changeEventSignal.group, this._changeEventSignal.value, this._changeEventSignal.floatValue, this._changeEventSignal.payload);
      if (this._beatmapState.interactionMode == InteractionMode.Delete)
        this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(InteractionMode.Place));
      this._signalBus.Fire<SelectedEventChangedSignal>();
    }
  }
}
