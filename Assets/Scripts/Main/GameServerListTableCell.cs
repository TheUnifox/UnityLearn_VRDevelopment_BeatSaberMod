// Decompiled with JetBrains decompiler
// Type: GameServerListTableCell
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using UnityEngine;
using Zenject;

public class GameServerListTableCell : TableCell
{
  [SerializeField]
  protected CurvedTextMeshPro _serverName;
  [SerializeField]
  protected CurvedTextMeshPro _difficultiesText;
  [SerializeField]
  protected CurvedTextMeshPro _musicPackText;
  [SerializeField]
  protected CurvedTextMeshPro _playerCount;
  [SerializeField]
  protected GameObject _passwordProtected;
  [Inject]
  protected readonly SongPackMasksModel _songPackMasksModel;

  public virtual void SetData(INetworkPlayer player)
  {
    this._serverName.text = player.userName;
    BeatmapLevelSelectionMask selectionMask = player.selectionMask;
    GameplayServerConfiguration configuration = player.configuration;
    this._difficultiesText.text = Localization.Get(selectionMask.difficulties.LocalizedKey());
    this._musicPackText.text = this._songPackMasksModel.GetSongPackMaskText(in selectionMask.songPacks);
    this._playerCount.text = string.Format("{0}/{1}", (object) player.currentPartySize, (object) configuration.maxPlayerCount);
    this._passwordProtected.SetActive(player.requiresPassword);
  }
}
