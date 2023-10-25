// Decompiled with JetBrains decompiler
// Type: LightWithIdMonoBehaviour
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;
using Zenject;

[ExecuteAlways]
public abstract class LightWithIdMonoBehaviour : MonoBehaviour, ILightWithId
{
  [SerializeField]
  private int _ID = -1;
  [Inject]
  private LightWithIdManager _lightManager;
  private bool _isRegistered;

  public int lightId => this._ID;

  public bool isRegistered => this._isRegistered;

  public void __SetIsRegistered() => this._isRegistered = true;

  public void __SetIsUnRegistered() => this._isRegistered = false;

  public abstract void ColorWasSet(Color color);

  protected virtual void OnEnable() => this.RegisterLight();

  protected virtual void Start() => this.RegisterLight();

  protected virtual void OnDisable()
  {
    if (!((Object) this._lightManager != (Object) null))
      return;
    this._lightManager.UnregisterLight((ILightWithId) this);
  }

  private void RegisterLight()
  {
    if ((Object) this._lightManager == (Object) null)
      return;
    this._lightManager.RegisterLight((ILightWithId) this);
  }

  public void SetLightId(int newLightId)
  {
    if ((Object) this._lightManager == (Object) null)
      return;
    this._lightManager.UnregisterLight((ILightWithId) this);
    this._ID = newLightId;
    this._lightManager.RegisterLight((ILightWithId) this);
  }
}
