// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BaseInputFieldValidator`1
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace BeatmapEditor3D
{
  [RequireComponent(typeof (TMP_InputField))]
  public abstract class BaseInputFieldValidator<T> : MonoBehaviour
  {
    [SerializeField]
    protected TMP_InputField _inputField;
    [SerializeField]
    [NullAllowed]
    protected GameObject _valueModifiedHintGo;
    private T _value;
    private string _previousInputValue;

    public string text => this._inputField.text;

    public T value
    {
      get => this._value;
      set
      {
        this._value = value;
        this._inputField.SetTextWithoutNotify(string.Format("{0}", (object) value));
      }
    }

    public event Action<T> onInputValidated;

    public bool interactable
    {
      get => this._inputField.interactable;
      set => this._inputField.interactable = value;
    }

    public void SetValueWithoutNotify(T value, bool clearModifiedState)
    {
      this.value = value;
      if (!clearModifiedState)
        return;
      this.ClearDirtyState();
    }

    public void ClearDirtyState()
    {
      if (!((UnityEngine.Object) this._valueModifiedHintGo != (UnityEngine.Object) null))
        return;
      this._valueModifiedHintGo.SetActive(false);
    }

    protected void OnEnable()
    {
      this._inputField.onEndEdit.AddListener(new UnityAction<string>(this.HandleInputFieldOnEndEdit));
      this._previousInputValue = this._inputField.text;
    }

    protected void OnDisable() => this._inputField.onEndEdit.RemoveListener(new UnityAction<string>(this.HandleInputFieldOnEndEdit));

    protected void ResetInputValue() => this._inputField.text = this._previousInputValue;

    protected void TriggerOnValidated(T value)
    {
      if (this.value.Equals((object) value))
        return;
      this.value = value;
      this._previousInputValue = this._inputField.text;
      if ((UnityEngine.Object) this._valueModifiedHintGo != (UnityEngine.Object) null)
        this._valueModifiedHintGo.SetActive(true);
      Action<T> onInputValidated = this.onInputValidated;
      if (onInputValidated == null)
        return;
      onInputValidated(value);
    }

    protected abstract void ValidateInput(string input);

    private void HandleInputFieldOnEndEdit(string input) => this.ValidateInput(input);
  }
}
