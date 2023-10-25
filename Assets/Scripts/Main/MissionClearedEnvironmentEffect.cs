// Decompiled with JetBrains decompiler
// Type: MissionClearedEnvironmentEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MissionClearedEnvironmentEffect : MonoBehaviour
{
  [SerializeField]
  protected MissionObjectiveCheckersManager _missionObjectiveCheckersManager;
  [Inject]
  protected BeatmapCallbacksController _beatmapCallbacksController;

  public virtual void Awake() => this._missionObjectiveCheckersManager.objectiveWasClearedEvent += new System.Action(this.HandleMissionObjectiveCheckersManagerObjectiveWasCleared);

  public virtual void OnDestroy() => this._missionObjectiveCheckersManager.objectiveWasClearedEvent -= new System.Action(this.HandleMissionObjectiveCheckersManagerObjectiveWasCleared);

  public virtual void HandleMissionObjectiveCheckersManagerObjectiveWasCleared() => this._beatmapCallbacksController.TriggerBeatmapEvent((BeatmapEventData) new BasicBeatmapEventData(0.0f, BasicBeatmapEventType.Event8, 0, 1f));
}
