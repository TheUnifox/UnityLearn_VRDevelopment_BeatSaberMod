// Decompiled with JetBrains decompiler
// Type: PageControl
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class PageControl : MonoBehaviour
{
  [SerializeField]
  protected RectTransform _content;
  [Space]
  [SerializeField]
  protected float _spacing = 1f;
  [Space]
  [SerializeField]
  protected PageControlElement _elementPrefab;
  protected readonly List<PageControlElement> _activeElements = new List<PageControlElement>();
  protected readonly Queue<PageControlElement> _inactiveElements = new Queue<PageControlElement>();
  protected int _selectedPage = -1;
  protected int _pagesCount = -1;

  public virtual void SetPagesCount(int pagesCount)
  {
    if (this._pagesCount == pagesCount)
      return;
    this._pagesCount = pagesCount;
    foreach (PageControlElement activeElement in this._activeElements)
    {
      activeElement.gameObject.SetActive(false);
      this._inactiveElements.Enqueue(activeElement);
    }
    this._activeElements.Clear();
    float x = this._content.sizeDelta.x;
    for (int index = 0; index < pagesCount; ++index)
    {
      PageControlElement pageControlElement = this._inactiveElements.Count > 0 ? this._inactiveElements.Dequeue() : Object.Instantiate<PageControlElement>(this._elementPrefab, (Transform) this._content);
      pageControlElement.gameObject.SetActive(true);
      pageControlElement.SetSelected(index == this._selectedPage);
      pageControlElement.rectTransform.sizeDelta = new Vector2(x, x);
      float y = (float) (((double) pagesCount * 0.5 - (double) index - 0.5) * ((double) x + (double) this._spacing));
      pageControlElement.rectTransform.anchoredPosition = new Vector2(0.0f, y);
      this._activeElements.Add(pageControlElement);
    }
  }

  public virtual void SetSelectedPageIndex(int page)
  {
    if (page == this._selectedPage || this._activeElements.Count <= page)
      return;
    if (this._selectedPage < this._activeElements.Count && this._selectedPage >= 0)
      this._activeElements[this._selectedPage].SetSelected(false);
    this._selectedPage = page;
    this._activeElements[this._selectedPage].SetSelected(true);
  }

  public virtual void SetVisible(bool isVisible) => this._content.gameObject.SetActive(isVisible);
}
