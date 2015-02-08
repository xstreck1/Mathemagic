using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    float _dist_to_ground = 1f;
    public float _MOVE_FORCE = 750f;
    public float _MAX_SPEED = 5f;
    public float _JUMP_FORCE = 15f;
	private bool _jump = false;

	private Locomotion2D locomotion;

    bool IsGrounded()
    {
        return Physics.Raycast (transform.position, -Vector3.up, _dist_to_ground + 0.1f) ||
			Physics.Raycast (transform.position, new Vector3 (-0.05f, -1f, 0f), _dist_to_ground + 0.2f) ||
				Physics.Raycast (transform.position, new Vector3 (0.05f, -1f, 0f), _dist_to_ground + 0.2f);
    }

    // Use this for initialization
    void Start()
    {
		locomotion = new Locomotion2D (this.GetComponent<Animator> ());
    }

    void FixedUpdate()
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
			Vector3 oppositeForce = new Vector3(-rigidbody.velocity.x, 0f, 0f);
			rigidbody.AddForce(oppositeForce * _MOVE_FORCE * 0.5f * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            transform.position += Vector3.up * 0.25f;
            rigidbody.AddForce(Vector3.up * _JUMP_FORCE, ForceMode.VelocityChange);
			_jump = true;
        }
		if (_jump) {
			locomotion.Update (true, rigidbody.velocity.magnitude);
			_jump = false;
		} else {
			locomotion.Update (false, rigidbody.velocity.magnitude);
		}
		if (rigidbody.velocity.x >= 0f && !Input.GetKey (KeyCode.A)) {
			transform.rotation = Quaternion.AngleAxis (90f, Vector3.up);
		} else if (Input.GetKey (KeyCode.A) || rigidbody.velocity.x <= 0.2f) {
			transform.rotation = Quaternion.AngleAxis (-90f, Vector3.up);
		} else {
			transform.rotation = Quaternion.AngleAxis (90f, Vector3.up);
		}
    }
}
