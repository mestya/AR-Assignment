using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class TapToPlace : MonoBehaviour
{
    public GameObject objectToPlace;
    public ARRaycastManager raycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private readonly List<GameObject> placedObjects = new();
    public Camera arCamera;
    void Update()
    {
        if (Touchscreen.current == null) return;
        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            Ray ray = arCamera.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit objectHit))
            {
                foreach (GameObject placedObject in placedObjects)
                {
                    if (placedObject == null) continue;

                    Transform hitTransform = objectHit.collider.transform;

                    if (hitTransform == placedObject.transform ||
                        hitTransform.IsChildOf(placedObject.transform))
                    {
                        ChangeColor(placedObject);
                        return;
                    }
                }
            }
            if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                GameObject newObject = Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
                placedObjects.Add(newObject);
            }
        }
    }

    void ChangeColor(GameObject target)
    {
        Color randomColor = Random.ColorHSV(
            0f, 1f,
            0.6f, 1f,
            0.7f, 1f);

        foreach (Renderer objectRenderer
                 in target.GetComponentsInChildren<Renderer>())
        {
            // 使用独立材质，避免其他球体跟着变色。
            objectRenderer.material.color = randomColor;
        }
    }
}