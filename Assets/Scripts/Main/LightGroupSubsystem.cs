// Decompiled with JetBrains decompiler
// Type: LightGroupSubsystem
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof (LightGroup))]
public abstract class LightGroupSubsystem : MonoBehaviour
{
  [SerializeField]
  [HideInInspector]
  private LightGroup _lightGroup;

  public int groupId => this._lightGroup.groupId;

  protected LightGroup lightGroup => this._lightGroup;

  protected void OnEnable() => this._lightGroup = this.gameObject.GetComponent<LightGroup>();
}
