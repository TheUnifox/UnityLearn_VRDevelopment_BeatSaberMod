// Decompiled with JetBrains decompiler
// Type: ScreenCaptureAfterDelay
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class ScreenCaptureAfterDelay : MonoBehaviour
{
  [Inject]
  private MainEffectController _mainEffectController;
  [Inject]
  private ScreenCaptureCache _screenCaptureCache;
  [Inject]
  private ScreenCaptureAfterDelay.InitData _initData;
  private Texture2D _captureTexture;
  private RenderTexture _captureRenderTexture;

  protected IEnumerator Start()
  {
        yield return new WaitForSeconds(this._initData.screenCaptureTime);
        this._mainEffectController.afterImageEffectEvent += this.HandleMainEffectControllerAfterImageEffectEvent;
        this._captureRenderTexture = new RenderTexture(this._initData.pixelsWidth, this._initData.pixelsHeight, 0);
        this._captureTexture = new Texture2D(this._captureRenderTexture.width, this._captureRenderTexture.height, TextureFormat.RGB24, false);
        yield break;
    }

  protected void OnDestroy()
  {
    if ((UnityEngine.Object) this._mainEffectController != (UnityEngine.Object) null)
      this._mainEffectController.afterImageEffectEvent -= new Action<RenderTexture>(this.HandleMainEffectControllerAfterImageEffectEvent);
    this._captureRenderTexture.Release();
    EssentialHelpers.SafeDestroy((UnityEngine.Object) this._captureRenderTexture);
  }

  private void HandleMainEffectControllerAfterImageEffectEvent(RenderTexture renderTexture)
  {
    if ((UnityEngine.Object) this._mainEffectController != (UnityEngine.Object) null)
      this._mainEffectController.afterImageEffectEvent -= new Action<RenderTexture>(this.HandleMainEffectControllerAfterImageEffectEvent);
    if ((UnityEngine.Object) renderTexture == (UnityEngine.Object) null)
      return;
    RenderTexture active = RenderTexture.active;
    Graphics.Blit((Texture) renderTexture, this._captureRenderTexture, new Vector2(0.5f, -0.5f), new Vector2(0.5f, 0.75f));
    RenderTexture.active = this._captureRenderTexture;
    this._captureTexture.ReadPixels(new Rect(0.0f, 0.0f, (float) this._captureRenderTexture.width, (float) this._captureRenderTexture.height), 0, 0);
    this._captureTexture.Apply();
    RenderTexture.active = active;
    this._screenCaptureCache.StoreScreenshot(this._initData.screenshotType, this._captureTexture);
  }

  public class InitData
  {
    public readonly ScreenCaptureCache.ScreenshotType screenshotType;
    public readonly float screenCaptureTime = 5f;
    public readonly int pixelsWidth = 1920;
    public readonly int pixelsHeight = 1080;

    public InitData(
      ScreenCaptureCache.ScreenshotType screenshotType,
      float screenCaptureTime,
      int pixelsWidth,
      int pixelsHeight)
    {
      this.screenshotType = screenshotType;
      this.screenCaptureTime = screenCaptureTime;
      this.pixelsWidth = pixelsWidth;
      this.pixelsHeight = pixelsHeight;
    }
  }
}
