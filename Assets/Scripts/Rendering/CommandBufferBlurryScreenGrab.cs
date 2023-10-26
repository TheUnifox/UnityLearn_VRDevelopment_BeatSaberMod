// Decompiled with JetBrains decompiler
// Type: CommandBufferBlurryScreenGrab
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class CommandBufferBlurryScreenGrab : CommandBufferGOCore
{
  [SerializeField]
  private KawaseBlurRendererSO _kawaseBlurRenderer;
  [SerializeField]
  private KawaseBlurRendererSO.KernelSize _kernelSize = KawaseBlurRendererSO.KernelSize.Kernel63;
  [SerializeField]
  private CameraEvent _cameraEvent = CameraEvent.BeforeForwardAlpha;
  [SerializeField]
  private int _downsample;
  private static Dictionary<Camera, CommandBufferOwners> _cameras = new Dictionary<Camera, CommandBufferOwners>();

  protected override CommandBuffer CreateCommandBuffer(Camera camera)
  {
    int num1 = !camera.stereoEnabled || camera.stereoTargetEye != StereoTargetEyeMask.Both ? camera.pixelWidth : camera.pixelWidth * 2;
    int pixelHeight = camera.pixelHeight;
    int num2 = this._downsample & 31;
    CommandBuffer blurCommandBuffer = this._kawaseBlurRenderer.CreateBlurCommandBuffer(num1 >> num2, pixelHeight >> this._downsample, "_GrabBlurTexture", this._kernelSize, 0.0f);
    blurCommandBuffer.name = "Grab screen and blur";
    return blurCommandBuffer;
  }

  protected override Dictionary<Camera, CommandBufferOwners> CamerasDict() => CommandBufferBlurryScreenGrab._cameras;

  protected override CameraEvent CommandBufferCameraEvent() => this._cameraEvent;
}
