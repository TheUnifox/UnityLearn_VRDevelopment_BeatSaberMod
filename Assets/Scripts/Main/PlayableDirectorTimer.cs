// Decompiled with JetBrains decompiler
// Type: PlayableDirectorTimer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using UnityEngine.Playables;
using Zenject;

public class PlayableDirectorTimer : MonoBehaviour
{
  [SerializeField]
  protected PlayableDirector _playableDirector;
  [Inject]
  protected AudioTimeSyncController _audioTimeSyncController;

  public virtual void Update()
  {
    this._playableDirector.time = (double) this._audioTimeSyncController.songTime;
    this._playableDirector.Evaluate();
  }
}
