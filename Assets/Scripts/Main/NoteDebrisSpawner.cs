// Decompiled with JetBrains decompiler
// Type: NoteDebrisSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NoteDebrisSpawner : MonoBehaviour, INoteDebrisDidFinishEvent
{
  [SerializeField]
  protected float _rotation = 2f;
  [SerializeField]
  protected float _cutDirMultiplier = 0.1f;
  [SerializeField]
  protected float _fromCenterSpeed = 2f;
  [SerializeField]
  protected float _moveSpeedMultiplier = 0.2f;
  [Inject(Id = NoteData.GameplayType.Normal)]
  protected readonly NoteDebris.Pool _normalNotesDebrisPool;
  [Inject(Id = NoteData.GameplayType.BurstSliderHead)]
  protected readonly NoteDebris.Pool _burstSliderHeadNotesDebrisPool;
  [Inject(Id = NoteData.GameplayType.BurstSliderElement)]
  protected readonly NoteDebris.Pool _burstSliderElementNotesDebrisPool;
  protected const float kMinLifeTime = 0.2f;
  protected const float kMaxLifeTime = 2f;
  protected const float kLifeTimeOffset = 0.05f;
  protected Dictionary<NoteData.GameplayType, NoteDebris.Pool> _poolForNoteGameplayType;
  protected readonly Dictionary<NoteDebris, NoteDebris.Pool> _poolForNoteDebris = new Dictionary<NoteDebris, NoteDebris.Pool>();

  public virtual void Start() => this._poolForNoteGameplayType = new Dictionary<NoteData.GameplayType, NoteDebris.Pool>()
  {
    {
      NoteData.GameplayType.Normal,
      this._normalNotesDebrisPool
    },
    {
      NoteData.GameplayType.BurstSliderHead,
      this._burstSliderHeadNotesDebrisPool
    },
    {
      NoteData.GameplayType.BurstSliderElement,
      this._burstSliderElementNotesDebrisPool
    }
  };

  public virtual void SpawnDebris(
    NoteData.GameplayType noteGameplayType,
    Vector3 cutPoint,
    Vector3 cutNormal,
    float saberSpeed,
    Vector3 saberDir,
    Vector3 notePos,
    Quaternion noteRotation,
    Vector3 noteScale,
    ColorType colorType,
    float timeToNextColorNote,
    Vector3 moveVec)
  {
    NoteDebris debris0;
    NoteDebris debris1;
    this.SpawnNoteDebris(noteGameplayType, out debris0, out debris1);
    if ((Object) debris0 == (Object) null || (Object) debris1 == (Object) null)
      return;
    debris0.didFinishEvent.Add((INoteDebrisDidFinishEvent) this);
    debris0.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    debris1.didFinishEvent.Add((INoteDebrisDidFinishEvent) this);
    debris1.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    float magnitude = moveVec.magnitude;
    float lifeTime = Mathf.Clamp(timeToNextColorNote + 0.05f, 0.2f, 2f);
    Vector3 rhs = Vector3.ProjectOnPlane(saberDir, moveVec / magnitude);
    Vector3 vector3 = rhs * (saberSpeed * this._cutDirMultiplier) + moveVec * this._moveSpeedMultiplier;
    if ((double) cutPoint.y < 1.2999999523162842)
      vector3.y = Mathf.Min(vector3.y, 0.0f);
    else if ((double) cutPoint.y > 1.2999999523162842)
      vector3.y = Mathf.Max(vector3.y, 0.0f);
    Quaternion rotation = this.transform.rotation;
    Vector3 force1 = rotation * (-(cutNormal + Random.onUnitSphere * 0.1f) * this._fromCenterSpeed + vector3);
    Vector3 force2 = rotation * ((cutNormal + Random.onUnitSphere * 0.1f) * this._fromCenterSpeed + vector3);
    Vector3 torque = rotation * Vector3.Cross(cutNormal, rhs) * this._rotation / Mathf.Max(1f, timeToNextColorNote * 2f);
    Vector3 position = this.transform.position;
    debris0.Init(colorType, notePos, noteRotation, moveVec, noteScale, position, rotation, cutPoint, -cutNormal, force1, -torque, lifeTime);
    debris1.Init(colorType, notePos, noteRotation, moveVec, noteScale, position, rotation, cutPoint, cutNormal, force2, torque, lifeTime);
  }

  public virtual void HandleNoteDebrisDidFinish(NoteDebris noteDebris)
  {
    noteDebris.didFinishEvent.Remove((INoteDebrisDidFinishEvent) this);
    this.DespawnNoteDebris(noteDebris);
  }

  public virtual void SpawnNoteDebris(
    NoteData.GameplayType noteGameplayType,
    out NoteDebris debris0,
    out NoteDebris debris1)
  {
    NoteDebris.Pool pool;
    if (!this._poolForNoteGameplayType.TryGetValue(noteGameplayType, out pool))
    {
      debris0 = (NoteDebris) null;
      debris1 = (NoteDebris) null;
    }
    else
    {
      debris0 = pool.Spawn();
      debris1 = pool.Spawn();
      this._poolForNoteDebris[debris0] = pool;
      this._poolForNoteDebris[debris1] = pool;
    }
  }

  public virtual void DespawnNoteDebris(NoteDebris noteDebris)
  {
    this._poolForNoteDebris[noteDebris].Despawn(noteDebris);
    this._poolForNoteDebris.Remove(noteDebris);
  }
}
