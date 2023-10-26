// Decompiled with JetBrains decompiler
// Type: HMUI.CurvedCanvasSettingsHelper
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System.Collections.Generic;
using UnityEngine;

namespace HMUI
{
  public class CurvedCanvasSettingsHelper
  {
    protected Canvas _cachedCanvas;
    protected bool _cachedCanvasIsRootCanvas;
    protected CurvedCanvasSettings _curvedCanvasSettings;
    protected bool _hasCachedData;
    protected static Dictionary<Canvas, CurvedCanvasSettings> _curvedCanvasCache = new Dictionary<Canvas, CurvedCanvasSettings>();

    public virtual void Reset()
    {
      this._hasCachedData = false;
      this._cachedCanvas = (Canvas) null;
      this._cachedCanvasIsRootCanvas = false;
      this._curvedCanvasSettings = (CurvedCanvasSettings) null;
    }

    public virtual CurvedCanvasSettings GetCurvedCanvasSettings(Canvas canvas)
    {
      if ((Object) canvas == (Object) null)
        return (CurvedCanvasSettings) null;
      if (this._hasCachedData)
      {
        if (!canvas.transform.hasChanged || !this._cachedCanvasIsRootCanvas && (Object) this._cachedCanvas == (Object) canvas)
          return this._curvedCanvasSettings;
        Canvas rootCanvas = canvas.rootCanvas;
        if (this._cachedCanvasIsRootCanvas && (Object) this._cachedCanvas == (Object) rootCanvas)
          return this._curvedCanvasSettings;
      }
      this._curvedCanvasSettings = CurvedCanvasSettingsHelper.GetCurvedCanvasSettingsForCanvas(canvas);
      if ((Object) this._curvedCanvasSettings != (Object) null)
      {
        this._cachedCanvasIsRootCanvas = false;
        this._cachedCanvasIsRootCanvas = (bool) (Object) canvas;
        this._hasCachedData = true;
        return this._curvedCanvasSettings;
      }
      Canvas rootCanvas1 = canvas.rootCanvas;
      this._curvedCanvasSettings = CurvedCanvasSettingsHelper.GetCurvedCanvasSettingsForCanvas(rootCanvas1);
      this._cachedCanvasIsRootCanvas = true;
      this._cachedCanvas = rootCanvas1;
      this._hasCachedData = true;
      return this._curvedCanvasSettings;
    }

    private static CurvedCanvasSettings GetCurvedCanvasSettingsForCanvas(Canvas canvas)
    {
      CurvedCanvasSettings component;
      if (Application.isPlaying && CurvedCanvasSettingsHelper._curvedCanvasCache.TryGetValue(canvas, out component))
        return component;
      component = canvas.GetComponent<CurvedCanvasSettings>();
      CurvedCanvasSettingsHelper._curvedCanvasCache[canvas] = component;
      return component;
    }
  }
}
