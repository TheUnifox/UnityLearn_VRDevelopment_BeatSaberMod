// Decompiled with JetBrains decompiler
// Type: ShowTextOnGameEventController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Polyglot;
using System;
using UnityEngine;

public class ShowTextOnGameEventController : MonoBehaviour
{
  [SerializeField]
  protected TextFadeTransitions _textFadeTransitions;
  [SerializeField]
  protected ShowTextOnGameEventController.EventTextBinding[] _eventTextBindings;

  public virtual void Awake()
  {
    foreach (ShowTextOnGameEventController.EventTextBinding eventTextBinding in this._eventTextBindings)
      eventTextBinding.Init(this._textFadeTransitions);
  }

  public virtual void OnDestroy()
  {
    foreach (ShowTextOnGameEventController.EventTextBinding eventTextBinding in this._eventTextBindings)
      eventTextBinding.Deinit();
  }

  [Serializable]
  public class EventTextBinding
  {
    [SerializeField]
    protected Signal _signal;
    [TextArea(2, 2)]
    [SerializeField]
    protected string _text;
    protected TextFadeTransitions _textFadeTransitions;

    public virtual void Init(TextFadeTransitions textFadeTransitions)
    {
      this._textFadeTransitions = textFadeTransitions;
      this._signal.Subscribe(new System.Action(this.HandleGameEvent));
    }

    public virtual void Deinit() => this._signal.Unsubscribe(new System.Action(this.HandleGameEvent));

    public virtual void HandleGameEvent() => this._textFadeTransitions.ShowText(Localization.Get(this._text));
  }
}
