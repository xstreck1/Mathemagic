using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    float _dist_to_ground = 1f;
    public float _MOVE_SPEED = 200f;
    public float _JUMP_FORCE = 500f;

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
        if (Input.GetKeyDown(KeyCode.D))
        {
            rigidbody.AddForce(Vector3.right * _MOVE_SPEED);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            rigidbody.AddForce(Vector3.left * _MOVE_SPEED);
        }
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            rigidbody.AddForce(Vector3.up * _JUMP_FORCE);

        }
    }
}
