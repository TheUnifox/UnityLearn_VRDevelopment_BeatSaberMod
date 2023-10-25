// Decompiled with JetBrains decompiler
// Type: MultiplayerBadgeAwardData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

public class MultiplayerBadgeAwardData : IComparable
{
  protected readonly IConnectedPlayer _awardedPlayer;
  protected readonly float _weight;
  protected readonly string _title;
  protected readonly string _subtitle;
  protected readonly Sprite _icon;
  protected readonly MultiplayerBadgeDataSO _badgeData;

  public IConnectedPlayer awardedPlayer => this._awardedPlayer;

  public string titleLocalizationKey => this._badgeData.titleLocalizationKey;

  public string title => this._title;

  public string subtitle => this._subtitle;

  public Sprite icon => this._icon;

  public MultiplayerBadgeAwardData(
    IConnectedPlayer awardedPlayer,
    float weight,
    string title,
    string subtitle,
    MultiplayerBadgeDataSO badgeData)
  {
    this._awardedPlayer = awardedPlayer;
    this._weight = weight;
    this._title = title;
    this._subtitle = subtitle;
    this._icon = badgeData.icon;
    this._badgeData = badgeData;
  }

  public virtual int CompareTo(object obj)
  {
    if (obj == null)
      return 1;
    if (obj is MultiplayerBadgeAwardData multiplayerBadgeAwardData)
      return this._weight.CompareTo(multiplayerBadgeAwardData._weight) * -1;
    throw new ArgumentException("Comparing not comparable data.");
  }
}
