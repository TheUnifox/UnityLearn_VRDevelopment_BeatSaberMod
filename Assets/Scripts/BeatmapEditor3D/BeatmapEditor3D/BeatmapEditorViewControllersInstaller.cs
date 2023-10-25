// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorViewControllersInstaller
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorViewControllersInstaller : MonoInstaller
  {
    [SerializeField]
    private Transform _parentTransform;
    [Header("View Controllers")]
    [SerializeField]
    private SimpleEditorDialogViewController _simpleEditorDialogViewControllerPrefab;
    [SerializeField]
    private LoadingViewController _loadingViewControllerPrefab;
    [Header("Views")]
    [SerializeField]
    private BeatmapsListTableCell _beatmapsListTableCellPrefab;

    public override void InstallBindings()
    {
      this.Container.Bind<SimpleEditorDialogViewController>().FromComponentInNewPrefab((Object) this._simpleEditorDialogViewControllerPrefab).UnderTransform(this._parentTransform).AsTransient();
      this.Container.Bind<LoadingViewController>().FromComponentInNewPrefab((Object) this._loadingViewControllerPrefab).UnderTransform(this._parentTransform).AsTransient();
      this.Container.BindFactory<BeatmapsListTableCell, BeatmapsListTableCell.Factory>().FromComponentInNewPrefab((Object) this._beatmapsListTableCellPrefab);
    }
  }
}
