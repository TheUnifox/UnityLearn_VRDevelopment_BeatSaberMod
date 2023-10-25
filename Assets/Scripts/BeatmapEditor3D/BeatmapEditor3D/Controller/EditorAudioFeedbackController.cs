// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.EditorAudioFeedbackController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.Types;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Controller
{
  public class EditorAudioFeedbackController : MonoBehaviour
  {
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _notePassedFeedback;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly IBeatmapDataModel _beatmapDataModel;
    [Inject]
    private readonly AudioManagerSO _audioManagerSo;
    [Inject]
    private readonly ISongPreviewController _songPreviewController;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    private NoteEditorData[] _currentNoteObjects;
    private ChainEditorData[] _currentArcObjects;
    private int _currentNoteId;
    private int _currentArcId;

    public float sfxVolumeInDb => AudioHelpers.DBToNormalizedVolume(this._audioManagerSo.sfxVolume);

    protected void Start()
    {
      this._songPreviewController.playbackStartedEvent += new Action<float>(this.HandlePlaybackStarted);
      this._songPreviewController.playheadPositionChangedEvent += new Action<int>(this.HandlePlayHeadPositionChanged);
    }

    protected void OnDestroy()
    {
      this._songPreviewController.playbackStartedEvent -= new Action<float>(this.HandlePlaybackStarted);
      this._songPreviewController.playheadPositionChangedEvent -= new Action<int>(this.HandlePlayHeadPositionChanged);
    }

    public void NotesFeedbackVolume(float volume) => this._audioManagerSo.sfxVolume = AudioHelpers.NormalizedVolumeToDB(volume);

    private void HandlePlayHeadPositionChanged(int currentSample)
    {
      if (this._beatmapState.editingMode != BeatmapEditingMode.Objects || !this._songPreviewController.isPlaying)
        return;
      float beat = this._beatmapDataModel.bpmData.SampleToBeat(currentSample);
      bool flag = false;
      for (; this._currentNoteId < this._currentNoteObjects.Length && (double) beat > (double) this._currentNoteObjects[this._currentNoteId].beat; ++this._currentNoteId)
        flag |= this._currentNoteObjects[this._currentNoteId].noteType != NoteType.Bomb;
      for (; this._currentArcId < this._currentArcObjects.Length && (double) beat > (double) this._currentArcObjects[this._currentArcId].beat; ++this._currentArcId)
        flag = true;
      if (!flag)
        return;
      this._audioSource.PlayOneShot(this._notePassedFeedback);
    }

    private void HandlePlaybackStarted(float beat)
    {
      this._currentNoteObjects = this._beatmapLevelDataModel.GetNotesInterval(beat, this._beatmapDataModel.bpmData.totalBeats).ToArray<NoteEditorData>();
      this._currentArcObjects = this._beatmapLevelDataModel.GetChainsInterval(beat, this._beatmapDataModel.bpmData.totalBeats).ToArray<ChainEditorData>();
      this._currentNoteId = 0;
      this._currentArcId = 0;
    }
  }
}
