using Zenject;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEditor.Search;

public class EditModeController : MonoBehaviour, IEditModeController
{
    public GraphicRaycaster graphicRaycaster;
    public RectTransform canvasRect;
    public EditNodeView nodePrefab;
    public Transform parentTransform;

    public SliderInputSynchronizer approach;
    public SliderInputSynchronizer size;

    public BeatmapData beatmapDatas{ get; private set; } = new BeatmapData();  
    
    private List<EditNodeView> _nodes = new List<EditNodeView>();
    private Stack<EditNodeView> _nodesStack = new Stack<EditNodeView>();
    private Queue<EditNodeView> _nodesQueue = new Queue<EditNodeView>();

    private ISaveLoadManager _saveLoadManager;
    private IAudioTimer _audioTimer;

    [Inject]
    private void Construct(ISaveLoadManager saveLoadManager, IAudioTimer audioTimer)
    {
        _saveLoadManager = saveLoadManager;
        _audioTimer = audioTimer;
    }

    private void OnEnable()
    {
        beatmapDatas = new BeatmapData();
        beatmapDatas.beatmapID = 0;//_saveLoadManager.beatmapDatas.Count;
        beatmapDatas.beatmapName = "New Beatmap";
        beatmapDatas.nodeInfos = new List<NodeData>();

        _audioTimer.OnSpeedChange += OnAudioSpeedChange;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && CanCreateNode())
        {
            Vector2 mousePosition = Input.mousePosition;
            CreateNodeAtPosition(mousePosition);
        }

        if(_nodesStack.Count > 0 || _nodesQueue.Count > 0)
        {
            if(_audioTimer.speedMultiplier > 0 && _audioTimer.audioTime > _nodesQueue.Peek().spawnTime)
            {
                _nodesQueue.Dequeue().PlayForward();
            }
            else if(_audioTimer.speedMultiplier < 0 && _audioTimer.audioTime < _nodesStack.Peek().destroyTime)
            {
                _nodesStack.Pop().PlayBackwards();
            } 
        }
    }


    private bool CanCreateNode()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject == parentTransform.gameObject)
                return true;
        }
        return false;
    }

    private void CreateNodeAtPosition(Vector2 position)
    {
        GameObject nodeObject = Instantiate(nodePrefab.gameObject, parentTransform);
        nodeObject.transform.SetAsFirstSibling();

        EditNodeView nodeView = nodeObject.GetComponent<EditNodeView>();        

        _nodes.Add(nodeView);
        nodeView.Initialize(approach.value, size.value, position, _audioTimer.audioTime);
        
        if(_audioTimer.audioTime > 0)
            nodeView.PlayForward();
        else if(_audioTimer.audioTime < 0)
            nodeView.PlayBackwards();
        
        beatmapDatas.nodeInfos.Add(new NodeData
        {
            xPercent = (position.x / canvasRect.rect.width) * 2f,
            yPercent = (position.y / canvasRect.rect.height) * 2f,

            spawntime = _audioTimer.audioTime,
            approachTime = approach.value,
            size = size.value,
        });

        Debug.Log("Node created at: " + position);
    }

    private void OnAudioSpeedChange(float speedMultiplier)
    {
        _nodesStack.Clear();
        _nodesQueue.Clear();

        if(speedMultiplier > 0)
        {
            foreach(var node in _nodes)
            {
                if(node.spawnTime > _audioTimer.audioTime)
                    _nodesQueue.Enqueue(node);
            }
        }
        else if(speedMultiplier < 0)
        {
            foreach(var node in _nodes)
            {
                if(node.destroyTime < _audioTimer.audioTime)
                    _nodesStack.Push(node);
            }
        }
    }

    public void ClearAllNodes()
    {
        foreach (var node in _nodes)
        {
            Destroy(node.gameObject);
        }

        _nodes.Clear();
        beatmapDatas.nodeInfos.Clear();
    }
}
