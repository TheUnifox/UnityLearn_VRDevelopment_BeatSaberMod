// Decompiled with JetBrains decompiler
// Type: HMUI.HoverTextController
// Assembly: HMUI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A15B23B5-BA29-41D1-9B74-F31BC0F01F2D
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMUI.dll

using System.Collections;
using TMPro;
using UnityEngine;

namespace HMUI
{
  public class HoverTextController : MonoBehaviour
  {
    [SerializeField]
    protected TextMeshProUGUI _textMesh;
    [SerializeField]
    protected float _fadeInDelay = 0.3f;
    [SerializeField]
    protected float _fadeInSpeed = 4f;
    [SerializeField]
    protected float _fadeOutSpeed = 2f;
    protected bool _isFadingOut;
    protected bool _isFadingIn;

    public virtual void Awake() => this._textMesh.alpha = 0.0f;

    public virtual void OnDisable()
    {
      this._isFadingIn = false;
      this._isFadingOut = false;
      this._textMesh.alpha = 0.0f;
    }

    public virtual void OnApplicationFocus(bool hasFocus)
    {
      if (hasFocus)
        return;
      this._textMesh.alpha = 0.0f;
    }

    public virtual void ShowText(string text)
    {
      this._textMesh.text = text;
      this._isFadingOut = false;
      if (this._isFadingIn)
        return;
      this.StopAllCoroutines();
      this.StartCoroutine(this.ShowTextCoroutine());
    }

    public virtual IEnumerator ShowTextCoroutine()
    {
      yield return (object) new WaitForSeconds(this._fadeInDelay);
      this._isFadingIn = true;
      TextMeshProUGUI textMesh;
      for (; (double) this._textMesh.alpha < 0.99000000953674316; textMesh.alpha += Time.deltaTime * this._fadeInSpeed)
      {
        yield return (object) null;
        textMesh = this._textMesh;
      }
      this._textMesh.alpha = 1f;
      this._isFadingIn = false;
    }

    public virtual void HideText()
    {
      this._isFadingIn = false;
      if (this._isFadingOut)
        return;
      if (!this.isActiveAndEnabled)
      {
        this._textMesh.alpha = 0.0f;
      }
      else
      {
        this.StopAllCoroutines();
        this.StartCoroutine(this.HideTextCoroutine());
      }
    }

    public virtual IEnumerator HideTextCoroutine()
    {
      this._isFadingOut = true;
      TextMeshProUGUI textMesh;
      for (; (double) this._textMesh.alpha > 1.0 / 1000.0; textMesh.alpha -= Time.deltaTime * this._fadeOutSpeed)
      {
        yield return (object) null;
        textMesh = this._textMesh;
      }
      this._textMesh.alpha = 0.0f;
      this._isFadingOut = false;
    }
  }
}
