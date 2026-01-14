using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveLoadService
{
    void Save(SaveLoadData data);
    SaveLoadData Load();
    void DeleteSave();
}
