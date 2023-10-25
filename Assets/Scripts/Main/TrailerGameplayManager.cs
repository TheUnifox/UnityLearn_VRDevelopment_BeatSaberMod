// Decompiled with JetBrains decompiler
// Type: TrailerGameplayManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class TrailerGameplayManager : MonoBehaviour
{
  [SerializeField]
  protected bool _disableMainCamera;
  [Inject]
  protected GameScenesManager _gameScenesManager;
  [Inject]
  protected GameSongController _gameSongController;
  [Inject]
  protected MainCamera _mainCamera;

  public virtual IEnumerator Start()
  {
    yield return (object) this._gameScenesManager.waitUntilSceneTransitionFinish;
    yield return (object) null;
    yield return (object) this._gameSongController.waitUntilIsReadyToStartTheSong;
    this._mainCamera.enableCamera = !this._disableMainCamera;
    this._gameSongController.StartSong(0.0f);
  }
}
