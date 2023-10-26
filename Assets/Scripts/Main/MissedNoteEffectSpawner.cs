// Decompiled with JetBrains decompiler
// Type: MissedNoteEffectSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MissedNoteEffectSpawner : MonoBehaviour
{
  [SerializeField]
  protected FlyingSpriteSpawner _missedNoteFlyingSpriteSpawner;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  [Inject]
  protected readonly CoreGameHUDController.InitData _initData;
  protected float _spawnPosZ;

  public virtual void Start()
  {
    if (this._initData.hide)
    {
      this.enabled = false;
    }
    else
    {
      this._beatmapObjectManager.noteWasMissedEvent += new System.Action<NoteController>(this.HandleNoteWasMissed);
      this._spawnPosZ = this.transform.position.z;
    }
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapObjectManager == null)
      return;
    this._beatmapObjectManager.noteWasMissedEvent -= new System.Action<NoteController>(this.HandleNoteWasMissed);
  }

  public virtual void HandleNoteWasMissed(NoteController noteController)
  {
    if (noteController.hidden || (double) noteController.noteData.time + 0.5 < (double) this._audioTimeSyncController.songTime || noteController.noteData.colorType == ColorType.None)
      return;
        Vector3 vector = noteController.noteTransform.position;
        Quaternion worldRotation = noteController.worldRotation;
        vector = noteController.inverseWorldRotation * vector;
        vector.z = this._spawnPosZ;
        vector = worldRotation * vector;
        this._missedNoteFlyingSpriteSpawner.SpawnFlyingSprite(vector, noteController.worldRotation, noteController.inverseWorldRotation);
    }
}
