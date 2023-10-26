// Decompiled with JetBrains decompiler
// Type: CommandBufferGrabPass
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class CommandBufferGrabPass : CommandBufferGOCore
{
  [SerializeField]
  private string _textureName = "_GrabTexture1";
  [SerializeField]
  private CameraEvent _cameraEvent = CameraEvent.BeforeForwardAlpha;
  private static Dictionary<Camera, CommandBufferOwners> _cameras = new Dictionary<Camera, CommandBufferOwners>();

  protected override CommandBuffer CreateCommandBuffer(Camera camera)
  {
    int id = Shader.PropertyToID(this._textureName);
    int width = !camera.stereoEnabled || camera.stereoTargetEye != StereoTargetEyeMask.Both ? camera.pixelWidth : camera.pixelWidth * 2;
    int pixelHeight = camera.pixelHeight;
    CommandBuffer commandBuffer = new CommandBuffer();
    commandBuffer.GetTemporaryRT(id, width, pixelHeight, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
    commandBuffer.CopyTexture((RenderTargetIdentifier) BuiltinRenderTextureType.CurrentActive, (RenderTargetIdentifier) id);
    commandBuffer.SetGlobalTexture(this._textureName, (RenderTargetIdentifier) id);
    commandBuffer.ReleaseTemporaryRT(id);
    commandBuffer.name = "Command Buffer Grab Pass";
    return commandBuffer;
  }

  protected override Dictionary<Camera, CommandBufferOwners> CamerasDict() => CommandBufferGrabPass._cameras;

  protected override CameraEvent CommandBufferCameraEvent() => this._cameraEvent;
}
