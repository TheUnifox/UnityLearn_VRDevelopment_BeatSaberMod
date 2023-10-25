// Decompiled with JetBrains decompiler
// Type: MultiplayerLocalInactivePlayerOutroAnimator
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.Playables;
using Zenject;

public class MultiplayerLocalInactivePlayerOutroAnimator : MonoBehaviour
{
  [SerializeField]
  protected PlayableDirector _outroPlayableDirector;
  [Inject]
  protected readonly MultiplayerController _multiplayerController;

  public virtual void Start()
  {
    if (this._multiplayerController.state != MultiplayerController.State.Outro && this._multiplayerController.state != MultiplayerController.State.Finished)
      return;
    this._outroPlayableDirector.Play();
  }
}
