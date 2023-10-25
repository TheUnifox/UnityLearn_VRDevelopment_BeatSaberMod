// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.DeleteSelectedEventBoxGroupsCommand
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
  public class DeleteSelectedEventBoxGroupsCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand,
    IBeatmapEditorCommandMessageProvider
  {
    [Inject]
    private readonly EventBoxGroupsSelectionState _selectionState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly EventBoxGroupsClipboardHelper _eventBoxGroupsClipboardHelper;
    [Inject]
    private readonly SignalBus _signalBus;
    private List<EventBoxGroupEditorData> _selectedEventBoxGroups;
    private Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>> _selectedEventBoxes;
    private Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> _selectedBaseDataLists;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._selectionState.eventBoxGroups.Count == 0)
        return;
      this._selectedEventBoxGroups = this._selectionState.eventBoxGroups.Select<BeatmapEditorObjectId, EventBoxGroupEditorData>((Func<BeatmapEditorObjectId, EventBoxGroupEditorData>) (id => this._beatmapEventBoxGroupsDataModel.GetEventBoxGroupById(id))).ToList<EventBoxGroupEditorData>();
      this._eventBoxGroupsClipboardHelper.GetAllDataForEventBoxGroups(this._selectedEventBoxGroups, out this._selectedEventBoxes, out this._selectedBaseDataLists);
      this._selectionState.Clear();
      this._signalBus.Fire<EventBoxGroupsSelectionStateUpdatedSignal>();
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._eventBoxGroupsClipboardHelper.InsertEventBoxGroupData(this._selectedEventBoxGroups, this._selectedEventBoxes, this._selectedBaseDataLists);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._eventBoxGroupsClipboardHelper.RemoveEventBoxGroupData(this._selectedEventBoxGroups, this._selectedEventBoxes, this._selectedBaseDataLists);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Deleted {0} event box groups", (object) this._selectedEventBoxGroups?.Count));
  }
}
