using System.Collections;
using Zenject;
using UnityEngine;
using Unity.Collections;

public class HitDetection : MonoBehaviour, IHitDetection
{
    public float okHitThreshold = 1f;
    public float goodHitThreshold = 0.5f;
    public float perfectHitThreshold = 0.2f;

    private float hitTimeDelta;

    private INodeManager _nodeManager;
    private IAudioTimer _audioTimer;

    [Inject]
    private void Construct(INodeManager nodeManager, IAudioTimer audioTimer)
    {
        _nodeManager = nodeManager;
        _audioTimer = audioTimer;
    }


    public HitResult EvaluateHit(NodeView node)
    {
        if (node == _nodeManager.nearestNode)
        {
            hitTimeDelta = Mathf.Abs(node.spawnTime + _nodeManager.approachTime - _audioTimer.audioTime);
            
            switch (hitTimeDelta)
            {
                case float t when t < perfectHitThreshold:
                    _nodeManager.SucsessfulHit(node);
                    return HitResult.Perfect;

                case float t when t < goodHitThreshold:
                    _nodeManager.SucsessfulHit(node);
                    return HitResult.Good;

                case float t when t < okHitThreshold:
                    _nodeManager.SucsessfulHit(node);
                    return HitResult.Ok;

                default:
                    _nodeManager.MissedHit();
                    return HitResult.Miss;
            }
        }
        else
        {
            _nodeManager.MissedHit();
            return HitResult.Miss;
        }
    }
}
