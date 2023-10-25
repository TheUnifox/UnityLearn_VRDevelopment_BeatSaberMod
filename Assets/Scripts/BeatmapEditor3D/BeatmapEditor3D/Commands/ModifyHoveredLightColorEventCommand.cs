// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightColorEventCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public abstract class ModifyHoveredLightColorEventCommand : 
    IBeatmapEditorCommandWithHistoryMergeable,
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand
  {
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;
    private BeatmapEditorObjectId _eventBoxId;
    private int _eventIndex;
    private LightColorBaseEditorData _originalLightColorData;
    private LightColorBaseEditorData _modifiedLightColorData;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this._eventBoxId = this._eventBoxGroupsState.currentHoverEventBoxId;
      (this._originalLightColorData, this._eventIndex) = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataById<LightColorBaseEditorData>(this._eventBoxId, this._eventBoxGroupsState.currentHoverBaseEventId);
      if (this._eventIndex == -1 || !this.NeedsModification(this._originalLightColorData))
        return;
      this._modifiedLightColorData = this.GetModifiedEventData(this._originalLightColorData);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorData(this._eventBoxId, (BaseEditorData) this._modifiedLightColorData, this._eventIndex);
      this._beatmapEventBoxGroupsDataModel.InsertBaseEditorData(this._eventBoxId, (BaseEditorData) this._originalLightColorData, this._eventIndex);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorData(this._eventBoxId, (BaseEditorData) this._originalLightColorData, this._eventIndex);
      this._beatmapEventBoxGroupsDataModel.InsertBaseEditorData(this._eventBoxId, (BaseEditorData) this._modifiedLightColorData, this._eventIndex);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return previousCommand is ModifyHoveredLightColorEventCommand colorEventCommand && colorEventCommand._eventBoxId == this._eventBoxId && colorEventCommand._originalLightColorData.id == this._originalLightColorData.id;
    }

    public void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      this._originalLightColorData = ((ModifyHoveredLightColorEventCommand) previousCommand)._originalLightColorData;
    }

    protected abstract LightColorBaseEditorData GetModifiedEventData(
      LightColorBaseEditorData originalData);

    protected abstract bool NeedsModification(LightColorBaseEditorData originalData);
  }
}
