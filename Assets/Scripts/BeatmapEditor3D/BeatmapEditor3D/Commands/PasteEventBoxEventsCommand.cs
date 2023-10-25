// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Commands.PasteEventBoxEventsCommand
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
  public class PasteEventBoxEventsCommand : 
    IBeatmapEditorCommandWithHistory,
    IBeatmapEditorCommand,
    IBeatmapEditorCommandMessageProvider
  {
    [Inject]
    private readonly EventBoxesClipboardState _clipboardState;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly EventBoxGroupsClipboardHelper _eventBoxGroupsClipboardHelper;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly EventBoxGroupsState _eventBoxGroupsState;
    [Inject]
    private readonly SignalBus _signalBus;
    private Dictionary<BeatmapEditorObjectId, List<BaseEditorData>> _baseDataLists;
    private int _count;

    public bool shouldAddToHistory { get; private set; }

    public void Execute()
    {
      if (this._clipboardState.baseEditorDataList.Count == 0)
        return;
      if (this._eventBoxGroupsState.currentHoverGroupType != this._clipboardState.eventBoxGroupType)
      {
        this._signalBus.Fire<CommandMessageSignal>(new CommandMessageSignal(CommandMessageType.Warning, "Unable to paste events, unsupported event type conversion."));
      }
      else
      {
        List<EventBoxEditorData> byEventBoxGroupId = this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(this._eventBoxGroupsState.eventBoxGroupContext.id);
        int index1 = byEventBoxGroupId.FindIndex((Predicate<EventBoxEditorData>) (e => e.id == this._eventBoxGroupsState.currentHoverEventBoxId));
        this._baseDataLists = new Dictionary<BeatmapEditorObjectId, List<BaseEditorData>>();
        foreach (KeyValuePair<int, List<BaseEditorData>> baseEditorData in (IEnumerable<KeyValuePair<int, List<BaseEditorData>>>) this._clipboardState.baseEditorDataList)
        {
          int index2 = index1 + (baseEditorData.Key - this._clipboardState.startEventBoxIndex);
          if (index2 < byEventBoxGroupId.Count)
          {
            foreach (BaseEditorData d in baseEditorData.Value)
            {
              BeatmapEditorObjectId id = byEventBoxGroupId[index2].id;
              if (!this._baseDataLists.ContainsKey(id))
                this._baseDataLists[id] = new List<BaseEditorData>();
              float num = this._beatmapState.offsetBeat + (d.beat - this._clipboardState.startBeat);
              this._baseDataLists[id].Add(EventBoxGroupsClipboardHelper.CopyBaseEditorDataWithoutId(d, new float?(num)));
              ++this._count;
            }
          }
        }
        if (this._eventBoxGroupsClipboardHelper.CheckEventBoxesBaseListsCollisions(this._baseDataLists))
        {
          this._signalBus.Fire<CommandMessageSignal>(new CommandMessageSignal(CommandMessageType.Warning, "Unable to paste events, another event/s exist at beats."));
        }
        else
        {
          this.shouldAddToHistory = true;
          this.Redo();
        }
      }
    }

    public void Undo()
    {
      this._eventBoxGroupsClipboardHelper.RemoveEventBoxesBaseLists(this._baseDataLists);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public void Redo()
    {
      this._eventBoxGroupsClipboardHelper.InsertEventBoxesBaseLists(this._baseDataLists);
      this._signalBus.Fire<BeatmapLevelUpdatedSignal>();
    }

    public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, string.Format("Pasted {0} event box events", (object) this._count));
  }
}
