// Decompiled with JetBrains decompiler
// Type: PlatformUserAuthTokenData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Runtime.CompilerServices;

public class PlatformUserAuthTokenData
{
  [CompilerGenerated]
  protected string m_Ctoken;
  [CompilerGenerated]
  protected PlatformUserAuthTokenData.PlatformEnviroment m_CvalidPlatformEnvironment;

  public string token
  {
    get => this.m_Ctoken;
    private set => this.m_Ctoken = value;
  }

  public PlatformUserAuthTokenData.PlatformEnviroment validPlatformEnvironment
  {
    get => this.m_CvalidPlatformEnvironment;
    private set => this.m_CvalidPlatformEnvironment = value;
  }

  public PlatformUserAuthTokenData(
    string token,
    PlatformUserAuthTokenData.PlatformEnviroment validPlatformEnvironment)
  {
    this.token = token;
    this.validPlatformEnvironment = validPlatformEnvironment;
  }

  public enum PlatformEnviroment : byte
  {
    Development,
    Certification,
    Production,
  }
}
