using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadService : ISaveLoadService
{
    private readonly string _path;

    public SaveLoadService()
    {
        _path = Path.Combine(Application.persistentDataPath, "save.json");
    }

    public void Save(SaveLoadData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_path, json);
    }

    public SaveLoadData Load()
    {
        if (!File.Exists(_path))
            return new SaveLoadData();

        string json = File.ReadAllText(_path);
        return JsonUtility.FromJson<SaveLoadData>(json);
    }

    public void DeleteSave()
    {
        if (File.Exists(_path))
            File.Delete(_path);
    }
}
