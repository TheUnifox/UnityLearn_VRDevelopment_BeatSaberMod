// Decompiled with JetBrains decompiler
// Type: HMUI.ScrollToTopOnEnable
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;

namespace HMUI
{
  public class ScrollToTopOnEnable : MonoBehaviour
  {
    [SerializeField]
    protected ScrollView _scrollView;

    public virtual void OnEnable() => this._scrollView.ScrollTo(0.0f, false);
  }
}
