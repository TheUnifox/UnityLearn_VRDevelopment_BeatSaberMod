// Decompiled with JetBrains decompiler
// Type: EmptyPosesSerializer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class EmptyPosesSerializer : IPosesSerializer
{
  public virtual void SaveToOldFormat(string path, PosesRecordingData data)
  {
  }

  public virtual void SaveRecording(string path, PosesRecordingData data, bool saveToOldFormat)
  {
  }

  public virtual PosesRecordingData LoadRecording(string path) => (PosesRecordingData) null;

  public virtual bool RecordingExists(string path) => false;

  public virtual bool RecordingCanBeCreated(string path) => false;
}
