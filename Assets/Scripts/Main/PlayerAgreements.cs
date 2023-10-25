// Decompiled with JetBrains decompiler
// Type: PlayerAgreements
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class PlayerAgreements
{
  public const int kFirstEulaVersion = 1;
  public const int kFirstPrivacyPolicyVersion = 1;
  protected const int kCurrentEulaVersion = 3;
  protected const int kCurrentPrivacyPolicyVersion = 4;
  protected const int kCurrentHealthAndSafetyVersion = 1;
  public int eulaVersion;
  public int privacyPolicyVersion;
  public int healthAndSafetyVersion;

  public PlayerAgreements()
  {
    this.eulaVersion = 0;
    this.privacyPolicyVersion = 0;
    this.healthAndSafetyVersion = 0;
  }

  public PlayerAgreements(int eulaVersion, int privacyPolicyVersion, int healthAndSafetyVersion)
  {
    this.eulaVersion = eulaVersion;
    this.privacyPolicyVersion = privacyPolicyVersion;
    this.healthAndSafetyVersion = healthAndSafetyVersion;
  }

  public virtual void AgreeToEula() => this.eulaVersion = 3;

  public virtual void AgreeToPrivacyPolicy() => this.privacyPolicyVersion = 4;

  public virtual void AgreeToHealthAndSafety() => this.healthAndSafetyVersion = 1;

  public virtual bool AgreedToPreviousPrivacyPolicy() => this.privacyPolicyVersion != 0 && this.privacyPolicyVersion == 3;

  public virtual bool AgreedToAnyPreviousEula() => this.eulaVersion != 0 && this.eulaVersion < 3;

  public virtual bool AgreedToAnyPreviousPrivacyPolicy() => this.privacyPolicyVersion != 0 && this.privacyPolicyVersion < 4;

  public virtual bool AgreedToAnyPreviousHealthAndSafety() => this.healthAndSafetyVersion != 0 && this.healthAndSafetyVersion < 1;

  public virtual bool AgreedToEula() => this.eulaVersion == 3;

  public virtual bool AgreedToPrivacyPolicy() => this.privacyPolicyVersion == 4;

  public virtual bool AgreedToHealthAndSafety() => this.healthAndSafetyVersion == 1;
}
