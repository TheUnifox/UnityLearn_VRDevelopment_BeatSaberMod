// Decompiled with JetBrains decompiler
// Type: FadeOutSongPreviewPlayerOnSceneTransitionStart
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class FadeOutSongPreviewPlayerOnSceneTransitionStart : MonoBehaviour
{
  [SerializeField]
  protected AudioPlayerBase _songPreviewPlayer;
  [Inject]
  protected GameScenesManager _gameScenesManager;

  public virtual void Start() => this._gameScenesManager.transitionDidStartEvent += new System.Action<float>(this.HandleGameScenesManagerTransitionDidStart);

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._gameScenesManager != (UnityEngine.Object) null))
      return;
    this._gameScenesManager.transitionDidStartEvent -= new System.Action<float>(this.HandleGameScenesManagerTransitionDidStart);
  }

  public virtual void HandleGameScenesManagerTransitionDidStart(float duration) => this._songPreviewPlayer.FadeOut(Mathf.Max(0.2f, duration - 0.1f));
}
