using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] GameObject shotPrefab;
    private InputSystem_Actions controls;
    void Awake()
    {

        controls = new();
    }
    
    private void OnEnable()
    {
        controls.UI.Enable();
        controls.UI.Shoot.performed += Shoot;
    }

    private void OnDisable()
    {
        controls.UI.Shoot.performed -= Shoot;
        controls.UI.Disable();
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        Instantiate(shotPrefab, transform.position, Quaternion.identity);
    }
}