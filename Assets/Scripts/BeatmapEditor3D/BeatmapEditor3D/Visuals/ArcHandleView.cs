// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.ArcHandleView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace BeatmapEditor3D.Visuals
{
  public class ArcHandleView : MonoBehaviour
  {
    [SerializeField]
    private Transform _transform;
    [SerializeField]
    private Transform _handleTransform;
    [SerializeField]
    private GameObject _moveHandleGo;
    [SerializeField]
    private LineRenderer _lineRenderer;
    [Space]
    [SerializeField]
    private Transform _textTransform;
    [SerializeField]
    private TextMeshPro _text;
    [Space]
    [SerializeField]
    private MaterialPropertyBlockController _handleMaterialPropertyBlockController;
    [SerializeField]
    private MaterialPropertyBlockController _lineAndMoveHandleMaterialPropertyBlockController;
    private const float kZeroLength = 0.5f;
    private const float kBaseLength = 1f;

    public void Init(
      Vector3 localPosition,
      float angle,
      float lengthMultiplier,
      Color color,
      bool showHandle,
      bool showMoveHandle,
      bool showText)
    {
      this._transform.gameObject.SetActive(showHandle);
      this._text.gameObject.SetActive(showText);
      this._text.text = lengthMultiplier.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      this._transform.localPosition = localPosition;
      this._transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
      this._textTransform.localRotation = Quaternion.AngleAxis(-angle, Vector3.forward);
      Vector3 position = Vector3.LerpUnclamped(Vector3.up * 0.5f, Vector3.up * 1f, lengthMultiplier);
      this._handleTransform.localPosition = position;
      this._lineRenderer.SetPosition(1, position);
      this._moveHandleGo.SetActive(showMoveHandle);
      BeatmapObjectMaterialHelpers.SetColorDataToMaterialPropertyBlock(this._handleMaterialPropertyBlockController, color);
      BeatmapObjectMaterialHelpers.SetColorDataToMaterialPropertyBlock(this._lineAndMoveHandleMaterialPropertyBlockController, Color.gray);
    }

    public void SetState(bool onBeat, bool pastBeat)
    {
      BeatmapObjectMaterialHelpers.SetBeatDataToMaterialPropertyBlock(this._handleMaterialPropertyBlockController, onBeat, pastBeat);
      BeatmapObjectMaterialHelpers.SetBeatDataToMaterialPropertyBlock(this._lineAndMoveHandleMaterialPropertyBlockController, onBeat, pastBeat);
    }
  }
}
