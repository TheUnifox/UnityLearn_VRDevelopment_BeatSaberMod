// Decompiled with JetBrains decompiler
// Type: ColorNoteVisuals
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class ColorNoteVisuals : 
  MonoBehaviour,
  INoteControllerDidInitEvent,
  INoteControllerNoteDidPassJumpThreeQuartersEvent,
  INoteControllerNoteDidStartDissolvingEvent
{
  [SerializeField]
  protected float _defaultColorAlpha = 1f;
  [Space]
  [SerializeField]
  protected NoteControllerBase _noteController;
  [Space]
  [SerializeField]
  protected MaterialPropertyBlockController[] _materialPropertyBlockControllers;
  [SerializeField]
  protected MeshRenderer[] _arrowMeshRenderers;
  [SerializeField]
  protected MeshRenderer[] _circleMeshRenderers;
  [Inject]
  protected readonly ColorManager _colorManager;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _colorId = Shader.PropertyToID("_Color");
  protected Color _noteColor;

  public event System.Action<ColorNoteVisuals, NoteControllerBase> didInitEvent;

  private bool showArrow
  {
    set
    {
      foreach (Renderer arrowMeshRenderer in this._arrowMeshRenderers)
        arrowMeshRenderer.enabled = value;
    }
  }

  private bool showCircle
  {
    set
    {
      foreach (Renderer circleMeshRenderer in this._circleMeshRenderers)
        circleMeshRenderer.enabled = value;
    }
  }

  public virtual void Awake()
  {
    this._noteController.didInitEvent.Add((INoteControllerDidInitEvent) this);
    this._noteController.noteDidPassJumpThreeQuartersEvent.Add((INoteControllerNoteDidPassJumpThreeQuartersEvent) this);
    this._noteController.noteDidStartDissolvingEvent.Add((INoteControllerNoteDidStartDissolvingEvent) this);
  }

  public virtual void OnDestroy()
  {
    if (!(bool) (UnityEngine.Object) this._noteController)
      return;
    this._noteController.didInitEvent.Remove((INoteControllerDidInitEvent) this);
    this._noteController.noteDidPassJumpThreeQuartersEvent.Remove((INoteControllerNoteDidPassJumpThreeQuartersEvent) this);
    this._noteController.noteDidStartDissolvingEvent.Remove((INoteControllerNoteDidStartDissolvingEvent) this);
  }

  public virtual void HandleNoteControllerDidInit(NoteControllerBase noteController)
  {
    NoteData noteData = this._noteController.noteData;
    if (noteData.cutDirection == NoteCutDirection.Any)
    {
      this.showArrow = false;
      this.showCircle = true;
    }
    else
    {
      this.showArrow = true;
      this.showCircle = false;
    }
    this._noteColor = this._colorManager.ColorForType(noteData.colorType);
    foreach (MaterialPropertyBlockController propertyBlockController in this._materialPropertyBlockControllers)
    {
      propertyBlockController.materialPropertyBlock.SetColor(ColorNoteVisuals._colorId, this._noteColor.ColorWithAlpha(this._defaultColorAlpha));
      propertyBlockController.ApplyChanges();
    }
    System.Action<ColorNoteVisuals, NoteControllerBase> didInitEvent = this.didInitEvent;
    if (didInitEvent == null)
      return;
    didInitEvent(this, this._noteController);
  }

  public virtual void HandleNoteControllerNoteDidPassJumpThreeQuarters(
    NoteControllerBase noteController)
  {
    this.showArrow = false;
    this.showCircle = false;
  }

  public virtual void HandleNoteControllerNoteDidStartDissolving(
    NoteControllerBase noteController,
    float duration)
  {
    this.showArrow = false;
    this.showCircle = false;
  }
}
