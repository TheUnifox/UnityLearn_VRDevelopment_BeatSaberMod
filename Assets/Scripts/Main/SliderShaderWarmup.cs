// Decompiled with JetBrains decompiler
// Type: SliderShaderWarmup
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class SliderShaderWarmup : MonoBehaviour
{
  [SerializeField]
  protected SliderMeshController _sliderMeshController;
  [SerializeField]
  protected MaterialPropertyBlockController _materialPropertyBlockController;

  public virtual void Start()
  {
    SliderData sliderData = SliderData.CreateSliderData(ColorType.ColorA, 4f, 0, NoteLineLayer.Base, NoteLineLayer.Base, 1f, NoteCutDirection.Up, 6f, 1, NoteLineLayer.Upper, NoteLineLayer.Upper, 1f, NoteCutDirection.Down, SliderMidAnchorMode.Straight);
    this._sliderMeshController.CreateBezierPathAndMesh(sliderData, Vector3.left, Vector3.right, 4f, 1f);
    MaterialPropertyBlock materialPropertyBlock = this._materialPropertyBlockController.materialPropertyBlock;
    SliderShaderHelper.SetInitialProperties(materialPropertyBlock, Color.red, 0.0f, 0.0f, 4f, 24f, 1f, 1f, sliderData.hasHeadNote, sliderData.hasTailNote, Random.value);
    SliderShaderHelper.SetTimeSinceHeadNoteJump(materialPropertyBlock, 4f);
    this._materialPropertyBlockController.ApplyChanges();
  }
}
