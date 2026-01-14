using System;
using System.Collections.Generic;
using UnityEngine;

public interface INodeManager 
{
    Action onEnded { get; set; }

    NodeView nearestNode { get; }
    float approachTime { get; }
    
    void Initialize(List<NodeData> nodeInfos);
    void SucsessfulHit(NodeView node);
    void MissedHit();
    void OnHit(NodeView node);
}
