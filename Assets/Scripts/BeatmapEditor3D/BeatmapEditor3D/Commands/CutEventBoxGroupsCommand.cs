// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.CutEventBoxGroupsCommand
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
  public class CutEventBoxGroupsCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand,
    IBeatmapEditorCommandMessageProvider
  {
    [Inject]
    private readonly EventBoxGroupsClipboardState _clipboardState;
    [Inject]
    private readonly EventBoxGroupsSelectionState _selectionState;
    [Inject]
    private readonly EventBoxGroupsClipboardHelper _eventBoxGroupsClipboardHelper;
    [Inject]
    private readonly SignalBus _signalBus;
    private List<EventBoxGroupEditorData> _clipboardEventBoxGroups;
    private readonly Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>> _clipboardEventBoxes = new Dictionary<BeatmapEditorObjectId, List<EventBoxEditorData>>();
    private readonly Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> _clipboardBaseDataLists = new Dictionary<BeatmapEditorObjectId, List<BaseEditorData>>();

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._selectionState.eventBoxGroups.Count == 0)
        return;
      this._eventBoxGroupsClipboardHelper.CopySelectedEventBoxGroups();
      this._selectionState.Clear();
      this._clipboardEventBoxGroups = this._clipboardState.eventBoxGroups.ToList<EventBoxGroupEditorData>();
      foreach (EventBoxGroupEditorData clipboardEventBoxGroup in this._clipboardEventBoxGroups)
      {
        List<EventBoxEditorData> eventBox = this._clipboardState.eventBoxes[clipboardEventBoxGroup.id];
        foreach (EventBoxEditorData eventBoxEditorData in eventBox)
          this._clipboardBaseDataLists[eventBoxEditorData.id] = this._clipboardState.baseEditorDataLists[eventBoxEditorData.id].ToList<BaseEditorData>();
        this._clipboardEventBoxes[clipboardEventBoxGroup.id] = eventBox.ToList<EventBoxEditorData>();
      }
      this._signalBus.Fire<EventBoxGroupsClipboardStateUpdatedSignal>();
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._eventBoxGroupsClipboardHelper.InsertEventBoxGroupData(this._clipboardEventBoxGroups, this._clipboardEventBoxes, this._clipboardBaseDataLists);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._eventBoxGroupsClipboardHelper.RemoveEventBoxGroupData(this._clipboardEventBoxGroups, this._clipboardEventBoxes, this._clipboardBaseDataLists);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Cut {0} event box groups", (object) this._clipboardEventBoxGroups?.Count));
  }
}
