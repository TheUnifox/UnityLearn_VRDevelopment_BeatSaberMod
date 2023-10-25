// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Views.BeatmapObjectGridHoverView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.Commands.LevelEditor;
using BeatmapEditor3D.DataModels;
using BeatmapEditor3D.LevelEditor;
using BeatmapEditor3D.Types;
using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Views
{
  public class BeatmapObjectGridHoverView : MonoBehaviour
  {
    [SerializeField]
    private NoteBeatmapObjectPreview _noteBeatmapObjectPreview;
    [SerializeField]
    private BombBeatmapObjectPreview _bombBeatmapObjectPreview;
    [SerializeField]
    private ObstacleBeatmapObjectPreview _obstacleBeatmapObjectPreview;
    [SerializeField]
    private ChainBeatmapObjectPreview _chainBeatmapObjectPreview;
    [SerializeField]
    private ArcBeatmapObjectPreview _arcBeatmapObjectPreview;
    [Inject]
    private readonly SignalBus _signalBus;
    [Inject]
    private readonly BeatmapObjectsState _beatmapObjectsState;
    [Inject]
    private readonly BeatmapLevelDataModel _beatmapLevelDataModel;
    [Inject]
    private readonly IReadonlyBeatmapState _beatmapState;
    private AbstractBeatmapObjectPreview _beatmapObjectPreview;
    private bool _previewEnabled;
    private RectInt _currentHoverObjectRect;
    private float _startBeat;

    private void Start()
    {
      this._noteBeatmapObjectPreview.Hide();
      this._bombBeatmapObjectPreview.Hide();
      this._obstacleBeatmapObjectPreview.Hide();
      this._chainBeatmapObjectPreview.Hide();
      this._arcBeatmapObjectPreview.Hide();
      this._signalBus.Subscribe<BeatmapObjectTypeChangedSignal>(new Action(this.HandleBeatmapObjectTypeChangedSignal));
      this._signalBus.Subscribe<InteractionModeChangedSignal>(new Action(this.HandleBeatmapObjectModeChangedSignal));
    }

    private void OnDestroy()
    {
      this._signalBus.TryUnsubscribe<BeatmapObjectTypeChangedSignal>(new Action(this.HandleBeatmapObjectTypeChangedSignal));
      this._signalBus.TryUnsubscribe<InteractionModeChangedSignal>(new Action(this.HandleBeatmapObjectModeChangedSignal));
    }

    public void ShowPreview(BeatmapObjectType type, RectInt rect)
    {
      this._currentHoverObjectRect = rect;
      this._previewEnabled = true;
      this.UpdatePreview(type);
    }

    public void HidePreview()
    {
      this._previewEnabled = false;
      this.UpdatePreview(this._beatmapObjectsState.beatmapObjectType);
    }

    public void SetObstacleData(float startBeat, float obstacleDuration)
    {
      this._startBeat = startBeat;
      this._obstacleBeatmapObjectPreview.SetObstacleData(startBeat, obstacleDuration);
    }

    public void SetSliderData(
      ColorType colorType,
      float startBeat,
      Vector2Int startCoords,
      NoteCutDirection startCutDirection,
      float endBeat,
      Vector2Int endCoords,
      NoteCutDirection endCutDirection)
    {
      this._startBeat = startBeat;
      this._arcBeatmapObjectPreview.SetSliderData(colorType, startBeat, startCoords, startCutDirection, endBeat, endCoords, endCutDirection);
    }

    public void SetChainData(
      ColorType colorType,
      float startBeat,
      Vector2Int startCoords,
      NoteCutDirection cutDirection,
      int sliceCount,
      float endBeat,
      Vector2Int endCoords)
    {
      this._startBeat = startBeat;
      this._chainBeatmapObjectPreview.SetChainData(colorType, startBeat, startCoords, cutDirection, sliceCount, endBeat, endCoords);
    }

    private void UpdatePreview(BeatmapObjectType type)
    {
      if ((UnityEngine.Object) this._beatmapObjectPreview != (UnityEngine.Object) null)
        this._beatmapObjectPreview.Hide();
      this._beatmapObjectPreview = (AbstractBeatmapObjectPreview) null;
      if (!this._previewEnabled)
        return;
      switch (type)
      {
        case BeatmapObjectType.Note:
          this.PreviewNoteObject();
          break;
        case BeatmapObjectType.Bomb:
          this.PreviewBombObject();
          break;
        case BeatmapObjectType.Obstacle:
          this.PreviewObstacleObject();
          break;
        case BeatmapObjectType.Arc:
          this.PreviewArcObject();
          break;
        case BeatmapObjectType.Chain:
          this.PreviewChainObject();
          break;
      }
      if (!((UnityEngine.Object) this._beatmapObjectPreview != (UnityEngine.Object) null))
        return;
      this._beatmapObjectPreview.Show();
    }

    private void HandleBeatmapObjectTypeChangedSignal() => this.UpdatePreview(this._beatmapObjectsState.beatmapObjectType);

    private void HandleBeatmapObjectModeChangedSignal() => this.UpdatePreview(this._beatmapObjectsState.beatmapObjectType);

    private void PreviewNoteObject()
    {
      int x = this._currentHoverObjectRect.position.x;
      int y = this._currentHoverObjectRect.position.y;
      if (this._beatmapState.interactionMode == InteractionMode.Modify || this._beatmapLevelDataModel.AnyBeatmapObjectExists(this._beatmapState.beat, x, y, this._beatmapState.beat, x, y))
        return;
      this._noteBeatmapObjectPreview.Preview(this._currentHoverObjectRect);
      this.SetPreviewedObject((AbstractBeatmapObjectPreview) this._noteBeatmapObjectPreview);
    }

    private void PreviewBombObject()
    {
      int x = this._currentHoverObjectRect.position.x;
      int y = this._currentHoverObjectRect.position.y;
      if (this._beatmapState.interactionMode == InteractionMode.Modify || this._beatmapLevelDataModel.AnyBeatmapObjectExists(this._beatmapState.beat, x, y, this._beatmapState.beat, x, y))
        return;
      this._bombBeatmapObjectPreview.Preview(this._currentHoverObjectRect);
      this.SetPreviewedObject((AbstractBeatmapObjectPreview) this._bombBeatmapObjectPreview);
    }

    private void PreviewObstacleObject()
    {
      int x = this._currentHoverObjectRect.position.x;
      int y = this._currentHoverObjectRect.position.y;
      int endColumn = this._currentHoverObjectRect.position.x + this._currentHoverObjectRect.width;
      int endRow = this._currentHoverObjectRect.position.y + this._currentHoverObjectRect.height;
      if (this._beatmapState.interactionMode == InteractionMode.Modify || this._beatmapLevelDataModel.AnyBeatmapObjectExists(this._beatmapState.beat, x, y, this._startBeat + this._beatmapObjectsState.obstacleDuration, endColumn, endRow))
        return;
      this._obstacleBeatmapObjectPreview.Preview(this._currentHoverObjectRect);
      this.SetPreviewedObject((AbstractBeatmapObjectPreview) this._obstacleBeatmapObjectPreview);
    }

    private void PreviewChainObject()
    {
      int x = this._currentHoverObjectRect.position.x;
      int y = this._currentHoverObjectRect.position.y;
      if (this._beatmapState.interactionMode == InteractionMode.Modify || this._beatmapLevelDataModel.AnyBeatmapObjectExists(this._beatmapState.beat, x, y, this._beatmapState.beat, x, y))
        return;
      this._chainBeatmapObjectPreview.Preview(this._currentHoverObjectRect);
      this.SetPreviewedObject((AbstractBeatmapObjectPreview) this._chainBeatmapObjectPreview);
    }

    private void PreviewArcObject()
    {
      if (this._beatmapState.interactionMode == InteractionMode.Modify)
        return;
      this._arcBeatmapObjectPreview.Preview(this._currentHoverObjectRect);
      this.SetPreviewedObject((AbstractBeatmapObjectPreview) this._arcBeatmapObjectPreview);
    }

    private void SetPreviewedObject(AbstractBeatmapObjectPreview previewObject)
    {
      this._beatmapObjectPreview = previewObject;
      this._beatmapObjectPreview.transform.SetParent(this.transform, false);
    }
  }
}
