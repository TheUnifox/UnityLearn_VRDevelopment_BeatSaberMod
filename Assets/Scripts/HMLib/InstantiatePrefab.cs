// Decompiled with JetBrains decompiler
// Type: InstantiatePrefab
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class InstantiatePrefab : MonoBehaviour
{
  public GameObject _prefab;

  public virtual void Awake()
  {
    Transform transform1 = Object.Instantiate<GameObject>(this._prefab).transform;
    Transform transform2 = this.transform;
    transform1.parent = transform2.parent;
    transform1.localPosition = transform2.localPosition;
    transform1.localRotation = transform2.localRotation;
    transform1.localScale = transform2.localScale;
  }
}
