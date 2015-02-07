using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    float _dist_to_ground = 1f;
    public float _MOVE_FORCE = 750f;
    public float _MAX_SPEED = 5f;
    public float _JUMP_FORCE = 750f;

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, _dist_to_ground + 0.1f);
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("velocity " + rigidbody.velocity.x);
        if (Input.GetKey(KeyCode.D))
        {
            if (rigidbody.velocity.x < _MAX_SPEED)
            {
                rigidbody.AddForce(Vector3.right * _MOVE_FORCE * Time.deltaTime);
            }
        }
        else if(Input.GetKey(KeyCode.A))
        {
            if (rigidbody.velocity.x > -_MAX_SPEED)
            {
                rigidbody.AddForce(Vector3.left * _MOVE_FORCE * Time.deltaTime);
            }
        }
        else if (rigidbody.velocity.x != 0f) // Apply breaking in the opposing direction of movement (slow down)
        {
            float old_velocity = rigidbody.velocity.x;
            rigidbody.AddForce((rigidbody.velocity.x > 0 ? Vector3.left : Vector3.right) * _MOVE_FORCE * Time.deltaTime);
            if (Mathf.Sign(old_velocity) != Mathf.Sign(rigidbody.velocity.x))
            {
                Vector3 new_vleocity = rigidbody.velocity;
                new_vleocity.x = 0f;
                rigidbody.velocity = new_vleocity;
            }
        }
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            rigidbody.AddForce(Vector3.up * _JUMP_FORCE);

        }
    }
}
