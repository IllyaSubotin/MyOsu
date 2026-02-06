using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NodeManager : MonoBehaviour, INodeManager
{
    public List<NodeView> inactiveNodes = new List<NodeView>();
    public RectTransform canvasRect;
    public NodeView nodePrefab;
    public Transform nodeParent;
    public NodeView nearestNode{ get;  private set; }
    public Action onEnded{ get; set; }

    public float approachTime { get; private set; } = 1f;
    private float tolerance = 0.01f; 

    private List<NodeData> nodeInfos = new List<NodeData>();
    private List<NodeView> activeNodes = new List<NodeView>();

    private IAudioTimer _audioTimer;
    private IScoreManager _scoreManager;    
    private IHitDetection _hitDetection;
    private IHealthManager _healthManager;

    [Inject]
    private void Construct(IAudioTimer audioTimer, IScoreManager scoreManager, IHitDetection hitDetection, 
                            IHealthManager healthManager)
    {
        _audioTimer = audioTimer;
        _scoreManager = scoreManager;   
        _hitDetection = hitDetection;
        _healthManager = healthManager;
    }


    private void Update()
    {
        if(nodeInfos.Count == 0 && activeNodes.Count == 0)
        {
            onEnded?.Invoke();
            return; 
        }

        if(nearestNode != null && _audioTimer.audioTime > nearestNode.spawnTime + approachTime + 0.2f)
        {
            FailedHit(nearestNode);
        }

        if (nodeInfos.Count == 0) return;

        if (nodeInfos[0].spawnTime <= _audioTimer.audioTime + tolerance)
        {
            NodeData nodeInfo = nodeInfos[0];
            NodeView newNode = SpawnNode(nodeInfo);

            Debug.Log("Spawned node at time: " + _audioTimer.audioTime);
        }

        
    }

    public void Initialize(List<NodeData> nodeInfos)
    {
        for(int i = 0; i < nodeInfos.Count; i++)
        {
            this.nodeInfos.Add(nodeInfos[i]);
        }
    }

    private NodeView SpawnNode(NodeData info)
    {
        NodeView node;

        if (inactiveNodes.Count > 0)
        {
            node = inactiveNodes[0];
            inactiveNodes.RemoveAt(0);
        }
        else
        {
            node = Instantiate(nodePrefab, nodeParent);
        }
        
        node.gameObject.SetActive(true);    
        node.gameObject.transform.SetAsFirstSibling();
        node.Initialize(approachTime, 1f, new Vector2((info.xPercent - 0.5f) * canvasRect.rect.width, 
                                                        (info.yPercent - 0.5f) * canvasRect.rect.height), info.spawnTime); 

        activeNodes.Add(node);

        if (nearestNode == null)
            nearestNode = node;

        nodeInfos.RemoveAt(0);

        return node;
    }

    public void OnHit(NodeView node)
    {
        HitResult hitResult = _hitDetection.EvaluateHit(node);
        
        _scoreManager.RegisterHit(hitResult);

        _healthManager.UpdateHealth(hitResult);
    }    

    public void SucsessfulHit(NodeView node)
    {
        node.OnHit();
        activeNodes.Remove(node);
        inactiveNodes.Add(node);

        nearestNode = activeNodes.Count > 0 ? activeNodes[0] : null;
        Debug.Log("Sucsessful hit on node at time: " + _audioTimer.audioTime);
    } 

    public void MissedHit()
    {
        if(nearestNode == null)
            return;
            
        nearestNode.OnMiss();
    }

    private void FailedHit(NodeView node)
    {   
        node.OnFail();
        activeNodes.Remove(node);
        inactiveNodes.Add(node);

        nearestNode = activeNodes.Count > 0 ? activeNodes[0] : null;
        Debug.Log("Missed node at time: " + _audioTimer.audioTime);
    }   


    public void CollectNodes()
    {
        inactiveNodes.Clear();

        if (nodeParent == null)
        {
            Debug.LogWarning("Node Parent не заданий!");
            return;
        }

        foreach (Transform child in nodeParent)
        {
            inactiveNodes.Add(child.gameObject.GetComponent<NodeView>());
        }

        Debug.Log($"Зібрано {inactiveNodes.Count} нод.");
    }
}
