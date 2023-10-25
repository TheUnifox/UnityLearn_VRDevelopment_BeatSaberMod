// Decompiled with JetBrains decompiler
// Type: GameServerListDetailTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GameServerListDetailTableCell : TableCell
{
  [SerializeField]
  protected Button _joinServerButton;
  protected readonly ButtonBinder _buttonBinder = new ButtonBinder();

  public event System.Action joinServerButtonWasPressedEvent;

  protected override void Start()
  {
    base.Start();
    this._buttonBinder.AddBinding(this._joinServerButton, (System.Action) (() =>
    {
      System.Action buttonWasPressedEvent = this.joinServerButtonWasPressedEvent;
      if (buttonWasPressedEvent == null)
        return;
      buttonWasPressedEvent();
    }));
  }

  [CompilerGenerated]
  public virtual void m_CStartm_Eb__5_0()
  {
    System.Action buttonWasPressedEvent = this.joinServerButtonWasPressedEvent;
    if (buttonWasPressedEvent == null)
      return;
    buttonWasPressedEvent();
  }
}
