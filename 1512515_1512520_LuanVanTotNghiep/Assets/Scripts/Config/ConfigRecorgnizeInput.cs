using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KAINamespace;

public class ConfigRecorgnizeInputRecord : KAIBaseConfigRecord{

    public int id;
    public string data;

    public RecognizeInputStruct GetRecorgnizeInputStruct()
    {
        RecognizeInputStruct recorgnizeInputStruct = JsonUtility.FromJson<RecognizeInputStruct>(data);
        if (recorgnizeInputStruct == null)
            return new RecognizeInputStruct();
        return recorgnizeInputStruct;
    }
}

public class ConfigRecorgnizeInput : KAIBaseConfig<ConfigRecorgnizeInputRecord> {
    public override void LoadConfigFromTextAsset(string path)
    {
        base.LoadConfigFromTextAsset(path);
        RebuildConfigById();
    }

    public override void ReadFileFromPath(string path)
    {
        base.ReadFileFromPath(path);
        RebuildConfigById();
    }

    public void RebuildConfigById()
    {
        RebuidConfig("id");
    }

    public ConfigRecorgnizeInputRecord GetConfigById(int id)
    {
        return GetConfig<int>(id);
    }
}

[System.Serializable]
public class RecognizeInputStruct
{
    public RecognizeObject[] recognizeObjects;
}

[System.Serializable]
public class RecognizeObject
{
    public float x;
    public float y;
    public float width;
    public float height;
    public float score;
    public string name;
    public string description;
}