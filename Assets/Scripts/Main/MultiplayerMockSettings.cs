// Decompiled with JetBrains decompiler
// Type: MultiplayerMockSettings
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMockSettings : ScriptableObject
{
  [SerializeField]
  protected bool _isEnabled;
  [SerializeField]
  protected MockPlayerSettings _localPlayer;
  [SerializeField]
  protected List<MockPlayerSettings> _otherPlayers = new List<MockPlayerSettings>();
  [SerializeField]
  protected MockServerSettings _quickplayServer;
  [SerializeField]
  protected MultiplayerStatusData _multiplayerStatusData;
  [SerializeField]
  protected QuickPlaySetupData _quickPlaySetupData;

  public MockServerSettings quickplayServer => this._quickplayServer;

  public MockPlayerSettings localPlayer
  {
    get
    {
      if (this._localPlayer == null)
        this._localPlayer = new MockPlayerSettings()
        {
          userId = Guid.NewGuid().ToString(),
          userName = "ThisIsMe"
        };
      return this._localPlayer;
    }
  }

  public QuickPlaySetupData quickPlaySetupData
  {
    get => this._quickPlaySetupData;
    set => this._quickPlaySetupData = value;
  }

  public MultiplayerStatusData multiplayerStatusData
  {
    get => this._multiplayerStatusData;
    set => this._multiplayerStatusData = value;
  }

  public List<MockPlayerSettings> otherPlayers => this._otherPlayers;

  public bool isEnabled
  {
    get => false;
    set => this._isEnabled = value;
  }

  public static MultiplayerMockSettings SharedSettings() => (MultiplayerMockSettings) null;
}
