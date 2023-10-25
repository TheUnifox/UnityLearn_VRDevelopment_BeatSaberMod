using System;
using LiteNetLib.Utils;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200000D RID: 13
[Preserve]
[Serializable]
public class GameplayModifiers : INetImmutableSerializable<GameplayModifiers>
{
    // Token: 0x1700000A RID: 10
    // (get) Token: 0x06000063 RID: 99 RVA: 0x00003224 File Offset: 0x00001424
    public GameplayModifiers.EnergyType energyType
    {
        get
        {
            return this._energyType;
        }
    }

    // Token: 0x1700000B RID: 11
    // (get) Token: 0x06000064 RID: 100 RVA: 0x0000322C File Offset: 0x0000142C
    public bool noFailOn0Energy
    {
        get
        {
            return this._noFailOn0Energy;
        }
    }

    // Token: 0x1700000C RID: 12
    // (get) Token: 0x06000065 RID: 101 RVA: 0x00003234 File Offset: 0x00001434
    public bool instaFail
    {
        get
        {
            return this._instaFail;
        }
    }

    // Token: 0x1700000D RID: 13
    // (get) Token: 0x06000066 RID: 102 RVA: 0x0000323C File Offset: 0x0000143C
    public bool failOnSaberClash
    {
        get
        {
            return this._failOnSaberClash;
        }
    }

    // Token: 0x1700000E RID: 14
    // (get) Token: 0x06000067 RID: 103 RVA: 0x00003244 File Offset: 0x00001444
    public GameplayModifiers.EnabledObstacleType enabledObstacleType
    {
        get
        {
            return this._enabledObstacleType;
        }
    }

    // Token: 0x1700000F RID: 15
    // (get) Token: 0x06000068 RID: 104 RVA: 0x0000324C File Offset: 0x0000144C
    public bool fastNotes
    {
        get
        {
            return this._fastNotes;
        }
    }

    // Token: 0x17000010 RID: 16
    // (get) Token: 0x06000069 RID: 105 RVA: 0x00003254 File Offset: 0x00001454
    public bool strictAngles
    {
        get
        {
            return this._strictAngles;
        }
    }

    // Token: 0x17000011 RID: 17
    // (get) Token: 0x0600006A RID: 106 RVA: 0x0000325C File Offset: 0x0000145C
    public bool disappearingArrows
    {
        get
        {
            return this._disappearingArrows;
        }
    }

    // Token: 0x17000012 RID: 18
    // (get) Token: 0x0600006B RID: 107 RVA: 0x00003264 File Offset: 0x00001464
    public bool ghostNotes
    {
        get
        {
            return this._ghostNotes;
        }
    }

    // Token: 0x17000013 RID: 19
    // (get) Token: 0x0600006C RID: 108 RVA: 0x0000326C File Offset: 0x0000146C
    public bool noBombs
    {
        get
        {
            return this._noBombs;
        }
    }

    // Token: 0x17000014 RID: 20
    // (get) Token: 0x0600006D RID: 109 RVA: 0x00003274 File Offset: 0x00001474
    public GameplayModifiers.SongSpeed songSpeed
    {
        get
        {
            return this._songSpeed;
        }
    }

    // Token: 0x17000015 RID: 21
    // (get) Token: 0x0600006E RID: 110 RVA: 0x0000327C File Offset: 0x0000147C
    public bool noArrows
    {
        get
        {
            return this._noArrows;
        }
    }

    // Token: 0x17000016 RID: 22
    // (get) Token: 0x0600006F RID: 111 RVA: 0x00003284 File Offset: 0x00001484
    public bool proMode
    {
        get
        {
            return this._proMode;
        }
    }

    // Token: 0x17000017 RID: 23
    // (get) Token: 0x06000070 RID: 112 RVA: 0x0000328C File Offset: 0x0000148C
    public bool zenMode
    {
        get
        {
            return this._zenMode;
        }
    }

    // Token: 0x17000018 RID: 24
    // (get) Token: 0x06000071 RID: 113 RVA: 0x00003294 File Offset: 0x00001494
    public bool smallCubes
    {
        get
        {
            return this._smallCubes;
        }
    }

    // Token: 0x17000019 RID: 25
    // (get) Token: 0x06000072 RID: 114 RVA: 0x0000329C File Offset: 0x0000149C
    public float songSpeedMul
    {
        get
        {
            switch (this.songSpeed)
            {
                case GameplayModifiers.SongSpeed.Normal:
                    return 1f;
                case GameplayModifiers.SongSpeed.Faster:
                    return 1.2f;
                case GameplayModifiers.SongSpeed.Slower:
                    return 0.85f;
                case GameplayModifiers.SongSpeed.SuperFast:
                    return 1.5f;
                default:
                    return 1f;
            }
        }
    }

    // Token: 0x1700001A RID: 26
    // (get) Token: 0x06000073 RID: 115 RVA: 0x000032E5 File Offset: 0x000014E5
    public float cutAngleTolerance
    {
        get
        {
            if (this.strictAngles)
            {
                return 40f;
            }
            return 60f;
        }
    }

    // Token: 0x1700001B RID: 27
    // (get) Token: 0x06000074 RID: 116 RVA: 0x000032FA File Offset: 0x000014FA
    public float notesUniformScale
    {
        get
        {
            if (this.smallCubes)
            {
                return 0.5f;
            }
            return 1f;
        }
    }

    // Token: 0x06000075 RID: 117 RVA: 0x00003310 File Offset: 0x00001510
    public GameplayModifiers() : this(GameplayModifiers.EnergyType.Bar, false, false, false, GameplayModifiers.EnabledObstacleType.All, false, false, false, false, GameplayModifiers.SongSpeed.Normal, false, false, false, false, false)
    {
    }

    // Token: 0x06000076 RID: 118 RVA: 0x00003334 File Offset: 0x00001534
    public GameplayModifiers(GameplayModifiers.EnergyType energyType, bool noFailOn0Energy, bool instaFail, bool failOnSaberClash, GameplayModifiers.EnabledObstacleType enabledObstacleType, bool noBombs, bool fastNotes, bool strictAngles, bool disappearingArrows, GameplayModifiers.SongSpeed songSpeed, bool noArrows, bool ghostNotes, bool proMode, bool zenMode, bool smallCubes)
    {
        this._energyType = energyType;
        this._noFailOn0Energy = noFailOn0Energy;
        this._instaFail = instaFail;
        this._failOnSaberClash = failOnSaberClash;
        this._enabledObstacleType = enabledObstacleType;
        this._noBombs = noBombs;
        this._fastNotes = fastNotes;
        this._strictAngles = strictAngles;
        this._disappearingArrows = disappearingArrows;
        this._songSpeed = songSpeed;
        this._noArrows = noArrows;
        this._ghostNotes = ghostNotes;
        this._proMode = proMode;
        this._zenMode = zenMode;
        this._smallCubes = smallCubes;
    }

    // Token: 0x06000077 RID: 119 RVA: 0x000033BC File Offset: 0x000015BC
    public virtual GameplayModifiers CopyWith(GameplayModifiers.EnergyType? energyType = null, bool? noFailOn0Energy = null, bool? instaFail = null, bool? failOnSaberClash = null, GameplayModifiers.EnabledObstacleType? enabledObstacleType = null, bool? noBombs = null, bool? fastNotes = null, bool? strictAngles = null, bool? disappearingArrows = null, GameplayModifiers.SongSpeed? songSpeed = null, bool? noArrows = null, bool? ghostNotes = null, bool? proMode = null, bool? zenMode = null, bool? smallCubes = null)
    {
        return new GameplayModifiers(energyType ?? this._energyType, noFailOn0Energy ?? this._noFailOn0Energy, instaFail ?? this._instaFail, failOnSaberClash ?? this._failOnSaberClash, enabledObstacleType ?? this._enabledObstacleType, noBombs ?? this._noBombs, fastNotes ?? this._fastNotes, strictAngles ?? this._strictAngles, disappearingArrows ?? this._disappearingArrows, songSpeed ?? this._songSpeed, noArrows ?? this._noArrows, ghostNotes ?? this._ghostNotes, proMode ?? this._proMode, zenMode ?? this._zenMode, smallCubes ?? this._smallCubes);
    }

    // Token: 0x06000078 RID: 120 RVA: 0x00003560 File Offset: 0x00001760
    public virtual bool IsWithoutModifiers()
    {
        return this.energyType == GameplayModifiers.EnergyType.Bar && !this.noFailOn0Energy && !this.instaFail && !this.failOnSaberClash && this.enabledObstacleType == GameplayModifiers.EnabledObstacleType.All && !this.noBombs && !this.fastNotes && !this.strictAngles && !this.disappearingArrows && this.songSpeed == GameplayModifiers.SongSpeed.Normal && !this.noArrows && !this.ghostNotes && !this.proMode && !this.zenMode && !this.smallCubes;
    }

    // Token: 0x06000079 RID: 121 RVA: 0x000035E8 File Offset: 0x000017E8
    public virtual void Serialize(NetDataWriter writer)
    {
        int num = 0;
        num |= (int)(this.energyType & (GameplayModifiers.EnergyType)15);
        num |= (this.instaFail ? 0b1000000 : 0);
        num |= (this.failOnSaberClash ? 0b10000000 : 0);
        num |= (int)((int)(this.enabledObstacleType & (GameplayModifiers.EnabledObstacleType)15) << 8);
        num |= (this.noBombs ? 0b10000000000000 : 0);
        num |= (this.fastNotes ? 0b100000000000000 : 0);
        num |= (this.strictAngles ? 0b1000000000000000 : 0);
        num |= (this.disappearingArrows ? 0b10000000000000000 : 0);
        num |= (this.ghostNotes ? 0b100000000000000000 : 0);
        num |= (int)((int)(this.songSpeed & (GameplayModifiers.SongSpeed)15) << 18);
        num |= (this.noArrows ? 0b10000000000000000000000 : 0);
        num |= (this.noFailOn0Energy ? 0b100000000000000000000000 : 0);
        num |= (this.proMode ? 0b1000000000000000000000000 : 0);
        num |= (this.zenMode ? 0b10000000000000000000000000 : 0);
        num |= (this.smallCubes ? 0b100000000000000000000000000 : 0);
        writer.Put(num);
    }

    // Token: 0x0600007A RID: 122 RVA: 0x00003708 File Offset: 0x00001908
    GameplayModifiers INetImmutableSerializable<GameplayModifiers>.CreateFromSerializedData(NetDataReader reader)
    {
        return GameplayModifiers.CreateFromSerializedData(reader);
    }

    // Token: 0x0600007B RID: 123 RVA: 0x00003710 File Offset: 0x00001910
    public static GameplayModifiers CreateFromSerializedData(NetDataReader reader)
    {
        int @int = reader.GetInt();
        GameplayModifiers.EnergyType energyType = (GameplayModifiers.EnergyType)(@int & 15);
        bool instaFail = (@int & 64) != 0;
        bool failOnSaberClash = (@int & 128) != 0;
        GameplayModifiers.EnabledObstacleType enabledObstacleType = (GameplayModifiers.EnabledObstacleType)(@int >> 8 & 15);
        bool noBombs = (@int & 8192) != 0;
        bool fastNotes = (@int & 16384) != 0;
        bool strictAngles = (@int & 32768) != 0;
        bool disappearingArrows = (@int & 65536) != 0;
        bool ghostNotes = (@int & 131072) != 0;
        GameplayModifiers.SongSpeed songSpeed = (GameplayModifiers.SongSpeed)(@int >> 18 & 15);
        bool noArrows = (@int & 4194304) != 0;
        return new GameplayModifiers(energyType, (@int & 8388608) != 0, instaFail, failOnSaberClash, enabledObstacleType, noBombs, fastNotes, strictAngles, disappearingArrows, songSpeed, noArrows, ghostNotes, (@int & 16777216) != 0, (@int & 33554432) != 0, (@int & 67108864) != 0);
    }

    // Token: 0x0400002B RID: 43
    [SerializeField]
    protected GameplayModifiers.EnergyType _energyType;

    // Token: 0x0400002C RID: 44
    [SerializeField]
    protected bool _noFailOn0Energy;

    // Token: 0x0400002D RID: 45
    [SerializeField]
    protected bool _instaFail;

    // Token: 0x0400002E RID: 46
    [SerializeField]
    protected bool _failOnSaberClash;

    // Token: 0x0400002F RID: 47
    [SerializeField]
    protected GameplayModifiers.EnabledObstacleType _enabledObstacleType;

    // Token: 0x04000030 RID: 48
    [SerializeField]
    protected bool _fastNotes;

    // Token: 0x04000031 RID: 49
    [SerializeField]
    protected bool _strictAngles;

    // Token: 0x04000032 RID: 50
    [SerializeField]
    protected bool _disappearingArrows;

    // Token: 0x04000033 RID: 51
    [SerializeField]
    protected bool _ghostNotes;

    // Token: 0x04000034 RID: 52
    [SerializeField]
    protected bool _noBombs;

    // Token: 0x04000035 RID: 53
    [SerializeField]
    protected GameplayModifiers.SongSpeed _songSpeed;

    // Token: 0x04000036 RID: 54
    [SerializeField]
    protected bool _noArrows;

    // Token: 0x04000037 RID: 55
    [SerializeField]
    protected bool _proMode;

    // Token: 0x04000038 RID: 56
    [SerializeField]
    protected bool _zenMode;

    // Token: 0x04000039 RID: 57
    [SerializeField]
    protected bool _smallCubes;

    // Token: 0x0400003A RID: 58
    [DoesNotRequireDomainReloadInit]
    public static readonly GameplayModifiers noModifiers = new GameplayModifiers();

    // Token: 0x0200000E RID: 14
    public enum EnabledObstacleType
    {
        // Token: 0x0400003C RID: 60
        All,
        // Token: 0x0400003D RID: 61
        FullHeightOnly,
        // Token: 0x0400003E RID: 62
        NoObstacles
    }

    // Token: 0x0200000F RID: 15
    public enum EnergyType
    {
        // Token: 0x04000040 RID: 64
        Bar,
        // Token: 0x04000041 RID: 65
        Battery
    }

    // Token: 0x02000010 RID: 16
    public enum SongSpeed
    {
        // Token: 0x04000043 RID: 67
        Normal,
        // Token: 0x04000044 RID: 68
        Faster,
        // Token: 0x04000045 RID: 69
        Slower,
        // Token: 0x04000046 RID: 70
        SuperFast
    }
}
