// Decompiled with JetBrains decompiler
// Type: BaseNoteVisuals
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class BaseNoteVisuals : 
  MonoBehaviour,
  INoteControllerDidInitEvent,
  INoteControllerNoteDidStartDissolvingEvent
{
  [SerializeField]
  protected NoteControllerBase _noteController;
  [SerializeField]
  protected CutoutAnimateEffect _cutoutAnimateEffect;

  public virtual void Awake()
  {
    this._noteController.didInitEvent.Add((INoteControllerDidInitEvent) this);
    this._noteController.noteDidStartDissolvingEvent.Add((INoteControllerNoteDidStartDissolvingEvent) this);
  }

  public virtual void OnDestroy()
  {
    if (!(bool) (Object) this._noteController)
      return;
    this._noteController.didInitEvent.Remove((INoteControllerDidInitEvent) this);
    this._noteController.noteDidStartDissolvingEvent.Remove((INoteControllerNoteDidStartDissolvingEvent) this);
  }

  public virtual void HandleNoteControllerDidInit(NoteControllerBase noteController) => this._cutoutAnimateEffect.ResetEffect();

  public virtual void HandleNoteControllerNoteDidStartDissolving(
    NoteControllerBase noteController,
    float duration)
  {
    this.AnimateCutout(0.0f, 1f, duration);
  }

  public virtual void AnimateCutout(float cutoutStart, float cutoutEnd, float duration)
  {
    if (this._cutoutAnimateEffect.animating)
      return;
    this._cutoutAnimateEffect.AnimateCutout(cutoutStart, cutoutEnd, duration);
  }
}
