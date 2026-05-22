using UnityEngine;
[System.Serializable]
public class ParallaxLayer
{
    [SerializeField] private Transform background;
    [SerializeField] private float parallaxMultiplier;
    [SerializeField] private float imageWidthOffset = 6;

    private float imageFullWidth;
    private float imageHalfWidth;

    
    public void calculateImageWidths()
    {
        SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
        imageFullWidth = spriteRenderer.bounds.size.x;
        imageHalfWidth = imageFullWidth / 2f;
    }
    public void Move(float distanceToMove)
    {
        background.position += new Vector3(distanceToMove * parallaxMultiplier, 0f, 0f);
    }

    public void loopBackground (float cameraLeftEdge, float cameraRightEdge)
    {
        float imageLeftEdge = background.position.x - imageHalfWidth + imageWidthOffset;
        float imageRightEdge = background.position.x + imageHalfWidth - imageWidthOffset;

        if (imageRightEdge < cameraLeftEdge)
        {
            background.position += new Vector3(imageFullWidth, 0f, 0f);
        }
        else if (imageLeftEdge > cameraRightEdge)
        {
            background.position -= new Vector3(imageFullWidth, 0f, 0f);
        }
    }
}
