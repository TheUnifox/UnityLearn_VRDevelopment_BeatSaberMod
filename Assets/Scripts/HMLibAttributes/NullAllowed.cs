using System;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class NullAllowed : PropertyAttribute
{
    // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
    public NullAllowed() : this(NullAllowed.Context.Everywhere)
    {
    }

    // Token: 0x06000002 RID: 2 RVA: 0x00002059 File Offset: 0x00000259
    public NullAllowed(NullAllowed.Context context)
    {
        this.context = context;
    }

    // Token: 0x06000003 RID: 3 RVA: 0x00002068 File Offset: 0x00000268
    public NullAllowed(string propertyName, object ifNotValue)
    {
        this.context = NullAllowed.Context.Everywhere;
        this.propertyName = propertyName;
        this.ifNotValue = ifNotValue;
    }

    // Token: 0x04000001 RID: 1
    public readonly NullAllowed.Context context;

    // Token: 0x04000002 RID: 2
    public readonly string propertyName;

    // Token: 0x04000003 RID: 3
    public readonly object ifNotValue;

    // Token: 0x02000003 RID: 3
    public enum Context
    {
        // Token: 0x04000005 RID: 5
        Everywhere,
        // Token: 0x04000006 RID: 6
        Prefab
    }
}
