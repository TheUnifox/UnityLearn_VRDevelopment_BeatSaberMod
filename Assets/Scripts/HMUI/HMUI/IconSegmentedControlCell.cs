// Decompiled with JetBrains decompiler
// Type: HMUI.IconSegmentedControlCell
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;
using UnityEngine.UI;

namespace HMUI
{
  public class IconSegmentedControlCell : SegmentedControlCell
  {
    [SerializeField]
    protected Image _icon;
    [SerializeField]
    protected HoverHint _hoverHint;
    [NullAllowed]
    [SerializeField]
    protected GameObject _backgroundGameObject;

    public Sprite sprite
    {
      set => this._icon.sprite = value;
      get => this._icon.sprite;
    }

    public string hintText
    {
      set => this._hoverHint.text = value;
    }

    public float iconSize
    {
      set => this._icon.rectTransform.sizeDelta = new Vector2(value, value);
    }

    public bool hideBackgroundImage
    {
      set
      {
        if (!((Object) this._backgroundGameObject != (Object) null))
          return;
        this._backgroundGameObject.SetActive(!value);
      }
    }
  }
}
