// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatmapObjectMaterialHelpers
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using System.Collections.Generic;
using UnityEngine;

namespace BeatmapEditor3D
{
  public static class BeatmapObjectMaterialHelpers
  {
    [DoesNotRequireDomainReloadInit]
    public static readonly int colorId = Shader.PropertyToID("_Color");
    [DoesNotRequireDomainReloadInit]
    public static readonly int highlightId = Shader.PropertyToID("_Highlight");
    [DoesNotRequireDomainReloadInit]
    public static readonly int dimId = Shader.PropertyToID("_Dim");

    public static void SetColorDataToMaterialPropertyBlock(
      MaterialPropertyBlockController materialPropertyBlockController,
      Color color)
    {
      materialPropertyBlockController.materialPropertyBlock.SetColor(BeatmapObjectMaterialHelpers.colorId, color.ColorWithAlpha(1f));
      materialPropertyBlockController.ApplyChanges();
    }

    public static void SetBeatDataToMaterialPropertyBlock(
      MaterialPropertyBlockController materialPropertyBlockController,
      bool onBeat,
      bool pastBeat)
    {
      MaterialPropertyBlock materialPropertyBlock = materialPropertyBlockController.materialPropertyBlock;
      materialPropertyBlock.SetFloat(BeatmapObjectMaterialHelpers.highlightId, onBeat ? 1f : 0.0f);
      materialPropertyBlock.SetFloat(BeatmapObjectMaterialHelpers.dimId, pastBeat ? 1f : 0.0f);
      materialPropertyBlockController.ApplyChanges();
    }

    public static void SetColorDataToMaterialPropertyBlocks(
      IEnumerable<MaterialPropertyBlockController> materialPropertyBlockControllers,
      Color color)
    {
      foreach (MaterialPropertyBlockController propertyBlockController in materialPropertyBlockControllers)
        BeatmapObjectMaterialHelpers.SetColorDataToMaterialPropertyBlock(propertyBlockController, color);
    }

    public static void SetBeatDataToMaterialPropertyBlocks(
      IEnumerable<MaterialPropertyBlockController> materialPropertyBlockControllers,
      bool onBeat,
      bool pastBeat)
    {
      foreach (MaterialPropertyBlockController propertyBlockController in materialPropertyBlockControllers)
        BeatmapObjectMaterialHelpers.SetBeatDataToMaterialPropertyBlock(propertyBlockController, onBeat, pastBeat);
    }
  }
}
