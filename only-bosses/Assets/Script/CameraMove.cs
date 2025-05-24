using UnityEngine;

public class CameraMove : MonoBehaviour
{

    Vector3 base_pos;

    void Start()
    {
        base_pos = Camera.main.gameObject.transform.position;
    }

    void LateUpdate()
    {
        Vector3 pos = this.transform.position;
        pos.z = -10;
        pos.y = base_pos.y;
        Camera.main.gameObject.transform.position = pos;
    }
}
