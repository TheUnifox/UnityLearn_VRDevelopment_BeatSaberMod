// Decompiled with JetBrains decompiler
// Type: HMUI.AlphabetScrollbar
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HMUI
{
  [RequireComponent(typeof (RectTransform))]
  public class AlphabetScrollbar : 
    Interactable,
    IPointerDownHandler,
    IEventSystemHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler
  {
    [SerializeField]
    protected TableView _tableView;
    [Space]
    [SerializeField]
    protected float _characterHeight = 1f;
    [SerializeField]
    protected Color _normalColor = new Color(0.0f, 0.0f, 0.0f, 0.25f);
    [Space]
    [SerializeField]
    protected TextMeshProUGUI _textPrefab;
    [SerializeField]
    protected TextMeshProUGUI[] _prealocatedTexts;
    [SerializeField]
    protected Image _highlightImage;
    protected IReadOnlyList<AlphabetScrollInfo.Data> _characterScrollData;
    protected List<TextMeshProUGUI> _texts;
    protected int _highlightedCharacterIndex = -1;
    protected bool _pointerIsDown;

    public virtual void Awake() => this._highlightImage.enabled = false;

    public virtual void SetData(
      IReadOnlyList<AlphabetScrollInfo.Data> characterScrollData)
    {
      if (this._texts == null)
        this._texts = new List<TextMeshProUGUI>((IEnumerable<TextMeshProUGUI>) this._prealocatedTexts);
      this._characterScrollData = (IReadOnlyList<AlphabetScrollInfo.Data>) ((object) characterScrollData ?? (object) Array.Empty<AlphabetScrollInfo.Data>());
      for (int index = 0; index < ((IReadOnlyCollection<AlphabetScrollInfo.Data>) this._characterScrollData).Count && index < this._texts.Count; ++index)
        this.InitText(this._texts[index], this._characterScrollData[index].character);
      this.PrepareTransforms();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
      this._pointerIsDown = true;
      this._tableView.ScrollToCellWithIdx(this._characterScrollData[this.GetPointerCharacterIndex(eventData)].cellIdx, TableView.ScrollPositionType.Beginning, true);
    }

    public virtual void OnPointerUp(PointerEventData eventData) => this._pointerIsDown = false;

    public virtual void OnPointerEnter(PointerEventData eventData) => this.StartCoroutine(this.PointerMoveInsideCoroutine(eventData));

    public virtual void OnPointerExit(PointerEventData eventData)
    {
      this._highlightedCharacterIndex = -1;
      this.RefreshHighlight();
      this.StopAllCoroutines();
    }

    public virtual void PrepareTransforms()
    {
      this._highlightImage.rectTransform.sizeDelta = new Vector2(0.0f, this._characterHeight);
      for (int count = this._texts.Count; count < ((IReadOnlyCollection<AlphabetScrollInfo.Data>) this._characterScrollData).Count; ++count)
      {
        TextMeshProUGUI text = UnityEngine.Object.Instantiate<TextMeshProUGUI>(this._textPrefab, this.transform);
        this.InitText(text, this._characterScrollData[count].character);
        this._texts.Add(text);
      }
      RectTransform transform = (RectTransform) this.transform;
      float x = (float) -((double) transform.pivot.x - 0.5) * transform.rect.size.x;
      float y = (float) ((double) (((IReadOnlyCollection<AlphabetScrollInfo.Data>) this._characterScrollData).Count - 1) * (double) this._characterHeight * 0.5);
      for (int index = 0; index < ((IReadOnlyCollection<AlphabetScrollInfo.Data>) this._characterScrollData).Count; ++index)
      {
        this._texts[index].rectTransform.localPosition = (Vector3) new Vector2(x, y);
        this._texts[index].enabled = true;
        y -= this._characterHeight;
      }
      for (int count = ((IReadOnlyCollection<AlphabetScrollInfo.Data>) this._characterScrollData).Count; count < this._texts.Count; ++count)
        this._texts[count].enabled = false;
    }

    public virtual void RefreshHighlight()
    {
      if (this._highlightedCharacterIndex < 0)
      {
        this._highlightImage.enabled = false;
      }
      else
      {
        this._highlightImage.enabled = true;
        this._highlightImage.rectTransform.localPosition = new Vector3(this._highlightImage.rectTransform.localPosition.x, (float) ((double) (((IReadOnlyCollection<AlphabetScrollInfo.Data>) this._characterScrollData).Count - 1) * (double) this._characterHeight * 0.5 - (double) this._highlightedCharacterIndex * (double) this._characterHeight));
      }
    }

    public virtual IEnumerator PointerMoveInsideCoroutine(PointerEventData eventData)
    {
      while (true)
      {
        int pointerCharacterIndex = this.GetPointerCharacterIndex(eventData);
        if (pointerCharacterIndex != this._highlightedCharacterIndex)
        {
          this._highlightedCharacterIndex = pointerCharacterIndex;
          if (this._pointerIsDown)
            this._tableView.ScrollToCellWithIdx(this._characterScrollData[pointerCharacterIndex].cellIdx, TableView.ScrollPositionType.Beginning, true);
          this.RefreshHighlight();
        }
        yield return (object) null;
      }
    }

    public virtual int GetPointerCharacterIndex(PointerEventData eventData)
    {
      Vector2 localPoint;
      if (!RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform) this.transform, eventData.position, eventData.pressEventCamera, out localPoint))
        return -1;
      float num = (float) ((double) (((IReadOnlyCollection<AlphabetScrollInfo.Data>) this._characterScrollData).Count - 1) * (double) this._characterHeight * 0.5);
      return Mathf.Clamp(Mathf.RoundToInt((float) -((double) localPoint.y - (double) num) / this._characterHeight), 0, ((IReadOnlyCollection<AlphabetScrollInfo.Data>) this._characterScrollData).Count - 1);
    }

    public virtual void InitText(TextMeshProUGUI text, char character)
    {
      text.color = this._normalColor;
      text.text = character.ToString();
      RectTransform rectTransform = text.rectTransform;
      rectTransform.pivot = new Vector2(0.5f, 0.5f);
      rectTransform.anchorMin = new Vector2(0.0f, 0.5f);
      rectTransform.anchorMax = new Vector2(1f, 0.5f);
    }
  }
}
