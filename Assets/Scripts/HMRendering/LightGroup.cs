// Decompiled with JetBrains decompiler
// Type: LightGroup
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using System;
using UnityEngine;

public class LightGroup : MonoBehaviour
{
  [SerializeField]
  protected LightGroupSO _lightGroupSO;

  public LightGroupSO lightGroupSO => this._lightGroupSO;

  public int numberOfElements => !(bool) (UnityEngine.Object) this.lightGroupSO ? 0 : this.lightGroupSO.numberOfElements;

  public int startLightId => !(bool) (UnityEngine.Object) this.lightGroupSO ? 0 : this.lightGroupSO.startLightId;

  public int groupId => !(bool) (UnityEngine.Object) this.lightGroupSO ? -1 : this.lightGroupSO.groupId;

  public int sameIdElements => !(bool) (UnityEngine.Object) this.lightGroupSO ? 0 : this.lightGroupSO.sameIdElements;

  public bool ignoreLightGroupEffectManager => (bool) (UnityEngine.Object) this.lightGroupSO && this.lightGroupSO.ignoreLightGroupEffectManager;

  public event Action<GameObject> respawnEvent;

  public event Action<GameObject> didRefreshContentEvent;
}
