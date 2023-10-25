// Decompiled with JetBrains decompiler
// Type: MockPlayerLobbyPoseGeneratorRecording
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using BGNet.Logging;

public class MockPlayerLobbyPoseGeneratorRecording : MockPlayerLobbyPoseGenerator
{
  public MockPlayerLobbyPoseGeneratorRecording(
    IMultiplayerSessionManager multiplayerSessionManager)
    : base(multiplayerSessionManager)
  {
  }

  public override void Init() => Debug.LogError("Not Implemented");

  public override void Tick() => Debug.LogError("Not Implemented");
}
