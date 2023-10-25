// Decompiled with JetBrains decompiler
// Type: MaterialPropertyBlockAnimator
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

[ExecuteAlways]
public class MaterialPropertyBlockAnimator : MonoBehaviour
{
  [SerializeField]
  protected string _property;
  [SerializeField]
  protected MaterialPropertyBlockController _materialPropertyBlockController;
  protected int propertyId;
  protected bool _isInitialized;

  public MaterialPropertyBlockController materialPropertyBlockController
  {
    get => this._materialPropertyBlockController;
    set
    {
      this._materialPropertyBlockController = value;
      this.enabled = (Object) this._materialPropertyBlockController != (Object) null;
    }
  }

  protected virtual void SetProperty()
  {
  }

  public virtual void Awake()
  {
    this.LazyInit();
    this.enabled = (Object) this._materialPropertyBlockController != (Object) null;
  }

  public virtual void Update()
  {
    this.SetProperty();
    this._materialPropertyBlockController.ApplyChanges();
  }

  public virtual void LazyInit()
  {
    if (this._isInitialized)
      return;
    this._isInitialized = true;
    this.propertyId = Shader.PropertyToID(this._property);
  }

  [ContextMenu("RefreshPropertyID")]
  public virtual void RefreshProperty() => this.propertyId = Shader.PropertyToID(this._property);
}
