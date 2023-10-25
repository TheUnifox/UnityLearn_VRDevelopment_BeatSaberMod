// Decompiled with JetBrains decompiler
// Type: LightWithIds
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[ExecuteAlways]
public abstract class LightWithIds : MonoBehaviour
{
  [Inject]
  private LightWithIdManager _lightManager;
  private IEnumerable<LightWithIds.LightWithId> _lightWithIds;
  private bool _isRegistered;
  private bool _childrenColorWasSet;

  public IEnumerable<LightWithIds.LightWithId> lightWithIds => this._lightWithIds;

  protected virtual void Awake() => this.SetNewLightsWithIds(this.GetLightWithIds());

  protected void Start() => this.RegisterForColorChanges();

  protected virtual void OnEnable() => this.RegisterForColorChanges();

  public void MarkChildrenColorAsSet() => this._childrenColorWasSet = true;

  protected void SetNewLightsWithIds(
    IEnumerable<LightWithIds.LightWithId> lightsWithIds)
  {
    this.UnregisterFromColorChanges();
    this._lightWithIds = lightsWithIds;
    this.RegisterForColorChanges();
  }

  protected abstract IEnumerable<LightWithIds.LightWithId> GetLightWithIds();

  private void RegisterForColorChanges()
  {
    if (this._isRegistered || !this.enabled || this._lightWithIds == null || (UnityEngine.Object) this._lightManager == (UnityEngine.Object) null)
      return;
    this._lightManager.didChangeSomeColorsThisFrameEvent += new Action(this.HandleLightManagerDidChangeSomeColorsThisFrame);
    foreach (LightWithIds.LightWithId lightWithId in this._lightWithIds)
    {
      lightWithId.__SetParentLightWithIds(this);
      this._lightManager.RegisterLight((ILightWithId) lightWithId);
    }
    this._isRegistered = true;
  }

  private void UnregisterFromColorChanges()
  {
    if (!this._isRegistered || (UnityEngine.Object) this._lightManager == (UnityEngine.Object) null)
      return;
    this._lightManager.didChangeSomeColorsThisFrameEvent -= new Action(this.HandleLightManagerDidChangeSomeColorsThisFrame);
    foreach (ILightWithId lightWithId in this._lightWithIds)
      this._lightManager.UnregisterLight(lightWithId);
    this._isRegistered = false;
  }

  protected void OnDisable() => this.UnregisterFromColorChanges();

  private void HandleLightManagerDidChangeSomeColorsThisFrame()
  {
    if (!this._childrenColorWasSet)
      return;
    this._childrenColorWasSet = false;
    this.ProcessNewColorData();
  }

  protected abstract void ProcessNewColorData();

  [Serializable]
  public abstract class LightWithId : ILightWithId
  {
    [SerializeField]
    private int _lightId;
    private Color _color;
    private bool _isRegistered;
    private LightWithIds _parentLightWithIds;

    public int lightId => this._lightId;

    public Color color => this._color;

    public bool isRegistered => this._isRegistered;

    public void __SetIsRegistered() => this._isRegistered = true;

    public void __SetIsUnRegistered() => this._isRegistered = false;

    protected LightWithId()
    {
    }

    protected LightWithId(int lightId) => this._lightId = lightId;

    public void __SetParentLightWithIds(LightWithIds parentLightWithIds) => this._parentLightWithIds = parentLightWithIds;

    public virtual void ColorWasSet(Color newColor)
    {
      this._color = newColor;
      this._parentLightWithIds.MarkChildrenColorAsSet();
    }
  }
}
