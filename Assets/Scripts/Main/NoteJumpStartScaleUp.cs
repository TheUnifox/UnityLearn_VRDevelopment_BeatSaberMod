// Decompiled with JetBrains decompiler
// Type: NoteJumpStartScaleUp
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class NoteJumpStartScaleUp : MonoBehaviour, INoteControllerDidInitEvent
{
  [SerializeField]
  protected float _fullScaleJumpPart = 0.125f;
  [Space]
  [SerializeField]
  protected Transform _targetTransform;
  [SerializeField]
  protected NoteController _noteController;
  [SerializeField]
  protected NoteJump _noteJump;

  public virtual void Awake()
  {
    this._noteController.didInitEvent.Add((INoteControllerDidInitEvent) this);
    this.UpdateScale(0.0f);
  }

  public virtual void OnDestroy()
  {
    if ((UnityEngine.Object) this._noteController != (UnityEngine.Object) null)
      this._noteController.didInitEvent.Remove((INoteControllerDidInitEvent) this);
    if (!((UnityEngine.Object) this._noteJump != (UnityEngine.Object) null))
      return;
    this._noteJump.noteJumpDidUpdateProgressEvent -= new System.Action<float>(this.HandleNoteJumpDidUpdateProgress);
  }

  public virtual void UpdateScale(float progress)
  {
    if ((double) progress >= (double) this._fullScaleJumpPart)
    {
      this._targetTransform.localScale = Vector3.one * this._noteController.uniformScale;
      this._noteJump.noteJumpDidUpdateProgressEvent -= new System.Action<float>(this.HandleNoteJumpDidUpdateProgress);
    }
    else
    {
      float num = Easing.OutQuad(Mathf.Clamp01(progress / this._fullScaleJumpPart));
      this._targetTransform.localScale = new Vector3(num, num, num) * this._noteController.uniformScale;
    }
  }

  public virtual void HandleNoteJumpDidUpdateProgress(float progress) => this.UpdateScale(progress);

  public virtual void HandleNoteControllerDidInit(NoteControllerBase noteController)
  {
    this.UpdateScale(0.0f);
    this._noteJump.noteJumpDidUpdateProgressEvent -= new System.Action<float>(this.HandleNoteJumpDidUpdateProgress);
    this._noteJump.noteJumpDidUpdateProgressEvent += new System.Action<float>(this.HandleNoteJumpDidUpdateProgress);
  }
}
