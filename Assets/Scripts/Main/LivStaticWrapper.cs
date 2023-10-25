// Decompiled with JetBrains decompiler
// Type: LivStaticWrapper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class LivStaticWrapper
{
  private static LIV.SDK.Unity.LIV _liv;

  public static event System.Action didActivateEvent;

  public static event System.Action didDeactivateEvent;

  public static bool isActive => (bool) (UnityEngine.Object) LivStaticWrapper._liv && LivStaticWrapper._liv.isActive;

  public static void Init(MainCamera mainCamera)
  {
    if (!Application.isPlaying || (UnityEngine.Object) LivStaticWrapper._liv != (UnityEngine.Object) null)
      return;
    GameObject target = new GameObject("LIV");
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) target);
    target.gameObject.SetActive(false);
    LivStaticWrapper._liv = target.AddComponent<LIV.SDK.Unity.LIV>();
    LivStaticWrapper._liv.fixPostEffectsAlpha = true;
    LivStaticWrapper._liv.excludeBehaviours = new string[3]
    {
      "AudioListener",
      "Collider",
      "MainCamera"
    };
    LivStaticWrapper._liv.spectatorLayerMask = (LayerMask) -1;
    LIV.SDK.Unity.LIV liv1 = LivStaticWrapper._liv;
    liv1.spectatorLayerMask = (LayerMask) ((int) liv1.spectatorLayerMask & -1025);
    LIV.SDK.Unity.LIV liv2 = LivStaticWrapper._liv;
    liv2.spectatorLayerMask = (LayerMask) ((int) liv2.spectatorLayerMask & -65537);
    LIV.SDK.Unity.LIV liv3 = LivStaticWrapper._liv;
    liv3.spectatorLayerMask = (LayerMask) ((int) liv3.spectatorLayerMask & -33554433);
    LIV.SDK.Unity.LIV liv4 = LivStaticWrapper._liv;
    liv4.spectatorLayerMask = (LayerMask) ((int) liv4.spectatorLayerMask & -134217729);
    LIV.SDK.Unity.LIV liv5 = LivStaticWrapper._liv;
    liv5.spectatorLayerMask = (LayerMask) ((int) liv5.spectatorLayerMask & int.MaxValue);
    LivStaticWrapper._liv.HMDCamera = mainCamera.camera;
    LivStaticWrapper._liv.stage = mainCamera.transform.parent;
    LivStaticWrapper._liv.onActivate += new System.Action(LivStaticWrapper.HandleLIVOnActivate);
    LivStaticWrapper._liv.onDeactivate += new System.Action(LivStaticWrapper.HandleLIVOnDeactivate);
    LivStaticWrapper._liv.gameObject.SetActive(true);
  }

  public static void Update(MainCamera mainCamera)
  {
    if ((UnityEngine.Object) LivStaticWrapper._liv == (UnityEngine.Object) null)
      return;
    LivStaticWrapper._liv.HMDCamera = mainCamera.camera;
    LivStaticWrapper._liv.stage = mainCamera.transform.parent;
  }

  public static void Activate()
  {
    if ((UnityEngine.Object) LivStaticWrapper._liv == (UnityEngine.Object) null)
      return;
    LivStaticWrapper._liv.Activate();
    if (!LivStaticWrapper._liv.isActive)
      return;
    LivStaticWrapper._liv.render.UpdateCameraSettings();
  }

  public static Camera GetExternalCamera() => LivStaticWrapper._liv.render.cameraInstance;

  private static void HandleLIVOnActivate()
  {
    System.Action didActivateEvent = LivStaticWrapper.didActivateEvent;
    if (didActivateEvent == null)
      return;
    didActivateEvent();
  }

  private static void HandleLIVOnDeactivate()
  {
    System.Action didDeactivateEvent = LivStaticWrapper.didDeactivateEvent;
    if (didDeactivateEvent == null)
      return;
    didDeactivateEvent();
  }
}
