// Decompiled with JetBrains decompiler
// Type: HMUI.HoverHint
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace HMUI
{
  public class HoverHint : 
    MonoBehaviour,
    IPointerEnterHandler,
    IEventSystemHandler,
    IPointerExitHandler
  {
    [SerializeField]
    protected string _text;
    [Inject]
    protected readonly HoverHintController _hoverHintController;
    protected readonly Vector3[] _worldCornersTemp = new Vector3[4];

    public string text
    {
      get => this._text;
      set => this._text = value;
    }

    public Vector2 size => ((RectTransform) this.transform).rect.size;

    public Vector3 worldCenter
    {
      get
      {
        ((RectTransform) this.transform).GetWorldCorners(this._worldCornersTemp);
        Vector3 zero = Vector3.zero;
        for (int index = 0; index < 4; ++index)
          zero += this._worldCornersTemp[index];
        return zero * 0.25f;
      }
    }

    public virtual void OnPointerEnter(PointerEventData eventData) => this._hoverHintController.ShowHint(this);

    public virtual void OnPointerExit(PointerEventData eventData)
    {
      if ((Object) eventData.currentInputModule == (Object) null || !eventData.currentInputModule.enabled)
        this._hoverHintController.HideHintInstant();
      else
        this._hoverHintController.HideHint();
    }

    public virtual void OnDisable()
    {
      if (!((Object) this._hoverHintController != (Object) null))
        return;
      this._hoverHintController.HideHintInstant();
    }
  }
}
