// Decompiled with JetBrains decompiler
// Type: RecordingConverter
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using Zenject;

public class RecordingConverter
{
  protected readonly IBeatSaberLogger _logger;

  public RecordingConverter([Inject(Id = "RecordingTool")] IBeatSaberLogger logger) => this._logger = logger;

  public virtual void SaveToOldFormat(string path, PosesRecordingData data) => this._logger.LogException((Exception) new NotImplementedException("Recording conversion to old format is not implemented."));
}
