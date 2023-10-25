// Decompiled with JetBrains decompiler
// Type: OculusMRCManager
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

public class OculusMRCManager : MonoBehaviour, OVRMixedRealityCaptureConfiguration
{
  [Inject]
  protected IVRPlatformHelper _vrPlatformHelper;
  protected Func<GameObject, GameObject> _instantiateMixedRealityBackgroundCameraGameObject;
  protected Func<GameObject, GameObject> _instantiateMixedRealityForegroundCameraGameObject;
  [CompilerGenerated]
  protected bool m_enableMixedReality;
  [CompilerGenerated]
  protected LayerMask m_extraHiddenLayers;
  [CompilerGenerated]
  protected LayerMask m_extraVisibleLayers;
  [CompilerGenerated]
  protected bool m_dynamicCullingMask;
  [CompilerGenerated]
  protected OVRManager.CompositionMethod m_compositionMethod;
  [CompilerGenerated]
  protected Color m_externalCompositionBackdropColorRift = new Color(0.0f, 1f, 0.0f, 0.0f);
  [CompilerGenerated]
  protected Color m_externalCompositionBackdropColorQuest = new Color(0.0f, 0.0f, 0.0f, 0.0f);
  [CompilerGenerated]
  protected OVRManager.CameraDevice m_capturingCameraDevice;
  [CompilerGenerated]
  protected bool m_flipCameraFrameHorizontally;
  [CompilerGenerated]
  protected bool m_flipCameraFrameVertically;
  [CompilerGenerated]
  protected float m_handPoseStateLatency;
  [CompilerGenerated]
  protected float m_sandwichCompositionRenderLatency;
  [CompilerGenerated]
  protected int m_sandwichCompositionBufferedFrames = 8;
  [CompilerGenerated]
  protected Color m_chromaKeyColor = Color.green;
  [CompilerGenerated]
  protected float m_chromaKeySimilarity = 0.6f;
  [CompilerGenerated]
  protected float m_chromaKeySmoothRange = 0.03f;
  [CompilerGenerated]
  protected float m_chromaKeySpillRange = 0.06f;
  [CompilerGenerated]
  protected bool m_useDynamicLighting;
  [CompilerGenerated]
  protected OVRManager.DepthQuality m_depthQuality = OVRManager.DepthQuality.Medium;
  [CompilerGenerated]
  protected float m_dynamicLightingSmoothFactor = 8f;
  [CompilerGenerated]
  protected float m_dynamicLightingDepthVariationClampingValue = 1f / 1000f;
  [CompilerGenerated]
  protected OVRManager.VirtualGreenScreenType m_virtualGreenScreenType;
  [CompilerGenerated]
  protected float m_virtualGreenScreenTopY = 10f;
  [CompilerGenerated]
  protected float m_virtualGreenScreenBottomY = -10f;
  [CompilerGenerated]
  protected bool m_virtualGreenScreenApplyDepthCulling;
  [CompilerGenerated]
  protected float m_virtualGreenScreenDepthTolerance = 0.2f;
  [CompilerGenerated]
  protected OVRManager.MrcActivationMode m_mrcActivationMode;
  protected OVRManager.InstantiateMrcCameraDelegate _instantiateMixedRealityCameraGameObject;

  public bool enableMixedReality
  {
    get => this.m_enableMixedReality;
    set => this.m_enableMixedReality = value;
  }

  public LayerMask extraHiddenLayers
  {
    get => this.m_extraHiddenLayers;
    set => this.m_extraHiddenLayers = value;
  }

  public LayerMask extraVisibleLayers
  {
    get => this.m_extraVisibleLayers;
    set => this.m_extraVisibleLayers = value;
  }

  public bool dynamicCullingMask
  {
    get => this.m_dynamicCullingMask;
    set => this.m_dynamicCullingMask = value;
  }

  public OVRManager.CompositionMethod compositionMethod
  {
    get => this.m_compositionMethod;
    set => this.m_compositionMethod = value;
  }

  public Color externalCompositionBackdropColorRift
  {
    get => this.m_externalCompositionBackdropColorRift;
    set => this.m_externalCompositionBackdropColorRift = value;
  }

  public Color externalCompositionBackdropColorQuest
  {
    get => this.m_externalCompositionBackdropColorQuest;
    set => this.m_externalCompositionBackdropColorQuest = value;
  }

  public OVRManager.CameraDevice capturingCameraDevice
  {
    get => this.m_capturingCameraDevice;
    set => this.m_capturingCameraDevice = value;
  }

  public bool flipCameraFrameHorizontally
  {
    get => this.m_flipCameraFrameHorizontally;
    set => this.m_flipCameraFrameHorizontally = value;
  }

  public bool flipCameraFrameVertically
  {
    get => this.m_flipCameraFrameVertically;
    set => this.m_flipCameraFrameVertically = value;
  }

  public float handPoseStateLatency
  {
    get => this.m_handPoseStateLatency;
    set => this.m_handPoseStateLatency = value;
  }

  public float sandwichCompositionRenderLatency
  {
    get => this.m_sandwichCompositionRenderLatency;
    set => this.m_sandwichCompositionRenderLatency = value;
  }

  public int sandwichCompositionBufferedFrames
  {
    get => this.m_sandwichCompositionBufferedFrames;
    set => this.m_sandwichCompositionBufferedFrames = value;
  }

  public Color chromaKeyColor
  {
    get => this.m_chromaKeyColor;
    set => this.m_chromaKeyColor = value;
  }

  public float chromaKeySimilarity
  {
    get => this.m_chromaKeySimilarity;
    set => this.m_chromaKeySimilarity = value;
  }

  public float chromaKeySmoothRange
  {
    get => this.m_chromaKeySmoothRange;
    set => this.m_chromaKeySmoothRange = value;
  }

  public float chromaKeySpillRange
  {
    get => this.m_chromaKeySpillRange;
    set => this.m_chromaKeySpillRange = value;
  }

  public bool useDynamicLighting
  {
    get => this.m_useDynamicLighting;
    set => this.m_useDynamicLighting = value;
  }

  public OVRManager.DepthQuality depthQuality
  {
    get => this.m_depthQuality;
    set => this.m_depthQuality = value;
  }

  public float dynamicLightingSmoothFactor
  {
    get => this.m_dynamicLightingSmoothFactor;
    set => this.m_dynamicLightingSmoothFactor = value;
  }

  public float dynamicLightingDepthVariationClampingValue
  {
    get => this.m_dynamicLightingDepthVariationClampingValue;
    set => this.m_dynamicLightingDepthVariationClampingValue = value;
  }

  public OVRManager.VirtualGreenScreenType virtualGreenScreenType
  {
    get => this.m_virtualGreenScreenType;
    set => this.m_virtualGreenScreenType = value;
  }

  public float virtualGreenScreenTopY
  {
    get => this.m_virtualGreenScreenTopY;
    set => this.m_virtualGreenScreenTopY = value;
  }

  public float virtualGreenScreenBottomY
  {
    get => this.m_virtualGreenScreenBottomY;
    set => this.m_virtualGreenScreenBottomY = value;
  }

  public bool virtualGreenScreenApplyDepthCulling
  {
    get => this.m_virtualGreenScreenApplyDepthCulling;
    set => this.m_virtualGreenScreenApplyDepthCulling = value;
  }

  public float virtualGreenScreenDepthTolerance
  {
    get => this.m_virtualGreenScreenDepthTolerance;
    set => this.m_virtualGreenScreenDepthTolerance = value;
  }

  public OVRManager.MrcActivationMode mrcActivationMode
  {
    get => this.m_mrcActivationMode;
    set => this.m_mrcActivationMode = value;
  }

  public OVRManager.InstantiateMrcCameraDelegate instantiateMixedRealityCameraGameObject
  {
    get
    {
      if (this._instantiateMixedRealityCameraGameObject == null)
        this._instantiateMixedRealityCameraGameObject = new OVRManager.InstantiateMrcCameraDelegate(this.InstantiateMixedRealityCameraGameObject);
      return this._instantiateMixedRealityCameraGameObject;
    }
    set
    {
    }
  }

  public virtual void Update() => OVRManager.StaticUpdateMixedRealityCapture((OVRMixedRealityCaptureConfiguration) this, this.gameObject, OVRManager.TrackingOrigin.FloorLevel);

  public virtual void OnDestroy() => OVRManager.StaticShutdownMixedRealityCapture((OVRMixedRealityCaptureConfiguration) this);

  public virtual void Init(
    Func<GameObject, GameObject> instantiateMixedRealityBackgroundCameraGameObject,
    Func<GameObject, GameObject> instantiateMixedRealityForegroundCameraGameObject)
  {
    if (this._vrPlatformHelper.vrPlatformSDK != VRPlatformSDK.Oculus)
    {
      this.enabled = false;
    }
    else
    {
      this.enabled = true;
      this._instantiateMixedRealityBackgroundCameraGameObject = instantiateMixedRealityBackgroundCameraGameObject;
      this._instantiateMixedRealityForegroundCameraGameObject = instantiateMixedRealityForegroundCameraGameObject;
      Debug.Log((object) "Init OculusMRCManager");
      this.ProcessCommandLineSettings();
      OVRManager.StaticInitializeMixedRealityCapture((OVRMixedRealityCaptureConfiguration) this);
    }
  }

  public virtual void ProcessCommandLineSettings()
  {
    bool flag1 = false;
    bool flag2 = false;
    foreach (string commandLineArg in Environment.GetCommandLineArgs())
    {
      switch (commandLineArg.ToLower())
      {
        case "-mixedreality":
          this.enableMixedReality = true;
          break;
        case "-directcomposition":
          this.compositionMethod = OVRManager.CompositionMethod.Direct;
          break;
        case "-externalcomposition":
          this.compositionMethod = OVRManager.CompositionMethod.External;
          break;
        case "-create_mrc_config":
          flag1 = true;
          break;
        case "-load_mrc_config":
          flag2 = true;
          break;
      }
    }
    if (!(flag2 | flag1))
      return;
    OVRMixedRealityCaptureSettings instance = ScriptableObject.CreateInstance<OVRMixedRealityCaptureSettings>();
    instance.ReadFrom((OVRMixedRealityCaptureConfiguration) this);
    if (flag2)
    {
      instance.CombineWithConfigurationFile();
      instance.ApplyTo((OVRMixedRealityCaptureConfiguration) this);
    }
    if (flag1)
      instance.WriteToConfigurationFile();
    UnityEngine.Object.Destroy((UnityEngine.Object) instance);
  }

  public virtual GameObject InstantiateMixedRealityCameraGameObject(
    GameObject mainCameraGameObject,
    OVRManager.MrcCameraType cameraType)
  {
    if (cameraType == OVRManager.MrcCameraType.Foreground)
      return this._instantiateMixedRealityForegroundCameraGameObject(mainCameraGameObject);
    return this._instantiateMixedRealityBackgroundCameraGameObject(mainCameraGameObject);
  }
}
