// Decompiled with JetBrains decompiler
// Type: LevelFailedTextEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class LevelFailedTextEffect : MonoBehaviour
{
  [SerializeField]
  protected Animator _animator;

  public virtual void ShowEffect()
  {
    this.gameObject.SetActive(true);
    this._animator.enabled = true;
  }
}
