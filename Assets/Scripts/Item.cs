using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemConfig item;
    private SpriteRenderer sr;
    private PlayerStats playerStats;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
        playerStats = player.GetComponent<PlayerStats>();
        sr.sprite = item._icon;
    }

    private void GetItem(ItemConfig item)
    {
        playerStats.IncreaseMaxHp(item._health);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetItem(item);
            Destroy(gameObject);
        }
    }
}
