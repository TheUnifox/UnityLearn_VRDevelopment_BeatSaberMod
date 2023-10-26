// Decompiled with JetBrains decompiler
// Type: HMUI.HoverHintController
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System.Collections;
using UnityEngine;

namespace HMUI
{
  public class HoverHintController : MonoBehaviour
  {
    [SerializeField]
    protected HoverHintPanel _hoverHintPanelPrefab;
    protected const float kShowHintDelay = 0.6f;
    protected const float kHideHintDelay = 0.3f;
    protected HoverHintPanel _hoverHintPanel;
    protected bool _isHiding;

    public virtual void Awake()
    {
      this._hoverHintPanel = Object.Instantiate<HoverHintPanel>(this._hoverHintPanelPrefab, this.transform);
      this._hoverHintPanel.Hide();
    }

    public virtual void OnApplicationFocus(bool hasFocus)
    {
      if (hasFocus || !this._hoverHintPanel.isShown)
        return;
      this._hoverHintPanel.Hide();
    }

    public virtual void ShowHint(HoverHint hoverHint)
    {
      if (string.IsNullOrEmpty(hoverHint.text))
        return;
      this._isHiding = false;
      this.StopAllCoroutines();
      if (this._hoverHintPanel.isShown)
        this.SetupAndShowHintPanel(hoverHint);
      else
        this.StartCoroutine(this.ShowHintAfterDelay(hoverHint, 0.6f));
    }

    public virtual void HideHint()
    {
      if (this._isHiding)
        return;
      this.StopAllCoroutines();
      this.StartCoroutine(this.HideHintAfterDelay(0.3f));
    }

    public virtual void HideHintInstant()
    {
      this.StopAllCoroutines();
      if (!this._hoverHintPanel.isShown)
        return;
      this._hoverHintPanel.Hide();
    }

    public virtual IEnumerator ShowHintAfterDelay(HoverHint hoverHint, float delay)
    {
      yield return (object) new WaitForSeconds(delay);
      if ((Object) hoverHint != (Object) null)
        this.SetupAndShowHintPanel(hoverHint);
    }

    public virtual IEnumerator HideHintAfterDelay(float delay)
    {
      this._isHiding = true;
      yield return (object) new WaitForSeconds(delay);
      this._hoverHintPanel.Hide();
      this._isHiding = false;
    }

    public virtual void SetupAndShowHintPanel(HoverHint hoverHint)
    {
      RectTransform transformForHoverHint = (RectTransform) HoverHintController.GetScreenTransformForHoverHint(hoverHint);
      Rect spawnRect = new Rect();
      spawnRect.size = hoverHint.size;
      spawnRect.position = (Vector2) transformForHoverHint.InverseTransformPoint(hoverHint.worldCenter);
      spawnRect.position -= spawnRect.size * 0.5f;
      this._hoverHintPanel.Show(hoverHint.text, (Transform) transformForHoverHint, transformForHoverHint.rect.size, spawnRect);
    }

    private static Transform GetScreenTransformForHoverHint(HoverHint hoverHint)
    {
      for (Transform transformForHoverHint = hoverHint.transform; (Object) transformForHoverHint != (Object) null; transformForHoverHint = transformForHoverHint.parent)
      {
        if ((bool) (Object) transformForHoverHint.GetComponent<Canvas>() && (bool) (Object) transformForHoverHint.GetComponent<Screen>())
          return transformForHoverHint;
      }
      return hoverHint.transform;
    }
  }
}
