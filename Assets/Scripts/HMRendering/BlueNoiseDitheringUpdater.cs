// Decompiled with JetBrains decompiler
// Type: BlueNoiseDitheringUpdater
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

[ExecuteInEditMode]
public class BlueNoiseDitheringUpdater : MonoBehaviour
{
  [SerializeField]
  protected BlueNoiseDithering _blueNoiseDithering;
  [SerializeField]
  protected RandomValueToShader _randomValueToShader;

  public virtual void OnEnable()
  {
    Camera.onPreRender -= new Camera.CameraCallback(this.HandleCameraPreRender);
    Camera.onPreRender += new Camera.CameraCallback(this.HandleCameraPreRender);
  }

  public virtual void OnDisable() => Camera.onPreRender -= new Camera.CameraCallback(this.HandleCameraPreRender);

  public virtual void HandleCameraPreRender(Camera camera)
  {
    this._randomValueToShader.SetRandomValueToShaders();
    this._blueNoiseDithering.SetBlueNoiseShaderParams(camera.pixelWidth, camera.pixelHeight);
  }
}
