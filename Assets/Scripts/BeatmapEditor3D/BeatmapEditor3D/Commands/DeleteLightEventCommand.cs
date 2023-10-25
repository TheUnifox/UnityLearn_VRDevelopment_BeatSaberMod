// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.DeleteLightEventCommand
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
  public class DeleteLightEventCommand : IBeatmapEditorCommandWithHistory, IBeatmapEditorCommand
  {
    [Inject]
    private readonly DeleteLightEventSignal _signal;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private int _eventIndex;
    private BaseEditorData _event;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      List<BaseEditorData> listByEventBoxId = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId(this._signal.eventBoxId);
      this._eventIndex = listByEventBoxId.FindIndex((Predicate<BaseEditorData>) (e => e.id == this._signal.lightColorEventId));
      if (this._eventIndex == -1)
        return;
      this._event = listByEventBoxId[this._eventIndex];
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapEventBoxGroupsDataModel.InsertBaseEditorData(this._signal.eventBoxId, this._event, this._eventIndex);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorData(this._signal.eventBoxId, this._event, this._eventIndex);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }
  }
}
