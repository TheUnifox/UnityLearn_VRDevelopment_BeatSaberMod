// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.DeleteEventBoxGroupCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class DeleteEventBoxGroupCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly DeleteEventBoxGroupSignal _signal;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private EventBoxGroupEditorData _eventBoxGroupEditorData;
    private List<EventBoxEditorData> _eventBoxesEditorData;
    private Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> _baseEditorDataLists;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this._eventBoxGroupEditorData = this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupById(this._signal.id);
      if (this._eventBoxGroupEditorData == (EventBoxGroupEditorData) null)
        return;
      this._eventBoxesEditorData = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupEditorData.id).ToList<EventBoxEditorData>();
      this._baseEditorDataLists = this._eventBoxesEditorData.ToDictionary<EventBoxEditorData, BeatmapEditorObjectId, List<BaseEditorData>>((Func<EventBoxEditorData, BeatmapEditorObjectId>) (eventBox => eventBox.id), (Func<EventBoxEditorData, List<BaseEditorData>>) (eventBox => this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId(eventBox.id).ToList<BaseEditorData>()));
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapEventBoxGroupsDataModel.InsertEventBoxGroup(this._eventBoxGroupEditorData);
      this._beatmapEventBoxGroupsDataModel.InsertEventBoxes(this._eventBoxGroupEditorData.id, (IEnumerable<EventBoxEditorData>) this._eventBoxesEditorData);
      if (this._baseEditorDataLists != null)
      {
        foreach (KeyValuePair<BeatmapEditorObjectId, List<BaseEditorData>> baseEditorDataList in this._baseEditorDataLists)
          this._beatmapEventBoxGroupsDataModel.InsertBaseEditorDataList(baseEditorDataList.Key, (IEnumerable<BaseEditorData>) baseEditorDataList.Value);
      }
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      if (this._baseEditorDataLists != null)
      {
        foreach (KeyValuePair<BeatmapEditorObjectId, List<BaseEditorData>> baseEditorDataList in this._baseEditorDataLists)
          this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorDataList(baseEditorDataList.Key);
      }
      this._beatmapEventBoxGroupsDataModel.RemoveEventBoxes(this._eventBoxGroupEditorData.id, (IEnumerable<EventBoxEditorData>) this._eventBoxesEditorData);
      this._beatmapEventBoxGroupsDataModel.RemoveEventBoxGroup(this._eventBoxGroupEditorData);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
