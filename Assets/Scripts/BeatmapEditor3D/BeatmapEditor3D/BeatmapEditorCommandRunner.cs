// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorCommandRunner
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands;
using System.Diagnostics;
using Zenject;

namespace BeatmapEditor3D
{
  public class BeatmapEditorCommandRunner : IBeatmapEditorCommandRunner
  {
    [Inject]
    private readonly SignalBus _signalBus;
    private readonly IBeatmapEditorHistory _beatmapEditorHistory = (IBeatmapEditorHistory) new BeatmapEditorHistory();

    public void ExecuteCommand(IBeatmapEditorCommand command)
    {
      command.Execute();
      this.Log(command, "EXECUTE");
      if (!(command is IBeatmapEditorCommandWithHistory commandWithHistory) || !commandWithHistory.shouldAddToHistory)
        return;
      if (command is IBeatmapEditorCommandWithHistoryMergeable commandWithHistoryMergeable && this._beatmapEditorHistory.last is IBeatmapEditorCommandWithHistoryMergeable last && commandWithHistoryMergeable.ShouldMergeWith(last))
      {
        commandWithHistoryMergeable.MergeWith(last);
        this._beatmapEditorHistory.ReplaceLast(commandWithHistoryMergeable);
      }
      else
        this._beatmapEditorHistory.Add(commandWithHistory);
    }

    public void Undo()
    {
      IBeatmapEditorCommandWithHistory command = this._beatmapEditorHistory.Undo();
      command?.Undo();
      this.Log((IBeatmapEditorCommand) command, "UNDO");
    }

    public void Redo()
    {
      IBeatmapEditorCommandWithHistory command = this._beatmapEditorHistory.Redo();
      command?.Redo();
      this.Log((IBeatmapEditorCommand) command, "REDO");
    }

    public void ClearHistory() => this._beatmapEditorHistory.Clear();

    [Conditional("BeatmapEditorCommandRunnerLogging")]
    private void Log(IBeatmapEditorCommand command, string operation)
    {
      if (command is IBeatmapEditorCommandMessageProvider commandMessageProvider)
      {
        (CommandMessageType type, string commandName) = commandMessageProvider.GetMessage();
        this._signalBus.Fire<CommandMessageSignal>(new CommandMessageSignal(type, CommandMessageFormatterHelper.FormatMessage(operation, commandName)));
      }
      if (!(command is IBeatmapEditorCommandWithHistory commandWithHistory) || !commandWithHistory.shouldAddToHistory)
        return;
      UnityEngine.Debug.Log((object) ("(" + operation + "): " + command.GetType().Name));
    }
  }
}
