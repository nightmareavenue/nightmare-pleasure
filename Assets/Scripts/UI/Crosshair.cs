using UnityEngine;
using UnityEngine.InputSystem;

public class Crosshair : MonoBehaviour
{
    private RectTransform rect;

    private void Awake()
    {
        rect = (RectTransform)transform;
    }

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (Mouse.current == null) return;
        rect.position = Mouse.current.position.ReadValue();
    }
}