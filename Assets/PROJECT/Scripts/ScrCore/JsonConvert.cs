using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;

public static class JsonConvert
{   // Token: 0x06000AAA RID: 2730 RVA: 0x00032C2C File Offset: 0x0003102C
    public static string SerializeObject(object serialize)
    {
        return Json.Serialize(serialize);
    }

    // Token: 0x06000AAB RID: 2731 RVA: 0x00032C41 File Offset: 0x00031041
    public static T DeserializeObject<T>(string serialize)
    {
        return Json.Deserialize<T>(serialize);
    }

    // Token: 0x06000AAC RID: 2732 RVA: 0x00032C49 File Offset: 0x00031049
    //public static object DeserializeObject(string serialize, Type type)
    //{
    //    return Json.Deserialize(serialize, type);
    //}
}
