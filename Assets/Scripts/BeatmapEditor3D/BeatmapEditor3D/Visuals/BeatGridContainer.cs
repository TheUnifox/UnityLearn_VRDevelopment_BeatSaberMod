// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.BeatGridContainer
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Visuals
{
  public class BeatGridContainer : MonoBehaviour
  {
    [SerializeField]
    private BeatlineContainer _mainBeatlineContainer;
    [SerializeField]
    private BeatlineContainer _normalBeatlineContainer;
    [Space]
    [SerializeField]
    private BeatNumberContainer _beatNumberContainer;
    [Space]
    [SerializeField]
    private Transform _currentBeatLineTransform;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    [Inject]
    private readonly SignalBus _signalBus;
    private float _tracksCount;
    private float _trackWidth;

    public void Enable()
    {
      this._signalBus.Subscribe<SubdivisionChangedSignal>(new Action(this.HandleSubdivisionChanged));
      this._mainBeatlineContainer.Enable();
      this._beatNumberContainer.Enable();
      this._normalBeatlineContainer.Enable();
    }

    public void Disable()
    {
      this._signalBus.TryUnsubscribe<SubdivisionChangedSignal>(new Action(this.HandleSubdivisionChanged));
      this._mainBeatlineContainer.Disable();
      this._normalBeatlineContainer.Disable();
      this._beatNumberContainer.Disable();
    }

        public void SetDataToBeatContainers(float tracksCount, float trackWidth)
        {
            this._tracksCount = tracksCount;
            this._trackWidth = trackWidth;
            float x = tracksCount * trackWidth * 0.5f + 0.05f;
            this._mainBeatlineContainer.SetData(tracksCount * trackWidth, this._mainBeatlineContainer.transform.position, 4, 1f, new Func<float, bool>(this.ShouldSkipMainBeatline));
            this._normalBeatlineContainer.SetData(tracksCount * trackWidth, this._normalBeatlineContainer.transform.position, this._beatmapState.beatSubdivisionsModel.currentStartSubdivision, this._beatmapState.beatSubdivisionsModel.currentSubdivisionIncrement, new Func<float, bool>(this.ShouldSkipItem));
            this._beatNumberContainer.SetData(this._beatNumberContainer.transform.position + new Vector3(x, 0f, 0f), 4, 1f, null);
            Vector3 localScale = this._currentBeatLineTransform.localScale;
            localScale.x = tracksCount * trackWidth + 1f;
            this._currentBeatLineTransform.localScale = localScale;
        }

        public void ForceUpdate()
    {
      this._mainBeatlineContainer.ForceUpdate();
      this._beatNumberContainer.ForceUpdate();
      this._normalBeatlineContainer.ForceUpdate();
    }

    private void HandleSubdivisionChanged()
    {
      this._normalBeatlineContainer.SetData(this._tracksCount * this._trackWidth, this._normalBeatlineContainer.transform.position, this._beatmapState.beatSubdivisionsModel.currentStartSubdivision, this._beatmapState.beatSubdivisionsModel.currentSubdivisionIncrement, new Func<float, bool>(this.ShouldSkipItem));
      this._normalBeatlineContainer.UpdateItems();
    }

    private bool ShouldSkipMainBeatline(float beat) => (double) beat - (double) this._beatmapState.beat - (double) this._beatmapState.beatOffset == 0.0;

    private bool ShouldSkipItem(float beat) => (double) beat % 1.0 == 0.0 || this.ShouldSkipMainBeatline(beat);
  }
}
