// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.EndEventsSelectionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using System;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class EndEventsSelectionCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly EndEventsSelectionSignal _signal;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;

    public void Execute()
    {
      EnvironmentTracksDefinitionSO.BasicEventTrackInfo trackInfo = this._beatmapDataModel.environmentTrackDefinition[this._signal.BasicBeatmapEventType];
      int index = Array.FindIndex<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>(this._beatmapDataModel.environmentTrackDefinition.visibleTrackInfos, (Predicate<EnvironmentTracksDefinitionSO.BasicEventTrackInfo>) (info => info.basicBeatmapEventType == trackInfo.basicBeatmapEventType));
      this._signalBus.Fire<EventsChangeRectangleSelectionSignal>(new EventsChangeRectangleSelectionSignal()
      {
        changeType = EventsChangeRectangleSelectionSignal.ChangeType.End,
        beat = this._signal.beat,
        trackIndex = index
      });
      this._signalBus.Fire<ChangeInteractionModeSignal>(new ChangeInteractionModeSignal(InteractionMode.Place));
      this._signalBus.Fire<SelectMultipleEventsSignal>(new SelectMultipleEventsSignal()
      {
        addToSelection = false,
        commit = this._signal.commit
      });
    }
  }
}
