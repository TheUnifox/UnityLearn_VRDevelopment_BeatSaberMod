// Decompiled with JetBrains decompiler
// Type: StandardLevelRestartController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class StandardLevelRestartController : MonoBehaviour, ILevelRestartController
{
  [SerializeField]
  protected StandardLevelScenesTransitionSetupDataSO _standardLevelSceneSetupData;
  [Inject]
  protected readonly PrepareLevelCompletionResults _prepareLevelCompletionResults;

  public virtual void RestartLevel() => this._standardLevelSceneSetupData.Finish(this._prepareLevelCompletionResults.FillLevelCompletionResults(LevelCompletionResults.LevelEndStateType.Incomplete, LevelCompletionResults.LevelEndAction.Restart));
}
