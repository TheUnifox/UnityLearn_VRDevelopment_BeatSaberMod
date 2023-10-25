// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorCommandRunnerSignalBinder
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorCommandRunnerSignalBinder
  {
    [Inject]
    private readonly IBeatmapEditorCommandRunner _commandRunner;
    [Inject(Id = "SignalsContainer")]
    private readonly DiContainer _commandContainer;

    public void BindSignal<TSignal, TCommand>(TSignal signal) where TCommand : IBeatmapEditorCommand
    {
      this._commandContainer.Bind<TSignal>().FromInstance(signal);
      this._commandRunner.ExecuteCommand((IBeatmapEditorCommand) this._commandContainer.Resolve<PlaceholderFactory<TCommand>>().Create());
      this._commandContainer.Unbind<TSignal>();
    }
  }
}
