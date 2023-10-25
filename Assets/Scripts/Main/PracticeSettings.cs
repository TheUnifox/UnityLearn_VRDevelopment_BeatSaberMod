// Decompiled with JetBrains decompiler
// Type: PracticeSettings
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System;
using UnityEngine;

[Serializable]
public class PracticeSettings
{
  public const float kDelayBeforeStart = 1f;
  [SerializeField]
  protected float _startSongTime;
  [SerializeField]
  protected float _songSpeedMul;
  protected bool _startInAdvanceAndClearNotes = true;

  public float startSongTime
  {
    get => this._startSongTime;
    set => this._startSongTime = value;
  }

  public float songSpeedMul
  {
    get => this._songSpeedMul;
    set => this._songSpeedMul = value;
  }

  public bool startInAdvanceAndClearNotes
  {
    get => this._startInAdvanceAndClearNotes;
    set => this._startInAdvanceAndClearNotes = value;
  }

  public static PracticeSettings defaultPracticeSettings => new PracticeSettings();

  public PracticeSettings() => this.ResetToDefault();

  public PracticeSettings(PracticeSettings practiceSettings)
  {
    this._startSongTime = practiceSettings._startSongTime;
    this._songSpeedMul = practiceSettings._songSpeedMul;
    this._startInAdvanceAndClearNotes = true;
  }

  public PracticeSettings(float startSongTime, float songSpeedMul)
  {
    this._startSongTime = startSongTime;
    this._songSpeedMul = songSpeedMul;
    this._startInAdvanceAndClearNotes = true;
  }

  public virtual void ResetToDefault()
  {
    this._startSongTime = 0.0f;
    this._songSpeedMul = 1f;
    this._startInAdvanceAndClearNotes = true;
  }
}
