// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Controller.BeatmapEditorBeatmapObjectSpawnController
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Controller
{
  public class BeatmapEditorBeatmapObjectSpawnController : 
    MonoBehaviour,
    IBeatmapObjectSpawnController
  {
    [Inject]
    private readonly IJumpOffsetYProvider _jumpOffsetYProvider;
    private BeatmapObjectSpawnMovementData _beatmapObjectSpawnMovementData;

    public int noteLinesCount => 4;

    public float jumpOffsetY => this._jumpOffsetYProvider.jumpOffsetY;

    public float currentBpm => 0.0f;

    public float moveDuration => 0.0f;

    public float jumpDuration => 0.0f;

    public float jumpDistance => 0.0f;

    public float verticalLayerDistance => 0.0f;

    public float noteJumpMovementSpeed => 0.0f;

    public float noteLinesDistance => 0.0f;

    public BeatmapObjectSpawnMovementData beatmapObjectSpawnMovementData
    {
      get
      {
        if (this._beatmapObjectSpawnMovementData == null)
        {
          this._beatmapObjectSpawnMovementData = new BeatmapObjectSpawnMovementData();
          this._beatmapObjectSpawnMovementData.Init(4, 0.0f, 1f, BeatmapObjectSpawnMovementData.NoteJumpValueType.BeatOffset, 0.0f, this._jumpOffsetYProvider, Vector3.right, Vector3.forward);
        }
        return this._beatmapObjectSpawnMovementData;
      }
    }

    public bool isInitialized => true;

    public event Action didInitEvent;

    public Vector2 Get2DNoteOffset(int noteLineIndex, NoteLineLayer noteLineLayer) => Vector2.zero;

    public float JumpPosYForLineLayerAtDistanceFromPlayerWithoutJumpOffset(
      NoteLineLayer lineLayer,
      float distanceFromPlayer)
    {
      return 0.0f;
    }

    protected void Start()
    {
      Action didInitEvent = this.didInitEvent;
      if (didInitEvent == null)
        return;
      didInitEvent();
    }
  }
}
