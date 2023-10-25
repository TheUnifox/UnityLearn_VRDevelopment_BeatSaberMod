// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.ModifyHoveredLightTranslationEventCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public abstract class ModifyHoveredLightTranslationEventCommand : 
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
    private LightTranslationBaseEditorData _originalLightTranslationData;
    private LightTranslationBaseEditorData _modifiedLightTranslationData;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      this._eventBoxId = this.eventBoxGroupsState.currentHoverEventBoxId;
      (this._originalLightTranslationData, this._eventIndex) = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataById<LightTranslationBaseEditorData>(this._eventBoxId, this.eventBoxGroupsState.currentHoverBaseEventId);
      if (this._eventIndex == -1 || !this.NeedsModification(this._originalLightTranslationData))
        return;
      this._modifiedLightTranslationData = this.GetModifiedEventData(this._originalLightTranslationData);
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorData(this._eventBoxId, (BaseEditorData) this._modifiedLightTranslationData, this._eventIndex);
      this._beatmapEventBoxGroupsDataModel.InsertBaseEditorData(this._eventBoxId, (BaseEditorData) this._originalLightTranslationData, this._eventIndex);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._beatmapEventBoxGroupsDataModel.RemoveBaseEditorData(this._eventBoxId, (BaseEditorData) this._originalLightTranslationData, this._eventIndex);
      this._beatmapEventBoxGroupsDataModel.InsertBaseEditorData(this._eventBoxId, (BaseEditorData) this._modifiedLightTranslationData, this._eventIndex);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public bool ShouldMergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      return previousCommand is ModifyHoveredLightTranslationEventCommand translationEventCommand && translationEventCommand._eventBoxId == this._eventBoxId && translationEventCommand._originalLightTranslationData.id == this._originalLightTranslationData.id;
    }

    public void MergeWith(
      IBeatmapEditorCommandWithHistoryMergeable previousCommand)
    {
      this._originalLightTranslationData = ((ModifyHoveredLightTranslationEventCommand) previousCommand)._originalLightTranslationData;
    }

    protected abstract LightTranslationBaseEditorData GetModifiedEventData(
      LightTranslationBaseEditorData originalData);

    protected abstract bool NeedsModification(LightTranslationBaseEditorData originalData);
  }
}
