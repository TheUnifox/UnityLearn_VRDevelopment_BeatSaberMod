// Decompiled with JetBrains decompiler
// Type: StretchableObstacle
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

public class StretchableObstacle : MonoBehaviour
{
  [SerializeField]
  protected float _edgeSize = 0.05f;
  [SerializeField]
  protected float _coreOffset = 0.01f;
  [SerializeField]
  protected float _addColorMultiplier = 0.1f;
  [SerializeField]
  protected float _obstacleCoreLerpToWhiteFactor = 0.75f;
  [SerializeField]
  protected Vector3 _fakeGlowOffset = new Vector3(0.01f, 0.01f, 0.01f);
  [Space]
  [SerializeField]
  [NullAllowed]
  protected Transform _obstacleCore;
  [SerializeField]
  protected MaterialPropertyBlockController[] _materialPropertyBlockControllers;
  [SerializeField]
  protected ParametricBoxFrameController _obstacleFrame;
  [SerializeField]
  [NullAllowed]
  protected ParametricBoxFakeGlowController _obstacleFakeGlow;
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _uvScaleID = Shader.PropertyToID("_UVScale");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _tintColorID = Shader.PropertyToID("_TintColor");
  [DoesNotRequireDomainReloadInit]
  protected static readonly int _addColorID = Shader.PropertyToID("_AddColor");
  protected Bounds _bounds;

  public Bounds bounds => this._bounds;

  public virtual void SetSizeAndColor(float width, float height, float length, Color color)
  {
    this._bounds = new Bounds(new Vector3(0.0f, height * 0.5f, length * 0.5f), new Vector3(width, height, length));
    Vector3 vector3 = new Vector3(width - this._coreOffset, height - this._coreOffset, length - this._coreOffset);
    if ((Object) this._obstacleCore != (Object) null)
    {
      this._obstacleCore.localScale = vector3;
      this._obstacleCore.localPosition = new Vector3(0.0f, height * 0.5f, length * 0.5f);
    }
    this._obstacleFrame.width = width;
    this._obstacleFrame.height = height;
    this._obstacleFrame.length = length;
    this._obstacleFrame.edgeSize = this._edgeSize;
    this._obstacleFrame.localPosition = new Vector3(0.0f, height * 0.5f, length * 0.5f);
    this._obstacleFrame.color = color;
    this._obstacleFrame.Refresh();
    if ((Object) this._obstacleFakeGlow != (Object) null)
    {
      this._obstacleFakeGlow.width = width + this._fakeGlowOffset.x;
      this._obstacleFakeGlow.height = height + this._fakeGlowOffset.y;
      this._obstacleFakeGlow.length = length + this._fakeGlowOffset.z;
      this._obstacleFakeGlow.edgeSize = this._edgeSize * 1.5f;
      this._obstacleFakeGlow.localPosition = new Vector3(0.0f, height * 0.5f, length * 0.5f);
      this._obstacleFakeGlow.color = color;
      this._obstacleFakeGlow.Refresh();
    }
    Color color1 = (color * this._addColorMultiplier) with
    {
      a = 0.0f
    };
    foreach (MaterialPropertyBlockController propertyBlockController in this._materialPropertyBlockControllers)
    {
      propertyBlockController.materialPropertyBlock.SetVector(StretchableObstacle._uvScaleID, (Vector4) vector3);
      propertyBlockController.materialPropertyBlock.SetColor(StretchableObstacle._addColorID, color1);
      propertyBlockController.materialPropertyBlock.SetColor(StretchableObstacle._tintColorID, Color.Lerp(color, Color.white, this._obstacleCoreLerpToWhiteFactor));
      propertyBlockController.ApplyChanges();
    }
  }

  public virtual void OnValidate()
  {
    if (!this.gameObject.scene.IsValid())
      return;
    this.SetSizeAndColor(1f, 1f, 1f, Color.white);
  }
}
