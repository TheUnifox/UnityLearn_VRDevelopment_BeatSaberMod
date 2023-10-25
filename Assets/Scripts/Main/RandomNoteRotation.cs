// Decompiled with JetBrains decompiler
// Type: RandomNoteRotation
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class RandomNoteRotation : MonoBehaviour, INoteControllerDidInitEvent
{
  [SerializeField]
  protected NoteControllerBase _noteController;
  [SerializeField]
  protected Transform _transform;

  public virtual void Awake() => this._noteController.didInitEvent.Add((INoteControllerDidInitEvent) this);

  public virtual void OnDestroy()
  {
    if (!(bool) (Object) this._noteController)
      return;
    this._noteController.didInitEvent.Remove((INoteControllerDidInitEvent) this);
  }

  public virtual void HandleNoteControllerDidInit(NoteControllerBase noteController) => this._transform.rotation = Random.rotation;
}
