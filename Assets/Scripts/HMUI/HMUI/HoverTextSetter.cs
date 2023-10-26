// Decompiled with JetBrains decompiler
// Type: HMUI.HoverTextSetter
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;
using UnityEngine.EventSystems;

namespace HMUI
{
  public class HoverTextSetter : 
    MonoBehaviour,
    IPointerEnterHandler,
    IEventSystemHandler,
    IPointerExitHandler
  {
    [SerializeField]
    protected HoverTextController _hoverTextController;
    [SerializeField]
    protected string _text;

    public string text
    {
      get => this._text;
      set => this._text = value;
    }

    public virtual void OnPointerEnter(PointerEventData eventData) => this._hoverTextController.ShowText(this._text);

    public virtual void OnPointerExit(PointerEventData eventData) => this._hoverTextController.HideText();

    public virtual void OnDisable() => this._hoverTextController.HideText();
  }
}
