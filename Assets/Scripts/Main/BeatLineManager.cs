// Decompiled with JetBrains decompiler
// Type: BeatLineManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BeatLineManager : MonoBehaviour
{
  [SerializeField]
  protected float _linesYPosition;
  [Inject]
  protected BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected BeatLine.Pool _beatLinePool;
  [Inject]
  protected AudioTimeSyncController _audioTimeSyncController;
  protected Dictionary<Vector4, BeatLine> _activeBeatLines = new Dictionary<Vector4, BeatLine>(16);
  protected List<Vector4> _removeBeatLineKeyList = new List<Vector4>(8);
  protected bool _isMidRotationValid;
  protected float _midRotation;
  protected float _rotationRange;

  public bool isMidRotationValid => this._isMidRotationValid;

  public float midRotation => this._midRotation;

  public float rotationRange => this._rotationRange;

  public virtual void Start() => this._beatmapObjectManager.noteWasSpawnedEvent += new System.Action<NoteController>(this.HandleNoteWasSpawned);

  public virtual void OnDestroy() => this._beatmapObjectManager.noteWasSpawnedEvent -= new System.Action<NoteController>(this.HandleNoteWasSpawned);

  public virtual void Update()
  {
    Dictionary<Vector4, BeatLine>.ValueCollection values = this._activeBeatLines.Values;
    this._removeBeatLineKeyList.Clear();
    float num1 = 0.0f;
    float num2 = 0.0f;
    float current = 0.0f;
    int num3 = 0;
    foreach (KeyValuePair<Vector4, BeatLine> activeBeatLine in this._activeBeatLines)
    {
      BeatLine beatLine = activeBeatLine.Value;
      beatLine.ManualUpdate(this._audioTimeSyncController.songTime);
      if (beatLine.isFinished)
      {
        this._removeBeatLineKeyList.Add(activeBeatLine.Key);
      }
      else
      {
        if (num3 == 0)
        {
          current = beatLine.rotation;
        }
        else
        {
          float num4 = Mathf.DeltaAngle(current, beatLine.rotation);
          if ((double) num4 > 0.0 && (double) num2 < (double) num4)
            num2 = num4;
          else if ((double) num4 < 0.0 && (double) num1 > (double) num4)
            num1 = num4;
        }
        ++num3;
      }
    }
    if (num3 > 0)
    {
      this._rotationRange = num2 - num1;
      this._midRotation = current + (float) (((double) num1 + (double) num2) * 0.5);
      this._isMidRotationValid = true;
    }
    else
      this._isMidRotationValid = false;
    foreach (Vector4 removeBeatLineKey in this._removeBeatLineKeyList)
    {
      this._beatLinePool.Despawn(this._activeBeatLines[removeBeatLineKey]);
      this._activeBeatLines.Remove(removeBeatLineKey);
    }
  }

    public virtual void HandleNoteWasSpawned(NoteController noteController)
    {
        Vector4 vector = noteController.beatPos;
        vector.z = -this._linesYPosition;
        vector.w = noteController.worldRotation.eulerAngles.y;
        BeatLine beatLine;
        if (!this._activeBeatLines.TryGetValue(vector, out beatLine))
        {
            beatLine = this._beatLinePool.Spawn();
            beatLine.Init(vector, vector.w);
            this._activeBeatLines[vector] = beatLine;
        }
        beatLine.AddHighlight(noteController.moveStartTime, noteController.moveDuration, noteController.jumpDuration);
    }
}
