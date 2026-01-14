using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class GameplayController : MonoBehaviour
{
    public GraphicRaycaster graphicRaycaster;
    private INodeManager _nodeManager;

    [Inject]
    private void Constract(INodeManager nodeManager)
    {
        _nodeManager = nodeManager;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            NodeView node = RaycastNodeUnderCursor();
            
            if (node != null)
            {
                Debug.Log("Node selected: " + node.name);
                _nodeManager.OnHit(node);
            }
            else
            {
                Debug.Log("No node under cursor.");
                _nodeManager.MissedHit();
            }
        }  
    }

    private NodeView RaycastNodeUnderCursor()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent<NodeView>(out var node))
                return node;
        }

        return null;
    }


    
}
