// Decompiled with JetBrains decompiler
// Type: OnlineServices.API.ApiResponse`1
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

namespace OnlineServices.API
{
  public readonly struct ApiResponse<T>
  {
    public readonly Response response;
    public readonly T responseDto;

    public bool isError => this.response != 0;

    public ApiResponse(Response response, T responseDto)
    {
      this.response = response;
      this.responseDto = responseDto;
    }
  }
}
