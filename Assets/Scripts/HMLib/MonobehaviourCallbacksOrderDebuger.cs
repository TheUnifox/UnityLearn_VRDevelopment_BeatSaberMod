// Decompiled with JetBrains decompiler
// Type: MonobehaviourCallbacksOrderDebuger
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class MonobehaviourCallbacksOrderDebuger : MonoBehaviour
{
  public virtual void Awake() => Debug.Log((object) ("Awake " + this.gameObject.name));

  public virtual void OnEnable() => Debug.Log((object) ("OnEnable " + this.gameObject.name));

  public virtual void Start() => Debug.Log((object) ("Start " + this.gameObject.name));
}
