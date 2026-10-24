using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject player;
    private EntityStats playerStats;

    [Header("Effects")]
    public Transform EffectsPanel;
    public GameObject EffectUIPrefab;
    private StatusList statusList;
    private readonly Dictionary<StatusEffectConfig, TextMeshProUGUI> statusTexts = new();

    private InputSystem_Actions controls;

    private void Awake()
    {
        controls = new();
        playerStats = player.GetComponent<EntityStats>();
        statusList = player.GetComponent<StatusList>();
        SetPaused(false);
    }

    private void OnEnable()
    {
        controls.UI.Enable();
        controls.UI.Escape.performed += ToggleEscapeScreen;
        statusList.OnStatusesChanged += RefreshStatusUI;
    }

    private void OnDisable()
    {
        controls.UI.Escape.performed -= ToggleEscapeScreen;
        controls.UI.Disable();
        statusList.OnStatusesChanged -= RefreshStatusUI;
    }

    private void Update()
    {
        healthText.text = $"Health: {playerStats.CurrentHealth}/{playerStats.MaxHealth}";
        UpdateStatusTimers();
    }

    private void UpdateStatusTimers()
    {
        foreach (var info in statusList.ActiveStatuses)
        {
            if (statusTexts.TryGetValue(info.Config, out var text))
                text.text = FormatTime(info.RemainingTime);
        }
    }

    private void RefreshStatusUI()
    {
        foreach (Transform child in EffectsPanel)
        {
            Destroy(child.gameObject);
        }
        statusTexts.Clear();

        foreach (var info in statusList.ActiveStatuses)
        {
            var statusUIObj = Instantiate(EffectUIPrefab, EffectsPanel);
            statusUIObj.name = info.Config.statusName;
            var effectIcon = statusUIObj.GetComponentInChildren<Image>();
            var effectDuration = statusUIObj.GetComponentInChildren<TextMeshProUGUI>();
            effectDuration.text = FormatTime(info.RemainingTime);
            effectIcon.sprite = info.Config.icon;
            statusTexts[info.Config] = effectDuration;
        }
    }

    private static string FormatTime(float seconds) => seconds.ToString("F1") + "s";

    private void SetPaused(bool paused)
    {
        PausePanel.SetActive(paused);
        if (crosshair != null) crosshair.SetActive(!paused);
    }

    public void ToggleEscapeScreen(InputAction.CallbackContext context) => SetPaused(!PausePanel.activeSelf);
    public void ContinueButton() => SetPaused(false);
}