using System;
// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class UI_TreeConnection : MonoBehaviour
{

    [SerializeField] private RectTransform rotationPoint;
    [SerializeField] private RectTransform connectLength;
    [SerializeField] private RectTransform childConnectionPoint;


    public void DirectConnection(NodeDirectionType direction, float length, float offset)
    {
        bool shouldBeActive = direction != NodeDirectionType.None;
        float finalLength = shouldBeActive ? length : 0;
        float angle = GetDirectionAngle(direction);

        rotationPoint.localRotation = Quaternion.Euler(0,0,angle + offset);
        connectLength.sizeDelta = new Vector2(finalLength, connectLength.sizeDelta.y);
    }

    public Image GetConnectionImage() => connectLength.GetComponent<Image>();

    public Vector2 GetConnectionPoint(RectTransform rect)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (
            rect.parent as RectTransform,
            childConnectionPoint.position,
            null,
            out var localPosition
        );
        return localPosition;
    }
    private float GetDirectionAngle(NodeDirectionType type)
    {
        switch(type)
        {
            case NodeDirectionType.Up: return 90f;
            case NodeDirectionType.UpLeft: return 135f;
            case NodeDirectionType.UpRight: return 45f;
            case NodeDirectionType.Down: return -90f;
            case NodeDirectionType.DownLeft: return -135f;
            case NodeDirectionType.DownRight: return -45f;
            case NodeDirectionType.Left: return 180f;
            case NodeDirectionType.Right: return 0;
            default: return 0;
        }
    }
}



public enum NodeDirectionType
{
    None,
    UpLeft,
    Up,
    UpRight,
    Down,
    DownLeft,
    DownRight,
    Left,
    Right
}
