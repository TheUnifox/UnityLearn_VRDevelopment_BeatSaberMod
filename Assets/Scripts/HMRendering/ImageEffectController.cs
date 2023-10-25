// Decompiled with JetBrains decompiler
// Type: ImageEffectController
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof (Camera))]
public class ImageEffectController : MonoBehaviour
{
  protected ImageEffectController.RenderImageCallback _renderImageCallback;

  public virtual void SetCallback(
    ImageEffectController.RenderImageCallback renderImageCallback)
  {
    this._renderImageCallback = renderImageCallback;
  }

  public virtual void OnRenderImage(RenderTexture src, RenderTexture dest)
  {
    ImageEffectController.RenderImageCallback renderImageCallback = this._renderImageCallback;
    if (renderImageCallback == null)
      return;
    renderImageCallback(src, dest);
  }

  public delegate void RenderImageCallback(RenderTexture src, RenderTexture dest);
}
