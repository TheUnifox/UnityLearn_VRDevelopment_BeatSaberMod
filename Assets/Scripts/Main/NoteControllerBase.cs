// Decompiled with JetBrains decompiler
// Type: NoteControllerBase
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public abstract class NoteControllerBase : MonoBehaviour
{
  public abstract ILazyCopyHashSet<INoteControllerDidInitEvent> didInitEvent { get; }

  public abstract ILazyCopyHashSet<INoteControllerNoteDidPassJumpThreeQuartersEvent> noteDidPassJumpThreeQuartersEvent { get; }

  public abstract ILazyCopyHashSet<INoteControllerNoteDidStartDissolvingEvent> noteDidStartDissolvingEvent { get; }

  public abstract NoteData noteData { get; }
}
