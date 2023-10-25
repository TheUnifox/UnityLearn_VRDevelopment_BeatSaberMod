// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.BeatmapEditorNoteCutController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Controller
{
  public class BeatmapEditorNoteCutController : MonoBehaviour
  {
    [SerializeField]
    private BeatmapEditorNoteController _noteControllerStub;
    [Inject]
    private readonly BeatmapEditorBeatmapObjectManager _beatmapEditorBeatmapObjectManager;
    [Inject]
    private readonly ISongPreviewController _songPreviewController;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly IBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly BeatmapEditorSettingsDataModel _beatmapEditorSettingsDataModel;
    private NoteEditorData[] _currentBeatmapObjects;
    private int _currentNoteId;

    protected void Start()
    {
      this._songPreviewController.playbackStartedEvent += new Action<float>(this.HandleSongPreviewControllerPlaybackStarted);
      this._songPreviewController.playheadPositionChangedEvent += new Action<int>(this.HandleSongPreviewControllerPlayHeadPositionChanged);
    }

    protected void OnDestroy()
    {
      this._songPreviewController.playbackStartedEvent -= new Action<float>(this.HandleSongPreviewControllerPlaybackStarted);
      this._songPreviewController.playheadPositionChangedEvent -= new Action<int>(this.HandleSongPreviewControllerPlayHeadPositionChanged);
    }

    private void HandleSongPreviewControllerPlaybackStarted(float beat)
    {
      this._currentBeatmapObjects = this._beatmapLevelDataModel.GetNotesInterval(beat, this._beatmapDataModel.bpmData.totalBeats).ToArray<NoteEditorData>();
      this._currentNoteId = 0;
    }

    private void HandleSongPreviewControllerPlayHeadPositionChanged(int currentSample)
    {
      if (this._beatmapEditorSettingsDataModel.staticLights || !this._songPreviewController.isPlaying)
        return;
      float beat = this._beatmapDataModel.bpmData.SampleToBeat(currentSample);
      List<NoteEditorData> noteEditorDataList = new List<NoteEditorData>();
      for (; this._currentNoteId < this._currentBeatmapObjects.Length && (double) beat > (double) this._currentBeatmapObjects[this._currentNoteId].beat; ++this._currentNoteId)
      {
        if (this._currentBeatmapObjects[this._currentNoteId].noteType != NoteType.Bomb)
          noteEditorDataList.Add(this._currentBeatmapObjects[this._currentNoteId]);
      }
      foreach (NoteEditorData noteEditorData in noteEditorDataList)
      {
        this._noteControllerStub.Init(NoteData.CreateBasicNoteData(0.0f, noteEditorData.column, NoteLineLayer.Base, noteEditorData.type, NoteCutDirection.Any));
        this._beatmapEditorBeatmapObjectManager.TriggerNoteCut((NoteController) this._noteControllerStub, new NoteCutInfo());
      }
    }
  }
}
