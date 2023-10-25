// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapEditorPreviewBeatmapLevel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace BeatmapEditor3D
{
  public class BeatmapEditorPreviewBeatmapLevel : IPreviewBeatmapLevel
  {
    private readonly Texture2D _coverTexture;

    public string levelID => (string) null;

    public string songName => (string) null;

    public string songSubName => (string) null;

    public string songAuthorName => (string) null;

    public string levelAuthorName => (string) null;

    public float beatsPerMinute => 0.0f;

    public float songTimeOffset => 0.0f;

    public float shuffle => 0.0f;

    public float shufflePeriod => 0.0f;

    public float previewStartTime => 0.0f;

    public float previewDuration => 0.0f;

    public float songDuration => 0.0f;

    public EnvironmentInfoSO environmentInfo => (EnvironmentInfoSO) null;

    public EnvironmentInfoSO allDirectionsEnvironmentInfo => (EnvironmentInfoSO) null;

    public IReadOnlyList<PreviewDifficultyBeatmapSet> previewDifficultyBeatmapSets => (IReadOnlyList<PreviewDifficultyBeatmapSet>) null;

    public BeatmapEditorPreviewBeatmapLevel(Texture2D coverTexture) => this._coverTexture = coverTexture;

    public Task<Sprite> GetCoverImageAsync(CancellationToken cancellationToken) => (Object) this._coverTexture == (Object) null ? Task.FromResult<Sprite>((Sprite) null) : Task.FromResult<Sprite>(Sprite.Create(this._coverTexture, new Rect(0.0f, 0.0f, (float) this._coverTexture.width, (float) this._coverTexture.height), Vector2.zero));
  }
}
