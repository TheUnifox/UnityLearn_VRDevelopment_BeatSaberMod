// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.CalculateBeatmapObjectsCountCommand
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.SerializedData;
using BeatmapEditor3D.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Zenject;

namespace BeatmapEditor3D
{
  public class CalculateBeatmapObjectsCountCommand : 
    BaseAsyncBeatmapEditorCommand<object, (int, int, int, int, int, int, int, int)>
  {
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly BeatmapBasicEventsDataModel _beatmapBasicEventsDataModel;
    [Inject]
    private readonly BeatmapEventBoxGroupsDataModel _beatmapEventBoxGroupsDataModel;
    [Inject]
    private readonly SignalBus _signalBus;

    protected override BaseAsyncBeatmapEditorCommand<object, (int, int, int, int, int, int, int, int)>.MultipleSameTasksRunPolicy multipleSameTasksRunPolicy => BaseAsyncBeatmapEditorCommand<object, (int, int, int, int, int, int, int, int)>.MultipleSameTasksRunPolicy.CancelRunning;

    protected override (int, int, int, int, int, int, int, int) ExecuteAsync(
      object input,
      CancellationToken cancellationToken)
    {
      int num1 = this._beatmapLevelDataModel.notes.Count<NoteEditorData>((Func<NoteEditorData, bool>) (note => note.noteType != NoteType.Bomb));
      int num2 = this._beatmapLevelDataModel.notes.Count - num1;
      int count1 = this._beatmapLevelDataModel.obstacles.Count;
      int count2 = this._beatmapLevelDataModel.chains.Count;
      int count3 = this._beatmapLevelDataModel.arcs.Count;
      int num3 = this._beatmapBasicEventsDataModel.Count();
      int num4 = this._beatmapEventBoxGroupsDataModel.GetAllEventBoxGroups().Count<BeatmapEditorEventBoxGroupInput>();
      int num5 = 0;
      foreach (EventBoxGroupEditorData eventBoxGroupsAs in this._beatmapEventBoxGroupsDataModel.GetAllEventBoxGroupsAsList())
      {
        foreach (EventBoxEditorData eventBoxEditorData in this._beatmapEventBoxGroupsDataModel.GetEventBoxesByEventBoxGroupId(eventBoxGroupsAs.id))
        {
          List<BaseEditorData> listByEventBoxId = this._beatmapEventBoxGroupsDataModel.GetBaseEditorDataListByEventBoxId(eventBoxEditorData.id);
          int groupSize;
          if (this._beatmapEventBoxGroupsDataModel.TryGetGroupSizeByEventBoxGroupId(eventBoxGroupsAs.groupId, out groupSize))
          {
            IndexFilter indexFilter = BeatmapDataLoader.IndexFilterConvertor.Convert(BeatmapLevelDataModelSaver.CreateIndexFilter(eventBoxEditorData.indexFilter), groupSize);
            num5 += listByEventBoxId.Count * indexFilter.VisibleCount;
          }
        }
      }
      return (num1, num2, count1, count2, count3, num3, num4, num5);
    }

    protected override void TaskFinished((int, int, int, int, int, int, int, int) result)
    {
      (int, int, int, int, int, int, int, int) tuple = result;
      this._signalBus.Fire<BeatmapObjectsCountUpdatedSignal>(new BeatmapObjectsCountUpdatedSignal(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5, tuple.Item6, tuple.Item7, tuple.Item8));
    }
  }
}
