using System;
using Newtonsoft.Json;

// Token: 0x0200003D RID: 61
public class SongPackMaskConverter : JsonConverter
{
    // Token: 0x0600016C RID: 364 RVA: 0x00007003 File Offset: 0x00005203
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(SongPackMask);
    }

    // Token: 0x0600016D RID: 365 RVA: 0x00007015 File Offset: 0x00005215
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return SongPackMask.Parse(serializer.Deserialize<string>(reader));
    }

    // Token: 0x0600016E RID: 366 RVA: 0x0000702C File Offset: 0x0000522C
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        serializer.Serialize(writer, ((SongPackMask)value).ToShortString());
    }
}
