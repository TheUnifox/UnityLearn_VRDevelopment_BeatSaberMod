// Decompiled with JetBrains decompiler
// Type: BeatmapEditor3D.BeatSubdivisionsModel
// Assembly: BeatmapEditor3D, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F08665C-E1B6-4752-A219-2B54516F316A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\BeatmapEditor3D.dll

using UnityEngine;

namespace BeatmapEditor3D
{
  public class BeatSubdivisionsModel
  {
    private const int kSubdivisionLimit = 800;
    private int _subdivisionId;
    private int _multiplicationId;
    private int _prevSubdivisionId;
    private int _prevMultiplicationId;
    private readonly int[] _baseSubdivision = new int[6]
    {
      4,
      12,
      20,
      28,
      36,
      44
    };
    private readonly int[] _multiplications = new int[7]
    {
      1,
      2,
      4,
      8,
      16,
      32,
      64
    };
    private readonly int[] _subdivisionStartSubdivisions = new int[6]
    {
      16,
      12,
      20,
      28,
      36,
      44
    };
    private readonly float[] _subdivisionIncrements = new float[6]
    {
      0.25f,
      0.334f,
      0.2f,
      0.143f,
      0.112f,
      0.091f
    };

    public int currentSubdivision => this._baseSubdivision[this._subdivisionId] * this._multiplications[this._multiplicationId];

    public int prevSubdivision => this._baseSubdivision[this._prevSubdivisionId] * this._multiplications[this._prevMultiplicationId];

    public float currentSubdivisionIncrement => this._subdivisionIncrements[this._subdivisionId];

    public int currentStartSubdivision => this._subdivisionStartSubdivisions[this._subdivisionId];

    public BeatSubdivisionsModel() => this.Reset();

    public void Reset()
    {
      this._subdivisionId = 0;
      this._multiplicationId = 0;
      this._prevSubdivisionId = 1;
      this._prevMultiplicationId = 0;
    }

    public void Swap()
    {
      int prevSubdivisionId = this._prevSubdivisionId;
      int subdivisionId = this._subdivisionId;
      this._subdivisionId = prevSubdivisionId;
      this._prevSubdivisionId = subdivisionId;
      int multiplicationId1 = this._prevMultiplicationId;
      int multiplicationId2 = this._multiplicationId;
      this._multiplicationId = multiplicationId1;
      this._prevMultiplicationId = multiplicationId2;
    }

    public void NextSubdivision()
    {
      this._subdivisionId = Mathf.Min(this._subdivisionId + 1, this._baseSubdivision.Length - 1);
      this._multiplicationId = Mathf.Min(this._multiplicationId, (int) Mathf.Log((float) Mathf.ClosestPowerOfTwo(800 / this._baseSubdivision[this._subdivisionId]), 2f));
    }

    public void PreviousSubdivision() => this._subdivisionId = Mathf.Max(this._subdivisionId - 1, 0);

    public void NextMultiplication()
    {
      this._multiplicationId = Mathf.Min(this._multiplicationId + 1, this._multiplications.Length - 1);
      this._multiplicationId = Mathf.Min(this._multiplicationId, (int) Mathf.Log((float) Mathf.ClosestPowerOfTwo(800 / this._baseSubdivision[this._subdivisionId]), 2f));
    }

    public void PreviousMultiplication() => this._multiplicationId = Mathf.Max(this._multiplicationId - 1, 0);
  }
}
