using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEditLevelChooseController
{
    bool isNewLevel {get; set;}
    
    void Initialize();
    int SaveCurrentLevel();
}
