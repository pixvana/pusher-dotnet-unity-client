using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


/*
 * Utilites + thin wrapper around JSON serialization.  If all JSON serializing / deserializing
 * goes through JSONHelper.Serialize / Deserialize then underlying JSON library can
 * be changed more easily
 */
public class JSONHelper
{
    private IJsonSerializer m_Serializer;
    public JSONHelper(IJsonSerializer serializer)
    {
        m_Serializer = serializer;
    }
    public T Deserialize<T>(string data)
    {
        return m_Serializer.Deserialize<T>(data);
    }

    public object Deserialize(string data)
    {
        return m_Serializer.Deserialize(data);
    }

    public string Serialize(object data)
    {
        return m_Serializer.Serialize(data);
    }
}

public interface IJsonSerializer
{
    T Deserialize<T>(string data);
    object Deserialize(string data);
    string Serialize(object data);
}