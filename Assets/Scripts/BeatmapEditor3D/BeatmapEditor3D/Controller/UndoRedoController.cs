// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.UndoRedoController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Controller
{
  public class UndoRedoController : ITickable
  {
    [Inject]
    private readonly SignalBus _signalBus;

    public void Tick()
    {
      if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
        this._signalBus.Fire<UndoEditorHistorySignal>();
      if (!Input.GetKey(KeyCode.LeftControl) || !Input.GetKeyDown(KeyCode.Y))
        return;
      this._signalBus.Fire<RedoEditorHistorySignal>();
    }
  }
}
