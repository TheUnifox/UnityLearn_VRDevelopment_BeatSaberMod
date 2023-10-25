// Decompiled with JetBrains decompiler
// Type: TrackLaneRingsManager
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class TrackLaneRingsManager : MonoBehaviour
{
  [SerializeField]
  protected TrackLaneRing _trackLaneRingPrefab;
  [SerializeField]
  protected int _ringCount = 10;
  [SerializeField]
  protected float _ringPositionStep = 2f;
  [SerializeField]
  protected bool _spawnAsChildren;
  [Inject]
  protected readonly DiContainer _container;
  protected TrackLaneRing[] _rings;

  public float ringPositionStep => this._ringPositionStep;

  public TrackLaneRing[] Rings => this._rings;

  public virtual void Start()
  {
    this._rings = new TrackLaneRing[this._ringCount];
    Vector3 forward = this.transform.forward;
    if (this._spawnAsChildren)
    {
      for (int index = 0; index < this._rings.Length; ++index)
      {
        this._rings[index] = this._container.InstantiatePrefabForComponent<TrackLaneRing>((Object) this._trackLaneRingPrefab);
        this._rings[index].transform.parent = this.transform;
        Vector3 position = new Vector3(0.0f, 0.0f, (float) index * this._ringPositionStep);
        this._rings[index].Init(position, Vector3.zero);
      }
    }
    else
    {
      for (int index = 0; index < this._rings.Length; ++index)
      {
        this._rings[index] = this._container.InstantiatePrefabForComponent<TrackLaneRing>((Object) this._trackLaneRingPrefab);
        Vector3 position = forward * ((float) index * this._ringPositionStep);
        this._rings[index].Init(position, this.transform.position);
      }
    }
  }

  public virtual void FixedUpdate()
  {
    float fixedDeltaTime = TimeHelper.fixedDeltaTime;
    for (int index = 0; index < this._rings.Length; ++index)
      this._rings[index].FixedUpdateRing(fixedDeltaTime);
  }

  public virtual void LateUpdate()
  {
    float interpolationFactor = TimeHelper.interpolationFactor;
    for (int index = 0; index < this._rings.Length; ++index)
      this._rings[index].LateUpdateRing(interpolationFactor);
  }

  public virtual void OnDrawGizmosSelected()
  {
    Vector3 forward = this.transform.forward;
    Vector3 position = this.transform.position;
    float num1 = 0.5f;
    float num2 = 45f;
    Gizmos.DrawRay(position, forward);
    Vector3 vector3_1 = Quaternion.LookRotation(forward) * Quaternion.Euler(0.0f, 180f + num2, 0.0f) * new Vector3(0.0f, 0.0f, 1f);
    Vector3 vector3_2 = Quaternion.LookRotation(forward) * Quaternion.Euler(0.0f, 180f - num2, 0.0f) * new Vector3(0.0f, 0.0f, 1f);
    Gizmos.DrawRay(position + forward, vector3_1 * num1);
    Gizmos.DrawRay(position + forward, vector3_2 * num1);
  }
}
