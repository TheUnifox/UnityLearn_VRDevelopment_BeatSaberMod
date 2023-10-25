// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.EditBeatmapLevelNavigationViewController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BeatmapEditor3D
{
  public class EditBeatmapLevelNavigationViewController : BeatmapEditorViewController
  {
    [SerializeField]
    private Toggle _deleteModeToggle;

    public event Action<bool> deleteModeChangedEvent;

    public void SetDeleteMode(bool deleteEnabled) => this._deleteModeToggle.SetIsOnWithoutNotify(deleteEnabled);

    protected override void DidActivate(
      bool firstActivation,
      bool addedToHierarchy,
      bool screenSystemEnabling)
    {
      this._deleteModeToggle.onValueChanged.AddListener(new UnityAction<bool>(this.HandleDeleteModeToggleValueChanged));
    }

    protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling) => this._deleteModeToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.HandleDeleteModeToggleValueChanged));

    private void HandleDeleteModeToggleValueChanged(bool isOn)
    {
      Action<bool> modeChangedEvent = this.deleteModeChangedEvent;
      if (modeChangedEvent == null)
        return;
      modeChangedEvent(isOn);
    }
  }
}
