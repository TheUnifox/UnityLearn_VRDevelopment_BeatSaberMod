// Decompiled with JetBrains decompiler
// Type: GameServerBrowserViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using UnityEngine.UI;

public class GameServerBrowserViewController : ViewController
{
  [Header("Filtering")]
  [SerializeField]
  protected Button _filterServersButton;
  [SerializeField]
  protected GameServersFilterText _filterText;
  [SerializeField]
  protected Toggle _canBeInvitedOnLocalNetworkToggle;
  [Header("Buttons")]
  [SerializeField]
  protected Button _createServerButton;
  [Header("Game List")]
  [SerializeField]
  protected GameServersListTableView _gameServersListTableView;
  [Header("Loadings")]
  [SerializeField]
  protected LoadingControl _mainLoadingControl;
  [SerializeField]
  protected LoadingControl _smallLoadingControl;
}
