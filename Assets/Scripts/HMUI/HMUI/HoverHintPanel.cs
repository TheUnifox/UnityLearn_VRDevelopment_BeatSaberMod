// Decompiled with JetBrains decompiler
// Type: HMUI.HoverHintPanel
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace HMUI
{
  public class HoverHintPanel : MonoBehaviour
  {
    [SerializeField]
    protected TextMeshProUGUI _text;
    [SerializeField]
    protected Vector2 _padding = new Vector2(6f, 4f);
    [SerializeField]
    protected Vector2 _containerPadding = new Vector2(8f, 8f);
    [SerializeField]
    protected float _separator = 2f;
    [SerializeField]
    protected float _zOffset = 0.1f;
    [CompilerGenerated]
    protected bool isShown_k__BackingField;

    public bool isShown
    {
      get => this.isShown_k__BackingField;
      private set => this.isShown_k__BackingField = value;
    }

    public virtual void Awake()
    {
      RectTransform transform = (RectTransform) this.transform;
      transform.pivot = new Vector2(0.5f, 0.5f);
      transform.anchorMin = new Vector2(0.5f, 0.5f);
      transform.anchorMax = new Vector2(0.5f, 0.5f);
    }

    public virtual void Show(string text, Transform parent, Vector2 containerSize, Rect spawnRect)
    {
      this.isShown = true;
      RectTransform transform = (RectTransform) this.transform;
      transform.SetParent(parent, false);
      transform.SetAsLastSibling();
      transform.localScale = Vector3.one;
      transform.localRotation = Quaternion.identity;
      this.gameObject.SetActive(true);
      this._text.text = text;
      this._text.ForceMeshUpdate();
      Vector2 panelSize = (Vector2) this._text.bounds.size + this._padding;
      transform.sizeDelta = panelSize;
      transform.anchoredPosition = this.CalculatePanelPosition(containerSize, spawnRect, panelSize);
            Vector3 localPosition = transform.localPosition;
            localPosition.z = -this._zOffset;
            transform.localPosition = localPosition;
        }

    public virtual void Hide()
    {
      this.isShown = false;
      this.gameObject.SetActive(false);
    }

    public virtual Vector2 CalculatePanelPosition(
      Vector2 containerSize,
      Rect spawnRect,
      Vector2 panelSize)
    {
      float x = spawnRect.center.x;
      if ((double) x < -(double) containerSize.x * 0.5 + (double) this._containerPadding.x + (double) panelSize.x * 0.5)
        x = (float) (-(double) containerSize.x * 0.5 + (double) this._containerPadding.x + (double) panelSize.x * 0.5);
      else if ((double) x > (double) containerSize.x * 0.5 - (double) this._containerPadding.x - (double) panelSize.x * 0.5)
        x = (float) ((double) containerSize.x * 0.5 - (double) this._containerPadding.x - (double) panelSize.x * 0.5);
      float y = (float) ((double) spawnRect.center.y + (double) spawnRect.size.y * 0.5 + (double) this._separator + (double) panelSize.y * 0.5);
      if ((double) y > (double) containerSize.y * 0.5 - (double) this._containerPadding.y - (double) panelSize.y * 0.5)
        y = (float) ((double) spawnRect.center.y - (double) spawnRect.size.y * 0.5 - (double) this._separator - (double) panelSize.y * 0.5);
      return new Vector2(x, y);
    }
  }
}
