using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject player;
    private PlayerStats playerStats;

    private InputSystem_Actions controls;
    private bool IsEscape = false;

    private void Awake()
    {
        _pauseMenu.SetActive(IsEscape);
        controls = new();
        playerStats = player.GetComponent<PlayerStats>();
    }

    private void FixedUpdate()
    {
        healthText.text = $"Health: {playerStats.CurrentHealth}/{playerStats.MaxHealth}";
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