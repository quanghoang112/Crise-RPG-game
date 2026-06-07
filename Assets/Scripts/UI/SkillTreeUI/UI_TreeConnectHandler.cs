using System;
using System.Collections.Generic;

// using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UI_TreeConnectDetails
{
    public UI_TreeConnectHandler childNode;
    public NodeDirectionType direction;
    [Range(100f,350f)] public float length;
    [Range(-45f,45f)] public float rotation;
}



public class UI_TreeConnectHandler : MonoBehaviour
{
    private RectTransform rect => GetComponent<RectTransform>();
    [SerializeField] private UI_TreeConnectDetails[] details;
    [SerializeField] private UI_TreeConnection[] connections;

    private Image connectionImage;
    private Color originalColor;

    private void Awake()
    {
        if(connectionImage != null)
            originalColor = connectionImage.color;
    }

    private void OnValidate()
    {
        
        if(details.Length != connections.Length)
        {
            Debug.Log("Amount of details shoulb be same as");
            return;
        }
        UpdateConnection();        
    }

    private void UpdateConnection()
    {
        for(int i = 0;i < details.Length;i++)
        {
            // if(connections[i] == null || details[i] == null) continue;

            connections[i].DirectConnection(details[i].direction,details[i].length,details[i].rotation);

            Vector2 targetPosition = connections[i].GetConnectionPoint(rect);
            Image connectionImage = connections[i].GetConnectionImage();
            
            if(details[i].childNode == null)
                continue;

            details[i].childNode.SetPosition(targetPosition);
            details[i].childNode.SetConnectionImage(connectionImage);
            details[i].childNode.transform.SetAsLastSibling();
        }
    }

    public void UpdateAllConnections()
    {
        UpdateConnection();
        
        foreach (var node in details)
        {
            if(node.childNode == null)  continue;
            node.childNode.UpdateConnection();
        }
    }

    public UI_TreeNode[] GetChildNodes()
    {
        List<UI_TreeNode> children = new List<UI_TreeNode>();

        foreach (var node in details)
        {
            if(node.childNode == null) continue;

            children.Add(node.childNode.GetComponent<UI_TreeNode>());
        }

        return children.ToArray();
    }

    public void UnlockConnectionImage(bool unlocked)
    {
        if(connectionImage == null)
            return;
        connectionImage.color = unlocked? Color.white : originalColor;
    }

    public void SetConnectionImage (Image image) => connectionImage = image;
    public void SetPosition(Vector2 position) => rect.anchoredPosition = position;
}
