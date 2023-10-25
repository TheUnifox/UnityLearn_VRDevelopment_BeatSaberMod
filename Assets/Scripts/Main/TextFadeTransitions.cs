// Decompiled with JetBrains decompiler
// Type: TextFadeTransitions
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;

public class TextFadeTransitions : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _textLabel;
  [Tooltip("If Canvas Group is specified, it is used for fadeing instead of Text Label color.")]
  [SerializeField]
  protected CanvasGroup _canvasGroup;
  [SerializeField]
  protected float _fadeDuration = 0.4f;
  protected TextFadeTransitions.State _state;
  protected string _nextText;
  protected float _fade;

  public virtual void Awake()
  {
    this.enabled = false;
    this._state = TextFadeTransitions.State.NotInTransition;
    this._fade = 0.0f;
    this._textLabel.text = "";
    this.RefreshTextAlpha();
  }

  public virtual void Update() => this.RefreshState();

  public virtual void RefreshState()
  {
    switch (this._state)
    {
      case TextFadeTransitions.State.NotInTransition:
        if (this._nextText != null)
        {
          if ((double) this._fade == 0.0)
          {
            this._textLabel.text = this._nextText;
            this._nextText = (string) null;
            this._state = TextFadeTransitions.State.FadingIn;
            break;
          }
          this._state = TextFadeTransitions.State.FadingOut;
          break;
        }
        this.enabled = false;
        break;
      case TextFadeTransitions.State.FadingOut:
        this.RefreshTextAlpha();
        this._fade = Mathf.Max(this._fade - Time.deltaTime / this._fadeDuration, 0.0f);
        if ((double) this._fade != 0.0)
          break;
        this._state = TextFadeTransitions.State.NotInTransition;
        break;
      case TextFadeTransitions.State.FadingIn:
        this._fade = Mathf.Min(this._fade + Time.deltaTime / this._fadeDuration, 1f);
        this.RefreshTextAlpha();
        if ((double) this._fade != 1.0)
          break;
        this._state = TextFadeTransitions.State.NotInTransition;
        break;
    }
  }

  public virtual void RefreshTextAlpha()
  {
    if ((Object) this._canvasGroup != (Object) null)
      this._canvasGroup.alpha = this._fade;
    else
      this._textLabel.color = this._textLabel.color with
      {
        a = this._fade
      };
    this._textLabel.enabled = (double) this._fade != 0.0;
  }

  public virtual void ShowText(string text)
  {
    if (this._nextText == text || this._textLabel.text == text)
      return;
    this._nextText = text;
    this.enabled = true;
  }

  public enum State
  {
    NotInTransition,
    FadingOut,
    FadingIn,
  }
}
