// Decompiled with JetBrains decompiler
// Type: MainCameraCullingMask
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using UnityEngine;
using Zenject;

public class MainCameraCullingMask : MonoBehaviour
{
  [SerializeField]
  private Camera _camera;
  [InjectOptional]
  private readonly MainCameraCullingMask.InitData _initData = new MainCameraCullingMask.InitData(true);

  protected void Start()
  {
    if (this._initData.showDebris)
      this._camera.cullingMask |= 1 << LayerMasks.noteDebrisLayer;
    else
      this._camera.cullingMask &= ~(1 << LayerMasks.noteDebrisLayer);
  }

  public class InitData
  {
    public readonly bool showDebris;

    public InitData(bool showDebris) => this.showDebris = showDebris;
  }
}
