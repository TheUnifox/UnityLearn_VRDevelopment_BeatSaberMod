// Decompiled with JetBrains decompiler
// Type: MultiplayerBadgesModelSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;

public class MultiplayerBadgesModelSO : ScriptableObject
{
  [SerializeField]
  protected List<MultiplayerBadgeDataSO> _positiveBadges;
  [SerializeField]
  protected List<MultiplayerBadgeDataSO> _negativeBadges;

  public IReadOnlyList<MultiplayerBadgeDataSO> positiveBadges => (IReadOnlyList<MultiplayerBadgeDataSO>) this._positiveBadges;

  public IReadOnlyList<MultiplayerBadgeDataSO> negativeBadges => (IReadOnlyList<MultiplayerBadgeDataSO>) this._negativeBadges;
}
