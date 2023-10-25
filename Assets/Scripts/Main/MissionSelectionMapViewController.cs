// Decompiled with JetBrains decompiler
// Type: MissionSelectionMapViewController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using HMUI;
using UnityEngine;
using Zenject;

public class MissionSelectionMapViewController : ViewController
{
  [SerializeField]
  protected ScrollView _mapScrollView;
  [SerializeField]
  protected MissionNodeSelectionManager _missionNodeSelectionManager;
  [SerializeField]
  protected MissionMapAnimationController _missionMapAnimationController;
  [Inject]
  protected readonly SongPreviewPlayer _songPreviewPlayer;
  [Inject]
  protected readonly PerceivedLoudnessPerLevelModel _perceivedLoudnessPerLevelModel;
  [Inject]
  protected readonly AudioClipAsyncLoader _audioClipAsyncLoader;
  protected MissionNode _selectedMissionNode;

  public event System.Action<MissionSelectionMapViewController, MissionNode> didSelectMissionLevelEvent;

  public bool animatedUpdateIsRequired => this._missionMapAnimationController.animatedUpdateIsRequired;

  protected override void DidActivate(
    bool firstActivation,
    bool addedToHierarchy,
    bool screenSystemEnabling)
  {
    if (addedToHierarchy)
    {
      this._missionNodeSelectionManager.didSelectMissionNodeEvent += new System.Action<MissionNodeVisualController>(this.HandleMissionNodeSelectionManagerDidSelectMissionNode);
      this._selectedMissionNode = (MissionNode) null;
    }
    if (firstActivation)
      this._missionMapAnimationController.ScrollToTopMostNotClearedMission();
    if (!((UnityEngine.Object) this._selectedMissionNode != (UnityEngine.Object) null))
      return;
    this._mapScrollView.ScrollToWorldPositionIfOutsideArea(this._selectedMissionNode.transform.position, 0.75f, 0.25f, 0.75f, false);
  }

  protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
  {
    if (!removedFromHierarchy)
      return;
    if ((UnityEngine.Object) this._missionNodeSelectionManager != (UnityEngine.Object) null)
    {
      this._missionNodeSelectionManager.didSelectMissionNodeEvent -= new System.Action<MissionNodeVisualController>(this.HandleMissionNodeSelectionManagerDidSelectMissionNode);
      this._missionNodeSelectionManager.DeselectSelectedNode();
    }
    if (!((UnityEngine.Object) this._songPreviewPlayer != (UnityEngine.Object) null))
      return;
    this._songPreviewPlayer.CrossfadeToDefault();
  }

  public virtual void HandleMissionNodeSelectionManagerDidSelectMissionNode(
    MissionNodeVisualController missionNodeVisualController)
  {
    this._selectedMissionNode = missionNodeVisualController.missionNode;
    this.SongPlayerCrossfadeToLevelAsync((IPreviewBeatmapLevel) this._selectedMissionNode.missionData.level);
    System.Action<MissionSelectionMapViewController, MissionNode> missionLevelEvent = this.didSelectMissionLevelEvent;
    if (missionLevelEvent == null)
      return;
    missionLevelEvent(this, missionNodeVisualController.missionNode);
  }

  public virtual async void SongPlayerCrossfadeToLevelAsync(IPreviewBeatmapLevel level)
  {
    float musicVolume = this._perceivedLoudnessPerLevelModel.GetLoudnessCorrectionByLevelId(level.levelID);
    this._songPreviewPlayer.CrossfadeTo(await this._audioClipAsyncLoader.LoadPreview(level), musicVolume, level.previewStartTime, level.previewDuration, (System.Action) (() => this._audioClipAsyncLoader.UnloadPreview(level)));
  }

  public virtual void ShowMissionClearedAnimation(System.Action finishCallback) => this._missionMapAnimationController.UpdateMissionMapAfterMissionWasCleared(true, finishCallback);

  public virtual void DeselectSelectedNode() => this._missionNodeSelectionManager.DeselectSelectedNode();
}
