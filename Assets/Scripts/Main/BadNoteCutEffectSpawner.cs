// Decompiled with JetBrains decompiler
// Type: BadNoteCutEffectSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class BadNoteCutEffectSpawner : MonoBehaviour
{
  [SerializeField]
  protected FlyingSpriteSpawner _failFlyingSpriteSpawner;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  [Inject]
  protected readonly CoreGameHUDController.InitData _initData;

  public virtual void Start()
  {
    if (this._initData.hide)
      this.enabled = false;
    else
      this._beatmapObjectManager.noteWasCutEvent += new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapObjectManager == null)
      return;
    this._beatmapObjectManager.noteWasCutEvent -= new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
  }

  public virtual void HandleNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
  {
    if ((double) noteController.noteData.time + 0.5 < (double) this._audioTimeSyncController.songTime)
      return;
    if (noteController.noteData.colorType == ColorType.None)
    {
      this._failFlyingSpriteSpawner.SpawnFlyingSprite(noteCutInfo.cutPoint, noteController.worldRotation, noteController.inverseWorldRotation);
    }
    else
    {
      if (noteCutInfo.allIsOK)
        return;
      this._failFlyingSpriteSpawner.SpawnFlyingSprite(noteCutInfo.cutPoint, noteController.worldRotation, noteController.inverseWorldRotation);
    }
  }
}
