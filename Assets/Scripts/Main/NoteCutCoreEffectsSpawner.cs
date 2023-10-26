// Decompiled with JetBrains decompiler
// Type: NoteCutCoreEffectsSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class NoteCutCoreEffectsSpawner : MonoBehaviour
{
  [SerializeField]
  protected float _shockWaveYPos = 0.1f;
  [Space]
  [SerializeField]
  protected NoteCutParticlesEffect _noteCutParticlesEffect;
  [SerializeField]
  protected NoteDebrisSpawner _noteDebrisSpawner;
  [SerializeField]
  protected NoteCutHapticEffect _noteCutHapticEffect;
  [SerializeField]
  protected ShockwaveEffect _shockwaveEffect;
  [SerializeField]
  protected BombExplosionEffect _bombExplosionEffect;
  [Inject]
  protected readonly ColorManager _colorManager;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  protected const int kNormalNoteSparkleParticlesCount = 150;
  protected const int kNormalNoteExplosionParticlesCount = 50;
  protected const int kBurstSliderElementSparkleParticlesCount = 50;
  protected const int kBurstSliderElementParticlesCount = 20;

  public virtual void Start() => this._beatmapObjectManager.noteWasCutEvent += new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);

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
    switch (noteController.noteData.gameplayType)
    {
      case NoteData.GameplayType.Normal:
        this.SpawnNoteCutEffect(in noteCutInfo, noteController, 150, 50);
        this._noteCutHapticEffect.HitNote(noteCutInfo.saberType, NoteCutHapticEffect.Type.Normal);
        break;
      case NoteData.GameplayType.Bomb:
        this.SpawnBombCutEffect(in noteCutInfo, noteController);
        this._noteCutHapticEffect.HitNote(noteCutInfo.saberType, NoteCutHapticEffect.Type.Normal);
        break;
      case NoteData.GameplayType.BurstSliderHead:
        this.SpawnNoteCutEffect(in noteCutInfo, noteController, 150, 50);
        this._noteCutHapticEffect.HitNote(noteCutInfo.saberType, NoteCutHapticEffect.Type.ShortNormal);
        break;
      case NoteData.GameplayType.BurstSliderElement:
        this.SpawnNoteCutEffect(in noteCutInfo, noteController, 50, 20);
        this._noteCutHapticEffect.HitNote(noteCutInfo.saberType, NoteCutHapticEffect.Type.ShortWeak);
        break;
    }
  }

  public virtual void SpawnNoteCutEffect(
    in NoteCutInfo noteCutInfo,
    NoteController noteController,
    int sparkleParticlesCount,
    int explosionParticlesCount)
  {
    Vector3 moveVec = noteController.moveVec;
    NoteData noteData = noteController.noteData;
    if (noteCutInfo.allIsOK)
    {
      Color color = this._colorManager.ColorForType(noteData.colorType).ColorWithAlpha(0.5f);
      Vector3 cutPoint = noteCutInfo.cutPoint;
      this._noteCutParticlesEffect.SpawnParticles(noteCutInfo.cutPoint, noteCutInfo.cutNormal, noteCutInfo.saberDir, noteCutInfo.saberSpeed, noteController.moveVec, (Color32) color, sparkleParticlesCount, explosionParticlesCount, Mathf.Clamp(noteData.timeToNextColorNote + 0.1f, 0.5f, 2f));
            Vector3 pos = cutPoint;
            pos.y = this._shockWaveYPos;
            this._shockwaveEffect.SpawnShockwave(pos);
    }
    Transform noteTransform = noteController.noteTransform;
    this._noteDebrisSpawner.SpawnDebris(noteData.gameplayType, noteCutInfo.cutPoint, noteCutInfo.cutNormal, noteCutInfo.saberSpeed, noteCutInfo.saberDir, noteTransform.position, noteTransform.rotation, noteTransform.localScale, noteController.noteData.colorType, noteController.noteData.timeToNextColorNote, moveVec);
  }

  public virtual void SpawnBombCutEffect(in NoteCutInfo noteCutInfo, NoteController noteController)
  {
    Vector3 cutPoint = noteCutInfo.cutPoint;
    this._bombExplosionEffect.SpawnExplosion(cutPoint);
        Vector3 pos = cutPoint;
        pos.y = this._shockWaveYPos;
        this._shockwaveEffect.SpawnShockwave(pos);
  }
}
