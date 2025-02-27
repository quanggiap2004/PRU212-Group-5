using System;
using System.Collections.Generic;
using UnityEngine;


public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{\"questions\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.questions;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] questions;
    }
}

