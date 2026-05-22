using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastCameraPositionX;
    private float cameraHalfWidth;

    [SerializeField] private ParallaxLayer[] backgroundLayers;

    private void Awake()
    {
        mainCamera = Camera.main;
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
        calculateImageWidth();
    }
    private void Update()
    {
        float currCameraPositionX = mainCamera.transform.position.x; 
        float distanceCameraMoved = currCameraPositionX - lastCameraPositionX;
        lastCameraPositionX = currCameraPositionX;

        float cameraRightEdge = currCameraPositionX + cameraHalfWidth;
        float cameraLeftEdge = currCameraPositionX - cameraHalfWidth;

        foreach(ParallaxLayer layer in backgroundLayers)
        {
            layer.Move(distanceCameraMoved);
            layer.loopBackground(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void calculateImageWidth()
    {
        foreach(ParallaxLayer layer in backgroundLayers)
        {
            layer.calculateImageWidths();
        }
    }
}
