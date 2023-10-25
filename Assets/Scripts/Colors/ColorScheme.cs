using System;
using UnityEngine;

// Token: 0x02000003 RID: 3
[Serializable]
public class ColorScheme
{
    // Token: 0x17000002 RID: 2
    // (get) Token: 0x06000004 RID: 4 RVA: 0x0000206F File Offset: 0x0000026F
    public string colorSchemeId
    {
        get
        {
            return this._colorSchemeId;
        }
    }

    // Token: 0x17000003 RID: 3
    // (get) Token: 0x06000005 RID: 5 RVA: 0x00002077 File Offset: 0x00000277
    public string colorSchemeNameLocalizationKey
    {
        get
        {
            return this._colorSchemeNameLocalizationKey;
        }
    }

    // Token: 0x17000004 RID: 4
    // (get) Token: 0x06000006 RID: 6 RVA: 0x0000207F File Offset: 0x0000027F
    public string nonLocalizedName
    {
        get
        {
            return this._nonLocalizedName;
        }
    }

    // Token: 0x17000005 RID: 5
    // (get) Token: 0x06000007 RID: 7 RVA: 0x00002087 File Offset: 0x00000287
    public bool useNonLocalizedName
    {
        get
        {
            return this._useNonLocalizedName;
        }
    }

    // Token: 0x17000006 RID: 6
    // (get) Token: 0x06000008 RID: 8 RVA: 0x0000208F File Offset: 0x0000028F
    public bool isEditable
    {
        get
        {
            return this._isEditable;
        }
    }

    // Token: 0x17000007 RID: 7
    // (get) Token: 0x06000009 RID: 9 RVA: 0x00002097 File Offset: 0x00000297
    public Color saberAColor
    {
        get
        {
            return this._saberAColor;
        }
    }

    // Token: 0x17000008 RID: 8
    // (get) Token: 0x0600000A RID: 10 RVA: 0x0000209F File Offset: 0x0000029F
    public Color saberBColor
    {
        get
        {
            return this._saberBColor;
        }
    }

    // Token: 0x17000009 RID: 9
    // (get) Token: 0x0600000B RID: 11 RVA: 0x000020A7 File Offset: 0x000002A7
    public Color environmentColor0
    {
        get
        {
            return this._environmentColor0;
        }
    }

    // Token: 0x1700000A RID: 10
    // (get) Token: 0x0600000C RID: 12 RVA: 0x000020AF File Offset: 0x000002AF
    public Color environmentColor1
    {
        get
        {
            return this._environmentColor1;
        }
    }

    // Token: 0x1700000B RID: 11
    // (get) Token: 0x0600000D RID: 13 RVA: 0x000020B7 File Offset: 0x000002B7
    public Color environmentColorW
    {
        get
        {
            return this._environmentColorW;
        }
    }

    // Token: 0x1700000C RID: 12
    // (get) Token: 0x0600000E RID: 14 RVA: 0x000020BF File Offset: 0x000002BF
    public bool supportsEnvironmentColorBoost
    {
        get
        {
            return this._supportsEnvironmentColorBoost;
        }
    }

    // Token: 0x1700000D RID: 13
    // (get) Token: 0x0600000F RID: 15 RVA: 0x000020C7 File Offset: 0x000002C7
    public Color environmentColor0Boost
    {
        get
        {
            return this._environmentColor0Boost;
        }
    }

    // Token: 0x1700000E RID: 14
    // (get) Token: 0x06000010 RID: 16 RVA: 0x000020CF File Offset: 0x000002CF
    public Color environmentColor1Boost
    {
        get
        {
            return this._environmentColor1Boost;
        }
    }

    // Token: 0x1700000F RID: 15
    // (get) Token: 0x06000011 RID: 17 RVA: 0x000020D7 File Offset: 0x000002D7
    public Color environmentColorWBoost
    {
        get
        {
            return this._environmentColorWBoost;
        }
    }

    // Token: 0x17000010 RID: 16
    // (get) Token: 0x06000012 RID: 18 RVA: 0x000020DF File Offset: 0x000002DF
    public Color obstaclesColor
    {
        get
        {
            return this._obstaclesColor;
        }
    }

    // Token: 0x06000013 RID: 19 RVA: 0x000020E8 File Offset: 0x000002E8
    public ColorScheme(string colorSchemeId, string colorSchemeNameLocalizationKey, bool useNonLocalizedName, string nonLocalizedName, bool isEditable, Color saberAColor, Color saberBColor, Color environmentColor0, Color environmentColor1, bool supportsEnvironmentColorBoost, Color environmentColor0Boost, Color environmentColor1Boost, Color obstaclesColor)
    {
        this._colorSchemeId = colorSchemeId;
        this._colorSchemeNameLocalizationKey = colorSchemeNameLocalizationKey;
        this._isEditable = isEditable;
        this._saberAColor = saberAColor;
        this._saberBColor = saberBColor;
        this._environmentColor0 = environmentColor0;
        this._environmentColor1 = environmentColor1;
        this._supportsEnvironmentColorBoost = supportsEnvironmentColorBoost;
        this._environmentColor0Boost = environmentColor0Boost;
        this._environmentColor1Boost = environmentColor1Boost;
        this._obstaclesColor = obstaclesColor;
        this._nonLocalizedName = nonLocalizedName;
        this._useNonLocalizedName = useNonLocalizedName;
    }

    // Token: 0x06000014 RID: 20 RVA: 0x00002178 File Offset: 0x00000378
    public ColorScheme(ColorScheme colorScheme, Color saberAColor, Color saberBColor, Color environmentColor0, Color environmentColor1, bool supportsEnvironmentColorBoost, Color environmentColor0Boost, Color environmentColor1Boost, Color obstaclesColor) : this(colorScheme.colorSchemeId, colorScheme.colorSchemeNameLocalizationKey, colorScheme.useNonLocalizedName, colorScheme.nonLocalizedName, colorScheme.isEditable, saberAColor, saberBColor, environmentColor0, environmentColor1, supportsEnvironmentColorBoost, environmentColor0Boost, environmentColor1Boost, obstaclesColor)
    {
    }

    // Token: 0x06000015 RID: 21 RVA: 0x000021B8 File Offset: 0x000003B8
    public ColorScheme(ColorScheme colorScheme) : this(colorScheme.colorSchemeId, colorScheme.colorSchemeNameLocalizationKey, colorScheme.useNonLocalizedName, colorScheme.nonLocalizedName, colorScheme.isEditable, colorScheme.saberAColor, colorScheme.saberBColor, colorScheme.environmentColor0, colorScheme.environmentColor1, colorScheme.supportsEnvironmentColorBoost, colorScheme.environmentColor0Boost, colorScheme.environmentColor1Boost, colorScheme.obstaclesColor)
    {
    }

    // Token: 0x06000016 RID: 22 RVA: 0x00002219 File Offset: 0x00000419
    public ColorScheme(ColorSchemeSO colorScheme) : this(colorScheme.colorScheme)
    {
    }

    // Token: 0x04000001 RID: 1
    [SerializeField]
    private string _colorSchemeId;

    // Token: 0x04000002 RID: 2
    [SerializeField]
    [LocalizationKey]
    private string _colorSchemeNameLocalizationKey;

    // Token: 0x04000003 RID: 3
    [SerializeField]
    private bool _useNonLocalizedName;

    // Token: 0x04000004 RID: 4
    [SerializeField]
    private string _nonLocalizedName;

    // Token: 0x04000005 RID: 5
    [SerializeField]
    private bool _isEditable;

    // Token: 0x04000006 RID: 6
    [Space]
    [SerializeField]
    private Color _saberAColor;

    // Token: 0x04000007 RID: 7
    [SerializeField]
    private Color _saberBColor;

    // Token: 0x04000008 RID: 8
    [Space]
    [SerializeField]
    private Color _obstaclesColor;

    // Token: 0x04000009 RID: 9
    [Space]
    [SerializeField]
    private Color _environmentColor0;

    // Token: 0x0400000A RID: 10
    [SerializeField]
    private Color _environmentColor1;

    // Token: 0x0400000B RID: 11
    [SerializeField]
    private Color _environmentColorW = Color.white;

    // Token: 0x0400000C RID: 12
    [SerializeField]
    private bool _supportsEnvironmentColorBoost;

    // Token: 0x0400000D RID: 13
    [SerializeField]
    private Color _environmentColor0Boost;

    // Token: 0x0400000E RID: 14
    [SerializeField]
    private Color _environmentColor1Boost;

    // Token: 0x0400000F RID: 15
    [SerializeField]
    private Color _environmentColorWBoost = Color.white;
}
