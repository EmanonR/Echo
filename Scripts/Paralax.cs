using UnityEngine;

[ExecuteAlways]
public class Paralax : MonoBehaviour
{
    private void Update()
    {
        Vector2 camPos = Camera.main.transform.position;
        transform.position = (Vector3)camPos / 4;
    }
}
