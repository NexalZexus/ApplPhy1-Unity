//for debug reason
using System.Collections;
using UnityEngine;

public class PlayerSpeed : MonoBehaviour
{
    [SerializeField] private float speed;

    void FixedUpdate()
    {
        StartCoroutine(CalculateSpeed());
    }

    IEnumerator CalculateSpeed()
    {
        Vector3 lastPos = transform.position;
        yield return new WaitForFixedUpdate();
        speed = (lastPos - transform.position).magnitude / Time.deltaTime;
    }
}
