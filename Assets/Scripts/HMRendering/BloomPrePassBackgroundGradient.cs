// Decompiled with JetBrains decompiler
// Type: BloomPrePassBackgroundGradient
// Assembly: HMRendering, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C55B48F-2592-4126-9F83-BBF1ACE1B216
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMRendering.dll

using Unity.Collections;
using UnityEngine;

public class BloomPrePassBackgroundGradient : BloomPrePassBackgroundTextureGradient
{
  [SerializeField]
  protected Gradient _gradient;

  protected override void UpdatePixels(NativeArray<Color32> pixels, int numberOfPixels)
  {
    for (int index = 0; index < numberOfPixels; ++index)
      pixels[index] = (Color32) this._gradient.Evaluate((float) index / (float) (numberOfPixels - 1));
  }
}
