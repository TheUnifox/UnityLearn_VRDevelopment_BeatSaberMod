// Decompiled with JetBrains decompiler
// Type: IMultiplayerSpectatingSpot
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public interface IMultiplayerSpectatingSpot
{
  event System.Action<IMultiplayerSpectatingSpot> hasBeenRemovedEvent;

  bool isMain { get; }

  IMultiplayerObservable observable { get; }

  Transform transform { get; }

  string spotName { get; }

  void SetIsObserved(bool isObserved);
}
