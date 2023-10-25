// Decompiled with JetBrains decompiler
// Type: BombCutSoundEffectManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class BombCutSoundEffectManager : MonoBehaviour
{
  [SerializeField]
  protected float _volume = 0.3f;
  [SerializeField]
  protected AudioClip[] _bombExplosionAudioClips;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly SaberManager saberManager;
  [Inject]
  protected readonly BombCutSoundEffect.Pool _bombCutSoundEffectPool;
  protected RandomObjectPicker<AudioClip> _randomSoundPicker;

  public virtual void Start()
  {
    this._randomSoundPicker = new RandomObjectPicker<AudioClip>(this._bombExplosionAudioClips, 0.0f);
    this._beatmapObjectManager.noteWasCutEvent += new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
  }

  public virtual void HandleNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
  {
    if (!(noteController is BombNoteController))
      return;
    float volumeMultiplier = noteController.noteData.cutSfxVolumeMultiplier;
    if ((double) volumeMultiplier <= 0.0)
      return;
    BombCutSoundEffect bombCutSoundEffect = this._bombCutSoundEffectPool.Spawn();
    bombCutSoundEffect.transform.SetPositionAndRotation(this.transform.localPosition, Quaternion.identity);
    bombCutSoundEffect.didFinishEvent += new System.Action<BombCutSoundEffect>(this.HandleBombCutSoundEffectDidFinish);
    Saber saber = this.saberManager.SaberForType(noteCutInfo.saberType);
    bombCutSoundEffect.Init(this._randomSoundPicker.PickRandomObject(), saber, this._volume * volumeMultiplier);
  }

  public virtual void OnDestroy()
  {
    if (this._beatmapObjectManager == null)
      return;
    this._beatmapObjectManager.noteWasCutEvent -= new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
  }

  public virtual void HandleBombCutSoundEffectDidFinish(BombCutSoundEffect bombCutSoundEffect)
  {
    bombCutSoundEffect.didFinishEvent -= new System.Action<BombCutSoundEffect>(this.HandleBombCutSoundEffectDidFinish);
    this._bombCutSoundEffectPool.Despawn(bombCutSoundEffect);
  }
}
