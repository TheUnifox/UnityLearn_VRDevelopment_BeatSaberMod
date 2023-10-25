// Decompiled with JetBrains decompiler
// Type: MultiplayerBadgeDataGoodCutsSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class MultiplayerBadgeDataGoodCutsSO : MultiplayerBadgeDataMinMaxIntSO
{
  protected override int GetValue(MultiplayerPlayerResultsData result) => result.multiplayerLevelCompletionResults.levelCompletionResults.goodCutsCount;
}
