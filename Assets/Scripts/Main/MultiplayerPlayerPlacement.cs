// Decompiled with JetBrains decompiler
// Type: MultiplayerPlayerPlacement
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MultiplayerPlayerPlacement
{
  public static Vector3 GetPlayerWorldPosition(
    float outerCircleRadius,
    float outerCirclePositionAngle,
    MultiplayerPlayerLayout layout)
  {
    if (layout == MultiplayerPlayerLayout.Duel)
      return ((double) outerCirclePositionAngle + 360.0) % 360.0 > 90.0 ? new Vector3(-5f, 0.0f, 12f) : Vector3.zero;
    Quaternion quaternion = Quaternion.Euler(0.0f, outerCirclePositionAngle, 0.0f);
    Vector3 vector3_1 = Vector3.forward * outerCircleRadius;
    Vector3 vector3_2 = -vector3_1;
    return quaternion * vector3_2 + vector3_1;
  }

  public static float GetOuterCirclePositionAngleForPlayer(
    int playerIndex,
    int localPlayerIndex,
    float angleBetweenPlayers)
  {
    return -angleBetweenPlayers * (float) (playerIndex - localPlayerIndex);
  }

  public static float GetOuterCircleRadius(float angleBetweenPlayers, float innerCircleRadius)
  {
    float num = (float) ((180.0 - (double) angleBetweenPlayers / 2.0) / 2.0);
    return innerCircleRadius / Mathf.Cos((float) Math.PI / 180f * num);
  }

  public static float GetAngleBetweenPlayersWithEvenAdjustment(
    int numberOfPlayers,
    MultiplayerPlayerLayout layout)
  {
    if (numberOfPlayers % 2 == 0 && layout != MultiplayerPlayerLayout.Duel)
      ++numberOfPlayers;
    return 360f / (float) numberOfPlayers;
  }

  public static void SortPlayers(List<IConnectedPlayer> players) => players.Sort((Comparison<IConnectedPlayer>) ((p1, p2) => p1.sortIndex.CompareTo(p2.sortIndex)));

  public static int GetLocalPlayerIndex(
    IList<IConnectedPlayer> otherPlayers,
    IConnectedPlayer localPlayer)
  {
    for (int index = 0; index < otherPlayers.Count; ++index)
    {
      if (otherPlayers[index].sortIndex > localPlayer.sortIndex)
        return index;
    }
    return otherPlayers.Count;
  }
}
