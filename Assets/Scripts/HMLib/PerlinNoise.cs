// Decompiled with JetBrains decompiler
// Type: PerlinNoise
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

public class PerlinNoise
{
  [DoesNotRequireDomainReloadInit]
  protected static readonly int[] permutation = new int[256]
  {
    151,
    160,
    137,
    91,
    90,
    15,
    131,
    13,
    201,
    95,
    96,
    53,
    194,
    233,
    7,
    225,
    140,
    36,
    103,
    30,
    69,
    142,
    8,
    99,
    37,
    240,
    21,
    10,
    23,
    190,
    6,
    148,
    247,
    120,
    234,
    75,
    0,
    26,
    197,
    62,
    94,
    252,
    219,
    203,
    117,
    35,
    11,
    32,
    57,
    177,
    33,
    88,
    237,
    149,
    56,
    87,
    174,
    20,
    125,
    136,
    171,
    168,
    68,
    175,
    74,
    165,
    71,
    134,
    139,
    48,
    27,
    166,
    77,
    146,
    158,
    231,
    83,
    111,
    229,
    122,
    60,
    211,
    133,
    230,
    220,
    105,
    92,
    41,
    55,
    46,
    245,
    40,
    244,
    102,
    143,
    54,
    65,
    25,
    63,
    161,
    1,
    216,
    80,
    73,
    209,
    76,
    132,
    187,
    208,
    89,
    18,
    169,
    200,
    196,
    135,
    130,
    116,
    188,
    159,
    86,
    164,
    100,
    109,
    198,
    173,
    186,
    3,
    64,
    52,
    217,
    226,
    250,
    124,
    123,
    5,
    202,
    38,
    147,
    118,
    126,
    (int) byte.MaxValue,
    82,
    85,
    212,
    207,
    206,
    59,
    227,
    47,
    16,
    58,
    17,
    182,
    189,
    28,
    42,
    223,
    183,
    170,
    213,
    119,
    248,
    152,
    2,
    44,
    154,
    163,
    70,
    221,
    153,
    101,
    155,
    167,
    43,
    172,
    9,
    129,
    22,
    39,
    253,
    19,
    98,
    108,
    110,
    79,
    113,
    224,
    232,
    178,
    185,
    112,
    104,
    218,
    246,
    97,
    228,
    251,
    34,
    242,
    193,
    238,
    210,
    144,
    12,
    191,
    179,
    162,
    241,
    81,
    51,
    145,
    235,
    249,
    14,
    239,
    107,
    49,
    192,
    214,
    31,
    181,
    199,
    106,
    157,
    184,
    84,
    204,
    176,
    115,
    121,
    50,
    45,
    (int) sbyte.MaxValue,
    4,
    150,
    254,
    138,
    236,
    205,
    93,
    222,
    114,
    67,
    29,
    24,
    72,
    243,
    141,
    128,
    195,
    78,
    66,
    215,
    61,
    156,
    180
  };
  [DoesNotRequireDomainReloadInit]
  protected static readonly int[] p = new int[512];

  static PerlinNoise()
  {
    for (int index = 0; index < 512; ++index)
      PerlinNoise.p[index] = PerlinNoise.permutation[index % 256];
  }

  public static float OctavePerlin3D(
    float x,
    float y,
    float z,
    int octaves,
    float persistence,
    int repeat = 0)
  {
    float num1 = 0.0f;
    float num2 = 1f;
    float num3 = 1f;
    float num4 = 0.0f;
    for (int index = 0; index < octaves; ++index)
    {
      num1 += PerlinNoise.Perlin3D(x * num2, y * num2, z * num2, repeat) * num3;
      num4 += num3;
      num3 *= persistence;
      num2 *= 2f;
    }
    return num1 / num4;
  }

  public static float Perlin3D(float x, float y, float z, int repeat)
  {
    if (repeat > 0)
    {
      x %= (float) repeat;
      y %= (float) repeat;
      z %= (float) repeat;
    }
    int num1 = (int) x & (int) byte.MaxValue;
    int num2 = (int) y & (int) byte.MaxValue;
    int num3 = (int) z & (int) byte.MaxValue;
    float num4 = x - (float) (int) x;
    float num5 = y - (float) (int) y;
    float num6 = z - (float) (int) z;
    float x1 = PerlinNoise.Fade(num4);
    float x2 = PerlinNoise.Fade(num5);
    float x3 = PerlinNoise.Fade(num6);
    int hash1 = PerlinNoise.p[PerlinNoise.p[PerlinNoise.p[num1] + num2] + num3];
    int hash2 = PerlinNoise.p[PerlinNoise.p[PerlinNoise.p[num1] + PerlinNoise.Inc(num2, repeat)] + num3];
    int hash3 = PerlinNoise.p[PerlinNoise.p[PerlinNoise.p[num1] + num2] + PerlinNoise.Inc(num3, repeat)];
    int hash4 = PerlinNoise.p[PerlinNoise.p[PerlinNoise.p[num1] + PerlinNoise.Inc(num2, repeat)] + PerlinNoise.Inc(num3, repeat)];
    int hash5 = PerlinNoise.p[PerlinNoise.p[PerlinNoise.p[PerlinNoise.Inc(num1, repeat)] + num2] + num3];
    int hash6 = PerlinNoise.p[PerlinNoise.p[PerlinNoise.p[PerlinNoise.Inc(num1, repeat)] + PerlinNoise.Inc(num2, repeat)] + num3];
    int hash7 = PerlinNoise.p[PerlinNoise.p[PerlinNoise.p[PerlinNoise.Inc(num1, repeat)] + num2] + PerlinNoise.Inc(num3, repeat)];
    int hash8 = PerlinNoise.p[PerlinNoise.p[PerlinNoise.p[PerlinNoise.Inc(num1, repeat)] + PerlinNoise.Inc(num2, repeat)] + PerlinNoise.Inc(num3, repeat)];
    double x4 = (double) num4;
    double y1 = (double) num5;
    double z1 = (double) num6;
    return (float) (((double) PerlinNoise.Lerp(PerlinNoise.Lerp(PerlinNoise.Lerp(PerlinNoise.Grad3D(hash1, (float) x4, (float) y1, (float) z1), PerlinNoise.Grad3D(hash5, num4 - 1f, num5, num6), x1), PerlinNoise.Lerp(PerlinNoise.Grad3D(hash2, num4, num5 - 1f, num6), PerlinNoise.Grad3D(hash6, num4 - 1f, num5 - 1f, num6), x1), x2), PerlinNoise.Lerp(PerlinNoise.Lerp(PerlinNoise.Grad3D(hash3, num4, num5, num6 - 1f), PerlinNoise.Grad3D(hash7, num4 - 1f, num5, num6 - 1f), x1), PerlinNoise.Lerp(PerlinNoise.Grad3D(hash4, num4, num5 - 1f, num6 - 1f), PerlinNoise.Grad3D(hash8, num4 - 1f, num5 - 1f, num6 - 1f), x1), x2), x3) + 1.0) / 2.0);
  }

  private static int Inc(int num, int repeat)
  {
    ++num;
    if (repeat > 0)
      num %= repeat;
    return num;
  }

  private static float Grad3D(int hash, float x, float y, float z)
  {
    switch (hash & 15)
    {
      case 0:
        return x + y;
      case 1:
        return -x + y;
      case 2:
        return x - y;
      case 3:
        return -x - y;
      case 4:
        return x + z;
      case 5:
        return -x + z;
      case 6:
        return x - z;
      case 7:
        return -x - z;
      case 8:
        return y + z;
      case 9:
        return -y + z;
      case 10:
        return y - z;
      case 11:
        return -y - z;
      case 12:
        return y + x;
      case 13:
        return -y + z;
      case 14:
        return y - x;
      case 15:
        return -y - z;
      default:
        return 0.0f;
    }
  }

  private static float Fade(float t) => (float) ((double) t * (double) t * (double) t * ((double) t * ((double) t * 6.0 - 15.0) + 10.0));

  private static float Lerp(float a, float b, float x) => a + x * (b - a);
}
