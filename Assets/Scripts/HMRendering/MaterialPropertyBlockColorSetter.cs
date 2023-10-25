// Decompiled with JetBrains decompiler
// Type: MaterialPropertyBlockColorSetter
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class MaterialPropertyBlockColorSetter : MonoBehaviour
{
  [SerializeField]
  protected bool _useTestColor;
  [SerializeField]
  [DrawIf("_useTestColor", true, DrawIfAttribute.DisablingType.DontDraw)]
  [ColorUsage(true, true)]
  protected Color _testColor = Color.white;
  [SerializeField]
  protected string _property;
  [SerializeField]
  protected MaterialPropertyBlockController _materialPropertyBlockController;
  [SerializeField]
  protected bool _inverseAlpha;
  [SerializeField]
  protected bool _multiplyWithAlpha;
  protected int _propertyId;
  protected bool _isInitialized;

  public Color color => this._materialPropertyBlockController.materialPropertyBlock.GetColor(this._propertyId);

  public MaterialPropertyBlockController materialPropertyBlockController
  {
    get => this._materialPropertyBlockController;
    set => this._materialPropertyBlockController = value;
  }

  public virtual void Awake() => this.InitIfNeeded();

  public virtual void InitIfNeeded()
  {
    if (this._isInitialized)
      return;
    this._isInitialized = true;
    this._propertyId = Shader.PropertyToID(this._property);
  }

  public virtual void SetColor(Color color)
  {
    if (this._inverseAlpha)
      color.a = 1f - color.a;
    if (this._multiplyWithAlpha)
    {
      color.r *= color.a;
      color.g *= color.a;
      color.b *= color.a;
    }
    this.InitIfNeeded();
    this._materialPropertyBlockController.materialPropertyBlock.SetColor(this._propertyId, color);
    this._materialPropertyBlockController.ApplyChanges();
  }

  public virtual void OnValidate()
  {
    if (Application.isPlaying || !this._useTestColor)
      return;
    this.SetColor(this._testColor);
  }
}
