// Decompiled with JetBrains decompiler
// Type: SliderController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class SliderController : SliderControllerBase, IBeatmapObjectController
{
  [SerializeField]
  protected SliderIntensityEffect _sliderIntensityEffect;
  [SerializeField]
  protected SliderMeshController _sliderMeshController;
  [SerializeField]
  protected SliderMovement _sliderMovement;
  [Space]
  [SerializeField]
  protected float _closeInteractionSaberPosSmoothParam = 2f;
  [Inject]
  protected readonly IBeatmapObjectSpawnController _beatmapObjectSpawnController;
  [Inject]
  protected readonly ColorManager _colorManager;
  [Inject]
  protected readonly BeatmapObjectManager _beatmapObjectManager;
  [Inject]
  protected readonly SaberManager _saberManager;
  protected const float kSaberAttractPointNormalizedPosition = 0.7f;
  protected readonly LazyCopyHashSet<ISliderDidFinishJumpEvent> _sliderDidFinishMovement = new LazyCopyHashSet<ISliderDidFinishJumpEvent>();
  protected readonly LazyCopyHashSet<ISliderDidStartDissolvingEvent> _sliderDidStartDissolving = new LazyCopyHashSet<ISliderDidStartDissolvingEvent>();
  protected readonly LazyCopyHashSet<ISliderDidDissolveEvent> _sliderDidDissolve = new LazyCopyHashSet<ISliderDidDissolveEvent>();
  protected readonly LazyCopyHashSet<ISliderHeadDidMovePastCutMarkEvent> _sliderHeadDidMovePastCutMark = new LazyCopyHashSet<ISliderHeadDidMovePastCutMarkEvent>();
  protected readonly LazyCopyHashSet<ISliderTailDidMovePastCutMarkEvent> _sliderTailDidMovePastCutMark = new LazyCopyHashSet<ISliderTailDidMovePastCutMarkEvent>();
  protected SliderController.LengthType _lengthType;
  protected SliderData _sliderData;
  protected Saber _saber;
  protected float _headJumpOffsetY;
  protected float _sliderDuration;
  protected Color _initColor;
  protected bool _attractingSaber;
  protected float _randomValue;
  protected float _zDistanceBetweenNotes;
  protected float _jumpDistance;
  protected FixedUpdateVector3SmoothValue _closeSmoothedSaberInteractionPos;

  public ILazyCopyHashSet<ISliderDidFinishJumpEvent> sliderDidFinishJumpEvent => (ILazyCopyHashSet<ISliderDidFinishJumpEvent>) this._sliderDidFinishMovement;

  public ILazyCopyHashSet<ISliderDidStartDissolvingEvent> sliderDidStartDissolvingEvent => (ILazyCopyHashSet<ISliderDidStartDissolvingEvent>) this._sliderDidStartDissolving;

  public ILazyCopyHashSet<ISliderDidDissolveEvent> sliderDidDissolveEvent => (ILazyCopyHashSet<ISliderDidDissolveEvent>) this._sliderDidDissolve;

  public ILazyCopyHashSet<ISliderHeadDidMovePastCutMarkEvent> sliderHeadDidMovePastCutMark => (ILazyCopyHashSet<ISliderHeadDidMovePastCutMarkEvent>) this._sliderHeadDidMovePastCutMark;

  public ILazyCopyHashSet<ISliderTailDidMovePastCutMarkEvent> sliderTailDidMovePastCutMark => (ILazyCopyHashSet<ISliderTailDidMovePastCutMarkEvent>) this._sliderTailDidMovePastCutMark;

  public SliderController.LengthType lengthType => this._lengthType;

  public SliderData sliderData => this._sliderData;

  public float saberInteractionParam => !this._attractingSaber ? 0.0f : this._sliderIntensityEffect.intensity;

  public SliderMeshController sliderMeshController => this._sliderMeshController;

  public SliderMovement sliderMovement => this._sliderMovement;

  public Color initColor => this._initColor;

  public float randomValue => this._randomValue;

  public float zDistanceBetweenNotes => this._zDistanceBetweenNotes;

  public float jumpDistance => this._jumpDistance;

  public float headJumpOffsetY => this._headJumpOffsetY;

  public float sliderDuration => this._sliderDuration;

  public FixedUpdateVector3SmoothValue closeSmoothedSaberInteractionPos => this._closeSmoothedSaberInteractionPos;

  public SliderIntensityEffect sliderIntensityEffect => this._sliderIntensityEffect;

  public virtual void Init(
    SliderController.LengthType lengthType,
    SliderData sliderData,
    float worldRotation,
    Vector3 headNoteJumpStartPos,
    Vector3 tailNoteJumpStartPos,
    Vector3 headNoteJumpEndPos,
    Vector3 tailNoteJumpEndPos,
    float jumpDuration,
    float startNoteJumpGravity,
    float endNoteJumpGravity,
    float noteUniformScale)
  {
    this._lengthType = lengthType;
    this._sliderData = sliderData;
    this._saber = this._saberManager.SaberForType(sliderData.colorType.ToSaberType());
    this._cutoutAnimateEffect.ResetEffect();
    this._sliderMovement.Init(sliderData.time, sliderData.tailTime, worldRotation, headNoteJumpStartPos, headNoteJumpEndPos, jumpDuration, startNoteJumpGravity, endNoteJumpGravity);
    this._sliderDuration = sliderData.tailTime - sliderData.time;
    this._initColor = this._colorManager.ColorForType(sliderData.colorType);
    this._headJumpOffsetY = this._beatmapObjectSpawnController.jumpOffsetY;
    float halfJumpDuration = jumpDuration * 0.5f;
    this._sliderIntensityEffect.Init(this._sliderDuration, halfJumpDuration, this._sliderData.hasHeadNote);
    float jumpMovementSpeed1 = this._beatmapObjectSpawnController.noteJumpMovementSpeed;
    this._zDistanceBetweenNotes = (sliderData.tailTime - sliderData.time) * jumpMovementSpeed1;
    this._jumpDistance = jumpMovementSpeed1 * jumpDuration;
    Vector3 headNotePos = new Vector3(headNoteJumpEndPos.x, headNoteJumpStartPos.y + (float) ((double) this._sliderMovement.headNoteGravity * (double) halfJumpDuration * (double) halfJumpDuration * 0.5), 0.0f);
    Vector3 tailNotePos = new Vector3(tailNoteJumpEndPos.x, tailNoteJumpStartPos.y + (float) ((double) this._sliderMovement.tailNoteGravity * (double) halfJumpDuration * (double) halfJumpDuration * 0.5), this._zDistanceBetweenNotes);
    this._sliderMeshController.CreateBezierPathAndMesh(sliderData, headNotePos, tailNotePos, jumpMovementSpeed1, noteUniformScale);
    MaterialPropertyBlock materialPropertyBlock = this._materialPropertyBlockController.materialPropertyBlock;
    this._randomValue = UnityEngine.Random.value;
    double jumpMovementSpeed2 = (double) this._beatmapObjectSpawnController.noteJumpMovementSpeed;
    SliderShaderHelper.SetInitialProperties(materialPropertyBlock, this, (float) jumpMovementSpeed2);
    this.SetSaberAttraction(false);
    this._closeSmoothedSaberInteractionPos.SetStartValue(SliderController.GetSaberInteractionPoint(this._saber));
    this.UpdateMaterialPropertyBlock(-halfJumpDuration);
    this._sliderMovement.StartMovement();
  }

  public virtual void Awake()
  {
    this._closeSmoothedSaberInteractionPos = new FixedUpdateVector3SmoothValue(this._closeInteractionSaberPosSmoothParam);
    this._sliderMovement.movementDidFinishEvent += new System.Action(this.HandleMovementDidFinish);
    this._sliderMovement.headDidMovePastCutMarkEvent += new System.Action(this.HandleHeadDidMovePastCutMark);
    this._sliderMovement.tailDidMovePastCutMarkEvent += new System.Action(this.HandleTailDidMovePastCutMark);
  }

  public virtual void Start()
  {
    this._beatmapObjectManager.noteWasCutEvent += new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
    this._beatmapObjectManager.noteWasMissedEvent += new System.Action<NoteController>(this.HandleNoteWasMissed);
    this._sliderIntensityEffect.fadeInDidStartEvent += new System.Action(this.HandleFadeInDidStart);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._sliderMovement != (UnityEngine.Object) null)
    {
      this._sliderMovement.movementDidFinishEvent -= new System.Action(this.HandleMovementDidFinish);
      this._sliderMovement.headDidMovePastCutMarkEvent -= new System.Action(this.HandleHeadDidMovePastCutMark);
      this._sliderMovement.tailDidMovePastCutMarkEvent -= new System.Action(this.HandleTailDidMovePastCutMark);
    }
    if (this._beatmapObjectManager != null)
    {
      this._beatmapObjectManager.noteWasCutEvent -= new BeatmapObjectManager.NoteWasCutDelegate(this.HandleNoteWasCut);
      this._beatmapObjectManager.noteWasMissedEvent -= new System.Action<NoteController>(this.HandleNoteWasMissed);
    }
    if (!((UnityEngine.Object) this._sliderIntensityEffect != (UnityEngine.Object) null))
      return;
    this._sliderIntensityEffect.fadeInDidStartEvent -= new System.Action(this.HandleFadeInDidStart);
  }

  public virtual void Update() => this.ManualUpdate();

  public virtual void FixedUpdate() => this._closeSmoothedSaberInteractionPos.FixedUpdate(SliderController.GetSaberInteractionPoint(this._saber));

  public virtual void ManualUpdate()
  {
    float sinceHeadNoteJump = this._sliderMovement.timeSinceHeadNoteJump;
    this._sliderMovement.ManualUpdate();
    this._sliderIntensityEffect.ManualUpdate(sinceHeadNoteJump);
    this.UpdateMaterialPropertyBlock(sinceHeadNoteJump);
  }

  public virtual void UpdateMaterialPropertyBlock(float timeSinceHeadNoteJump)
  {
    SliderShaderHelper.UpdateMaterialPropertyBlock(this._materialPropertyBlockController.materialPropertyBlock, this, timeSinceHeadNoteJump, this._beatmapObjectSpawnController.jumpOffsetY);
    this._materialPropertyBlockController.ApplyChanges();
  }

  public virtual IEnumerator DissolveCoroutine(float duration)
  {
    SliderController sliderController = this;
    foreach (ISliderDidStartDissolvingEvent startDissolvingEvent in sliderController._sliderDidStartDissolving.items)
      startDissolvingEvent?.HandleSliderDidStartDissolving(sliderController, duration);
    yield return (object) new WaitForSeconds(duration);
    sliderController._dissolving = false;
    foreach (ISliderDidDissolveEvent didDissolveEvent in sliderController._sliderDidDissolve.items)
      didDissolveEvent?.HandleSliderDidDissolve(sliderController);
  }

  public virtual void Dissolve(float duration)
  {
    if (this._dissolving)
      return;
    this._dissolving = true;
    this.AnimateCutout(0.0f, 1f, duration);
    this.StartCoroutine(this.DissolveCoroutine(duration));
  }

  public virtual void Hide(bool hide) => this.gameObject.SetActive(!hide);

  public virtual void Pause(bool pause) => this.enabled = !pause;

  public virtual bool IsNoteStartOfThisSlider(NoteData noteData) => Mathf.Approximately(noteData.time, this._sliderData.time) && noteData.colorType == this._sliderData.colorType && noteData.lineIndex == this._sliderData.headLineIndex && noteData.noteLineLayer == this._sliderData.headLineLayer;

  public virtual void HandleMovementDidFinish()
  {
    foreach (ISliderDidFinishJumpEvent didFinishJumpEvent in this._sliderDidFinishMovement.items)
      didFinishJumpEvent?.HandleSliderDidFinishJump(this);
  }

  public virtual void HandleHeadDidMovePastCutMark()
  {
    if (!this._sliderData.hasHeadNote)
      this._sliderIntensityEffect.StartIntensityFadeInEffect();
    foreach (ISliderHeadDidMovePastCutMarkEvent pastCutMarkEvent in this._sliderHeadDidMovePastCutMark.items)
      pastCutMarkEvent?.HandleSliderStartDidMovePastCutMark(this);
  }

  public virtual void HandleTailDidMovePastCutMark()
  {
    this.SetSaberAttraction(false);
    foreach (ISliderTailDidMovePastCutMarkEvent pastCutMarkEvent in this._sliderTailDidMovePastCutMark.items)
      pastCutMarkEvent?.HandleSliderTailDidMovePastCutMark(this);
  }

  public virtual void HandleNoteWasMissed(NoteController noteController)
  {
    if (!this.IsNoteStartOfThisSlider(noteController.noteData))
      return;
    this._sliderIntensityEffect.StartIntensityDipEffect();
  }

  public virtual void HandleNoteWasCut(NoteController noteController, in NoteCutInfo noteCutInfo)
  {
    if (!this.IsNoteStartOfThisSlider(noteController.noteData))
      return;
    if (noteCutInfo.allIsOK)
      this.SetSaberAttraction(true);
    else
      this._sliderIntensityEffect.StartIntensityDipEffect();
  }

  public virtual void HandleFadeInDidStart() => this.SetSaberAttraction(true);

  public virtual void SetSaberAttraction(bool saberAttraction)
  {
    this._attractingSaber = saberAttraction;
    SliderShaderHelper.EnableSaberAttraction(this._materialPropertyBlockController.materialPropertyBlock, saberAttraction);
  }

  public static Vector3 GetSaberInteractionPoint(Saber saber) => saber.saberBladeBottomPos + (saber.saberBladeTopPos - saber.saberBladeBottomPos) * 0.7f;

  public enum LengthType
  {
    Short,
    Medium,
    Long,
  }

  public class Pool
  {
    protected const float kMinDistanceToUseMedium = 5f;
    protected const float kMinDistanceToUseLong = 15f;
    protected readonly SliderController.Pool.Short _shortPool;
    protected readonly SliderController.Pool.Medium _mediumPool;
    protected readonly SliderController.Pool.Long _longPool;

    public Pool(
      SliderController.Pool.Short shortPool,
      SliderController.Pool.Medium mediumPool,
      SliderController.Pool.Long longPool)
    {
      this._shortPool = shortPool;
      this._mediumPool = mediumPool;
      this._longPool = longPool;
    }

    public virtual MonoMemoryPool<SliderController> GetPool(SliderController.LengthType lengthType)
    {
      switch (lengthType)
      {
        case SliderController.LengthType.Short:
          return (MonoMemoryPool<SliderController>) this._shortPool;
        case SliderController.LengthType.Medium:
          return (MonoMemoryPool<SliderController>) this._mediumPool;
        case SliderController.LengthType.Long:
          return (MonoMemoryPool<SliderController>) this._longPool;
        default:
          Debug.LogError((object) "Unsupported pool length");
          return (MonoMemoryPool<SliderController>) this._longPool;
      }
    }

    public static SliderController.LengthType GetLengthFromSliderData(
      SliderData sliderNoteData,
      BeatmapObjectSpawnMovementData.SliderSpawnData sliderSpawnData)
    {
      float jumpDuration = sliderSpawnData.jumpDuration;
      float num = (sliderSpawnData.headJumpEndPos.z - sliderSpawnData.headJumpStartPos.z) / jumpDuration * (sliderNoteData.time - sliderNoteData.tailTime);
      if ((double) num >= 15.0)
        return SliderController.LengthType.Long;
      return (double) num >= 5.0 ? SliderController.LengthType.Medium : SliderController.LengthType.Short;
    }

    public class Short : MonoMemoryPool<SliderController>
    {
    }

    public class Medium : MonoMemoryPool<SliderController>
    {
    }

    public class Long : MonoMemoryPool<SliderController>
    {
    }
  }
}
