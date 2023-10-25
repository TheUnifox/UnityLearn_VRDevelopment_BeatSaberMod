// Decompiled with JetBrains decompiler
// Type: ShowHideAnimationController
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using System;
using System.Collections;
using UnityEngine;

public class ShowHideAnimationController : MonoBehaviour
{
  public Animator _animator;
  public bool _deactivateSelfAfterDelay;
  public float _deactivationDelay = 1f;
  protected bool _show;
  protected int _showAnimatorParam;

  public bool Show
  {
    set
    {
      if (this._show == value)
        return;
      this.gameObject.SetActive(true);
      this._animator.SetBool(this._showAnimatorParam, value);
      Func<float, IEnumerator> func = new Func<float, IEnumerator>(this.DeactivateSelfAfterDelayCoroutine);
      this.StopCoroutine(func.Method.Name);
      if (!value && this._deactivateSelfAfterDelay)
        this.StartCoroutine(func.Method.Name, (object) this._deactivationDelay);
      this._show = value;
    }
    get => this._show;
  }

  public virtual void Awake()
  {
    this._showAnimatorParam = Animator.StringToHash("Show");
    this._show = this._animator.GetBool(this._showAnimatorParam);
  }

    public virtual IEnumerator DeactivateSelfAfterDelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        base.gameObject.SetActive(false);
        yield break;
    }
}
