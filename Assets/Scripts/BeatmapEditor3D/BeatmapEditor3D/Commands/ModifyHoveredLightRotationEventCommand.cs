// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightRotationEventCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public abstract class ModifyHoveredLightRotationEventCommand : 
    IBeatmapEditorCommandWithHistoryMergeable,
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    protected readonly EventBoxGroupsState eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private BeatmapEditorObjectId _eventBoxId;
    private int _eventIndex;
    private LightRotationBaseEditorData _originalLightRotationData;
    private LightRotationBaseEditorData _modifiedLightRotationData;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this._eventBoxId = this.eventBoxGroupsState.currentHoverEventBoxId;
      (this._originalLightRotationData, this._eventIndex) = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataById<LightRotationBaseEditorData>(this._eventBoxId, this.eventBoxGroupsState.currentHoverBaseEventId);
      if (this._eventIndex == -1 || !this.NeedsModification(this._originalLightRotationData))
        return;
      this._modifiedLightRotationData = this.GetModifiedEventData(this._originalLightRotationData);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorData(this._eventBoxId, (BaseEditorData) this._modifiedLightRotationData, this._eventIndex);
      this._beatmapEventBoxGroupsDataModel.InsertBaseEditorData(this._eventBoxId, (BaseEditorData) this._originalLightRotationData, this._eventIndex);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorData(this._eventBoxId, (BaseEditorData) this._originalLightRotationData, this._eventIndex);
      this._beatmapEventBoxGroupsDataModel.InsertBaseEditorData(this._eventBoxId, (BaseEditorData) this._modifiedLightRotationData, this._eventIndex);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return previousCommand is ModifyHoveredLightRotationEventCommand rotationEventCommand && rotationEventCommand._eventBoxId == this._eventBoxId && rotationEventCommand._originalLightRotationData.id == this._originalLightRotationData.id;
    }

    public void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      this._originalLightRotationData = ((ModifyHoveredLightRotationEventCommand) previousCommand)._originalLightRotationData;
    }

    protected abstract LightRotationBaseEditorData GetModifiedEventData(
      LightRotationBaseEditorData originalData);

    protected abstract bool NeedsModification(LightRotationBaseEditorData originalData);
  }
}
