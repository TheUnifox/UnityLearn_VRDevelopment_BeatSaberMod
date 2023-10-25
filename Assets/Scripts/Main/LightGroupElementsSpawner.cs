// Decompiled with JetBrains decompiler
// Type: LightGroupElementsSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof (LightGroup))]
public class LightGroupElementsSpawner : MonoBehaviour
{
  [SerializeField]
  [HideInInspector]
  protected LightGroup _lightGroup;
  [SerializeField]
  protected GameObject _lightPrefab;
  [SerializeField]
  protected bool _useAlternatePrefab;
  [SerializeField]
  [NullAllowed]
  [DrawIf("_useAlternatePrefab", true, DrawIfAttribute.DisablingType.DontDraw)]
  protected GameObject _alternateLightPrefab;

  public virtual void OnEnable() => this._lightGroup = this.gameObject.GetComponent<LightGroup>();
}
