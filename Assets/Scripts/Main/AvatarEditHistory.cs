// Decompiled with JetBrains decompiler
// Type: AvatarEditHistory
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Menu.ViewControllers;
using System.Collections.Generic;

public class AvatarEditHistory
{
  public bool disableNextSnapshotOverride;
  protected List<EditAvatarHistorySnapshot> _snapShots = new List<EditAvatarHistorySnapshot>();
  protected int _currentDataId;

  public bool undoAvailable => this._currentDataId > 0;

  public bool redoAvailable => this._currentDataId < this._snapShots.Count - 1;

  public EditAvatarHistorySnapshot currentSnapShot => this._snapShots[this._currentDataId];

  public EditAvatarViewController.AvatarEditPart lastEditedPart => this.currentSnapShot.avatarEditPart;

  public virtual void Clear()
  {
    this._snapShots.Clear();
    this._currentDataId = 0;
  }

  public virtual void Undo()
  {
    if (this._currentDataId <= 0)
      return;
    --this._currentDataId;
    this.disableNextSnapshotOverride = true;
  }

  public virtual void Redo()
  {
    if (this._currentDataId >= this._snapShots.Count - 1)
      return;
    ++this._currentDataId;
    this.disableNextSnapshotOverride = true;
  }

  public virtual void UpdateEditHistory(
    AvatarData avatarData,
    EditAvatarViewController.AvatarEditPart avatarEditPart)
  {
    if (this.disableNextSnapshotOverride)
      this.disableNextSnapshotOverride = false;
    else if (this._snapShots.Count > 0 && this._snapShots[this._currentDataId].avatarEditPart == avatarEditPart)
      --this._currentDataId;
    if (this._currentDataId < this._snapShots.Count - 1)
    {
      int index = this._currentDataId + 1;
      this._snapShots.RemoveRange(index, this._snapShots.Count - index);
    }
    this._snapShots.Add(new EditAvatarHistorySnapshot(avatarData.Clone(), avatarEditPart));
    this._currentDataId = this._snapShots.Count - 1;
  }
}
