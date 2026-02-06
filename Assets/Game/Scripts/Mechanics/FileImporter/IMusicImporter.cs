using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public interface IMusicImporter
{
    string MusicPath { get; set;}
    void ImportMusic();
    Task<AudioClip> LoadAsync(string path);
}
