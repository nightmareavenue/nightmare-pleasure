using UnityEngine;

[CreateAssetMenu(menuName = "Game/Status Effect")]
public class StatusEffectConfig : ScriptableObject
{
    public string statusName;
    public float duration = 3f;
    public Sprite icon;

    [SerializeField] private StatusEffectBehavior behavior;

    public StatusEffectBehavior Behavior => behavior;
}