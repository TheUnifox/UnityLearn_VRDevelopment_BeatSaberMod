// Decompiled with JetBrains decompiler
// Type: ScreenCaptureCache
// Assembly: Rendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5B7D31E1-1F9E-4BE6-B735-D2EB47EFDA46
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Rendering.dll

using System.Collections.Generic;
using UnityEngine;

public class ScreenCaptureCache
{
  private Dictionary<ScreenCaptureCache.ScreenshotType, Texture2D> _cache = new Dictionary<ScreenCaptureCache.ScreenshotType, Texture2D>();

  public Texture2D GetLastScreenshot(ScreenCaptureCache.ScreenshotType screenshotType)
  {
    Texture2D texture2D;
    return this._cache.TryGetValue(screenshotType, out texture2D) ? texture2D : (Texture2D) null;
  }

  public void StoreScreenshot(ScreenCaptureCache.ScreenshotType screenshotType, Texture2D texture)
  {
    Texture2D texture2D;
    if (this._cache.TryGetValue(screenshotType, out texture2D))
      EssentialHelpers.SafeDestroy((Object) texture2D);
    this._cache[screenshotType] = texture;
  }

  public enum ScreenshotType
  {
    Game,
    Menu,
    Other,
  }
}
