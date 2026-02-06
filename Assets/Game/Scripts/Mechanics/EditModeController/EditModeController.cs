using Zenject;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class EditModeController : MonoBehaviour, IEditModeController
{
    public GraphicRaycaster graphicRaycaster;
    public RectTransform canvasRect;
    public EditNodeView nodePrefab;
    public Transform parentTransform;

    public Image backgroundImage;

    public SliderInputSynchronizer approach;
    public SliderInputSynchronizer size;

    public BeatmapData beatmapDatas{ get; private set; } = new BeatmapData();  
    
    private List<EditNodeView> _nodes = new List<EditNodeView>();
    private Stack<EditNodeView> _nodesStack = new Stack<EditNodeView>();
    private Queue<EditNodeView> _nodesQueue = new Queue<EditNodeView>();

    private IAudioTimer _audioTimer;
    private ISaveLoadManager _saveLoadManager;
    private IMusicImporter _musicImporter;
    private IEditLevelChooseController _levelChooseController;
    private IBackgroundImporter _backgroundImporter;

    [Inject]
    private void Construct(IAudioTimer audioTimer, IMusicImporter musicImporter, IBackgroundImporter backgroundImporter,
                             IEditLevelChooseController levelChooseController, ISaveLoadManager saveLoadManager)
    {
        _musicImporter = musicImporter;
        _audioTimer = audioTimer;
        _saveLoadManager = saveLoadManager;
        _levelChooseController = levelChooseController;
        _backgroundImporter = backgroundImporter;
    }

    private void OnEnable()
    {
        if(_levelChooseController.isNewLevel)
        {
            InitNewBeatmap();
        }
        else
        {
            InitChooseBeatmap();
        }
        
    }

    private void InitNewBeatmap()
    {
        beatmapDatas = new BeatmapData();
        beatmapDatas.beatmapID = 0;
        beatmapDatas.beatmapName = "New Beatmap";
        beatmapDatas.nodeInfos = new List<NodeData>();
        beatmapDatas.audioPath = _musicImporter.MusicPath;
        beatmapDatas.backgroundImagePath = _backgroundImporter.BackgroundPath;

        backgroundImage.sprite = _backgroundImporter.LoadBackground(_backgroundImporter.BackgroundPath);

        _ = LoadAudioClip(beatmapDatas.audioPath);
    }

    private void InitChooseBeatmap()
    {
        beatmapDatas = new BeatmapData();
        beatmapDatas = _saveLoadManager.beatmapDatas[_saveLoadManager.currentLevelIndex - 1];

        backgroundImage.sprite = _backgroundImporter.LoadBackground(beatmapDatas.backgroundImagePath);

        _ = LoadAudioClip(beatmapDatas.audioPath);
        
        CreateNodesFromSave();
    }

    private async Task LoadAudioClip(string path)
    {
        AudioClip clip = await _musicImporter.LoadAsync(path);
        _audioTimer.SetAudioClip(clip);

        _audioTimer.StartTimer();
        
        _audioTimer.OnSpeedChange += OnAudioSpeedChange;
    }

    private void CreateNodesFromSave()
    {
        foreach(var nodeInfo in beatmapDatas.nodeInfos)
        {
            var newNode = Instantiate(nodePrefab.gameObject, parentTransform);
            newNode.transform.SetAsFirstSibling();

            EditNodeView nodeView = newNode.GetComponent<EditNodeView>();

            _nodes.Add(nodeView);
            nodeView.Initialize(nodeInfo.approachTime, nodeInfo.size, new Vector2(nodeInfo.x, nodeInfo.y), nodeInfo.spawnTime);
            nodeView.gameObject.SetActive(false);

            _nodesQueue.Enqueue(nodeView);
        }
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
        GameObject newNode = Instantiate(nodePrefab.gameObject, parentTransform);
        newNode.transform.SetAsFirstSibling();

        EditNodeView nodeView = newNode.GetComponent<EditNodeView>();        

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

            x = position.x,
            y = position.y,

            spawnTime = _audioTimer.audioTime,
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
