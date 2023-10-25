// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorGameplayFpfcInit
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using VRUIControls;

namespace BeatmapEditor3D
{
  public class BeatmapEditorGameplayFpfcInit : MonoBehaviour
  {
    [SerializeField]
    private FirstPersonFlyingController _firstPersonFlyingController;
    [Space]
    [SerializeField]
    private Vector3 _defaultPlayerHeight = new Vector3(0.0f, 1.7f, 0.0f);

    [Zenject.Inject]
    public void Inject(
      BeatmapEditorGameplayFpfcInit.Init init,
      MainCamera mainCamera,
      VRCenterAdjust vrCenterAdjust,
      PlayerTransforms playerTransforms,
      PlayerVRControllersManager playerVRControllersManager)
    {
      if (init.enableFpfc)
        playerTransforms.OverrideHeadPos(this._defaultPlayerHeight);
      this._firstPersonFlyingController.Inject(mainCamera.camera, vrCenterAdjust, playerVRControllersManager.leftHandVRController, playerVRControllersManager.rightHandVRController, (VRInputModule) null, init.enableFpfc);
      this._firstPersonFlyingController.gameObject.SetActive(init.enableFpfc);
    }

    public class Init
    {
      public readonly bool enableFpfc;

      public Init(bool enableFpfc) => this.enableFpfc = enableFpfc;
    }
  }
}
