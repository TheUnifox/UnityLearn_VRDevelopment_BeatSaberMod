// Decompiled with JetBrains decompiler
// Type: MirroredSliderController
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MirroredSliderController : SliderControllerBase, ISliderDidStartDissolvingEvent
{
  [SerializeField]
  protected MeshFilter _meshFilter;
  [Inject]
  protected readonly IBeatmapObjectSpawnController _beatmapObjectSpawnController;
  protected SliderController _followedSlider;
  protected Transform _transform;
  protected Transform _followedTransform;

  public bool hide
  {
    set => this.gameObject.SetActive(!value);
  }

  public virtual void Awake() => this._transform = this.transform;

  public virtual void Update()
  {
    this._transform.SetPositionAndRotation(this._followedTransform.position, this._followedTransform.rotation);
    this.UpdateMaterialPropertyBlock(this._followedSlider.sliderMovement.timeSinceHeadNoteJump);
  }

  public virtual void OnDestroy() => this.RemoveListeners();

  public virtual void Mirror(SliderController sliderController)
  {
    this.RemoveListeners();
    this._dissolving = false;
    this._cutoutAnimateEffect.ResetEffect();
    sliderController.sliderDidStartDissolvingEvent.Add((ISliderDidStartDissolvingEvent) this);
    this._followedSlider = sliderController;
    this._followedTransform = sliderController.transform;
    this._meshFilter.sharedMesh = sliderController.sliderMeshController.mesh;
    SliderShaderHelper.SetInitialProperties(this._materialPropertyBlockController.materialPropertyBlock, sliderController, this._beatmapObjectSpawnController.noteJumpMovementSpeed);
    SliderShaderHelper.EnableSaberAttraction(this._materialPropertyBlockController.materialPropertyBlock, false);
    this.UpdateMaterialPropertyBlock(sliderController.sliderMovement.timeSinceHeadNoteJump);
  }

  public virtual void UpdateMaterialPropertyBlock(float timeSinceHeadNoteJump)
  {
    SliderShaderHelper.UpdateMaterialPropertyBlock(this._materialPropertyBlockController.materialPropertyBlock, this._followedSlider, timeSinceHeadNoteJump, this._beatmapObjectSpawnController.jumpOffsetY);
    this._materialPropertyBlockController.ApplyChanges();
  }

  public virtual void RemoveListeners()
  {
    if ((Object) this._followedSlider != (Object) null)
      this._followedSlider.sliderDidStartDissolvingEvent.Remove((ISliderDidStartDissolvingEvent) this);
    this._followedSlider = (SliderController) null;
  }

  public virtual void HandleSliderDidStartDissolving(
    SliderController sliderController,
    float duration)
  {
    this.Dissolve(duration);
  }

  public virtual void Dissolve(float duration)
  {
    if (this._dissolving)
      return;
    this._dissolving = true;
    this.AnimateCutout(0.0f, 1f, duration);
  }

  public class Pool : MonoMemoryPool<MirroredSliderController>
  {
  }
}
