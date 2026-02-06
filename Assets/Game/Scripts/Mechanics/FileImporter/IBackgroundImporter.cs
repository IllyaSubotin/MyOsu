using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IBackgroundImporter 
{
    string BackgroundPath { get; set;}
    void ImportBackground();
    Sprite LoadBackground(string path);
}
