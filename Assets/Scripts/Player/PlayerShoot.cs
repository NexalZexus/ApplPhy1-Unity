using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputActionAsset inputActions;
    private InputAction playerShoot;

    private GameObject boolet;

    [SerializeField] private float brrtForce;
    [SerializeField] private float booletLife;

    [SerializeField] private Transform bangPos;

    private void Awake()
    {
        playerShoot = InputSystem.actions.FindAction("Attack");
    }

    void Update()
    {
        if (playerShoot.WasPressedThisDynamicUpdate())
        {
            shoot();
        }
    }
    void shoot()
    {
        boolet = GameObject.Instantiate(Resources.Load("Prefabs/Boolet") as GameObject, bangPos.position, bangPos.rotation);
        boolet.GetComponent<Rigidbody>().AddForce(brrtForce * bangPos.transform.up, ForceMode.Impulse);

        StartCoroutine(BooletDestroy());
    }
    private IEnumerator BooletDestroy()
    {
        yield return new WaitForEndOfFrame();
        Destroy(boolet,booletLife);
    }
}
