// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapsListViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Views;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapsListViewController : BeatmapEditorViewController
  {
    [SerializeField]
    private RecentBeatmapsView _recentBeatmapsView;
    [SerializeField]
    private BeatmapsListTableView _beatmapsListTableView;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    [Inject]
    private readonly IReadonlyBeatmapCollectionDataModel _beatmapsCollectionDataModel;

    public event Action<IBeatmapInfoData> openBeatmapEvent;

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      this._signalBus.Subscribe<BeatmapsCollectionSignals.UpdatedSignal>(new Action(this.HandleBeatmapsCollectionDataModelUpdated));
      this._recentBeatmapsView.SetData(this._beatmapEditorSettingsDataModel.recentlyOpenedBeatmaps, this._beatmapsCollectionDataModel.beatmapInfos);
      this._beatmapsListTableView.SetData(this._beatmapsCollectionDataModel.beatmapInfos);
      this._recentBeatmapsView.openRecentBeatmapEvent += new Action<string>(this.HandleOpenRecentBeatmap);
      this._beatmapsListTableView.openBeatmapEvent += new Action<int>(this.HandleBeatmapListTableViewOpenBeatmap);
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
    {
      this._signalBus.TryUnsubscribe<BeatmapsCollectionSignals.UpdatedSignal>(new Action(this.HandleBeatmapsCollectionDataModelUpdated));
      this._beatmapsListTableView.openBeatmapEvent -= new Action<int>(this.HandleBeatmapListTableViewOpenBeatmap);
      this._recentBeatmapsView.openRecentBeatmapEvent -= new Action<string>(this.HandleOpenRecentBeatmap);
    }

    private void HandleBeatmapsCollectionDataModelUpdated() => this._beatmapsListTableView.SetData(this._beatmapsCollectionDataModel.beatmapInfos);

    private void HandleBeatmapListTableViewOpenBeatmap(int idx)
    {
      IBeatmapInfoData beatmapInfo = this._beatmapsCollectionDataModel.beatmapInfos[idx];
      Action<IBeatmapInfoData> openBeatmapEvent = this.openBeatmapEvent;
      if (openBeatmapEvent == null)
        return;
      openBeatmapEvent(beatmapInfo);
    }

    private void HandleOpenRecentBeatmap(string path)
    {
      IBeatmapInfoData beatmapInfoData = this._beatmapsCollectionDataModel.beatmapInfos.FirstOrDefault<IBeatmapInfoData>((Func<IBeatmapInfoData, bool>) (beatmapInfo => beatmapInfo.beatmapFolderPath == path));
      if (beatmapInfoData == null)
        return;
      Action<IBeatmapInfoData> openBeatmapEvent = this.openBeatmapEvent;
      if (openBeatmapEvent == null)
        return;
      openBeatmapEvent(beatmapInfoData);
    }
  }
}
