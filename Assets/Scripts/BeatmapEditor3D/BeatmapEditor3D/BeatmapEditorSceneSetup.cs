// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorSceneSetup
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorSceneSetup : MonoBehaviour
  {
    [SerializeField]
    private BeatmapEditorFlowCoordinator _rootFlowCoordinator;
    [Inject]
    private readonly BeatmapEditorHierarchyManager _hierarchyManager;

    private void Start() => this._hierarchyManager.StartWithFlowCoordinator(this._rootFlowCoordinator);
  }
}
