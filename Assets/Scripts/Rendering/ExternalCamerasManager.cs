// Decompiled with JetBrains decompiler
// Type: ExternalCamerasManager
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System;
using UnityEngine;

public class ExternalCamerasManager : MonoBehaviour
{
  [SerializeField]
  private OculusMRCManager _oculusMRCManager;
  [SerializeField]
  private Camera _mrcBackgroundCameraPrefab;
  [SerializeField]
  private Camera _mrcForegroundCameraPrefab;

  protected void OnEnable() => this._oculusMRCManager.Init(new Func<GameObject, GameObject>(this.InstantiateMixedRealityBackgroundCameraGameObject), new Func<GameObject, GameObject>(this.InstantiateMixedRealityForegroundCameraGameObject));

  private GameObject InstantiateMixedRealityBackgroundCameraGameObject(
    GameObject mainCameraGameObject)
  {
    return UnityEngine.Object.Instantiate<Camera>(this._mrcBackgroundCameraPrefab).gameObject;
  }

  private GameObject InstantiateMixedRealityForegroundCameraGameObject(
    GameObject mainCameraGameObject)
  {
    return UnityEngine.Object.Instantiate<Camera>(this._mrcForegroundCameraPrefab).gameObject;
  }
}
