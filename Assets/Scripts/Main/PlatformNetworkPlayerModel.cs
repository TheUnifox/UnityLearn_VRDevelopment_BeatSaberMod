// Decompiled with JetBrains decompiler
// Type: PlatformNetworkPlayerModel
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public abstract class PlatformNetworkPlayerModel : BaseNetworkPlayerModel
{
  public IEnumerable<INetworkPlayer> friends => this.GetOtherPlayers();

  public override void DestroyPartyConnection()
  {
    base.DestroyPartyConnection();
    this.DestroyConnectedPlayerManager();
  }

  public class CreatePartyConfig : 
    BaseNetworkPlayerModel.PartyConfig,
    INetworkPlayerModelPartyConfig<PlatformNetworkPlayerModel>
  {
  }
}
