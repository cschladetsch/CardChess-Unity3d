using UnityEngine;


public sealed class CameraRotation : MonoBehaviour {

    public Transform target;

    private float xAngle = 0.2f;
    private float zoom = 0f;
    
    private const float MAX_ROTATION_SPEED = 1f;
    private const float ZOOM_SPEED = 2f;


    private void Update() {
        if (!target)
            return;

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1)) {
            xAngle = Input.GetAxis("Mouse X");

            if (xAngle > MAX_ROTATION_SPEED)
                xAngle = MAX_ROTATION_SPEED;
            else if (xAngle < -MAX_ROTATION_SPEED)
                xAngle = -MAX_ROTATION_SPEED;
        }
        zoom = Input.GetAxis("Mouse ScrollWheel") * ZOOM_SPEED;

        Debug.Log(xAngle);

        transform.LookAt(target);
        transform.Translate(new Vector3(xAngle, 0f, zoom) );
    }

}
