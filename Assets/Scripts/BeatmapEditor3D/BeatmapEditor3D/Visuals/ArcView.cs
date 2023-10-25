// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.ArcView
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using BeatmapEditor3D.DataModels;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Visuals
{
  public class ArcView : MonoBehaviour
  {
    [SerializeField]
    private ArcHandleView _headPointHandle;
    [SerializeField]
    private ArcHandleView _tailPointHandle;
    [SerializeField]
    private ArcHandleView _midPointHandle;
    [Space]
    [SerializeField]
    private SliderMeshController _arcMeshController;
    [SerializeField]
    private ArcTubePhysicsMeshConstructor _arcTubePhysicsMeshConstructor;
    [SerializeField]
    private MaterialPropertyBlockController _arcMaterialPropertyBlockController;
    [DoesNotRequireDomainReloadInit]
    private static readonly int _startLocalSpacePosPropertyId = Shader.PropertyToID("_StartLocalSpacePos");
    [DoesNotRequireDomainReloadInit]
    private static readonly int _endLocalSpacePosPropertyId = Shader.PropertyToID("_EndLocalSpacePos");
    private Color _color;
    private SliderData _sliderData;
    private bool _headOnBeat;
    private bool _headPastBeat;
    private bool _midOnBeat;
    private bool _midPastBeat;
    private bool _tailOnBeat;
    private bool _tailPastBeat;
    private bool _showHeadMoveHandle;
    private bool _showTailMoveHandle;
    private bool _showHandles;
    private Vector3 _localStartPosition;
    private Vector3 _localEndPosition;

    public ArcEditorData arcData { get; private set; }

    public void Init(
      ArcEditorData arcData,
      Color color,
      Vector3 localStartPosition,
      Vector3 localEndPosition,
      bool showHandles,
      bool showHeadMoveHandle,
      bool showTailMoveHandle)
    {
      this.arcData = arcData;
      this._localStartPosition = localStartPosition;
      this._localEndPosition = localEndPosition;
      this._showHandles = showHandles;
      this._showHeadMoveHandle = showHeadMoveHandle;
      this._showTailMoveHandle = showTailMoveHandle;
      this._color = color;
      this._sliderData = this.CreateSliderData(arcData.controlPointLengthMultiplier, arcData.cutDirection, arcData.tailControlPointLengthMultiplier, arcData.tailCutDirection, arcData.midAnchorMode);
      this.CreateArc();
    }

    public void SetLength(Vector3 localStartPosition, Vector3 localEndPosition)
    {
      this._localStartPosition = localStartPosition;
      this._localEndPosition = localEndPosition;
      this.CreateArc();
    }

    public void SetState(bool headOnBeat, bool tailOnBeat, bool midOnBeat)
    {
      if (headOnBeat == this._headOnBeat && tailOnBeat == this._tailOnBeat && midOnBeat == this._midOnBeat)
        return;
      this._headOnBeat = headOnBeat;
      this._tailOnBeat = tailOnBeat;
      this._midOnBeat = midOnBeat;
      this._headPointHandle.SetState(this._headOnBeat, false);
      this._tailPointHandle.SetState(this._tailOnBeat, false);
      this._midPointHandle.SetState(this._midOnBeat, false);
    }

    public void SetHighlight(bool highlighted)
    {
    }

        public void SetControlPointsDataOverride(BeatmapObjectCellData cellData, float? overrideControlPointLength, NoteCutDirection? overrideCutDirection)
        {
            float num = this._sliderData.headControlPointLengthMultiplier;
            NoteCutDirection noteCutDirection = this._sliderData.headCutDirection;
            float num2 = this._sliderData.tailControlPointLengthMultiplier;
            NoteCutDirection noteCutDirection2 = this._sliderData.tailCutDirection;
            if (AudioTimeHelper.IsBeatSame(cellData.beat, this.arcData.beat) && cellData.cellPosition.x == this.arcData.column && cellData.cellPosition.y == this.arcData.row)
            {
                num = (overrideControlPointLength ?? num);
                noteCutDirection = (overrideCutDirection ?? noteCutDirection);
            }
            else if (AudioTimeHelper.IsBeatSame(cellData.beat, this.arcData.tailBeat) && cellData.cellPosition.x == this.arcData.tailColumn && cellData.cellPosition.y == this.arcData.tailRow)
            {
                num2 = (overrideControlPointLength ?? num2);
                noteCutDirection2 = (overrideCutDirection ?? noteCutDirection2);
            }
            this._sliderData = this.CreateSliderData(num, noteCutDirection, num2, noteCutDirection2, this._sliderData.midAnchorMode);
            this.CreateArc();
        }

        public void SetMidAnchorModeDataOverride(SliderMidAnchorMode? overrideMidAnchorMode)
        {
            SliderMidAnchorMode midAnchorMode = overrideMidAnchorMode ?? this.arcData.midAnchorMode;
            this._sliderData = this.CreateSliderData(this._sliderData.headControlPointLengthMultiplier, this._sliderData.headCutDirection, this._sliderData.tailControlPointLengthMultiplier, this._sliderData.tailCutDirection, midAnchorMode);
            this.CreateArc();
        }

        private SliderData CreateSliderData(
      float controlPointLength,
      NoteCutDirection cutDirection,
      float tailControlPointLength,
      NoteCutDirection tailCutDirection,
      SliderMidAnchorMode midAnchorMode)
    {
      return SliderData.CreateSliderData(ColorType.None, 0.0f, this.arcData.column, (NoteLineLayer) this.arcData.row, (NoteLineLayer) this.arcData.row, controlPointLength, cutDirection, 1f, this.arcData.tailColumn, (NoteLineLayer) this.arcData.tailRow, (NoteLineLayer) this.arcData.tailRow, tailControlPointLength, tailCutDirection, midAnchorMode);
    }

    private void CreateArc()
    {
      this._headPointHandle.Init(this._localStartPosition, this._sliderData.headCutDirection.RotationAngle() + 180f, this._sliderData.headControlPointLengthMultiplier, this._color, this._showHandles, this._showHeadMoveHandle, true);
      this._tailPointHandle.Init(this._localEndPosition, this._sliderData.tailCutDirection.RotationAngle(), this._sliderData.tailControlPointLengthMultiplier, this._color, this._showHandles, this._showTailMoveHandle, true);
      this._midPointHandle.Init(this._localStartPosition + (this._localEndPosition - this._localStartPosition) * 0.5f, this._sliderData.midAnchorMode.RotationAngle(), 1f, this._color, this._showHandles && this._sliderData.headLineIndex == this._sliderData.tailLineIndex, false, false);
      this.CreateBezierPathAndMesh(this._arcMeshController, this._localStartPosition, this._localEndPosition, this._localEndPosition.z);
      this.SetMaterialPropertyBlock();
    }

    private void CreateBezierPathAndMesh(
      SliderMeshController controller,
      Vector3 localStartPosition,
      Vector3 localEndPosition,
      float jumpSpeed)
    {
      controller.CreateBezierPathAndMesh(this._sliderData, localStartPosition, localEndPosition, jumpSpeed, 0.5f);
      this._arcTubePhysicsMeshConstructor.CreatePhysicsMeshFromController();
    }

    private void SetMaterialPropertyBlock()
    {
      MaterialPropertyBlock materialPropertyBlock = this._arcMaterialPropertyBlockController.materialPropertyBlock;
      materialPropertyBlock.SetVector(ArcView._startLocalSpacePosPropertyId, (Vector4) this._localStartPosition);
      materialPropertyBlock.SetVector(ArcView._endLocalSpacePosPropertyId, (Vector4) this._localEndPosition);
      SliderShaderHelper.SetInitialProperties(materialPropertyBlock, this._color, 0.0f, 0.0f, 1f, 1f, this._localEndPosition.z, this._arcMeshController.pathLength, !this._showHeadMoveHandle, !this._showTailMoveHandle, Random.value);
      SliderShaderHelper.SetTimeSinceHeadNoteJump(materialPropertyBlock, 0.0f);
      SliderShaderHelper.EnableSaberAttraction(materialPropertyBlock, false);
      SliderShaderHelper.SetTailHeadNoteJumpOffsetDifference(materialPropertyBlock, 0.0f);
      this._arcMaterialPropertyBlockController.ApplyChanges();
    }

    public class Pool : MonoMemoryPool<ArcView>
    {
    }
  }
}
