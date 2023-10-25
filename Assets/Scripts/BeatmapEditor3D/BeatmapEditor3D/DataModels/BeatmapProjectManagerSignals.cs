// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.DataModels.BeatmapProjectManagerSignals
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.BpmEditor;
using System.IO;
using System.Threading;
using Zenject;

namespace BeatmapEditor3D.DataModels
{
  public class BeatmapProjectManagerSignals
  {
    public class LoadSettingsSignal
    {
    }

    public class SaveSettingsSignal
    {
    }

    public class LoadBeatmapProjectSignal
    {
      public string beatmapFolderPath { get; }

      public bool openTempFolder { get; }

      public LoadBeatmapProjectSignal(string path, bool openTemp)
      {
        this.beatmapFolderPath = path;
        this.openTempFolder = openTemp;
      }
    }

    public class LoadBeatmapProjectFromLastSaveSignal
    {
    }

    public class SaveBeatmapProjectSignal
    {
    }

    public class BeatmapProjectSavedSignal
    {
    }

    public class SaveBeatmapProjectToTempSignal
    {
    }

    public class SaveBpmInfoSignal
    {
    }

    public class SaveBpmInfoToTempSignal
    {
    }

    public class CloseBpmInfoSignal
    {
    }

    public class CloseBeatmapProjectSignal
    {
    }

    public class LoadBeatmapLevelSignal
    {
      public BeatmapCharacteristicSO beatmapCharacteristic { get; }

      public BeatmapDifficulty beatmapDifficulty { get; }

      public LoadBeatmapLevelSignal(
        BeatmapCharacteristicSO characteristic,
        BeatmapDifficulty difficulty)
      {
        this.beatmapCharacteristic = characteristic;
        this.beatmapDifficulty = difficulty;
      }
    }

    public class BeatmapLevelDataModelLoaded
    {
    }

    public class SaveBeatmapLevelSignal
    {
    }

    public class SaveBeatmapLevelToTempSignal
    {
    }

    public class BeatmapSaveFailedSignal
    {
      public readonly string reason;

      public BeatmapSaveFailedSignal(string reason) => this.reason = reason;
    }

    public class LoadBeatmapLevelFromLastSaveSignal
    {
      public BeatmapCharacteristicSO beatmapCharacteristic { get; }

      public BeatmapDifficulty beatmapDifficulty { get; }

      public LoadBeatmapLevelFromLastSaveSignal(
        BeatmapCharacteristicSO characteristic,
        BeatmapDifficulty difficulty)
      {
        this.beatmapCharacteristic = characteristic;
        this.beatmapDifficulty = difficulty;
      }
    }

    public class BeatmapLevelWillCloseSignal
    {
    }

    public class CloseBeatmapLevelSignal
    {
    }

    public class LoadSettingsCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;

      public void Execute() => this._beatmapProjectManager.LoadBeatmapEditorSettings();
    }

    public class SaveSettingsCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;

      public void Execute() => this._beatmapProjectManager.SaveBeatmapEditorSettings();
    }

    public class LoadBeatmapProjectCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapProjectManagerSignals.LoadBeatmapProjectSignal _signal;
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;

      public void Execute() => this._beatmapProjectManager.LoadBeatmapProject(this._signal.beatmapFolderPath, this._signal.openTempFolder);
    }

    public class LoadBeatmapProjectFromLastSaveCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;
      [Inject]
      private readonly SignalBus _signalBus;

      public void Execute()
      {
        this._beatmapProjectManager.LoadBeatmapProjectFromLastSave();
        this._signalBus.Fire<BeatmapDataModelSignals.BeatmapLoadedSignal>();
      }
    }

    public class SaveBeatmapProjectCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;
      [Inject]
      private readonly BeatmapDataModel _beatmapDataModel;
      [Inject]
      private readonly SignalBus _signalBus;

      public void Execute()
      {
        if (!this._beatmapDataModel.beatmapDataLoaded)
          return;
        try
        {
          this._beatmapProjectManager.SaveBeatmapProject(true);
          this._signalBus.Fire<BeatmapProjectManagerSignals.BeatmapProjectSavedSignal>();
        }
        catch (IOException ex)
        {
          this._signalBus.Fire<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(new BeatmapProjectManagerSignals.BeatmapSaveFailedSignal(ex.Message));
        }
      }
    }

    public class SaveBeatmapProjectToTempCommand : BaseAsyncBeatmapEditorCommand<object, string>
    {
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;
      [Inject]
      private readonly BeatmapDataModel _beatmapDataModel;

      protected override BaseAsyncBeatmapEditorCommand<object, string>.MultipleSameTasksRunPolicy multipleSameTasksRunPolicy => BaseAsyncBeatmapEditorCommand<object, string>.MultipleSameTasksRunPolicy.CancelRunning;

      protected override string ExecuteAsync(object input, CancellationToken cancellationToken)
      {
        if (!this._beatmapDataModel.beatmapDataLoaded)
          return (string) null;
        try
        {
          this._beatmapProjectManager.SaveBeatmapProject(false);
        }
        catch (IOException ex)
        {
          return ex.Message;
        }
        return (string) null;
      }
    }

    public class SaveBpmInfoCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;
      [Inject]
      private readonly BeatmapDataModel _beatmapDataModel;
      [Inject]
      private readonly BpmEditorDataModel _bpmEditorDataModel;
      [Inject]
      private readonly SignalBus _signalBus;

      public void Execute()
      {
        if (!this._bpmEditorDataModel.loaded)
          return;
        try
        {
          this._beatmapProjectManager.SaveBpmInfo(true);
          this._beatmapDataModel.LoadBpmData(this._bpmEditorDataModel.bpmData, false);
        }
        catch (IOException ex)
        {
          this._signalBus.Fire<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(new BeatmapProjectManagerSignals.BeatmapSaveFailedSignal(ex.Message));
        }
      }
    }

    public class SaveBpmInfoToTempCommand : BaseAsyncBeatmapEditorCommand<object, string>
    {
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;
      [Inject]
      private readonly BpmEditorDataModel _bpmEditorDataModel;
      [Inject]
      private readonly SignalBus _signalBus;

      protected override BaseAsyncBeatmapEditorCommand<object, string>.MultipleSameTasksRunPolicy multipleSameTasksRunPolicy => BaseAsyncBeatmapEditorCommand<object, string>.MultipleSameTasksRunPolicy.CancelRunning;

      protected override string ExecuteAsync(object input, CancellationToken cancellationToken)
      {
        if (!this._bpmEditorDataModel.loaded)
          return (string) null;
        try
        {
          this._beatmapProjectManager.SaveBpmInfo(false);
        }
        catch (IOException ex)
        {
          return ex.Message;
        }
        return (string) null;
      }

      protected override void TaskFinished(string result)
      {
        if (result == null)
          return;
        this._signalBus.Fire<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(new BeatmapProjectManagerSignals.BeatmapSaveFailedSignal(result));
      }
    }

    public class CloseBpmInfoCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BpmEditorDataModel _bpmEditorDataModel;

      public void Execute() => this._bpmEditorDataModel.Close();
    }

    public class CloseBeatmapProjectCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;

      public void Execute() => this._beatmapProjectManager.CloseBeatmapProject();
    }

    public class LoadBeatmapLevelCommand : 
      IBeatmapEditorCommand,
      IBeatmapEditorCommandMessageProvider
    {
      [Inject]
      private readonly SignalBus _signalBus;
      [Inject]
      private readonly BeatmapProjectManagerSignals.LoadBeatmapLevelSignal _signal;
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;

      public void Execute()
      {
        if (!this._beatmapProjectManager.LoadBeatmapLevel(this._signal.beatmapCharacteristic, this._signal.beatmapDifficulty))
          return;
        this._signalBus.Fire<BeatmapProjectManagerSignals.BeatmapLevelDataModelLoaded>();
      }

      public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, "Load Beatmap Level");
    }

    public class SaveBeatmapLevelToTempCommand : BaseAsyncBeatmapEditorCommand<object, string>
    {
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;
      [Inject]
      private readonly SignalBus _signalBus;

      protected override BaseAsyncBeatmapEditorCommand<object, string>.MultipleSameTasksRunPolicy multipleSameTasksRunPolicy => BaseAsyncBeatmapEditorCommand<object, string>.MultipleSameTasksRunPolicy.CancelRunning;

      protected override string ExecuteAsync(object input, CancellationToken cancellationToken)
      {
        try
        {
          this._beatmapProjectManager.SaveBeatmapLevel(false);
        }
        catch (IOException ex)
        {
          return ex.Message;
        }
        return (string) null;
      }

      protected override void TaskFinished(string result)
      {
        if (result == null)
          return;
        this._signalBus.Fire<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(new BeatmapProjectManagerSignals.BeatmapSaveFailedSignal(result));
      }
    }

    public class SaveBeatmapLevelCommand : 
      IBeatmapEditorCommand,
      IBeatmapEditorCommandMessageProvider
    {
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;
      [Inject]
      private readonly SignalBus _signalBus;

      public void Execute()
      {
        try
        {
          this._beatmapProjectManager.SaveBeatmapLevel(true);
        }
        catch (IOException ex)
        {
          this._signalBus.Fire<BeatmapProjectManagerSignals.BeatmapSaveFailedSignal>(new BeatmapProjectManagerSignals.BeatmapSaveFailedSignal(ex.Message));
        }
      }

      public (CommandMessageType, string) GetMessage() => (CommandMessageType.Normal, "Save Beatmap Level");
    }

    public class LoadBeatmapLevelFromLastSaveCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapProjectManagerSignals.LoadBeatmapLevelFromLastSaveSignal _signal;
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;

      public void Execute() => this._beatmapProjectManager.ReplaceBeatmapLevelFromLatestSave(this._signal.beatmapCharacteristic, this._signal.beatmapDifficulty);
    }

    public class CloseBeatmapLevelCommand : IBeatmapEditorCommand
    {
      [Inject]
      private readonly BeatmapProjectManager _beatmapProjectManager;
      [Inject]
      private readonly SignalBus _signalBus;

      public void Execute()
      {
        this._signalBus.Fire<BeatmapProjectManagerSignals.BeatmapLevelWillCloseSignal>();
        this._beatmapProjectManager.CloseBeatmapLevel();
      }
    }
  }
}
