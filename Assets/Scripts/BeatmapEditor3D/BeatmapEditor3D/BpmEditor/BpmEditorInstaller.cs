// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BpmEditor.BpmEditorInstaller
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor.UI;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.BpmEditor
{
  public class BpmEditorInstaller : MonoInstaller
  {
    [SerializeField]
    private BpmRegionView _bpmRegionViewPrefab;
    [SerializeField]
    private BpmRegionDividerView _bpmRegionDividerViewPrefab;
    [SerializeField]
    private BpmBeatMarkerView _bpmBeatMarkerViewPrefab;
    [SerializeField]
    private BpmQuarterBeatMarkerView _bpmQuarterBeatMarkerViewPrefab;

    public override void InstallBindings()
    {
      this.Container.Bind<BpmEditorState>().AsSingle();
      this.Container.Bind<BpmEditorDataModel>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BpmEditorAudioScrollController>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BpmEditorSongPreviewController>().AsSingle();
      this.Container.BindInterfacesAndSelfTo<BpmEditorMetronomeController>().AsSingle();
      this.Container.BindMemoryPool<BpmRegionView, BpmRegionView.Pool>().WithInitialSize(3).ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._bpmRegionViewPrefab);
      this.Container.BindMemoryPool<BpmRegionDividerView, BpmRegionDividerView.Pool>().WithInitialSize(3).ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._bpmRegionDividerViewPrefab);
      this.Container.BindMemoryPool<BpmBeatMarkerView, BpmBeatMarkerView.Pool>().WithInitialSize(20).ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._bpmBeatMarkerViewPrefab);
      this.Container.BindMemoryPool<BpmQuarterBeatMarkerView, BpmQuarterBeatMarkerView.Pool>().WithInitialSize(20).ExpandByOneAtATime(false).FromComponentInNewPrefab((Object) this._bpmQuarterBeatMarkerViewPrefab);
    }
  }
}
