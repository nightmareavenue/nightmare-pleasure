using TMPro;
using UnityEngine;

public class HealthPointsShow : MonoBehaviour
{
    EntityStats entityStats;
    TextMeshProUGUI textMesh;
    void Awake()
    {
        GameObject UICanvas = new GameObject("UI Canvas");
        Canvas canvas = UICanvas.AddComponent<Canvas>();
        canvas.transform.position = new Vector3(0, 1, 0);
        RectTransform canvasRect = UICanvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(1, 1);

        GameObject healthTextGO = new GameObject("Health Text");
        RectTransform textRect = healthTextGO.AddComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(4, 1);
        textRect.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        textMesh = healthTextGO.AddComponent<TextMeshProUGUI>();
        textMesh.fontSize = 1;

        UICanvas.transform.SetParent(transform, false);
        healthTextGO.transform.SetParent(UICanvas.transform, false);

        entityStats = GetComponent<EntityStats>();

    }

    private void Update()
    {
        textMesh.text = entityStats.CurrentHealth.ToString() + "/" + entityStats.MaxHealth.ToString();
    }
}