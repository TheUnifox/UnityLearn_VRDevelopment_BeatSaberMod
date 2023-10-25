// Decompiled with JetBrains decompiler
// Type: DisappearingArrowControllerBase`1
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class DisappearingArrowControllerBase<T> : MonoBehaviour where T : ICubeNoteControllerInitializable<T>, INoteVisualModifierTypeProvider, INoteMovementProvider
{
  [SerializeField]
  private MaterialPropertyBlockController[] _transparentObjectMaterialPropertyBlocks;
  [SerializeField]
  private MeshRenderer _cubeMeshRenderer;
  [SerializeField]
  [NullAllowed]
  private CutoutEffect _arrowCutoutEffect;
  [Space]
  [SerializeField]
  private float _disappearingNormalStart = 14f;
  [SerializeField]
  private float _disappearingNormalEnd = 8f;
  [SerializeField]
  private float _disappearingGhostStart = 10f;
  [SerializeField]
  private float _disappearingGhostEnd = 4f;
  private float _prevArrowTransparency;
  private float _minDistance;
  private float _maxDistance;
  private bool _hideMesh;
  private bool _fadeArrow;
  [DoesNotRequireDomainReloadInit]
  private static readonly int _colorId = Shader.PropertyToID("_Color");

  protected abstract T gameNoteController { get; }

  protected void Awake() => this.gameNoteController.cubeNoteControllerDidInitEvent += new System.Action<T>(this.HandleCubeNoteControllerDidInit);

  protected void OnDestroy()
  {
    if ((object) this.gameNoteController == null)
      return;
    T gameNoteController = this.gameNoteController;
    gameNoteController.cubeNoteControllerDidInitEvent -= new System.Action<T>(this.HandleCubeNoteControllerDidInit);
    gameNoteController = this.gameNoteController;
    if (!((UnityEngine.Object) gameNoteController.noteMovement != (UnityEngine.Object) null))
      return;
    gameNoteController = this.gameNoteController;
    gameNoteController.noteMovement.noteDidMoveInJumpPhaseEvent -= new System.Action(this.HandleNoteMovementNoteDidMoveInJumpPhase);
  }

  private void CalculateMinMaxDistance()
  {
    Vector3 moveEndPos = this.gameNoteController.noteMovement.moveEndPos;
    this._maxDistance = Mathf.Min(moveEndPos.z * 0.8f, !this._hideMesh || !this._fadeArrow ? this._disappearingNormalStart : this._disappearingGhostStart);
    this._minDistance = Mathf.Min(moveEndPos.z * 0.5f, !this._hideMesh || !this._fadeArrow ? this._disappearingNormalEnd : this._disappearingGhostEnd);
  }

  private void HandleNoteMovementNoteDidMoveInJumpPhase() => this.SetArrowTransparency(Mathf.Clamp01((float) (((double) this.gameNoteController.noteMovement.distanceToPlayer - (double) this._minDistance) / ((double) this._maxDistance - (double) this._minDistance))));

  private void HandleCubeNoteControllerDidInit(T gameNoteController)
  {
    NoteMovement noteMovement = gameNoteController.noteMovement;
    switch (gameNoteController.noteVisualModifierType)
    {
      case NoteVisualModifierType.DisappearingArrow:
        noteMovement.noteDidMoveInJumpPhaseEvent -= new System.Action(this.HandleNoteMovementNoteDidMoveInJumpPhase);
        noteMovement.noteDidMoveInJumpPhaseEvent += new System.Action(this.HandleNoteMovementNoteDidMoveInJumpPhase);
        this._fadeArrow = true;
        this._hideMesh = false;
        this.CalculateMinMaxDistance();
        break;
      case NoteVisualModifierType.Ghost:
        noteMovement.noteDidMoveInJumpPhaseEvent -= new System.Action(this.HandleNoteMovementNoteDidMoveInJumpPhase);
        noteMovement.noteDidMoveInJumpPhaseEvent += new System.Action(this.HandleNoteMovementNoteDidMoveInJumpPhase);
        this._fadeArrow = true;
        this._hideMesh = true;
        this.CalculateMinMaxDistance();
        break;
      default:
        this._fadeArrow = false;
        this._hideMesh = false;
        break;
    }
    this._cubeMeshRenderer.enabled = !this._hideMesh;
    this.SetArrowTransparency(1f);
  }

  private void SetArrowTransparency(float arrowTransparency)
  {
    if ((double) arrowTransparency == (double) this._prevArrowTransparency)
      return;
    this._prevArrowTransparency = arrowTransparency;
    for (int index = 0; index < this._transparentObjectMaterialPropertyBlocks.Length; ++index)
    {
      MaterialPropertyBlockController materialPropertyBlock1 = this._transparentObjectMaterialPropertyBlocks[index];
      MaterialPropertyBlock materialPropertyBlock2 = materialPropertyBlock1.materialPropertyBlock;
      Color color = materialPropertyBlock2.GetColor(DisappearingArrowControllerBase<T>._colorId);
      materialPropertyBlock2.SetColor(DisappearingArrowControllerBase<T>._colorId, color.ColorWithAlpha(arrowTransparency));
      materialPropertyBlock1.ApplyChanges();
    }
    if (!((UnityEngine.Object) this._arrowCutoutEffect != (UnityEngine.Object) null))
      return;
    if (this._arrowCutoutEffect.useRandomCutoutOffset)
      this._arrowCutoutEffect.SetCutout(1f - arrowTransparency);
    else
      this._arrowCutoutEffect.SetCutout(1f - arrowTransparency, Vector3.zero);
  }
}
