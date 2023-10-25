// Decompiled with JetBrains decompiler
// Type: StaticJumpOffsetYProvider
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Zenject;

public class StaticJumpOffsetYProvider : IJumpOffsetYProvider
{
  [Inject]
  protected readonly StaticJumpOffsetYProvider.InitData _initData;

  public float jumpOffsetY => this._initData.jumpyYOffset;

  public class InitData
  {
    public readonly float jumpyYOffset;

    public InitData(float jumpyYOffset) => this.jumpyYOffset = jumpyYOffset;
  }
}
