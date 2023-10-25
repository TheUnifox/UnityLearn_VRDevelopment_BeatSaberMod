// Decompiled with JetBrains decompiler
// Type: BTSStarTextEffectValueParser
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public abstract class BTSStarTextEffectValueParser
{
  private const int kTextMask = 255;
  private const int kTextBitOffset = 0;
  private const int kPositionMask = 65280;
  private const int kPositionBitOffset = 8;

  public static int GetTextId(int value) => value & (int) byte.MaxValue;

  public static int GetPositionId(int value) => (value & 65280) >> 8;

  public static int MergeValuesIntoOneInt(int textId, int positionId) => textId + (positionId << 8);
}
