// Decompiled with JetBrains decompiler
// Type: HMUI.ImageWithHint
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;
using UnityEngine.UI;

namespace HMUI
{
  public class ImageWithHint : MonoBehaviour
  {
    [SerializeField]
    protected Image _image;
    [SerializeField]
    protected HoverHint _hoverHint;

    public Sprite sprite
    {
      set => this._image.sprite = value;
      get => this._image.sprite;
    }

    public string hintText
    {
      set => this._hoverHint.text = value;
    }
  }
}
