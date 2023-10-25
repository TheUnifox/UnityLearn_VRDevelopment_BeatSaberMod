// Decompiled with JetBrains decompiler
// Type: VRsenalScoreLogger
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections;
using UnityEngine;
using Zenject;

public class VRsenalScoreLogger : MonoBehaviour
{
  [Inject]
  protected IScoreController _scoreController;
  [Inject]
  protected IDifficultyBeatmap _difficultyBeatmap;
  [Inject]
  protected ILevelEndActions _levelEndActions;

  public virtual IEnumerator Start()
  {
    VRsenalScoreLogger vrsenalScoreLogger = this;
    vrsenalScoreLogger._levelEndActions.levelFinishedEvent += new System.Action(vrsenalScoreLogger.HandleLevelFinishedEvent);
    Debug.Log((object) string.Format("VRsenalLogger: Level started. Song={0}, Difficulty={1}, Characteristic={2}, Duration={3}", (object) vrsenalScoreLogger._difficultyBeatmap.level.songName, (object) vrsenalScoreLogger._difficultyBeatmap.difficulty, (object) vrsenalScoreLogger._difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.characteristicNameLocalizationKey, (object) vrsenalScoreLogger._difficultyBeatmap.level.beatmapLevelData.audioClip.length));
    yield return (object) null;
    YieldInstruction yieldInstruction = (YieldInstruction) new WaitForSeconds(10f);
    while (true)
    {
      // ISSUE: explicit non-virtual call
      __nonvirtual (vrsenalScoreLogger.LogScore());
      yield return (object) yieldInstruction;
    }
  }

  public virtual void OnDestroy()
  {
    if (this._levelEndActions == null)
      return;
    this._levelEndActions.levelFinishedEvent -= new System.Action(this.HandleLevelFinishedEvent);
  }

  public virtual void HandleLevelFinishedEvent() => this.LogScore();

  public virtual void LogScore()
  {
    if (this._scoreController == null)
      return;
    Debug.Log((object) string.Format("VRsenalLogger: Score={0}", (object) this._scoreController.modifiedScore));
  }
}
