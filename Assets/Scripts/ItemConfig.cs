using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item")]
public class ItemConfig : ScriptableObject
{
    [SerializeField] private string _name;
    enum Category
    {
        Passive
    }

    [SerializeField] private Category _category;

    public int _health;
    public Sprite _icon;
}
