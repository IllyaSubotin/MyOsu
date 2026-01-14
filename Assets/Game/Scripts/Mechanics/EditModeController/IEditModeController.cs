using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEditModeController
{
    BeatmapData beatmapDatas { get; }
    void ClearAllNodes();
}
