using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class MultipliedAndAddedColorSO : ColorSO
{
    // Token: 0x17000013 RID: 19
    // (get) Token: 0x0600001B RID: 27 RVA: 0x0000223F File Offset: 0x0000043F
    public override Color color
    {
        get
        {
            return this._multiplierColor * this._baseColor.color + this._addColor;
        }
    }

    // Token: 0x04000012 RID: 18
    [SerializeField]
    private SimpleColorSO _baseColor;

    // Token: 0x04000013 RID: 19
    [SerializeField]
    private Color _multiplierColor;

    // Token: 0x04000014 RID: 20
    [SerializeField]
    private Color _addColor;
}
