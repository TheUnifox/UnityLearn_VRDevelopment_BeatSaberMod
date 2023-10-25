// Decompiled with JetBrains decompiler
// Type: SignalOnUIButtonClick
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;
using UnityEngine.Events;

public class SignalOnUIButtonClick : MonoBehaviour
{
  [SerializeField]
  [SignalSender]
  protected Signal _buttonClickedSignal;
  [SerializeField]
  protected UnityEngine.UI.Button _button;

  public virtual void OnReset() => this._button = this.GetComponent<UnityEngine.UI.Button>();

  public virtual void Start() => this._button.onClick.AddListener(new UnityAction(this._buttonClickedSignal.Raise));

  public virtual void OnDestroy()
  {
    if (!(bool) (Object) this._button)
      return;
    this._button.onClick.RemoveListener(new UnityAction(this._buttonClickedSignal.Raise));
  }
}
