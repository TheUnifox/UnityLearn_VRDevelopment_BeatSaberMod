// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.PaginationView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class PaginationView : MonoBehaviour
  {
    [SerializeField]
    private GameObject _wrapperGo;
    [SerializeField]
    private PaginationView.PaginationPair[] _paginationPairs;
    [Inject]
    private readonly BeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;

    protected void OnEnable()
    {
      this._signalBus.Subscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitched));
      this.SetActiveState();
    }

    protected void OnDisable() => this._signalBus.TryUnsubscribe<BeatmapEditingModeSwitched>(new Action(this.HandleLevelEditorModeSwitched));

    private void HandleLevelEditorModeSwitched() => this.SetActiveState();

    private void SetActiveState()
    {
      BeatmapEditingMode editingMode = this._beatmapState.editingMode;
      bool flag = false;
      foreach (PaginationView.PaginationPair paginationPair in this._paginationPairs)
      {
        paginationPair.go.SetActive(paginationPair.mode == editingMode);
        flag |= paginationPair.mode == editingMode;
      }
      this._wrapperGo.SetActive(flag);
    }

    [Serializable]
    private class PaginationPair
    {
      [SerializeField]
      private BeatmapEditingMode _mode;
      [SerializeField]
      private GameObject _go;

      public BeatmapEditingMode mode => this._mode;

      public GameObject go => this._go;
    }
  }
}
