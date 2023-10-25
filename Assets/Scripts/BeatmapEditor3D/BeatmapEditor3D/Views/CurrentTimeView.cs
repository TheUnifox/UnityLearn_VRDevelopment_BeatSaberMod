// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.CurrentTimeView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using TMPro;
using UnityEngine;

namespace BeatmapEditor3D.Views
{
  public class CurrentTimeView : MonoBehaviour
  {
    [SerializeField]
    private TextMeshProUGUI _beatText;
    [SerializeField]
    private TextMeshProUGUI _timeText;
    private AudioClip _audioClip;

    public void SetAudioClip(AudioClip audioClip) => this._audioClip = audioClip;

    public void SetCurrentBeat(int sample, float beat)
    {
      this._timeText.text = this.FormatSeconds(AudioTimeHelper.SamplesToSeconds(sample, this._audioClip.frequency));
      this._beatText.text = string.Format("{0:F03}", (object) beat);
    }

    private string FormatSeconds(float inSeconds)
    {
      TimeSpan timeSpan = TimeSpan.FromSeconds((double) inSeconds);
      return string.Format("{0:D2}:{1:D2}:{2:D2}", (object) timeSpan.Minutes, (object) timeSpan.Seconds, (object) timeSpan.Milliseconds);
    }
  }
}
