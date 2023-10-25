// Decompiled with JetBrains decompiler
// Type: BeatmapEditorGameplaySceneSetupData
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class BeatmapEditorGameplaySceneSetupData : SceneSetupData
{
  public readonly bool useFirstPersonFlyingController;
  public readonly bool recordVRMovement;
  public readonly bool playVRMovement;

  public BeatmapEditorGameplaySceneSetupData(
    bool useFirstPersonFlyingController,
    bool recordVRMovement,
    bool playVRMovement)
  {
    this.useFirstPersonFlyingController = useFirstPersonFlyingController;
    this.recordVRMovement = recordVRMovement;
    this.playVRMovement = playVRMovement;
  }
}
