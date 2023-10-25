// Decompiled with JetBrains decompiler
// Type: WaypointsTestMenuViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WaypointsTestMenuViewController : MonoBehaviour
{
  [SerializeField]
  protected Button _btsButton;
  [SerializeField]
  protected Button _cancelButton;
  [SerializeField]
  protected TextMeshProUGUI _progressText;
  [Space]
  [SerializeField]
  protected StandardLevelScenesTransitionSetupDataSO _waypointsTestScenesTransitionSetupData;
  [SerializeField]
  protected List<BeatmapLevelSO> _levels;
  [SerializeField]
  protected List<BeatmapCharacteristicSO> _characteristics;
  [Inject]
  protected readonly GameScenesManager _gameScenesManager;
  [Inject]
  protected readonly PlayerDataModel _playerDataModel;
  protected ButtonBinder _buttonBinder;
  protected bool _isCancelled;
  protected bool _waitingForLevelFinish;

  public virtual void Start()
  {
    this._buttonBinder = new ButtonBinder();
    this._progressText.text = string.Empty;
    this._buttonBinder.AddBinding(this._btsButton, (System.Action) (() => PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(this.CheckBeatmaps(this._levels))));
    this._buttonBinder.AddBinding(this._cancelButton, (System.Action) (() =>
    {
      this._btsButton.interactable = true;
      this._cancelButton.interactable = false;
      this._isCancelled = true;
    }));
  }

  public virtual void OnDestroy()
  {
    this._buttonBinder.ClearBindings();
    if (!((UnityEngine.Object) this._waypointsTestScenesTransitionSetupData != (UnityEngine.Object) null))
      return;
    this._waypointsTestScenesTransitionSetupData.didFinishEvent -= new System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(this.HandleMainGameSceneDidFinish);
  }

  public virtual IEnumerator CheckBeatmaps(List<BeatmapLevelSO> levels)
  {
    WaypointsTestMenuViewController menuViewController = this;
    menuViewController._isCancelled = false;
    menuViewController._cancelButton.interactable = true;
    menuViewController._btsButton.interactable = false;
    int count = 0;
    List<BeatmapDifficulty> difficultiesToCheck = new List<BeatmapDifficulty>()
    {
      BeatmapDifficulty.Easy,
      BeatmapDifficulty.Normal,
      BeatmapDifficulty.Hard,
      BeatmapDifficulty.Expert,
      BeatmapDifficulty.ExpertPlus
    };
    foreach (BeatmapLevelSO level in levels)
    {
      foreach (BeatmapCharacteristicSO characteristic in menuViewController._characteristics)
      {
        List<BeatmapDifficulty>.Enumerator enumerator = difficultiesToCheck.GetEnumerator();
        try
        {
          do
          {
            if (enumerator.MoveNext())
            {
              BeatmapDifficulty difficulty = enumerator.Current;
              menuViewController._waitingForLevelFinish = true;
              IDifficultyBeatmap difficultyBeatmap = level.beatmapLevelData.GetDifficultyBeatmap(characteristic, difficulty);
              menuViewController._waypointsTestScenesTransitionSetupData.didFinishEvent -= new System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(menuViewController.HandleMainGameSceneDidFinish);
              menuViewController._waypointsTestScenesTransitionSetupData.didFinishEvent += new System.Action<StandardLevelScenesTransitionSetupDataSO, LevelCompletionResults>(menuViewController.HandleMainGameSceneDidFinish);
              menuViewController._waypointsTestScenesTransitionSetupData.Init("WaypointsTest", difficultyBeatmap, (IPreviewBeatmapLevel) level, menuViewController._playerDataModel.playerData.overrideEnvironmentSettings, (ColorScheme) null, GameplayModifiers.noModifiers.CopyWith(noFailOn0Energy: new bool?(true)), menuViewController._playerDataModel.playerData.playerSpecificSettings, (PracticeSettings) null, Localization.Get("BUTTON_MENU"));
              menuViewController._gameScenesManager.PushScenes((ScenesTransitionSetupDataSO) menuViewController._waypointsTestScenesTransitionSetupData);
              while (menuViewController._waitingForLevelFinish)
                yield return (object) null;
              menuViewController._progressText.text = string.Format("Finished: {0}/{1} levels, {2}", (object) count, (object) levels.Count, (object) difficulty);
              yield return (object) new WaitForSeconds(5f);
            }
            else
              goto label_12;
          }
          while (!menuViewController._isCancelled);
        }
        finally
        {
          enumerator.Dispose();
        }
        yield break;
label_12:
        enumerator = new List<BeatmapDifficulty>.Enumerator();
        ++count;
      }
    }
    menuViewController._progressText.text = "Completed, see log for issues";
  }

  public virtual void HandleMainGameSceneDidFinish(
    StandardLevelScenesTransitionSetupDataSO data,
    LevelCompletionResults results)
  {
    this._gameScenesManager.PopScenes(finishCallback: (System.Action<DiContainer>) (container => this._waitingForLevelFinish = false));
  }

  [CompilerGenerated]
  public virtual void m_CStartm_Eb__11_0() => PersistentSingleton<SharedCoroutineStarter>.instance.StartCoroutine(this.CheckBeatmaps(this._levels));

  [CompilerGenerated]
  public virtual void m_CStartm_Eb__11_1()
  {
    this._btsButton.interactable = true;
    this._cancelButton.interactable = false;
    this._isCancelled = true;
  }

  [CompilerGenerated]
  public virtual void m_CHandleMainGameSceneDidFinishm_Eb__14_0(DiContainer container) => this._waitingForLevelFinish = false;
}
