// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.SelectMultipleEventBoxesEventsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class SelectMultipleEventBoxesEventsCommand : IBeatmapEditorCommand
  {
    [Inject]
    private readonly SelectMultipleEventBoxesEventsSignal _signal;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly EventBoxesSelectionState _selectionState;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;

    public void Execute()
    {
      if (!this._signal.commit)
      {
        this._selectionState.Clear();
      }
      else
      {
        if (!this._signal.addToSelection)
          this._selectionState.Clear();
        float num1 = this._selectionState.startBeat - this._beatmapState.beatOffset;
        float num2 = this._selectionState.endBeat - this._beatmapState.beatOffset;
        List<EventBoxEditorData> byEventBoxGroupId = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupsState.eventBoxGroupContext.id);
        for (int startEventBoxIndex = this._selectionState.startEventBoxIndex; startEventBoxIndex <= this._selectionState.endEventBoxIndex; ++startEventBoxIndex)
        {
          EventBoxEditorData eventBoxEditorData = byEventBoxGroupId[startEventBoxIndex];
          foreach (BaseEditorData evt in this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId(eventBoxEditorData.id))
          {
            if ((double) evt.beat >= (double) num1)
            {
              if ((double) evt.beat <= (double) num2)
                this.AddSingle(eventBoxEditorData.id, evt);
              else
                break;
            }
          }
        }
        this._selectionState.eventBoxGroupType = this._eventBoxGroupsState.currentHoverGroupType;
        this._signalBus.Fire<EventBoxesSelectionStateUpdatedSignal>();
      }
    }

    private void AddSingle(BeatmapEditorObjectId eventBoxId, BaseEditorData evt)
    {
      if (this._selectionState.IsSelected(eventBoxId, evt.id))
        return;
      this._selectionState.Add(eventBoxId, evt.id);
    }
  }
}
