// Decompiled with JetBrains decompiler
// Type: DisconnectPromptView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DisconnectPromptView : MonoBehaviour
{
  [SerializeField]
  protected PanelAnimationSO _presentPanelAnimation;
  [SerializeField]
  protected PanelAnimationSO _dismissPanelAnimation;
  [Space]
  [SerializeField]
  protected GameObject _promptGameObject;
  [SerializeField]
  protected Button _okButton;
  [SerializeField]
  protected Button _cancelButton;
  protected readonly ButtonBinder _buttonBinder = new ButtonBinder();

  public event System.Action<bool> didViewFinishEvent;

  public virtual void OnEnable()
  {
    this._buttonBinder.AddBinding(this._okButton, (System.Action) (() =>
    {
      System.Action<bool> didViewFinishEvent = this.didViewFinishEvent;
      if (didViewFinishEvent == null)
        return;
      didViewFinishEvent(true);
    }));
    this._buttonBinder.AddBinding(this._cancelButton, (System.Action) (() =>
    {
      System.Action<bool> didViewFinishEvent = this.didViewFinishEvent;
      if (didViewFinishEvent == null)
        return;
      didViewFinishEvent(false);
    }));
  }

  public virtual void OnDisable() => this._buttonBinder.ClearBindings();

  public virtual void Show()
  {
    this._promptGameObject.SetActive(true);
    this._presentPanelAnimation.ExecuteAnimation(this._promptGameObject);
  }

  public virtual void Hide(System.Action finishedCallback) => this._dismissPanelAnimation.ExecuteAnimation(this._promptGameObject, (System.Action) (() =>
  {
    System.Action action = finishedCallback;
    if (action != null)
      action();
    this._promptGameObject.SetActive(false);
  }));

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__9_0()
  {
    System.Action<bool> didViewFinishEvent = this.didViewFinishEvent;
    if (didViewFinishEvent == null)
      return;
    didViewFinishEvent(true);
  }

  [CompilerGenerated]
  public virtual void m_COnEnablem_Eb__9_1()
  {
    System.Action<bool> didViewFinishEvent = this.didViewFinishEvent;
    if (didViewFinishEvent == null)
      return;
    didViewFinishEvent(false);
  }
}
