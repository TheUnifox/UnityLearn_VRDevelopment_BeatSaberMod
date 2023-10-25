// Decompiled with JetBrains decompiler
// Type: Facebook.SocialVR.ThirdParty.Newtonsoftjson.UnityEntitySerializer
// Assembly: Main, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 89B9E799-8342-47B3-85ED-672D6883482A
// Assembly location: C:\Program Files\Oculus\Software\Software\hyperbolic-magnetism-beat-saber\Beat Saber_Data\Managed\Main.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Facebook.SocialVR.ThirdParty.Newtonsoftjson
{
  public class UnityEntitySerializer : JsonConverter
  {
    public override bool CanConvert(Type objectType) => objectType.Namespace == "UnityEngine" && objectType.Name != "AnimationCurve";

    public override object ReadJson(
      JsonReader reader,
      Type objectType,
      object existingValue,
      JsonSerializer serializer)
    {
      return JsonUtility.FromJson(((object) JToken.Load(reader)).ToString(), objectType);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => JToken.FromObject((object) JsonUtility.ToJson(value)).WriteTo(writer);
  }
}
