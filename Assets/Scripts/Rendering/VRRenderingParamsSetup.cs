// Decompiled with JetBrains decompiler
// Type: VRRenderingParamsSetup
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using UnityEngine;
using UnityEngine.XR;
using Zenject;

public class VRRenderingParamsSetup : MonoBehaviour
{
  [SerializeField]
  private FloatSO _vrResolutionScale;
  [SerializeField]
  private FloatSO _menuVRResolutionScaleMultiplier;
  [SerializeField]
  private BoolSO _useFixedFoveatedRenderingDuringGameplay;
  [SerializeField]
  private VRRenderingParamsSetup.SceneType _sceneType;
  [Inject]
  private IVRPlatformHelper _vrPlatformHelper;

  protected void OnEnable()
  {
    if ((double) (float) (ObservableVariableSO<float>) this._vrResolutionScale == 0.0)
      this._vrResolutionScale.value = 1f;
    if ((double) (float) (ObservableVariableSO<float>) this._menuVRResolutionScaleMultiplier == 0.0)
      this._menuVRResolutionScaleMultiplier.value = 1f;
    XRSettings.eyeTextureResolutionScale = (float) (ObservableVariableSO<float>) this._vrResolutionScale * (this._sceneType == VRRenderingParamsSetup.SceneType.Menu ? (float) (ObservableVariableSO<float>) this._menuVRResolutionScaleMultiplier : 1f);
    if (this._vrPlatformHelper.vrPlatformSDK != VRPlatformSDK.Oculus)
      return;
    if (OVRManager.fixedFoveatedRenderingSupported)
      OVRManager.fixedFoveatedRenderingLevel = !(bool) (ObservableVariableSO<bool>) this._useFixedFoveatedRenderingDuringGameplay || this._sceneType != VRRenderingParamsSetup.SceneType.Game ? OVRManager.FixedFoveatedRenderingLevel.Off : OVRManager.FixedFoveatedRenderingLevel.HighTop;
    OVRPlugin.SetClientColorDesc(OVRPlugin.ColorSpace.Rift_CV1);
    foreach (float a in OVRPlugin.systemDisplayFrequenciesAvailable)
    {
      if (Mathf.Approximately(a, 90f))
      {
        OVRPlugin.systemDisplayFrequency = 90f;
        break;
      }
    }
  }

  public enum SceneType
  {
    Undefined,
    Menu,
    Game,
  }
}
