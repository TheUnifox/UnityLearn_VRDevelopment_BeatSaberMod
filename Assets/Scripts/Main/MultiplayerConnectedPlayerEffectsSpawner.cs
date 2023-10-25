// Decompiled with JetBrains decompiler
// Type: MultiplayerConnectedPlayerEffectsSpawner
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using UnityEngine;
using Zenject;

public class MultiplayerConnectedPlayerEffectsSpawner : MonoBehaviour
{
  [SerializeField]
  protected NoteDebrisSpawner _noteDebrisSpawner;
  [SerializeField]
  protected BombExplosionEffect _bombExplosionEffect;
  [Inject]
  protected readonly IConnectedPlayerBeatmapObjectEventManager _beatmapObjectEventManager;

  public virtual void Start() => this._beatmapObjectEventManager.connectedPlayerNoteWasCutEvent += new System.Action<NoteCutInfoNetSerializable>(this.HandleBeatmapObjectEventManagerConnectedPlayerBeatmapObjectWasCut);

  public virtual void OnDestroy()
  {
    if (this._beatmapObjectEventManager == null)
      return;
    this._beatmapObjectEventManager.connectedPlayerNoteWasCutEvent -= new System.Action<NoteCutInfoNetSerializable>(this.HandleBeatmapObjectEventManagerConnectedPlayerBeatmapObjectWasCut);
  }

  public virtual void HandleBeatmapObjectEventManagerConnectedPlayerBeatmapObjectWasCut(
    NoteCutInfoNetSerializable noteCutInfo)
  {
    if (noteCutInfo.colorType == ColorType.None)
      this._bombExplosionEffect.SpawnExplosion(this.transform.InverseTransformPoint((Vector3) noteCutInfo.notePosition));
    else
      this._noteDebrisSpawner.SpawnDebris(noteCutInfo.gameplayType, (Vector3) noteCutInfo.cutPoint, (Vector3) noteCutInfo.cutNormal, noteCutInfo.saberSpeed, (Vector3) noteCutInfo.saberDir, (Vector3) noteCutInfo.notePosition, (Quaternion) noteCutInfo.noteRotation, (Vector3) noteCutInfo.noteScale, noteCutInfo.colorType, noteCutInfo.timeToNextColorNote, (Vector3) noteCutInfo.moveVec);
  }
}
