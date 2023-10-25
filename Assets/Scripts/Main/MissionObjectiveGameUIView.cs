// Decompiled with JetBrains decompiler
// Type: MissionObjectiveGameUIView
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionObjectiveGameUIView : MonoBehaviour
{
  [SerializeField]
  protected Sprite _notFailedIcon;
  [SerializeField]
  protected Sprite _failedIcon;
  [SerializeField]
  protected Sprite _notClearedIcon;
  [SerializeField]
  protected Sprite _clearedIcon;
  [SerializeField]
  protected Image _resultIcon;
  [SerializeField]
  protected Color _finalClearIconColor;
  [SerializeField]
  protected Color _finalFailIconColor;
  [SerializeField]
  protected Color _nonFinalIconColor;
  [SerializeField]
  protected ParticleSystem _clearedPS;
  [SerializeField]
  protected int _numberOfParticles = 70;
  [Space]
  [SerializeField]
  protected TextMeshProUGUI _nameText;
  [SerializeField]
  protected TextMeshProUGUI _valueText;
  [SerializeField]
  protected TextMeshProUGUI _conditionText;
  protected MissionObjectiveChecker _missionObjectiveChecker;

  public virtual void SetMissionObjectiveChecker(MissionObjectiveChecker missionObjectiveChecker)
  {
    this._missionObjectiveChecker = missionObjectiveChecker;
    this._missionObjectiveChecker.statusDidChangeEvent += new System.Action<MissionObjectiveChecker>(this.HandleMissionObjectiveStatusDidChange);
    this._missionObjectiveChecker.checkedValueDidChangeEvent += new System.Action<MissionObjectiveChecker>(this.HandleMissionObjectiveCheckedValueDidChange);
    MissionObjective missionObjective = missionObjectiveChecker.missionObjective;
    if (missionObjective.type.noConditionValue)
    {
      this._conditionText.gameObject.SetActive(false);
      this._valueText.gameObject.SetActive(false);
      this._nameText.text = missionObjective.type.objectiveNameLocalized.Replace(" ", "\n");
    }
    else
    {
      this._conditionText.gameObject.SetActive(true);
      this._valueText.gameObject.SetActive(true);
      this._nameText.text = missionObjective.type.objectiveNameLocalized;
      this._conditionText.text = string.Format("{0} {1}", (object) missionObjective.referenceValueComparisonType.Name(), (object) missionObjective.type.objectiveValueFormater.FormatValue(missionObjective.referenceValue));
    }
    this.RefreshIcon();
    this.RefreshValue();
  }

  public virtual void HandleMissionObjectiveStatusDidChange(
    MissionObjectiveChecker missionObjectiveChecker)
  {
    this.RefreshIcon();
  }

  public virtual void HandleMissionObjectiveCheckedValueDidChange(
    MissionObjectiveChecker missionObjectiveChecker)
  {
    this.RefreshValue();
  }

  public virtual void RefreshIcon()
  {
    switch (this._missionObjectiveChecker.status)
    {
      case MissionObjectiveChecker.Status.NotClearedYet:
        this._resultIcon.sprite = this._notClearedIcon;
        this._resultIcon.color = this._nonFinalIconColor;
        break;
      case MissionObjectiveChecker.Status.NotFailedYet:
        this._resultIcon.sprite = this._notFailedIcon;
        this._resultIcon.color = this._nonFinalIconColor;
        break;
      case MissionObjectiveChecker.Status.Cleared:
        this._resultIcon.sprite = this._clearedIcon;
        this._resultIcon.color = this._finalClearIconColor;
        this._clearedPS.Emit(this._numberOfParticles);
        break;
      case MissionObjectiveChecker.Status.Failed:
        this._resultIcon.sprite = this._failedIcon;
        this._resultIcon.color = this._finalFailIconColor;
        break;
    }
  }

  public virtual void RefreshValue() => this._valueText.text = this._missionObjectiveChecker.missionObjectiveType.objectiveValueFormater.FormatValue(this._missionObjectiveChecker.checkedValue);
}
