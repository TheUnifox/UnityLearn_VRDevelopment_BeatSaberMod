// Decompiled with JetBrains decompiler
// Type: HMUI.InteractableTextMeshPro
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HMUI
{
  public class InteractableTextMeshPro : UIBehaviour
  {
    [SerializeField]
    protected float _interactionAlpha = 1f;
    [SerializeField]
    protected float _noInteractionAlpha = 0.25f;
    [Space]
    [SerializeField]
    protected TextMeshProUGUI _text;
    protected readonly List<CanvasGroup> _canvasGroupCache = new List<CanvasGroup>();

    protected override void OnCanvasGroupChanged()
    {
      bool flag1 = true;
      for (Transform transform = this.transform; (Object) transform != (Object) null; transform = transform.parent)
      {
        transform.GetComponents<CanvasGroup>(this._canvasGroupCache);
        bool flag2 = false;
        for (int index = 0; index < this._canvasGroupCache.Count; ++index)
        {
          if (!this._canvasGroupCache[index].interactable)
          {
            flag1 = false;
            flag2 = true;
          }
          if (this._canvasGroupCache[index].ignoreParentGroups)
            flag2 = true;
        }
        if (flag2)
          break;
      }
      this._text.alpha = flag1 ? this._interactionAlpha : this._noInteractionAlpha;
    }
  }
}
