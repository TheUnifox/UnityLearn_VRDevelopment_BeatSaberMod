// Decompiled with JetBrains decompiler
// Type: NoteBigCuttableColliderSize
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class NoteBigCuttableColliderSize : MonoBehaviour, INoteControllerDidInitEvent
{
  [SerializeField]
  protected NoteController _noteController;
  [SerializeField]
  protected BoxCollider _boxCollider;
  protected Vector3 _defaultColliderSize;

  public virtual void Awake()
  {
    this._defaultColliderSize = this._boxCollider.size;
    this._noteController.didInitEvent.Add((INoteControllerDidInitEvent) this);
  }

  public virtual void OnDestroy()
  {
    if (!((Object) this._noteController != (Object) null))
      return;
    this._noteController.didInitEvent.Remove((INoteControllerDidInitEvent) this);
  }

  public virtual void HandleNoteControllerDidInit(NoteControllerBase noteController)
  {
    if (noteController.noteData.cutDirection == NoteCutDirection.Any)
    {
      Vector3 size = this._boxCollider.size;
      float num = Mathf.Max(size.x, size.y);
      size.x = num;
      size.y = num;
      this._boxCollider.size = size;
    }
    else
      this._boxCollider.size = this._defaultColliderSize;
  }
}
