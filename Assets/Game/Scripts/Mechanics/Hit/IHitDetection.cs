using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitDetection 
{
    HitResult EvaluateHit(NodeView node);
}
