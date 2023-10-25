// Decompiled with JetBrains decompiler
// Type: RecordingToolManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;

public class RecordingToolManager
{
  public const string kRecordingToolId = "RecordingTool";
  protected readonly bool _recordingToolEnabled;
  protected readonly string _configJsonData;
  protected readonly RecordingToolSettings _recordingToolSettings;
  protected readonly ObjectsMovementRecorder.InitData _objectsMovementRecorderInitData;
  protected readonly MenuDestination _menuDestination;
  protected readonly ListLogger _listLogger;
  protected readonly IBeatSaberLogger _logger;
  protected readonly IPosesSerializer _posesSerializer;

  public bool recordingToolEnabled => this._recordingToolEnabled;

  public bool showRecordingToolScene => this._recordingToolEnabled;

  public string configJsonData => this._configJsonData;

  public RecordingToolSettings recordingToolSettings => this._recordingToolSettings;

  public ObjectsMovementRecorder.InitData objectsMovementRecorderInitData => this._objectsMovementRecorderInitData;

  public MenuDestination menuDestination => this._menuDestination;

  public ListLogger listLogger => this._listLogger;

  public IBeatSaberLogger logger => this._logger;

  public IPosesSerializer posesSerializer => this._posesSerializer;

  public RecordingToolManager(
    ProgramArguments programArguments,
    RecordingToolResourceContainerSO resourceContainer)
  {
    this._listLogger = new ListLogger();
    this._logger = (IBeatSaberLogger) new CompositeLogger(new List<IBeatSaberLogger>()
    {
      (IBeatSaberLogger) new UnityDebugLogger(),
      (IBeatSaberLogger) this._listLogger
    });
    this._posesSerializer = (IPosesSerializer) new PosesSerializer(this._logger);
    RecordingToolConfigurationProcessor configurationProcessor = new RecordingToolConfigurationProcessor(programArguments, this._logger, this._posesSerializer, resourceContainer);
    this._recordingToolEnabled = configurationProcessor.IsRecordingToolEnabled();
    string configFilePath = configurationProcessor.GetConfigFilePath();
    this._configJsonData = configurationProcessor.LoadConfigurationFile(configFilePath);
    RecordingToolConfigurationProcessor.RecordingToolConfiguration recordingToolConfiguration = configurationProcessor.DeserializeConfigurationFile(this.configJsonData);
    this._recordingToolSettings = configurationProcessor.CreateRecordingToolSettingsFromConfiguration(recordingToolConfiguration);
    this._objectsMovementRecorderInitData = configurationProcessor.CreateObjectsMovementRecorderInitDataFromConfiguration(this._recordingToolSettings);
    this._menuDestination = this._objectsMovementRecorderInitData != null ? configurationProcessor.CreateMenuDestinationFromConfiguration(this._recordingToolSettings) : (MenuDestination) null;
  }
}
