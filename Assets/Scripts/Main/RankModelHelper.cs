// Decompiled with JetBrains decompiler
// Type: RankModelHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public abstract class RankModelHelper
{
  public static RankModel.Rank MaxRankForGameplayModifiers(
    GameplayModifiers gameplayModifiers,
    GameplayModifiersModelSO gameplayModifiersModel,
    float energy)
  {
    List<GameplayModifierParamsSO> modifierParamsList = gameplayModifiersModel.CreateModifierParamsList(gameplayModifiers);
    int num = gameplayModifiersModel.MaxModifiedScoreForMaxMultipliedScore(1000000, modifierParamsList, energy);
    return RankModel.GetRankForScore(0, num, 1000000, num);
  }
}
