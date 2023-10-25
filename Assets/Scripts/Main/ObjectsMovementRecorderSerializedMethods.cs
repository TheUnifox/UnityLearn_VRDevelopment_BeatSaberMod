// Decompiled with JetBrains decompiler
// Type: ObjectsMovementRecorderSerializedMethods
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public static class ObjectsMovementRecorderSerializedMethods
{
  private const string kRecordRecordingMode = "Record";
  private const string kPlaybackRecordingMode = "Playback";
  private const string kOffRecordingMode = "Off";
  private const string kFirstPersonCameraView = "FirstPerson";
  private const string kThirdPersonCameraView = "ThirdPerson";
  private const string kBackgroundPlaybackScreenshotType = "Background";
  private const string kForegroundPlaybackScreenshotType = "Foreground";

  public static string SerializedName(this ObjectsMovementRecorder.Mode mode)
  {
    switch (mode)
    {
      case ObjectsMovementRecorder.Mode.Record:
        return "Record";
      case ObjectsMovementRecorder.Mode.Playback:
        return "Playback";
      case ObjectsMovementRecorder.Mode.Off:
        return "Off";
      default:
        return "Off";
    }
  }

  public static bool ModeFromSerializedName(this string name, out ObjectsMovementRecorder.Mode mode)
  {
    switch (name)
    {
      case "Record":
        mode = ObjectsMovementRecorder.Mode.Record;
        return true;
      case "Playback":
        mode = ObjectsMovementRecorder.Mode.Playback;
        return true;
      case "Off":
        mode = ObjectsMovementRecorder.Mode.Off;
        return true;
      default:
        mode = ObjectsMovementRecorder.Mode.Off;
        return false;
    }
  }

  public static string SerializedName(this ObjectsMovementRecorder.CameraView cameraView) => cameraView == ObjectsMovementRecorder.CameraView.FirstPerson || cameraView != ObjectsMovementRecorder.CameraView.ThirdPerson ? "FirstPerson" : "ThirdPerson";

  public static bool CameraViewFromSerializedName(
    this string name,
    out ObjectsMovementRecorder.CameraView cameraView)
  {
    switch (name)
    {
      case "FirstPerson":
        cameraView = ObjectsMovementRecorder.CameraView.FirstPerson;
        return true;
      case "ThirdPerson":
        cameraView = ObjectsMovementRecorder.CameraView.ThirdPerson;
        return true;
      default:
        cameraView = ObjectsMovementRecorder.CameraView.FirstPerson;
        return false;
    }
  }

  public static string SerializedName(this PlaybackRenderer.PlaybackScreenshot.Type type)
  {
    if (type == PlaybackRenderer.PlaybackScreenshot.Type.Foreground)
      return "Foreground";
    return "Background";
  }

  public static bool PlaybackScreenshotTypeFromSerializedName(
    this string name,
    out PlaybackRenderer.PlaybackScreenshot.Type type)
  {
    switch (name)
    {
      case "Background":
        type = PlaybackRenderer.PlaybackScreenshot.Type.Background;
        return true;
      case "Foreground":
        type = PlaybackRenderer.PlaybackScreenshot.Type.Foreground;
        return true;
      default:
        type = PlaybackRenderer.PlaybackScreenshot.Type.Background;
        return false;
    }
  }
}
