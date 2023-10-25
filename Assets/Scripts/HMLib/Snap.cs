// Decompiled with JetBrains decompiler
// Type: Snap
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public class Snap : MonoBehaviour
{
  public Vector3 snap = new Vector3(1f, 1f, 1f);
  public Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);

  public virtual void SnapPosition()
  {
    if (Application.isPlaying || (double) this.snap.x == 0.0 || (double) this.snap.y == 0.0 || (double) this.snap.z == 0.0)
      return;
    Vector3 position = this.transform.position;
    position.x = Mathf.Round(position.x / this.snap.x) * this.snap.x;
    position.y = Mathf.Round(position.y / this.snap.y) * this.snap.y;
    position.z = Mathf.Round(position.z / this.snap.z) * this.snap.z;
    this.transform.position = position + this.offset;
  }
}
