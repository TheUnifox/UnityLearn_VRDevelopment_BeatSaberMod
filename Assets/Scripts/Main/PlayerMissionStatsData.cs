// Decompiled with JetBrains decompiler
// Type: PlayerMissionStatsData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class PlayerMissionStatsData
{
  protected string _missionId;
  protected bool _cleared;

  public string missionId => this._missionId;

  public bool cleared
  {
    get => this._cleared;
    set => this._cleared = value;
  }

  public PlayerMissionStatsData(string missionId, bool cleared)
  {
    this._missionId = missionId;
    this._cleared = cleared;
  }
}
