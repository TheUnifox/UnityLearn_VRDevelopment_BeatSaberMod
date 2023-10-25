// Decompiled with JetBrains decompiler
// Type: BeatEffectSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class BeatEffectSpawner : MonoBehaviour, IBeatEffectDidFinishEvent
{
  [SerializeField]
  protected float _effectDuration = 1f;
  [SerializeField]
  protected Color _bombColorEffect = new Color(0.1f, 0.1f, 0.1f, 0.5f);
  [Inject]
  protected readonly ColorManager _colorManager;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly AudioTimeSyncController _audioTimeSyncController;
  [Inject]
  protected readonly BeatEffectSpawner.InitData _initData;
  [Inject]
  protected readonly BloomFogSO _bloomFog;
  protected SongController _songController;
  protected MemoryPoolContainer<BeatEffect> _beatEffectPoolContainer;

  [Inject]
  public virtual void Init(BeatEffect.Pool beatEffectPool) => this._beatEffectPoolContainer = new MemoryPoolContainer<BeatEffect>((IMemoryPool<BeatEffect>) beatEffectPool);

  public virtual void Start() => this._beatmapObjectManager.noteDidStartJumpEvent += new System.Action<NoteController>(this.HandleNoteDidStartJump);

  public virtual void OnDestroy()
  {
    if (this._beatmapObjectManager == null)
      return;
    this._beatmapObjectManager.noteDidStartJumpEvent -= new System.Action<NoteController>(this.HandleNoteDidStartJump);
  }

  public virtual void Update()
  {
    float deltaTime = Time.deltaTime;
    foreach (BeatEffect activeItem in this._beatEffectPoolContainer.activeItems)
      activeItem.ManualUpdate(deltaTime);
  }

  public virtual void HandleNoteDidStartJump(NoteController noteController)
  {
    if (this._initData.hideNoteSpawnEffect || noteController.hidden || (double) noteController.noteData.time + 0.10000000149011612 < (double) this._audioTimeSyncController.songTime)
      return;
    ColorType colorType = noteController.noteData.colorType;
    Color color = colorType != ColorType.None ? this._colorManager.ColorForType(colorType) : this._bombColorEffect;
    BeatEffect beatEffect = this._beatEffectPoolContainer.Spawn();
    beatEffect.didFinishEvent.Add((IBeatEffectDidFinishEvent) this);
    beatEffect.transform.SetPositionAndRotation(noteController.worldRotation * noteController.jumpStartPos - new Vector3(0.0f, 0.15f, 0.0f), Quaternion.identity);
    beatEffect.Init(color * this._bloomFog.noteSpawnIntensity, this._effectDuration, noteController.worldRotation);
  }

  public virtual void HandleBeatEffectDidFinish(BeatEffect beatEffect)
  {
    beatEffect.didFinishEvent.Remove((IBeatEffectDidFinishEvent) this);
    this._beatEffectPoolContainer.Despawn(beatEffect);
  }

  public class InitData
  {
    public readonly bool hideNoteSpawnEffect;

    public InitData(bool hideNoteSpawnEffect) => this.hideNoteSpawnEffect = hideNoteSpawnEffect;
  }
}
