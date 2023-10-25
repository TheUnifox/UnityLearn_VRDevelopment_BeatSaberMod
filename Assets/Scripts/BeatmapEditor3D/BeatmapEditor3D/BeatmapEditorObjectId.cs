// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorObjectId
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public readonly struct BeatmapEditorObjectId
  {
    private const uint kDefaultBaseId = 1;
    private static uint _currentId = 1;
    [DoesNotRequireDomainReloadInit]
    private static readonly BeatmapEditorObjectId _invalidId = new BeatmapEditorObjectId(0U);
    private readonly uint _id;

    public static BeatmapEditorObjectId invalid => BeatmapEditorObjectId._invalidId;

    public static BeatmapEditorObjectId NewId() => new BeatmapEditorObjectId(BeatmapEditorObjectId._currentId++);

    public static void Reset() => BeatmapEditorObjectId._currentId = 1U;

    [RuntimeInitializeOnLoadMethod]
    public static void Initialize() => BeatmapEditorObjectId._currentId = 1U;

    private BeatmapEditorObjectId(uint id) => this._id = id;

    private bool Equals(BeatmapEditorObjectId other) => (int) this._id == (int) other._id;

    public override bool Equals(object obj) => obj is BeatmapEditorObjectId other && this.Equals(other);

    public override int GetHashCode() => (int) this._id;

    public static bool operator ==(BeatmapEditorObjectId lhs, BeatmapEditorObjectId rhs) => lhs.Equals(rhs);

    public static bool operator !=(BeatmapEditorObjectId lhs, BeatmapEditorObjectId rhs) => !lhs.Equals(rhs);

    public override string ToString() => string.Format("{0}", (object) this._id);
  }
}
