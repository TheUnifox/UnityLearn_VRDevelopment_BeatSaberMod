using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class LayerMasks
{
    // Token: 0x06000005 RID: 5 RVA: 0x00002108 File Offset: 0x00000308
    private static LayerMask GetLayerMask(string layerName)
    {
        int num = LayerMask.NameToLayer(layerName);
        return new LayerMask
        {
            value = 1 << num
        };
    }

    // Token: 0x06000006 RID: 6 RVA: 0x00002134 File Offset: 0x00000334
    private static LayerMask GetLayerMask(int layerNum)
    {
        return new LayerMask
        {
            value = 1 << layerNum
        };
    }

    // Token: 0x06000007 RID: 7 RVA: 0x00002157 File Offset: 0x00000357
    private static int GetLayer(string layerName)
    {
        return LayerMask.NameToLayer(layerName);
    }

    // Token: 0x06000008 RID: 8 RVA: 0x00002100 File Offset: 0x00000300
    public LayerMasks()
    {
    }

    // Token: 0x06000009 RID: 9 RVA: 0x00002160 File Offset: 0x00000360
    // Note: this type is marked as 'beforefieldinit'.
    static LayerMasks()
    {
    }

    // Token: 0x04000002 RID: 2
    [DoesNotRequireDomainReloadInit]
    public static readonly LayerMask saberLayerMask = LayerMasks.GetLayerMask("Saber");

    // Token: 0x04000003 RID: 3
    [DoesNotRequireDomainReloadInit]
    public static readonly LayerMask noteLayerMask = LayerMasks.GetLayerMask("Note");

    // Token: 0x04000004 RID: 4
    [DoesNotRequireDomainReloadInit]
    public static readonly LayerMask noteDebrisLayerMask = LayerMasks.GetLayerMask(LayerMasks.noteDebrisLayer);

    // Token: 0x04000005 RID: 5
    [DoesNotRequireDomainReloadInit]
    public static readonly LayerMask cutEffectParticlesLayerMask = LayerMasks.GetLayerMask(LayerMasks.cutEffectParticlesLayer);

    // Token: 0x04000006 RID: 6
    [DoesNotRequireDomainReloadInit]
    public static readonly int noteDebrisLayer = LayerMasks.GetLayer("NoteDebris");

    // Token: 0x04000007 RID: 7
    [DoesNotRequireDomainReloadInit]
    public static readonly int cutEffectParticlesLayer = LayerMasks.GetLayer("CutEffectParticles");
}
