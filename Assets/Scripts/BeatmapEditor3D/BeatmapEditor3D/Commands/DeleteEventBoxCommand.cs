// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.DeleteEventBoxCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class DeleteEventBoxCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly DeleteEventBoxSignal _signal;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private BeatmapEditorObjectId _eventBoxGroupId;
    private int _eventBoxId;
    private List<BaseEditorData> _baseEditorData;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this._eventBoxGroupId = this._eventBoxGroupsState.eventBoxGroupContext.id;
      this._eventBoxId = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupId).FindIndex((Predicate<EventBoxEditorData>) (eventBox => eventBox.id == this._signal.eventBox.id));
      if (this._eventBoxId == -1)
        return;
      this._baseEditorData = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId(this._signal.eventBox.id);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapEventBoxGroupsDataModel.InsertEventBox(this._eventBoxGroupId, this._signal.eventBox, this._eventBoxId);
      if (this._baseEditorData != null)
        this._beatmapEventBoxGroupsDataModel.InsertBaseEditorDataList(this._signal.eventBox.id, (IEnumerable<BaseEditorData>) this._baseEditorData);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
      this._signalBus.Fire<EventBoxesUpdatedSignal>(new EventBoxesUpdatedSignal(this._eventBoxId));
    }

    public void Redo()
    {
      if (this._baseEditorData != null)
        this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorDataList(this._signal.eventBox.id);
      this._beatmapEventBoxGroupsDataModel.RemoveEventBox(this._eventBoxGroupId, this._signal.eventBox);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
      this._signalBus.Fire<EventBoxesUpdatedSignal>(new EventBoxesUpdatedSignal(this._eventBoxId - 1));
    }
  }
}
