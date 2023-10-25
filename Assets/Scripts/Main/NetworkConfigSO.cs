// Decompiled with JetBrains decompiler
// Type: NetworkConfigSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Oculus.Platform;
using UnityEngine;

public class NetworkConfigSO : PersistentScriptableObject, INetworkConfig
{
  [SerializeField]
  protected int _maxPartySize = 8;
  [SerializeField]
  protected int _discoveryPort = 8887;
  [SerializeField]
  protected int _partyPort = 8888;
  [SerializeField]
  protected int _multiplayerPort = 8889;
  [SerializeField]
  protected int _masterServerPort = 2328;
  [SerializeField]
  protected string _masterServerHostName = "localhost";
  [SerializeField]
  protected string _multiplayerStatusUrl = "";
  [SerializeField]
  protected string _quickPlaySetupUrl = "";
  [SerializeField]
  protected string _graphUrl = "";
  [SerializeField]
  protected string _graphAppId = "";
  [SerializeField]
  protected bool _forceGameLift;
  [SerializeField]
  protected ServiceEnvironment _serviceEnvironment;

  public int maxPartySize => this._maxPartySize;

  public int discoveryPort => this._discoveryPort;

  public int partyPort => this._partyPort;

  public int multiplayerPort => this._multiplayerPort;

  public DnsEndPoint masterServerEndPoint => new DnsEndPoint(this._masterServerHostName, this._masterServerPort);

  public string multiplayerStatusUrl => this._multiplayerStatusUrl;

  public string quickPlaySetupUrl => this._quickPlaySetupUrl;

  public string graphUrl => this._graphUrl;

  public string graphAccessToken => "OC|" + this.GetAppId() + "|";

  public bool forceGameLift => this._forceGameLift;

  public ServiceEnvironment serviceEnvironment => this._serviceEnvironment;

  public virtual string GetAppId() => UnityEngine.Application.platform != RuntimePlatform.Android ? PlatformSettings.AppID : PlatformSettings.MobileAppID;
}
