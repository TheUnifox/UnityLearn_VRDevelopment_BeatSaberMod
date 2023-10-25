// Decompiled with JetBrains decompiler
// Type: LightRotationBaseData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine.Assertions;

public class LightRotationBaseData
{
  public readonly float beat;
  public readonly bool usePreviousEventRotationValue;
  public readonly EaseType easeType;
  public readonly float rotation;
  public readonly int loopsCount;
  public readonly LightRotationDirection rotationDirection;

  public LightRotationBaseData(
    float beat,
    bool usePreviousEventRotationValue,
    EaseType easeType,
    float rotation,
    int loopsCount,
    LightRotationDirection rotationDirection)
  {
    Assert.IsTrue(loopsCount >= 0);
    this.beat = beat;
    this.usePreviousEventRotationValue = usePreviousEventRotationValue;
    this.easeType = easeType;
    this.loopsCount = loopsCount;
    this.rotation = rotation;
    this.rotationDirection = rotationDirection;
  }
}
