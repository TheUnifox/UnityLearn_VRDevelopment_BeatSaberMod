// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.BeatNumberContainer
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Views;
using Zenject;

namespace BeatmapEditor3D.Visuals
{
  public class BeatNumberContainer : BaseBeatBasedItemContainer<BpmRegionBeatNumberView>
  {
    [Inject]
    private readonly BpmRegionBeatNumberView.Pool _bpmRegionBeatNumberViewPool;

    protected override BpmRegionBeatNumberView SpawnItem() => this._bpmRegionBeatNumberViewPool.Spawn();

    protected override void DespawnItem(BpmRegionBeatNumberView item) => this._bpmRegionBeatNumberViewPool.Despawn(item);

    protected override void SetItemData(BpmRegionBeatNumberView item, float beat) => item.SetBeatNumber(beat);
  }
}
