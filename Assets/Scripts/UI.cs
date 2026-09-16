using UnityEngine;
using UnityEngine.InputSystem;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    private InputSystem_Actions controls;
    private bool IsEscape = false;

    private void Awake()
    {
        _pauseMenu.SetActive(IsEscape);
        controls = new();
    }

    public void ToggleEscapeScreen(InputAction.CallbackContext context)
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
    }

    public void ContinueButton()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
    }

    private void OnEnable()
    {
        controls.UI.Enable();
        controls.UI.Escape.performed += ToggleEscapeScreen;
    }

    private void OnDisable()
    {
        controls.UI.Escape.performed -= ToggleEscapeScreen;
        controls.UI.Disable();
    }
}