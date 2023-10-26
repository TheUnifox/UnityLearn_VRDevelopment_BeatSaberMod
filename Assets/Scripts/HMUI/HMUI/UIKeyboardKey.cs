// Decompiled with JetBrains decompiler
// Type: HMUI.UIKeyboardKey
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using TMPro;
using UnityEngine;

namespace HMUI
{
  public class UIKeyboardKey : MonoBehaviour
  {
    [SerializeField]
    protected KeyCode _keyCode;
    [SerializeField]
    protected TextMeshProUGUI _text;
    [SerializeField]
    protected string _overrideText;
    [SerializeField]
    protected bool _canBeUppercase;

    public KeyCode keyCode => this._keyCode;

    public bool canBeUppercase => this._canBeUppercase;

    public virtual void Awake() => this._text.text = string.IsNullOrEmpty(this._overrideText) ? this.keyCode.ToString() : this._overrideText;

    public virtual void OnValidate()
    {
      if (!((Object) this._text != (Object) null))
        return;
      this._text.text = string.IsNullOrEmpty(this._overrideText) ? this.keyCode.ToString() : this._overrideText;
    }
  }
}
