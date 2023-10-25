// Decompiled with JetBrains decompiler
// Type: ResetPitchOnGameplayFinished
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

public class ResetPitchOnGameplayFinished
{
  protected readonly GameplayLevelSceneTransitionEvents _gameplayLevelSceneTransitionEvents;
  protected readonly AudioManagerSO _audioManager;

  public ResetPitchOnGameplayFinished(
    GameplayLevelSceneTransitionEvents gameplayLevelSceneTransitionEvents,
    AudioManagerSO audioManager)
  {
    this._gameplayLevelSceneTransitionEvents = gameplayLevelSceneTransitionEvents;
    this._audioManager = audioManager;
    this._gameplayLevelSceneTransitionEvents.anyGameplayLevelDidFinishEvent += new System.Action(this.HandleAnyGameplayLevelDidFinish);
  }

  ~ResetPitchOnGameplayFinished() => this._gameplayLevelSceneTransitionEvents.anyGameplayLevelDidFinishEvent -= new System.Action(this.HandleAnyGameplayLevelDidFinish);

  public virtual void HandleAnyGameplayLevelDidFinish() => this._audioManager.musicPitch = 1f;
}
