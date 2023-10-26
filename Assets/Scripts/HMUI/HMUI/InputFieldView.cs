// Decompiled with JetBrains decompiler
// Type: HMUI.InputFieldView
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HMUI
{
  public class InputFieldView : Selectable
  {
    protected const float kBlinkingRate = 0.4f;
    [SerializeField]
    protected TextMeshProUGUI _textView;
    [SerializeField]
    [NullAllowed]
    protected CanvasGroup _textViewCanvasGroup;
    [SerializeField]
    protected ImageView _blinkingCaret;
    [SerializeField]
    protected GameObject _placeholderText;
    [SerializeField]
    protected Button _clearSearchButton;
    [Header("Input Specific Keyboard Settings")]
    [SerializeField]
    protected bool _useGlobalKeyboard = true;
    [SerializeField]
    protected Vector3 _keyboardPositionOffset = Vector3.zero;
    [Header("Input Field Settings")]
    [SerializeField]
    protected bool _useUppercase;
    [SerializeField]
    protected int _textLengthLimit;
    [SerializeField]
    protected float _caretOffset = 0.4f;
    protected InputFieldView.SelectionState _selectionState;
    protected string _text = "";
    protected bool _hasKeyboardAssigned;
    protected ButtonBinder _buttonBinder;
    protected InputFieldView.InputFieldChanged _onValueChanged = new InputFieldView.InputFieldChanged();
    protected readonly YieldInstruction _blinkWaitYieldInstruction = (YieldInstruction) new WaitForSeconds(0.4f);

    public InputFieldView.SelectionState selectionState => this._selectionState;

    public Vector3 keyboardPositionOffset => this._keyboardPositionOffset;

    public event Action<InputFieldView.SelectionState> selectionStateDidChangeEvent;

    public InputFieldView.InputFieldChanged onValueChanged
    {
      get => this._onValueChanged;
      set => this._onValueChanged = value;
    }

    public bool useGlobalKeyboard => this._useGlobalKeyboard;

    public string text
    {
      get => this._text;
      private set
      {
        this._text = value;
        this._textView.SetText(this._text);
        this._textView.ForceMeshUpdate(true);
        this.UpdateCaretPosition();
        this.UpdatePlaceholder();
      }
    }

    protected override void Awake()
    {
      this._blinkingCaret.enabled = false;
      this._buttonBinder = new ButtonBinder();
      this._buttonBinder.AddBinding(this._clearSearchButton, (Action) (() =>
      {
        this.text = "";
        this._clearSearchButton.interactable = false;
        this._onValueChanged.Invoke(this);
      }));
      this._clearSearchButton.interactable = false;
    }

    protected override void OnDestroy() => this._buttonBinder?.ClearBindings();

    protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
    {
      InputFieldView.SelectionState selectionState = InputFieldView.SelectionState.Normal;
      if (this._hasKeyboardAssigned)
      {
        selectionState = InputFieldView.SelectionState.Selected;
      }
      else
      {
        switch (state)
        {
          case Selectable.SelectionState.Normal:
            selectionState = InputFieldView.SelectionState.Normal;
            break;
          case Selectable.SelectionState.Highlighted:
            selectionState = InputFieldView.SelectionState.Highlighted;
            break;
          case Selectable.SelectionState.Pressed:
            selectionState = InputFieldView.SelectionState.Pressed;
            break;
          case Selectable.SelectionState.Selected:
            selectionState = InputFieldView.SelectionState.Selected;
            break;
          case Selectable.SelectionState.Disabled:
            selectionState = InputFieldView.SelectionState.Disabled;
            break;
        }
      }
      this._selectionState = selectionState;
      Action<InputFieldView.SelectionState> stateDidChangeEvent = this.selectionStateDidChangeEvent;
      if (stateDidChangeEvent != null)
        stateDidChangeEvent(selectionState);
      this.UpdatePlaceholder();
    }

    public virtual void ActivateKeyboard(UIKeyboard keyboard)
    {
      if (this._hasKeyboardAssigned)
        return;
      this._hasKeyboardAssigned = true;
      if ((UnityEngine.Object) this._textViewCanvasGroup != (UnityEngine.Object) null)
        this._textViewCanvasGroup.ignoreParentGroups = true;
      keyboard.keyWasPressedEvent += new Action<char>(this.KeyboardKeyPressed);
      keyboard.deleteButtonWasPressedEvent += new Action(this.KeyboardDeletePressed);
      this.UpdateCaretPosition();
      this._blinkingCaret.enabled = true;
      this.StopAllCoroutines();
      this.StartCoroutine(this.BlinkingCaretCoroutine());
      Action<InputFieldView.SelectionState> stateDidChangeEvent = this.selectionStateDidChangeEvent;
      if (stateDidChangeEvent != null)
        stateDidChangeEvent(InputFieldView.SelectionState.Selected);
      this._clearSearchButton.interactable = false;
    }

    public virtual void DeactivateKeyboard(UIKeyboard keyboard)
    {
      if (!this._hasKeyboardAssigned)
        return;
      this._hasKeyboardAssigned = false;
      if ((UnityEngine.Object) this._textViewCanvasGroup != (UnityEngine.Object) null)
        this._textViewCanvasGroup.ignoreParentGroups = false;
      keyboard.keyWasPressedEvent -= new Action<char>(this.KeyboardKeyPressed);
      keyboard.deleteButtonWasPressedEvent -= new Action(this.KeyboardDeletePressed);
      this.StopAllCoroutines();
      this._blinkingCaret.enabled = false;
      Action<InputFieldView.SelectionState> stateDidChangeEvent = this.selectionStateDidChangeEvent;
      if (stateDidChangeEvent != null)
        stateDidChangeEvent(InputFieldView.SelectionState.Normal);
      this._selectionState = InputFieldView.SelectionState.Normal;
      this.UpdateClearButton();
    }

    public virtual void SetText(string value)
    {
      this.text = value;
      this.UpdateClearButton();
    }

    public virtual void ClearInput()
    {
      this.text = "";
      this.UpdateClearButton();
    }

    public virtual void KeyboardKeyPressed(char letter)
    {
      if (this.text.Length < this._textLengthLimit)
      {
        this.text += (this._useUppercase ? char.ToUpper(letter) : letter).ToString();
        this._onValueChanged.Invoke(this);
      }
      this.UpdatePlaceholder();
      this._blinkingCaret.enabled = true;
      this.StopAllCoroutines();
      this.StartCoroutine(this.BlinkingCaretCoroutine());
    }

    public virtual void KeyboardDeletePressed()
    {
      this.text = this.text.Length <= 0 ? "" : this.text.Substring(0, this.text.Length - 1);
      this._onValueChanged.Invoke(this);
      this.UpdatePlaceholder();
      this._blinkingCaret.enabled = true;
      this.StopAllCoroutines();
      this.StartCoroutine(this.BlinkingCaretCoroutine());
    }

    public virtual IEnumerator BlinkingCaretCoroutine()
    {
      while (true)
      {
        yield return (object) this._blinkWaitYieldInstruction;
        this._blinkingCaret.enabled = !this._blinkingCaret.enabled;
      }
    }

        public virtual void UpdateCaretPosition()
        {
            float num = Mathf.Max(this._textView.GetRenderedValues(false).x, 0f);
            RectTransform rectTransform = (RectTransform)this._blinkingCaret.transform;
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.x = num + ((this._textView.text.Length > 0) ? this._caretOffset : 0f);
            rectTransform.anchoredPosition = anchoredPosition;
        }

        public virtual void UpdatePlaceholder() => this._placeholderText.SetActive(string.IsNullOrEmpty(this._text));

    public virtual void UpdateClearButton() => this._clearSearchButton.interactable = !string.IsNullOrEmpty(this._text);

    [CompilerGenerated]
    public virtual void Awake_b__34_0()
    {
      this.text = "";
      this._clearSearchButton.interactable = false;
      this._onValueChanged.Invoke(this);
    }

    public new enum SelectionState
    {
      Normal,
      Highlighted,
      Pressed,
      Disabled,
      Selected,
    }

    public class InputFieldChanged : UnityEvent<InputFieldView>
    {
    }
  }
}
