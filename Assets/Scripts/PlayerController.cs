using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("References")]
    public Rigidbody rb;
    public Transform head;
    public Camera camera;

    [Header("Configurations")]
    public float walkSpeed;
    public float runSpeed;

    [Header("Camera Effects")]
    public float baseCameraFov = 60f;
    public float baseCameraHeight = .85f;

    public float walkBobbingRate = .75f;
    public float runBobbingRate = 1f;
    public float maxWalkBobbingOffset = .2f;
    public float maxRunBobbingOffset = .3f;

    [Header("Audio")]
    public AudioSource audioWalk;
    public AudioSource heartBeat;
    public AudioSource breathe;
    public float beatMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Horizontal rotation
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * 2f);

        bool isMovingOnGround = (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f);

        if (isMovingOnGround) {
            float bobbingRate = Input.GetKey(KeyCode.LeftShift) ? runBobbingRate : walkBobbingRate;
            float bobbingOffset = Input.GetKey(KeyCode.LeftShift) ? maxRunBobbingOffset : maxWalkBobbingOffset;
            Vector3 targetHeadPosition = Vector3.up * baseCameraHeight + Vector3.up * (Mathf.PingPong(Time.time * bobbingRate, bobbingOffset) - bobbingOffset*.5f);
            head.localPosition = Vector3.Lerp(head.localPosition, targetHeadPosition, .1f);
        }


        //For Audio
        audioWalk.enabled = isMovingOnGround;
        audioWalk.pitch = Input.GetKey(KeyCode.LeftShift) ? 1.75f : 1f;

        heartBeat.enabled = true;
        // heartBeat.pitch = Mathf.Clamp(Mathf.Abs(rb.velocity.y * beatMultiplier), 0f, 2f) + Random.Range(-.1f, .1f);
        breathe.enabled = true;
        // breathe.pitch = Mathf.Clamp(Mathf.Abs(rb.velocity.y * beatMultiplier), 0f, 2f) + Random.Range(-.1f, .1f);

    }

    void FixedUpdate() {
        Vector3 newVelocity = Vector3.up * rb.velocity.y;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        newVelocity.x = Input.GetAxis("Horizontal") * speed;
        newVelocity.z = Input.GetAxis("Vertical") * speed;
        // rb.velocity = newVelocity;
        rb.velocity = transform.TransformDirection(newVelocity);

    }

    void LateUpdate() {
        // Vertical rotation
        Vector3 e = head.eulerAngles;
        e.x -= Input.GetAxis("Mouse Y") * 2f;   //  Edit the multiplier to adjust the rotate speed
        e.x = RestrictAngle(e.x, -85f, 85f);    //  This is clamped to 85 degrees. You may edit this.
        head.eulerAngles = e;
    }

    public static float RestrictAngle(float angle, float angleMin, float angleMax) {
        if (angle > 180)
            angle -= 360;
        else if (angle < -180)
            angle += 360;

        if (angle > angleMax)
            angle = angleMax;
        if (angle < angleMin)
            angle = angleMin;

        return angle;
    }

}
