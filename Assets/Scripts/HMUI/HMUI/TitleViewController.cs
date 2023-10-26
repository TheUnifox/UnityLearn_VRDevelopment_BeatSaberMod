// Decompiled with JetBrains decompiler
// Type: HMUI.TitleViewController
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using TMPro;
using UnityEngine;

namespace HMUI
{
  public class TitleViewController : ViewController
  {
    [SerializeField]
    protected TextMeshProUGUI _text;

    public virtual void SetText(string text) => this._text.text = text;
  }
}
