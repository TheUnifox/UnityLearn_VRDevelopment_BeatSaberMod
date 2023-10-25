// Decompiled with JetBrains decompiler
// Type: IPosesSerializer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public interface IPosesSerializer
{
  void SaveToOldFormat(string path, PosesRecordingData data);

  void SaveRecording(string path, PosesRecordingData data, bool saveToOldFormat);

  PosesRecordingData LoadRecording(string path);

  bool RecordingExists(string path);

  bool RecordingCanBeCreated(string path);
}
