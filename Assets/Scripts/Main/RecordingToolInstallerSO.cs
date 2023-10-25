// Decompiled with JetBrains decompiler
// Type: RecordingToolInstallerSO
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class RecordingToolInstallerSO : PersistentScriptableObject
{
  [SerializeField]
  protected RecordingToolResourceContainerSO _recordingToolResourceContainer;

  public virtual void InstallDependencies(DiContainer container, ProgramArguments programArguments)
  {
    RecordingToolManager instance = new RecordingToolManager(programArguments, this._recordingToolResourceContainer);
    container.Bind<RecordingToolManager>().FromInstance(instance).AsSingle();
    container.Bind<IBeatSaberLogger>().WithId((object) "RecordingTool").FromInstance(instance.logger).AsSingle();
    container.Bind<IPosesSerializer>().FromInstance(instance.posesSerializer).AsSingle();
    if (!instance.recordingToolEnabled || instance.recordingToolSettings == null)
      return;
    container.Bind<RecordingToolSettings>().FromInstance(instance.recordingToolSettings).AsSingle();
    if (instance.objectsMovementRecorderInitData == null)
      return;
    container.Bind<ObjectsMovementRecorder.InitData>().FromInstance(instance.objectsMovementRecorderInitData).AsSingle();
    container.Bind<RecordingUIController.InitData>().FromInstance(new RecordingUIController.InitData(instance.objectsMovementRecorderInitData.mode == ObjectsMovementRecorder.Mode.Record)).AsSingle();
    if (instance.menuDestination == null)
      return;
    container.Rebind<MenuDestination>().FromInstance(instance.menuDestination).AsSingle();
  }
}
