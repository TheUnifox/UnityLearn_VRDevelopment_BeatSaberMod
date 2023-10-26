// Decompiled with JetBrains decompiler
// Type: HMUI.TextSegmentedControlCell
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using TMPro;
using UnityEngine;

namespace HMUI
{
  public class TextSegmentedControlCell : SegmentedControlCell
  {
    [SerializeField]
    protected TextMeshProUGUI _text;
    [SerializeField]
    protected GameObject _backgroundGameObject;

    public string text
    {
      set => this._text.text = value;
      get => this._text.text;
    }

    public float fontSize
    {
      set => this._text.fontSize = value;
      get => this._text.fontSize;
    }

    public bool hideBackgroundImage
    {
      set => this._backgroundGameObject.SetActive(!value);
    }

    public float preferredWidth => this._text.preferredWidth;
  }
}
