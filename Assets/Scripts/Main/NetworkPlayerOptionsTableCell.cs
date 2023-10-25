// Decompiled with JetBrains decompiler
// Type: NetworkPlayerOptionsTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPlayerOptionsTableCell : TableCell
{
  [SerializeField]
  protected Button[] _buttons;
  [SerializeField]
  protected TextMeshProUGUI[] _buttonTexts;
  protected ButtonBinder _buttonBinder;
  protected INetworkPlayer _player;

  public INetworkPlayer player
  {
    get => this._player;
    set
    {
      this._player = value;
      this.Refresh();
    }
  }

  public virtual void Refresh()
  {
    if (this._player == null)
      return;
    if (this._buttonBinder == null)
      this._buttonBinder = new ButtonBinder();
    this._buttonBinder.ClearBindings();
    int index1 = 0;
    if (this._player.canBlock)
    {
      this._buttonBinder.AddBinding(this._buttons[index1], new System.Action(this.Block));
      this._buttons[index1].interactable = true;
      this._buttonTexts[index1].text = "BLOCK";
      ++index1;
    }
    if (this._player.canUnblock)
    {
      this._buttonBinder.AddBinding(this._buttons[index1], new System.Action(this.Unblock));
      this._buttons[index1].interactable = true;
      this._buttonTexts[index1].text = "UNBLOCK";
      ++index1;
    }
    if (this._player.canJoin || this._player.isWaitingOnJoin)
    {
      this._buttonBinder.AddBinding(this._buttons[index1], new System.Action(this.Join));
      this._buttons[index1].interactable = !this._player.isWaitingOnJoin;
      this._buttonTexts[index1].text = this._player.isWaitingOnJoin ? "JOINING.." : "JOIN";
      ++index1;
    }
    if (this._player.canInvite || this._player.isWaitingOnInvite)
    {
      this._buttonBinder.AddBinding(this._buttons[index1], new System.Action(this.Invite));
      this._buttons[index1].interactable = !this._player.isWaitingOnInvite;
      this._buttonTexts[index1].text = this._player.isWaitingOnInvite ? "INVITED.." : "INVITE";
      ++index1;
    }
    if (this._player.canKick)
    {
      this._buttonBinder.AddBinding(this._buttons[index1], new System.Action(this.Kick));
      this._buttons[index1].interactable = true;
      this._buttonTexts[index1].text = "KICK";
      ++index1;
    }
    if (this._player.canLeave)
    {
      this._buttonBinder.AddBinding(this._buttons[index1], new System.Action(this.Leave));
      this._buttons[index1].interactable = true;
      this._buttonTexts[index1].text = "LEAVE PARTY";
      ++index1;
    }
    for (int index2 = 0; index2 < this._buttons.Length; ++index2)
      this._buttons[index2].gameObject.SetActive(index2 < index1);
  }

  public virtual void Block()
  {
    this._player.Block();
    this.Refresh();
  }

  public virtual void Unblock()
  {
    this._player.Unblock();
    this.Refresh();
  }

  public virtual void Join()
  {
    this._player.Join();
    this.Refresh();
  }

  public virtual void Invite()
  {
    this._player.Invite();
    this.Refresh();
  }

  public virtual void Kick()
  {
    this._player.Kick();
    this.Refresh();
  }

  public virtual void Leave()
  {
    this._player.Leave();
    this.Refresh();
  }
}
