// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.Visuals.BeatlineContainer
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System;
using UnityEngine;
using Zenject;

namespace BeatmapEditor3D.Visuals
{
  public class BeatlineContainer : BaseBeatBasedItemContainer<BeatmapObjectBeatLine>
  {
    [SerializeField]
    private Color _barColor;
    [SerializeField]
    private float _barHeight;
    [Space]
    [SerializeField]
    private Color _beatColor;
    [SerializeField]
    private float _beatHeight;
    [Space]
    [SerializeField]
    private Color _offBeatColor;
    [SerializeField]
    private float _offBeatHeight;
    [Inject]
    private readonly BeatmapObjectBeatLine.Pool _beatmapObjectBeatLinePool;
    private readonly int colorId = Shader.PropertyToID("_Color");
    private float _width;
    private bool _dimmed;
    private MaterialPropertyBlock _barMaterialPropertyBlock;
    private MaterialPropertyBlock _beatMaterialPropertyBlock;
    private MaterialPropertyBlock _offBeatMaterialPropertyBlock;

    public void SetData(
      float width,
      Vector3 positionOffset,
      int subdivision,
      float spawnIncrement,
      Func<float, bool> shouldSkipItemFunc)
    {
      this.SetData(positionOffset, subdivision, spawnIncrement, shouldSkipItemFunc);
      this._width = width;
      this.CreateAndSetPropertyBlocks();
    }

    protected override BeatmapObjectBeatLine SpawnItem() => this._beatmapObjectBeatLinePool.Spawn(this._width);

    protected override void DespawnItem(BeatmapObjectBeatLine item) => this._beatmapObjectBeatLinePool.Despawn(item);

    protected override void SetItemData(BeatmapObjectBeatLine item, float beat)
    {
      bool flag = (double) beat % 1.0 == 0.0;
      int num = (double) beat % 4.0 == 0.0 ? 1 : 0;
      MaterialPropertyBlock propertyBlock = num != 0 ? this._barMaterialPropertyBlock : (flag ? this._beatMaterialPropertyBlock : this._offBeatMaterialPropertyBlock);
      float height = num != 0 ? this._barHeight : (flag ? this._beatHeight : this._offBeatHeight);
      item.SetData(propertyBlock, (double) beat % 1.0 == 0.0 ? 1f : 0.4f, height);
    }

    private void CreateAndSetPropertyBlocks()
    {
      if (this._barMaterialPropertyBlock == null)
        this._barMaterialPropertyBlock = new MaterialPropertyBlock();
      if (this._beatMaterialPropertyBlock == null)
        this._beatMaterialPropertyBlock = new MaterialPropertyBlock();
      if (this._offBeatMaterialPropertyBlock == null)
        this._offBeatMaterialPropertyBlock = new MaterialPropertyBlock();
      this._barMaterialPropertyBlock.SetColor(this.colorId, this._barColor);
      this._beatMaterialPropertyBlock.SetColor(this.colorId, this._beatColor);
      this._offBeatMaterialPropertyBlock.SetColor(this.colorId, this._offBeatColor);
    }
  }
}
