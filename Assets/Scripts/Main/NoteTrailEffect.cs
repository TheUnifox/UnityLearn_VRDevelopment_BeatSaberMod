// Decompiled with JetBrains decompiler
// Type: NoteTrailEffect
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class NoteTrailEffect : MonoBehaviour
{
  [SerializeField]
  protected int _particlesPerFrame = 2;
  [SerializeField]
  protected float _maxSpawnDistance = 70f;
  [SerializeField]
  protected NoteMovement _noteMovement;
  [Inject]
  protected NoteTrailParticleSystem _noteTrailParticleSystem;

  public virtual void Awake()
  {
    this._noteMovement.didInitEvent += new System.Action(this.HandleNoteMovementDidInit);
    this._noteMovement.noteDidStartJumpEvent += new System.Action(this.HandleNoteDidStartJump);
    this.enabled = this._noteMovement.enabled;
  }

  public virtual void OnDestroy()
  {
    if (!((UnityEngine.Object) this._noteMovement != (UnityEngine.Object) null))
      return;
    this._noteMovement.didInitEvent -= new System.Action(this.HandleNoteMovementDidInit);
    this._noteMovement.noteDidFinishJumpEvent -= new System.Action(this.HandleNoteDidStartJump);
  }

  public virtual void Update()
  {
    Vector3 position = this._noteMovement.position;
    if ((double) position.x * (double) position.x + (double) position.z * (double) position.z >= (double) this._maxSpawnDistance * (double) this._maxSpawnDistance)
      return;
    this._noteTrailParticleSystem.Emit(this._noteMovement.prevPosition, position, this._particlesPerFrame);
  }

  public virtual void HandleNoteMovementDidInit() => this.enabled = true;

  public virtual void HandleNoteDidStartJump() => this.enabled = false;
}
