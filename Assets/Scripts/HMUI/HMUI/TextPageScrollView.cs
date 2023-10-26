// Decompiled with JetBrains decompiler
// Type: HMUI.TextPageScrollView
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using TMPro;
using UnityEngine;

namespace HMUI
{
  public class TextPageScrollView : ScrollView
  {
    [SerializeField]
    protected TextMeshProUGUI _text;

    public virtual void SetText(string text)
    {
      this._text.text = text;
      this._text.ForceMeshUpdate();
      this.SetContentSize(this._text.preferredHeight);
    }
  }
}
