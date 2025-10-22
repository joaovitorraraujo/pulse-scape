using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed = 270f; // graus por segundo

    void Update()
    {
        transform.Rotate(0f, 0f, speed * Time.deltaTime);
    }
}
