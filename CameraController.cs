using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector2 targetOffset;
    [SerializeField] float zoomNear = 4, zoomFar = 10, zoomSpeed = .5f;
    

    float zoom;

    private void Start()
    {
        zoom = zoomFar;
    }

    private void Update()
    {
        if (player == null) return;

        transform.position = player.transform.position;
        transform.position += new Vector3(0,0,-10);

        float scroll = Input.mouseScrollDelta.y;

        if (scroll != 0)
            zoom = Mathf.Clamp(zoom - (scroll * zoomSpeed), zoomNear, zoomFar);

        Camera.main.orthographicSize = zoom;
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(player.position + (Vector3)targetOffset, .4f);
    }
}
