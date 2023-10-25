// Decompiled with JetBrains decompiler
// Type: CountdownElementController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;

public class CountdownElementController : MonoBehaviour
{
  [SerializeField]
  protected TextMeshProUGUI _text;
  [SerializeField]
  protected Animation _animation;

  public virtual void SetTextAndRunAnimation(string text)
  {
    this.gameObject.SetActive(true);
    this._text.text = text;
    this._animation.Play(PlayMode.StopAll);
  }

  public virtual void StopAndHide() => this.gameObject.SetActive(false);
}
