// Decompiled with JetBrains decompiler
// Type: MissionLevelDetailViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using Polyglot;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionLevelDetailViewController : ViewController
{
  [SerializeField]
  protected GameplayModifiersModelSO _gameplayModifiersModel;
  [Space]
  [SerializeField]
  protected Button _playButton;
  [SerializeField]
  protected LevelBar _levelBar;
  [SerializeField]
  protected ObjectiveListItemsList _objectiveListItems;
  [SerializeField]
  protected GameplayModifierInfoListItemsList _gameplayModifierInfoListItemsList;
  [SerializeField]
  protected GameObject _modifiersPanelGO;
  protected MissionNode _missionNode;

  public event System.Action<MissionLevelDetailViewController> didPressPlayButtonEvent;

  public MissionNode missionNode => this._missionNode;

  public virtual void Setup(MissionNode missionNode)
  {
    this._missionNode = missionNode;
    if (!this.isInViewControllerHierarchy)
      return;
    this.RefreshContent();
  }

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (firstActivation)
      this.buttonBinder.AddBinding(this._playButton, new System.Action(this.PlayButtonPressed));
    this.RefreshContent();
  }

  public virtual void RefreshContent()
  {
    MissionDataSO missionData = this._missionNode.missionData;
    this._levelBar.Setup((IPreviewBeatmapLevel) missionData.level, missionData.beatmapCharacteristic, missionData.beatmapDifficulty);
    MissionObjective[] missionObjectives = missionData.missionObjectives;
    this._objectiveListItems.SetData(missionObjectives.Length == 0 ? 1 : missionObjectives.Length, (UIItemsList<ObjectiveListItem>.DataCallback) ((idx, objectiveListItem) =>
    {
      if (idx == 0 && missionObjectives.Length == 0)
      {
        objectiveListItem.title = Localization.Get("CAMPAIGN_FINISH_LEVEL");
        objectiveListItem.conditionText = "";
        objectiveListItem.hideCondition = true;
      }
      else
      {
        MissionObjective missionObjective = missionObjectives[idx];
        if (missionObjective.type.noConditionValue)
        {
          objectiveListItem.title = missionObjective.type.objectiveNameLocalized.Replace(" ", "\n");
          objectiveListItem.hideCondition = true;
        }
        else
        {
          objectiveListItem.title = missionObjective.type.objectiveNameLocalized;
          objectiveListItem.hideCondition = false;
          ObjectiveValueFormatterSO objectiveValueFormater = missionObjective.type.objectiveValueFormater;
          objectiveListItem.conditionText = string.Format("{0} {1}", (object) missionObjective.referenceValueComparisonType.Name(), (object) objectiveValueFormater.FormatValue(missionObjective.referenceValue));
        }
      }
    }));
    List<GameplayModifierParamsSO> modifierParamsList = this._gameplayModifiersModel.CreateModifierParamsList(missionData.gameplayModifiers);
    this._modifiersPanelGO.SetActive(modifierParamsList.Count > 0);
    this._gameplayModifierInfoListItemsList.SetData(modifierParamsList.Count, (UIItemsList<GameplayModifierInfoListItem>.DataCallback) ((idx, gameplayModifierInfoListItem) => gameplayModifierInfoListItem.SetModifier(modifierParamsList[idx], true)));
  }

  public virtual void PlayButtonPressed()
  {
    System.Action<MissionLevelDetailViewController> pressPlayButtonEvent = this.didPressPlayButtonEvent;
    if (pressPlayButtonEvent == null)
      return;
    pressPlayButtonEvent(this);
  }
}
