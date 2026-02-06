using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditModeScreen : Screen
{
    [Header("Canvas")]
    [SerializeField] public GameObject editModeCanvas;

    [Header("Inputs")]
    [SerializeField] public SliderInputSynchronizer approachValue;
    [SerializeField] public SliderInputSynchronizer sizeValue;

    [Header("Input buttons")]
    [SerializeField] public Button stopButton;
    [SerializeField] public Button playButton;
    [SerializeField] public Button playFastButton;
    [SerializeField] public Button playBackButton;
    [SerializeField] public Button playBackFastButton;

    [Header("Setting buttons")]
    [SerializeField] public Button saveButton;
    [SerializeField] public Button loadButton;
    [SerializeField] public Button exitButton;

    [Header("MusicProgressBar")]
    [SerializeField] public Slider progressBar;
}
