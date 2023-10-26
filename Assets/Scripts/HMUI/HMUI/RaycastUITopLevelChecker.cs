// Decompiled with JetBrains decompiler
// Type: HMUI.RaycastUITopLevelChecker
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HMUI
{
  public class RaycastUITopLevelChecker : MonoBehaviour
  {
    protected List<RaycastResult> results = new List<RaycastResult>();
    protected Canvas _canvas;

    public virtual void Awake()
    {
      Canvas[] componentsInParent = this.GetComponentsInParent<Canvas>();
      this._canvas = componentsInParent[componentsInParent.Length - 1];
    }

    public bool isOnTop
    {
      get
      {
        RectTransform transform = this.transform as RectTransform;
        RectTransformUtility.WorldToScreenPoint(this._canvas.worldCamera, transform.TransformPoint((Vector3) transform.rect.center));
        EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current)
        {
          position = new Vector2(0.0f, 0.0f)
        }, this.results);
        return this.results.Count > 0 && this.results[0].gameObject == this.gameObject;
      }
    }
  }
}
