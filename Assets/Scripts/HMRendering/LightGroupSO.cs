// Decompiled with JetBrains decompiler
// Type: LightGroupSO
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using UnityEngine;

public class LightGroupSO : PersistentScriptableObject
{
  [SerializeField]
  [Tooltip("Automatically updated based on file name")]
  protected string _groupName;
  [SerializeField]
  [TextArea]
  [Tooltip("Only used for own descriptive purposes")]
  protected string _groupDescription;
  [SerializeField]
  [Min(0.0f)]
  protected int _groupId;
  [SerializeField]
  [Min(0.0f)]
  protected int _startLightId;
  [SerializeField]
  [Min(0.0f)]
  protected int _numberOfElements;
  [SerializeField]
  [Min(1f)]
  protected int _sameIdElements = 1;
  [SerializeField]
  protected bool _ignoreLightGroupEffectManager;

  public string groupName => this._groupName;

  public int groupId => this._groupId;

  public int startLightId => this._startLightId;

  public int numberOfElements => this._numberOfElements;

  public int sameIdElements => this._sameIdElements;

  public bool ignoreLightGroupEffectManager => this._ignoreLightGroupEffectManager;
}
