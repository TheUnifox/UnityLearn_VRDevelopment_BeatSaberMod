// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.ModifyEventBoxCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D
{
  public class ModifyEventBoxCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly ModifyEventBoxSignal _signal;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private BeatmapEditorObjectId _eventBoxEditorId;
    private int _prevEventBoxIndex;
    private EventBoxEditorData _prevEventBox;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this._eventBoxEditorId = this._eventBoxGroupsState.eventBoxGroupContext.id;
      List<EventBoxEditorData> byEventBoxGroupId = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxEditorId);
      this._prevEventBoxIndex = byEventBoxGroupId.FindIndex((Predicate<EventBoxEditorData>) (eventBox => eventBox.id == this._signal.modifiedEventBox.id));
      if (this._prevEventBoxIndex == -1)
        return;
      this._prevEventBox = byEventBoxGroupId[this._prevEventBoxIndex];
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapEventBoxGroupsDataModel.RemoveEventBox(this._eventBoxEditorId, this._signal.modifiedEventBox);
      this._beatmapEventBoxGroupsDataModel.InsertEventBox(this._eventBoxEditorId, this._prevEventBox, this._prevEventBoxIndex);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapEventBoxGroupsDataModel.RemoveEventBox(this._eventBoxEditorId, this._prevEventBox);
      this._beatmapEventBoxGroupsDataModel.InsertEventBox(this._eventBoxEditorId, this._signal.modifiedEventBox, this._prevEventBoxIndex);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
