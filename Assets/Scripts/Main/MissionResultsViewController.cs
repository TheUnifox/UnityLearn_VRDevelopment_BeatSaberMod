// Decompiled with JetBrains decompiler
// Type: MissionResultsViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MissionResultsViewController : ViewController
{
  [SerializeField]
  protected GameObject _failedBannerGo;
  [SerializeField]
  protected GameObject _clearedBannerGo;
  [SerializeField]
  protected TextMeshProUGUI _missionNameText;
  [SerializeField]
  protected TextMeshProUGUI _songNameText;
  [SerializeField]
  protected Sprite _successIcon;
  [SerializeField]
  protected Sprite _successIconGlow;
  [SerializeField]
  protected Color _successColor;
  [SerializeField]
  protected Sprite _failIcon;
  [SerializeField]
  protected Sprite _failIconGlow;
  [SerializeField]
  protected Color _failColor;
  [SerializeField]
  protected ResultObjectiveListItemsList _resultObjectiveListItemList;
  [SerializeField]
  protected Button _continueButton;
  [SerializeField]
  protected Button _retryButton;
  [Space]
  [SerializeField]
  protected AudioClip _levelClearedAudioClip;
  [Inject]
  protected readonly FireworksController _fireworksController;
  [Inject]
  protected readonly SongPreviewPlayer _songPreviewPlayer;
  protected MissionNode _missionNode;
  protected MissionCompletionResults _missionCompletionResults;
  protected Coroutine _startFireworksAfterDelayCoroutine;

  public event System.Action<MissionResultsViewController> continueButtonPressedEvent;

  public event System.Action<MissionResultsViewController> retryButtonPressedEvent;

  public virtual void Init(
    MissionNode missionNode,
    MissionCompletionResults missionCompletionResults)
  {
    this._missionNode = missionNode;
    this._missionCompletionResults = missionCompletionResults;
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
    {
      this.buttonBinder.AddBinding(this._continueButton, new System.Action(this.ContinueButtonPressed));
      this.buttonBinder.AddBinding(this._retryButton, new System.Action(this.RetryButtonPressed));
    }
    if (!addedToHierarchy)
      return;
    this.SetDataToUI();
    if (!this._missionCompletionResults.IsMissionComplete)
      return;
    this._startFireworksAfterDelayCoroutine = this.StartCoroutine(this.StartFireworksAfterDelay(1.95f));
    this._songPreviewPlayer.CrossfadeTo(this._levelClearedAudioClip, -4f, 0.0f, this._levelClearedAudioClip.length, (System.Action) null);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (this._startFireworksAfterDelayCoroutine != null)
    {
      this.StopCoroutine(this._startFireworksAfterDelayCoroutine);
      this._startFireworksAfterDelayCoroutine = (Coroutine) null;
    }
    this._fireworksController.enabled = false;
  }

  public virtual IEnumerator StartFireworksAfterDelay(float delay)
  {
    yield return (object) new WaitForSeconds(delay);
    this._fireworksController.enabled = true;
  }

  public virtual void SetDataToUI()
  {
    bool levelCleared = this._missionCompletionResults.levelCompletionResults.levelEndStateType == LevelCompletionResults.LevelEndStateType.Cleared;
    MissionObjectiveResult[] missionObjectiveResults = this._missionCompletionResults.missionObjectiveResults;
    MissionObjective[] missionObjectives = this._missionNode.missionData.missionObjectives;
    this._resultObjectiveListItemList.SetData(missionObjectives.Length + 1, (UIItemsList<ResultObjectiveListItem>.DataCallback) ((idx, objectiveListItem) =>
    {
      if (idx == 0)
      {
        objectiveListItem.title = Localization.Get("CAMPAIGN_FINISH_LEVEL");
        objectiveListItem.conditionText = "";
        objectiveListItem.valueText = "";
        objectiveListItem.icon = levelCleared ? this._successIcon : this._failIcon;
        objectiveListItem.iconGlow = levelCleared ? this._successIconGlow : this._failIconGlow;
        objectiveListItem.iconColor = levelCleared ? this._successColor : this._failColor;
        objectiveListItem.hideValueText = true;
        objectiveListItem.hideConditionText = true;
      }
      else
      {
        MissionObjective missionObjective = missionObjectives[idx - 1];
        MissionObjectiveResult missionObjectiveResult1 = (MissionObjectiveResult) null;
        foreach (MissionObjectiveResult missionObjectiveResult2 in missionObjectiveResults)
        {
          if (missionObjectiveResult2.missionObjective == missionObjective)
          {
            missionObjectiveResult1 = missionObjectiveResult2;
            break;
          }
        }
        ObjectiveValueFormatterSO objectiveValueFormater = missionObjective.type.objectiveValueFormater;
        if (missionObjective.type.noConditionValue)
        {
          objectiveListItem.title = missionObjective.type.objectiveNameLocalized.Replace(" ", "\n");
          objectiveListItem.hideValueText = true;
          objectiveListItem.hideConditionText = true;
        }
        else
        {
          objectiveListItem.hideValueText = false;
          objectiveListItem.hideConditionText = false;
          objectiveListItem.title = missionObjective.type.objectiveNameLocalized;
          objectiveListItem.conditionText = string.Format("{0} {1}", (object) missionObjective.referenceValueComparisonType.Name(), (object) objectiveValueFormater.FormatValue(missionObjective.referenceValue));
        }
        objectiveListItem.valueText = objectiveValueFormater.FormatValue(missionObjectiveResult1.value);
        objectiveListItem.icon = missionObjectiveResult1.cleared ? this._successIcon : this._failIcon;
        objectiveListItem.iconGlow = missionObjectiveResult1.cleared ? this._successIconGlow : this._failIconGlow;
        objectiveListItem.iconColor = missionObjectiveResult1.cleared ? this._successColor : this._failColor;
      }
    }));
    this._missionNameText.text = Localization.Get("CAMPAIGN_MISSION") + " " + this._missionNode.formattedMissionNodeName;
    this._songNameText.text = this._missionNode.missionData.level.songName;
    if (this._missionCompletionResults.IsMissionComplete)
    {
      this._clearedBannerGo.SetActive(true);
      this._failedBannerGo.SetActive(false);
      this._retryButton.gameObject.SetActive(false);
    }
    else
    {
      this._clearedBannerGo.SetActive(false);
      this._failedBannerGo.SetActive(true);
      this._retryButton.gameObject.SetActive(true);
    }
  }

  public virtual void ContinueButtonPressed()
  {
    System.Action<MissionResultsViewController> buttonPressedEvent = this.continueButtonPressedEvent;
    if (buttonPressedEvent == null)
      return;
    buttonPressedEvent(this);
  }

  public virtual void RetryButtonPressed()
  {
    System.Action<MissionResultsViewController> buttonPressedEvent = this.retryButtonPressedEvent;
    if (buttonPressedEvent == null)
      return;
    buttonPressedEvent(this);
  }
}
