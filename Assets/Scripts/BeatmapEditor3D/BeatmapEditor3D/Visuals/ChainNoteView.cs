// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.ChainNoteView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Visuals
{
  public class ChainNoteView : MonoBehaviour
  {
    [SerializeField]
    private Transform _headNoteTransform;
    [SerializeField]
    private ArcHandleView _endHandle;
    [Space]
    [SerializeField]
    private Transform _textTransform;
    [SerializeField]
    private TextMeshPro _text;
    [Space]
    [SerializeField]
    private float _arrowGlowIntensity = 0.6f;
    [Space]
    [SerializeField]
    private MaterialPropertyBlockController _arrowMaterialPropertyBlockController;
    [SerializeField]
    private MaterialPropertyBlockController[] _materialPropertyBlockControllers;
    [Header("Mouse Inputs")]
    [SerializeField]
    private BeatmapObjectMouseInputProvider _headMouseInputProvider;
    [SerializeField]
    private BeatmapObjectMouseInputProvider _tailMouseInputProvider;
    [Inject]
    private readonly ChainElementNoteView.Pool _chainElementNoteViewPool;
    private bool _headOnBeat;
    private bool _headPastBeat;
    private bool _tailOnBeat;
    private bool _tailPastBeat;
    private bool _selected;
    private Color _color;
    private NoteCutDirection _cutDirection;
    private int _sliceCount;
    private float _squishAmount;
    private readonly List<ChainElementNoteView> _chainElements = new List<ChainElementNoteView>();

    public ChainEditorData chainData { get; private set; }

    public BeatmapObjectMouseInputProvider headMouseInputProvider => this._headMouseInputProvider;

    public BeatmapObjectMouseInputProvider tailMouseInputProvider => this._tailMouseInputProvider;

    public void Init(
      ChainEditorData chainData,
      Color color,
      NoteCutDirection cutDirection,
      int sliceCount,
      float squishAmount,
      Vector3 localStartPosition,
      Vector3 localEndPosition,
      bool showHandle)
    {
      this.chainData = chainData;
      this._color = color;
      this._cutDirection = cutDirection;
      this._sliceCount = sliceCount;
      this._squishAmount = squishAmount;
      this._textTransform.localRotation = Quaternion.AngleAxis(-cutDirection.RotationAngle(), Vector3.forward);
      this._text.text = string.Format("S: {0:F1}\nC: {1}", (object) squishAmount, (object) sliceCount);
      BeatmapObjectMaterialHelpers.SetColorDataToMaterialPropertyBlock(this._arrowMaterialPropertyBlockController, color.ColorWithAlpha(this._arrowGlowIntensity));
      this._headNoteTransform.localRotation = Quaternion.AngleAxis(cutDirection.RotationAngle(), Vector3.forward);
      BeatmapObjectMaterialHelpers.SetColorDataToMaterialPropertyBlocks((IEnumerable<MaterialPropertyBlockController>) this._materialPropertyBlockControllers, color.ColorWithAlpha(1f));
      this._endHandle.Init(new Vector3(localEndPosition.x - localStartPosition.x, localEndPosition.y - localStartPosition.y, localEndPosition.z), 0.0f, 0.0f, color, showHandle, true, false);
      this.CreateSlider(localStartPosition, localEndPosition);
    }

    public void SetLength(Vector3 localStartPosition, Vector3 localEndPosition) => this.CreateSlider(localStartPosition, localEndPosition);

    public void SetState(
      bool headOnBeat,
      bool headPastBeat,
      bool tailOnBeat,
      bool tailPastBeat,
      bool selected)
    {
      if (headOnBeat == this._headOnBeat && headPastBeat == this._headPastBeat && tailOnBeat == this._tailOnBeat && tailPastBeat == this._tailPastBeat)
        return;
      this._headOnBeat = headOnBeat;
      this._headPastBeat = headPastBeat;
      this._tailOnBeat = tailOnBeat;
      this._tailPastBeat = tailPastBeat;
      BeatmapObjectMaterialHelpers.SetBeatDataToMaterialPropertyBlocks((IEnumerable<MaterialPropertyBlockController>) this._materialPropertyBlockControllers, headOnBeat, headPastBeat);
      this._endHandle.SetState(tailOnBeat, tailPastBeat);
      foreach (ChainElementNoteView chainElement in this._chainElements)
        chainElement.SetState(this._headPastBeat);
    }

    public void SetHighlight(bool highlighted)
    {
    }

    private void CreateSlider(Vector3 localStartPosition, Vector3 localEndPosition)
    {
      Vector2 p2 = new Vector2(localEndPosition.x - localStartPosition.x, localEndPosition.y - localStartPosition.y);
      float magnitude = p2.magnitude;
      float f = (float) (((double) this._cutDirection.RotationAngle() - 90.0) * (Math.PI / 180.0));
      Vector2 p1 = 0.5f * magnitude * new Vector2(Mathf.Cos(f), Mathf.Sin(f));
      int linkId;
      for (linkId = 1; linkId < this._sliceCount; ++linkId)
      {
        float t = (float) linkId / (float) (this._sliceCount - 1);
        Vector2 pos;
        Vector2 tangent;
        BurstSliderSpawner.BezierCurve(new Vector2(0.0f, 0.0f), p1, p2, t * this._squishAmount, out pos, out tangent);
        if (linkId - 1 >= this._chainElements.Count)
          this._chainElements.Add(this._chainElementNoteViewPool.Spawn());
        ChainElementNoteView chainElement = this._chainElements[linkId - 1];
        chainElement.transform.SetParent(this.transform, false);
        chainElement.Init(linkId, this._color, Vector2.SignedAngle(Vector2.down, tangent), new Vector3(pos.x, pos.y, Mathf.Lerp(localStartPosition.z, localEndPosition.z, t)));
        chainElement.SetState(this._headPastBeat);
      }
      for (int index = this._chainElements.Count - 1; index >= linkId - 1; --index)
      {
        this._chainElementNoteViewPool.Despawn(this._chainElements[index]);
        this._chainElements.RemoveAt(index);
      }
    }

    private void ClearChainElements()
    {
      foreach (ChainElementNoteView chainElement in this._chainElements)
        this._chainElementNoteViewPool.Despawn(chainElement);
      this._chainElements.Clear();
    }

    public class Pool : MonoMemoryPool<ChainNoteView>
    {
      protected override void OnDespawned(ChainNoteView item)
      {
        base.OnDespawned(item);
        item.ClearChainElements();
      }
    }
  }
}
