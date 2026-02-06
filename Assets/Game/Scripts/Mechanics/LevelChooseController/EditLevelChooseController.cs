using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class EditLevelChooseController : MonoBehaviour, IEditLevelChooseController, IPointerDownHandler, IDragHandler, IPointerUpHandler 
{
    [Header("Layout")]
    [SerializeField] public float baseX = -350;

    [Header("Input")]
    [SerializeField] public float wheelSensitivity = 1f;
    [SerializeField] public float dragSensitivity = 1f;
    [SerializeField] public float snapSpeed = 12f;
    
    [Header("ListLevelItems")]
    [SerializeField] public List<LevelItem> levelItems = new List<LevelItem>();

    
    [Header("NumCentralItem")]
    [SerializeField] public int currentLevelIndex = 0;
    
    [Header("Objects")]
    [SerializeField] public GameObject levelItemPrefab;
    [SerializeField] public Transform parentTransform;

    [HideInInspector] public bool isNewLevel {get; set;}

    private float LevelItemOffsetX = 15f;
    private float LevelItemOffsetY = 100f; 
    private float scrollPosition;
    private float targetScrollPosition;
    private float lastMouseY;
    private bool dragging;

    private ISaveLoadManager _saveLoadManager;



    [Inject]
    private void Construct(ISaveLoadManager saveLoadManager)
    {
        _saveLoadManager = saveLoadManager;
    }

    public void Initialize()
    {
        if(levelItems.Count < _saveLoadManager.beatmapDatas.Count + 1)
        {
            for(;_saveLoadManager.beatmapDatas.Count + 1 > levelItems.Count;)
            {
                var newItem = Instantiate(levelItemPrefab, parentTransform);

                LevelItem newLevelItem = newItem.GetComponent<LevelItem>();
                newLevelItem.gameObject.SetActive(false);

                levelItems.Add(newLevelItem);
            }
        }

        levelItems[0].nameText.text = "New Level";

        for(int i = 0; _saveLoadManager.beatmapDatas.Count > i ; i++)
        {
            levelItems[i + 1].nameText.text = _saveLoadManager.beatmapDatas[i].beatmapName;
        }


        for(int i = 0; levelItems.Count > i ; i++)
        {
            float rectTransformX = -350f + i * LevelItemOffsetX;
            float rectTransformY = 0 + i * LevelItemOffsetY;
            
            if(i != 0)
            {
                rectTransformX += 20f;
                rectTransformY += 20f;
            }

            if(currentLevelIndex - i >= 0)
            {
                levelItems[currentLevelIndex - i].rectTransform.anchoredPosition = new Vector2(rectTransformX, rectTransformY);
                levelItems[currentLevelIndex - i].gameObject.SetActive(true);
            }

            if(currentLevelIndex + i < levelItems.Count)
            {
                levelItems[currentLevelIndex + i].rectTransform.anchoredPosition = new Vector2(rectTransformX, -rectTransformY);
                levelItems[currentLevelIndex + i].gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        HandleMouseWheel();
        SnapIfNeeded();

        scrollPosition = Mathf.Lerp(scrollPosition, targetScrollPosition, Time.deltaTime * snapSpeed);

        UpdateLayout();
    }

    private void HandleMouseWheel()
    {
        if (dragging) return;

        float wheel = Input.mouseScrollDelta.y;
        if (Mathf.Abs(wheel) < 0.01f) return;

        targetScrollPosition -= wheel * wheelSensitivity;
        ClampScroll();
    }

    private void SnapIfNeeded()
    {
        if (dragging) return;

        float snapped = Mathf.Round(targetScrollPosition);
        targetScrollPosition = Mathf.Lerp(targetScrollPosition, snapped, Time.deltaTime * snapSpeed);
    }

    private void ClampScroll()
    {
        targetScrollPosition = Mathf.Clamp(targetScrollPosition, 0, levelItems.Count - 1);
    }

    private void UpdateLayout()
    {
        for (int i = 0; i < levelItems.Count; i++)
        {
            float offset = i - scrollPosition;
            float distance = Mathf.Abs(offset);

            float y = -offset * LevelItemOffsetY;

            float scale;
            if (distance < 1f)
                scale = Mathf.Lerp(1f, 0.95f, distance);
            else
                scale = Mathf.Max(0.6f, 0.95f - (distance - 1f) * 0.1f);

            float x = baseX + distance * LevelItemOffsetX;

            levelItems[i].rectTransform.anchoredPosition = new Vector2(x, y);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        lastMouseY = eventData.position.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float deltaY = eventData.position.y - lastMouseY;
        lastMouseY = eventData.position.y;

        targetScrollPosition += (deltaY / LevelItemOffsetY) * dragSensitivity;

        ClampScroll();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }

    public int SaveCurrentLevel()
    {
        if(Mathf.RoundToInt(targetScrollPosition) == 0)
        {
            isNewLevel = true;
            return Mathf.RoundToInt(targetScrollPosition);
        }
        else
        {
            
            isNewLevel = false; 
            _saveLoadManager.currentLevelIndex = Mathf.RoundToInt(targetScrollPosition);
            return Mathf.RoundToInt(targetScrollPosition);
        }
        
    }

}
