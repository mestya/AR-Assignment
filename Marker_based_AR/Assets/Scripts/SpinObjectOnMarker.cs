using UnityEngine;
using UnityEngine.InputSystem;
public class SpinObjectOnMarker : MonoBehaviour
{
    public float rotationSpeed = 100f;

    private bool isSpinning = true;
    private Camera arCamera;

    private void Update()
    {
        if (arCamera == null)
            arCamera = Camera.main;

        if (arCamera != null &&
            Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            Ray ray = arCamera.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.transform == transform ||
                    hit.collider.transform.IsChildOf(transform))
                {
                    isSpinning = !isSpinning;
                }
            }
        }

        if (isSpinning)
        {
            transform.Rotate(
                Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
}