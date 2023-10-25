// Decompiled with JetBrains decompiler
// Type: NetworkPlayerTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPlayerTableCell : TableCell
{
  [SerializeField]
  protected TextMeshProUGUI _playerNameText;
  [SerializeField]
  protected GameObject _separator;
  [SerializeField]
  protected Image _privateIcon;
  [SerializeField]
  protected Image _spectateIcon;
  [SerializeField]
  protected Image _partyLeaderIcon;
  [SerializeField]
  protected Image _bgImage;
  [SerializeField]
  protected Image _highlightImage;
  [SerializeField]
  protected Color _textColorNormal = new Color(1f, 1f, 1f, 1f);
  [SerializeField]
  protected Color _textColorMe = new Color(1f, 1f, 1f, 1f);
  [SerializeField]
  protected Color _textColorSelected = new Color(0.0f, 0.0f, 0.0f, 1f);
  protected bool _isMe;

  public bool showSeparator
  {
    get => this._separator.activeSelf;
    set
    {
      this._separator.SetActive(value);
      this.RefreshVisuals();
    }
  }

  public virtual void SetData(
    string userName,
    bool isOpenParty,
    bool wantsToPlayNextLevel,
    bool isMyPartyOwner,
    bool isMe)
  {
    this._playerNameText.text = userName;
    this._privateIcon.enabled = !isOpenParty;
    this._spectateIcon.enabled = !wantsToPlayNextLevel;
    this._partyLeaderIcon.enabled = isMyPartyOwner;
    this._isMe = isMe;
    this.RefreshVisuals();
  }

  protected override void SelectionDidChange(SelectableCell.TransitionType transitionType) => this.RefreshVisuals();

  protected override void HighlightDidChange(SelectableCell.TransitionType transitionType) => this.RefreshVisuals();

  public virtual void RefreshVisuals()
  {
    this._bgImage.enabled = this.selected;
    this._highlightImage.enabled = this.highlighted;
    this._playerNameText.color = this.activeColor;
    this._privateIcon.color = this.activeColor;
    this._spectateIcon.color = this.activeColor;
    this._partyLeaderIcon.color = this.activeColor;
  }

  private Color activeColor
  {
    get
    {
      if (this.selected)
        return this._textColorSelected;
      return !this._isMe ? this._textColorNormal : this._textColorMe;
    }
  }
}
