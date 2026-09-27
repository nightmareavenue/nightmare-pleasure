using UnityEngine;

[CreateAssetMenu(menuName = "Game/Status Effect")]
public class StatusEffectConfig : ScriptableObject
{
    public string statusName;
    public float duration = 3f;
    public int level = 1;
    public int healthDamage = 0;
    public float tickInterval = 1f;
    public Sprite icon;
}