// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.PruneEventBoxesCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class PruneEventBoxesCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private BeatmapEditorObjectId _eventBoxGroupId;
    private List<(EventBoxEditorData eventBox, List<BaseEditorData> baseList)> _previousEventBoxes;
    private List<(EventBoxEditorData eventBox, List<BaseEditorData> baseList)> _newEventBoxes;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this._eventBoxGroupId = this._eventBoxGroupsState.eventBoxGroupContext.id;
      List<EventBoxEditorData> byEventBoxGroupId = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupId);
      if (byEventBoxGroupId.Count == 0)
        return;
      this._previousEventBoxes = new List<(EventBoxEditorData, List<BaseEditorData>)>(byEventBoxGroupId.Count);
      this._newEventBoxes = new List<(EventBoxEditorData, List<BaseEditorData>)>();
      foreach (EventBoxEditorData eventBoxEditorData in byEventBoxGroupId)
      {
        List<BaseEditorData> list = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId(eventBoxEditorData.id).ToList<BaseEditorData>();
        this._previousEventBoxes.Add((eventBoxEditorData, list));
        if (list.Count != 0)
          this._newEventBoxes.Add((eventBoxEditorData, list));
      }
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      foreach ((EventBoxEditorData eventBox, List<BaseEditorData> baseList) newEventBox in this._newEventBoxes)
      {
        this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorDataList(newEventBox.eventBox.id);
        this._beatmapEventBoxGroupsDataModel.RemoveEventBox(this._eventBoxGroupId, newEventBox.eventBox);
      }
      foreach ((EventBoxEditorData eventBox, List<BaseEditorData> baseList) previousEventBox in this._previousEventBoxes)
      {
        this._beatmapEventBoxGroupsDataModel.InsertEventBox(this._eventBoxGroupId, previousEventBox.eventBox);
        if (previousEventBox.baseList != null)
          this._beatmapEventBoxGroupsDataModel.InsertBaseEditorDataList(previousEventBox.eventBox.id, (IEnumerable<BaseEditorData>) previousEventBox.baseList);
      }
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
      this._signalBus.Fire<EventBoxesUpdatedSignal>(new EventBoxesUpdatedSignal(0));
    }

    public void Redo()
    {
      foreach ((EventBoxEditorData eventBox, List<BaseEditorData> baseList) previousEventBox in this._previousEventBoxes)
      {
        this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorDataList(previousEventBox.eventBox.id);
        this._beatmapEventBoxGroupsDataModel.RemoveEventBox(this._eventBoxGroupId, previousEventBox.eventBox);
      }
      foreach ((EventBoxEditorData eventBox, List<BaseEditorData> baseList) newEventBox in this._newEventBoxes)
      {
        this._beatmapEventBoxGroupsDataModel.InsertEventBox(this._eventBoxGroupId, newEventBox.eventBox);
        if (newEventBox.baseList != null)
          this._beatmapEventBoxGroupsDataModel.InsertBaseEditorDataList(newEventBox.eventBox.id, (IEnumerable<BaseEditorData>) newEventBox.baseList);
      }
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
      this._signalBus.Fire<EventBoxesUpdatedSignal>(new EventBoxesUpdatedSignal(0));
    }
  }
}
