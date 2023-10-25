// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.AbstractBeatmapObjectView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D
{
  public abstract class AbstractBeatmapObjectView : MonoBehaviour
  {
    [SerializeField]
    protected float _beatmapObjectYOffset;
    [Inject]
    protected readonly BeatmapLevelDataModel beatmapLevelDataModel;
    [Inject]
    protected readonly BeatmapBasicEventsDataModel beatmapBasicEventsDataModel;
    [Inject]
    protected readonly IReadonlyBeatmapObjectsSelectionState beatmapObjectsSelectionState;
    [Inject]
    protected readonly BeatmapObjectPlacementHelper beatmapObjectPlacementHelper;
    [Inject]
    protected readonly IReadonlyBeatmapState beatmapState;
    [Inject]
    protected readonly BeatmapObjectsContainer beatmapObjectsContainer;

    public abstract void RefreshView(float startTime, float endTime, bool clearView = false);

    public abstract void UpdateTimeScale();

    public abstract void ClearView();

    public enum BeatmapObjectEditState
    {
      Default,
      Selected,
      Preview,
    }
  }
}
