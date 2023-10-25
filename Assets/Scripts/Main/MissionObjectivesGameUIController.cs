// Decompiled with JetBrains decompiler
// Type: MissionObjectivesGameUIController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MissionObjectivesGameUIController : MonoBehaviour
{
  [SerializeField]
  protected MissionObjectiveGameUIView _missionObjectiveGameUIViewPrefab;
  [SerializeField]
  protected float _separator = 1f;
  [SerializeField]
  protected float _elementWidth = 1f;
  [Inject]
  protected MissionObjectiveCheckersManager _missionObjectiveCheckersManager;
  protected List<MissionObjectiveGameUIView> _missionObjectiveGameUIViews;

  public virtual void Start()
  {
    this._missionObjectiveCheckersManager.objectivesListDidChangeEvent += new System.Action(this.HandleMissionObjectiveCheckersManagerObjectivesListDidChange);
    this.CreateUIElements();
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._missionObjectiveCheckersManager != (UnityEngine.Object) null))
      return;
    this._missionObjectiveCheckersManager.objectivesListDidChangeEvent -= new System.Action(this.HandleMissionObjectiveCheckersManagerObjectivesListDidChange);
  }

  public virtual void HandleMissionObjectiveCheckersManagerObjectivesListDidChange() => this.CreateUIElements();

  public virtual void CreateUIElements()
  {
    if (this._missionObjectiveGameUIViews != null)
    {
      foreach (Component objectiveGameUiView in this._missionObjectiveGameUIViews)
        UnityEngine.Object.Destroy((UnityEngine.Object) objectiveGameUiView.gameObject);
    }
    MissionObjectiveChecker[] objectiveCheckers = this._missionObjectiveCheckersManager.activeMissionObjectiveCheckers;
    this._missionObjectiveGameUIViews = new List<MissionObjectiveGameUIView>(objectiveCheckers.Length);
    int length = objectiveCheckers.Length;
    float x = (float) (-((double) this._elementWidth * (double) length + (double) this._separator * (double) (length - 1)) * 0.5 + (double) this._elementWidth * 0.5);
    for (int index = 0; index < objectiveCheckers.Length; ++index)
    {
      MissionObjectiveChecker objectiveChecker = this._missionObjectiveCheckersManager.activeMissionObjectiveCheckers[index];
      MissionObjectiveGameUIView objectiveGameUiView = (MissionObjectiveGameUIView) UnityEngine.Object.Instantiate((UnityEngine.Object) this._missionObjectiveGameUIViewPrefab, this.transform, false);
      objectiveGameUiView.transform.localPosition = (Vector3) new Vector2(x, 0.0f);
      objectiveGameUiView.SetMissionObjectiveChecker(objectiveChecker);
      this._missionObjectiveGameUIViews.Add(objectiveGameUiView);
      x += this._separator + this._elementWidth;
    }
  }
}
