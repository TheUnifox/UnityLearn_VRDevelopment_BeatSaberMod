// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.CutEventBoxEventsCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System.Collections.Generic;
using Zenject;

namespace BeatmapEditor3D.Commands
{
  public class CutEventBoxEventsCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand,
    IBeatmapEditorCommandMessageProvider
  {
    [Inject]
    private readonly EventBoxesSelectionState _selectionState;
    [Inject]
    private readonly EventBoxGroupsClipboardHelper _eventBoxGroupsClipboardHelper;
    [Inject]
    private readonly SignalBus _signalBus;
    private Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> _baseDataLists;
    private int _cutCount;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._selectionState.events.Count == 0)
        return;
      this._eventBoxGroupsClipboardHelper.CopySelectedEventBoxEvents();
      this._eventBoxGroupsClipboardHelper.GetBaseDataListsFromSelection(out this._baseDataLists);
      this._cutCount = this._selectionState.events.Count;
      this._selectionState.Clear();
      this._signalBus.Fire<EventBoxesSelectionStateUpdatedSignal>();
      this.shouldAddToHistory = true;
      this.Redo();
    }

    public void Undo()
    {
      this._eventBoxGroupsClipboardHelper.InsertEventBoxesBaseLists(this._baseDataLists);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._eventBoxGroupsClipboardHelper.RemoveEventBoxesBaseLists(this._baseDataLists);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Cut {0} event box groups", (object) this._cutCount));
  }
}
