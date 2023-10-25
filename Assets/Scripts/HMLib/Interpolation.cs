// Decompiled with JetBrains decompiler
// Type: Interpolation
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

public abstract class Interpolation
{
  public static float Interpolate(float t, EaseType easeType)
  {
    switch (easeType)
    {
      case EaseType.Linear:
        return Easing.Linear(t);
      case EaseType.InSine:
        return Easing.InSine(t);
      case EaseType.OutSine:
        return Easing.OutSine(t);
      case EaseType.InOutSine:
        return Easing.InOutSine(t);
      case EaseType.InQuad:
        return Easing.InQuad(t);
      case EaseType.OutQuad:
        return Easing.OutQuad(t);
      case EaseType.InOutQuad:
        return Easing.InOutQuad(t);
      case EaseType.InCubic:
        return Easing.InCubic(t);
      case EaseType.OutCubic:
        return Easing.OutCubic(t);
      case EaseType.InOutCubic:
        return Easing.InOutCubic(t);
      case EaseType.InQuart:
        return Easing.InQuart(t);
      case EaseType.OutQuart:
        return Easing.OutQuart(t);
      case EaseType.InOutQuart:
        return Easing.InOutQuart(t);
      case EaseType.InQuint:
        return Easing.InQuint(t);
      case EaseType.OutQuint:
        return Easing.OutQuint(t);
      case EaseType.InOutQuint:
        return Easing.InOutQuint(t);
      case EaseType.InExpo:
        return Easing.InExpo(t);
      case EaseType.OutExpo:
        return Easing.OutExpo(t);
      case EaseType.InOutExpo:
        return Easing.InOutExpo(t);
      case EaseType.InCirc:
        return Easing.InCirc(t);
      case EaseType.OutCirc:
        return Easing.OutCirc(t);
      case EaseType.InOutCirc:
        return Easing.InOutCirc(t);
      case EaseType.InBack:
        return Easing.InBack(t);
      case EaseType.OutBack:
        return Easing.OutBack(t);
      case EaseType.InOutBack:
        return Easing.InOutBack(t);
      case EaseType.InElastic:
        return Easing.InElastic(t);
      case EaseType.OutElastic:
        return Easing.OutElastic(t);
      case EaseType.InOutElastic:
        return Easing.InOutElastic(t);
      case EaseType.InBounce:
        return Easing.InBounce(t);
      case EaseType.OutBounce:
        return Easing.OutBounce(t);
      case EaseType.InOutBounce:
        return Easing.InOutBounce(t);
      default:
        return Easing.Linear(t);
    }
  }
}
