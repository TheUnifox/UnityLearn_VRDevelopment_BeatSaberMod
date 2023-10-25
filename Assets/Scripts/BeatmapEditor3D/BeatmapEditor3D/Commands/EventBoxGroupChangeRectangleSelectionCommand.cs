// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.EventBoxGroupChangeRectangleSelectionCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class EventBoxGroupChangeRectangleSelectionCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly EventBoxGroupChangeRectangleSelectionSignal _signal;
    [Inject]
    private readonly EventBoxGroupsSelectionState _selectionState;
    [Inject]
    private readonly BeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      int index = this._beatmapDataModel.environmentTrackDefinition.eventBoxGroupPageInfos[this._eventBoxGroupsState.currentPage].eventBoxGroupTrackInfos.FindIndex((Predicate<EnvironmentTracksDefinitionSO.EventBoxGroupTrackInfo>) (t => t.lightGroup.groupId == this._signal.groupId));
      bool flag = false;
      switch (this._signal.changeType)
      {
        case EventBoxGroupChangeRectangleSelectionSignal.ChangeType.Start:
          flag = this._selectionState.tempStartPageIndex != index || !Mathf.Approximately(this._selectionState.tempStartBeat, this._signal.beat);
          this._selectionState.tempStartPageIndex = index;
          this._selectionState.tempEndPageIndex = index;
          this._selectionState.tempStartBeat = this._signal.beat;
          this._selectionState.tempEndBeat = this._signal.beat;
          this._selectionState.showSelection = true;
          break;
        case EventBoxGroupChangeRectangleSelectionSignal.ChangeType.Drag:
        case EventBoxGroupChangeRectangleSelectionSignal.ChangeType.End:
          flag = this._selectionState.tempEndPageIndex != index || !Mathf.Approximately(this._selectionState.tempEndBeat, this._signal.beat);
          this._selectionState.tempEndPageIndex = index;
          this._selectionState.tempEndBeat = this._signal.beat;
          break;
      }
      if (this._signal.changeType == EventBoxGroupChangeRectangleSelectionSignal.ChangeType.End)
      {
        flag = true;
        this._selectionState.showSelection = false;
      }
      this._selectionState.startPageIndex = Mathf.Min(this._selectionState.tempStartPageIndex, this._selectionState.tempEndPageIndex);
      this._selectionState.endPageIndex = Mathf.Max(this._selectionState.tempStartPageIndex, this._selectionState.tempEndPageIndex);
      this._selectionState.startBeat = Mathf.Min(this._selectionState.tempStartBeat, this._selectionState.tempEndBeat);
      this._selectionState.endBeat = Mathf.Max(this._selectionState.tempStartBeat, this._selectionState.tempEndBeat);
      if (!flag)
        return;
      this._signalBus.Fire<EventBoxGroupsSelectionRectangleChangedSignal>();
    }
  }
}
