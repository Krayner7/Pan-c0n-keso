using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    private Vector2 Angle = new Vector2(90 * Mathf.Deg2Rad, 0);
    public Transform follow;
    public float distance;
    private Vector2 lookInput;
    public PlayerController player;
    private bool cameraActive = false;
    public void ActivateCamera()
    {
        cameraActive = true;

        Cursor.lockState =
            CursorLockMode.Locked;

        Cursor.visible = false;
    }


    public void OnLook(Vector2 input)
    {
        lookInput = input;
    }
    private void LateUpdate()
    {
        if (player == null || follow == null)
            return;

        Vector2 lookInput = player.GetLookInput();
        float hor = lookInput.x;
        if (hor != 0)
            Angle.x += hor * Mathf.Deg2Rad;
        float ver = lookInput.y;
        if (ver != 0)
        {
            Angle.y += ver * Mathf.Deg2Rad;
            Angle.y = Mathf.Clamp(Angle.y, -80 * Mathf.Deg2Rad, 80 * Mathf.Deg2Rad);

        }

        Vector3 orbit = new Vector3(
         Mathf.Cos(Angle.x) * Mathf.Cos(Angle.y),
         -Mathf.Sin(Angle.y),
         -Mathf.Sin(Angle.x) * Mathf.Cos(Angle.y)
         );
        transform.position = follow.position + orbit * distance;
        transform.rotation = Quaternion.LookRotation(follow.position - transform.position);

    }
}
