// Decompiled with JetBrains decompiler
// Type: PS4PublishingHelper
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public abstract class PS4PublishingHelper
{
  public static string GetServiceId(PS4PublisherSKUSettingsSO ps4PublisherSKUSettings) => PS4PublishingHelper.GetServiceIdFromTitleId(ps4PublisherSKUSettings.serviceIdPrefix, ps4PublisherSKUSettings.titleId);

  public static string GetContentId(PS4PublisherSKUSettingsSO ps4PublisherSKUSettings) => PS4PublishingHelper.GetContentIdFromTitleId(ps4PublisherSKUSettings.serviceIdPrefix, ps4PublisherSKUSettings.titleId, ps4PublisherSKUSettings.productLabel);

  public static string GetContentId(string serviceId, string productLabel) => serviceId + "-" + productLabel;

  public static string GetContentIdFromTitleId(
    string serviceIdPrefix,
    string titleId,
    string productLabel)
  {
    return PS4PublishingHelper.GetContentId(PS4PublishingHelper.GetServiceIdFromTitleId(serviceIdPrefix, titleId), productLabel);
  }

  public static string GetContentIdFromNpTitleId(
    string serviceIdPrefix,
    string npTitleId,
    string productLabel)
  {
    return PS4PublishingHelper.GetContentId(PS4PublishingHelper.GetServiceIdFromNpTitleId(serviceIdPrefix, npTitleId), productLabel);
  }

  public static string GetNpTitleId(string titleId) => titleId + "_00";

  public static string GetServiceIdFromTitleId(string serviceIdPrefix, string titleId) => PS4PublishingHelper.GetServiceIdFromNpTitleId(serviceIdPrefix, PS4PublishingHelper.GetNpTitleId(titleId));

  public static string GetServiceIdFromNpTitleId(string serviceIdPrefix, string npTitleId) => serviceIdPrefix + "-" + npTitleId;
}
