// Decompiled with JetBrains decompiler
// Type: BTSCharacterSpawnEventValueParser
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public abstract class BTSCharacterSpawnEventValueParser
{
  private const int kPrefabMask = 255;
  private const int kPrefabBitOffset = 0;
  private const int kAnimationMask = 65280;
  private const int kAnimationBitOffset = 8;
  private const int kAlternativeMaterialMask = 65536;
  private const int kAlternativeMaterialOffset = 16;

  public static int GetPrefabId(int value) => value & (int) byte.MaxValue;

  public static int GetAnimationId(int value) => (value & 65280) >> 8;

  public static bool GetIsAlternativeMaterial(int value) => (value & 65536) >> 16 == 1;

  public static int MergeValuesIntoOneInt(
    int prefabId,
    int animationId,
    bool isAlternativeMaterial)
  {
    int num = isAlternativeMaterial ? 1 : 0;
    return prefabId + (animationId << 8) + (num << 16);
  }
}
