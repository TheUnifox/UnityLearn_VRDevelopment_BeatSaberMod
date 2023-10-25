// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorHistory
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;

namespace BeatmapEditor3D
{
  public class BeatmapEditorHistory : IBeatmapEditorHistory
  {
    private readonly List<IBeatmapEditorCommandWithHistory> _commandHistory = new List<IBeatmapEditorCommandWithHistory>();
    private int _currentItemIndex = -1;

    public IBeatmapEditorCommandWithHistory last => this._currentItemIndex < 0 ? (IBeatmapEditorCommandWithHistory) null : this._commandHistory[this._currentItemIndex];

    public void Add(
      IBeatmapEditorCommandWithHistory commandWithHistory)
    {
      if (this._commandHistory.Count - (this._currentItemIndex + 1) > 0)
        this._commandHistory.RemoveRange(this._currentItemIndex + 1, this._commandHistory.Count - (this._currentItemIndex + 1));
      this._commandHistory.Add(commandWithHistory);
      ++this._currentItemIndex;
    }

    public void ReplaceLast(
      IBeatmapEditorCommandWithHistoryMergeable commandWithHistoryMergeable)
    {
      --this._currentItemIndex;
      this.Add((IBeatmapEditorCommandWithHistory) commandWithHistoryMergeable);
    }

    public IBeatmapEditorCommandWithHistory Undo()
    {
      if (this._currentItemIndex < 0)
        return (IBeatmapEditorCommandWithHistory) null;
      IBeatmapEditorCommandWithHistory commandWithHistory = this._commandHistory[this._currentItemIndex];
      --this._currentItemIndex;
      return commandWithHistory;
    }

    public IBeatmapEditorCommandWithHistory Redo()
    {
      if (this._currentItemIndex + 1 >= this._commandHistory.Count)
        return (IBeatmapEditorCommandWithHistory) null;
      ++this._currentItemIndex;
      return this._commandHistory[this._currentItemIndex];
    }

    public void Clear()
    {
      this._commandHistory.Clear();
      this._currentItemIndex = -1;
    }
  }
}
