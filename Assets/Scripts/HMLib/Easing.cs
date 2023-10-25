// Decompiled with JetBrains decompiler
// Type: Easing
// Assembly: HMLib, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8CC76D59-1DD6-42EE-8DB5-092A3F1E1FFA
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\HMLib.dll

using UnityEngine;

public abstract class Easing
{
  public static float Linear(float t) => t;

  public static float InSine(float t) => 1f - Mathf.Cos((float) ((double) t * 3.1415927410125732 / 2.0));

  public static float OutSine(float t) => Mathf.Sin((float) ((double) t * 3.1415927410125732 / 2.0));

  public static float InOutSine(float t) => (float) (-((double) Mathf.Cos(3.14159274f * t) - 1.0) / 2.0);

  public static float InQuad(float t) => t * t;

  public static float OutQuad(float t) => (float) (1.0 - (1.0 - (double) t) * (1.0 - (double) t));

  public static float InOutQuad(float t) => (double) t >= 0.5 ? (float) ((4.0 - 2.0 * (double) t) * (double) t - 1.0) : 2f * t * t;

  public static float InCubic(float t) => t * t * t;

  public static float OutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);

  public static float InOutCubic(float t) => (double) t >= 0.5 ? (float) (1.0 - (double) Mathf.Pow((float) (-2.0 * (double) t + 2.0), 3f) / 2.0) : 4f * t * t * t;

  public static float InQuart(float t) => t * t * t * t;

  public static float OutQuart(float t) => 1f - Mathf.Pow(1f - t, 4f);

  public static float InOutQuart(float t) => (double) t >= 0.5 ? (float) (1.0 - (double) Mathf.Pow((float) (-2.0 * (double) t + 2.0), 4f) * 0.5) : 8f * t * t * t * t;

  public static float InQuint(float t) => t * t * t * t * t;

  public static float OutQuint(float t) => 1f - Mathf.Pow(1f - t, 5f);

  public static float InOutQuint(float t) => (double) t >= 0.5 ? (float) (1.0 - (double) Mathf.Pow((float) (-2.0 * (double) t + 2.0), 5f) / 2.0) : 16f * t * t * t * t * t;

  public static float InExpo(float t) => (double) t != 0.0 ? Mathf.Pow(2f, (float) (10.0 * (double) t - 10.0)) : 0.0f;

  public static float OutExpo(float t) => (double) t != 1.0 ? 1f - Mathf.Pow(2f, -10f * t) : 1f;

  public static float InOutExpo(float t)
  {
    if ((double) t == 0.0)
      return 0.0f;
    if ((double) t == 1.0)
      return 1f;
    return (double) t >= 0.5 ? (float) ((2.0 - (double) Mathf.Pow(2f, (float) (-20.0 * (double) t + 10.0))) / 2.0) : Mathf.Pow(2f, (float) (20.0 * (double) t - 10.0)) / 2f;
  }

  public static float InCirc(float t) => 1f - Mathf.Sqrt(1f - Mathf.Pow(t, 2f));

  public static float OutCirc(float t) => Mathf.Sqrt(1f - Mathf.Pow(t - 1f, 2f));

  public static float InOutCirc(float t) => (double) t >= 0.5 ? (float) (((double) Mathf.Sqrt(1f - Mathf.Pow((float) (-2.0 * (double) t + 2.0), 2f)) + 1.0) / 2.0) : (float) ((1.0 - (double) Mathf.Sqrt(1f - Mathf.Pow(2f * t, 2f))) / 2.0);

  public static float InBack(float t) => (float) (2.7015800476074219 * (double) t * (double) t * (double) t - 1.7015800476074219 * (double) t * (double) t);

  public static float OutBack(float t) => (float) (1.0 + 2.7015800476074219 * (double) Mathf.Pow(t - 1f, 3f) + 1.7015800476074219 * (double) Mathf.Pow(t - 1f, 2f));

  public static float InOutBack(float t) => (double) t >= 0.5 ? (float) (((double) Mathf.Pow((float) (2.0 * (double) t - 2.0), 2f) * (3.5949094295501709 * ((double) t * 2.0 - 2.0) + 2.5949094295501709) + 2.0) / 2.0) : (float) ((double) Mathf.Pow(2f * t, 2f) * (7.1898188591003418 * (double) t - 2.5949094295501709) / 2.0);

  public static float InElastic(float t)
  {
    if ((double) t == 0.0)
      return 0.0f;
    return (double) t != 1.0 ? -Mathf.Pow(2f, (float) (10.0 * (double) t - 10.0)) * Mathf.Sin((float) (((double) t * 10.0 - 10.75) * 2.0943951606750488)) : 1f;
  }

  public static float OutElastic(float t)
  {
    if ((double) t == 0.0)
      return 0.0f;
    return (double) t != 1.0 ? (float) ((double) Mathf.Pow(2f, -10f * t) * (double) Mathf.Sin((float) (((double) t * 10.0 - 0.75) * 2.0943951606750488)) + 1.0) : 1f;
  }

  public static float InOutElastic(float t)
  {
    if ((double) t == 0.0)
      return 0.0f;
    if ((double) t == 1.0)
      return 1f;
    return (double) t >= 0.5 ? (float) ((double) Mathf.Pow(2f, (float) (-20.0 * (double) t + 10.0)) * (double) Mathf.Sin((float) ((20.0 * (double) t - 11.125) * 1.3962634801864624)) / 2.0 + 1.0) : (float) (-((double) Mathf.Pow(2f, (float) (20.0 * (double) t - 10.0)) * (double) Mathf.Sin((float) ((20.0 * (double) t - 11.125) * 1.3962634801864624))) / 2.0);
  }

  public static float InBounce(float t) => 1f - Easing.OutBounce(1f - t);

  public static float OutBounce(float t)
  {
    if ((double) t < 0.36363637447357178)
      return 121f / 16f * t * t;
    if ((double) t < 0.72727274894714355)
      return (float) (121.0 / 16.0 * (double) (t -= 0.545454562f) * (double) t + 0.75);
    return (double) t < 0.90909093618392944 ? (float) (121.0 / 16.0 * (double) (t -= 0.8181818f) * (double) t + 15.0 / 16.0) : (float) (121.0 / 16.0 * (double) (t -= 0.954545438f) * (double) t + 63.0 / 64.0);
  }

  public static float InOutBounce(float t) => (double) t >= 0.5 ? (float) ((1.0 + (double) Easing.OutBounce((float) (2.0 * (double) t - 1.0))) / 2.0) : (float) ((1.0 - (double) Easing.OutBounce((float) (1.0 - 2.0 * (double) t))) / 2.0);
}
