using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAim : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // 1. No Novo Input System, lemos a posição assim:
        Vector2 mousePos = Mouse.current.position.ReadValue();

        // 2. Converte para o mundo (passando o mousePos e uma distância Z qualquer)
        Vector3 worldMousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        Vector2 lookDir = worldMousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        HandleWeaponFlip(angle);
    }

    void HandleWeaponFlip(float angle)
    {
        Vector3 localScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            localScale.y = -1f;
        }
        else
        {
            localScale.y = 1f;
        }
        transform.localScale = localScale;
    }
}