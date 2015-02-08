using UnityEngine;
using System.Collections;
using System;

public class CharacterController : MonoBehaviour
{
    float _dist_to_ground = 1f;
    public float _MOVE_FORCE = 750f;
    public float _MAX_SPEED = 5f;
    public float _JUMP_FORCE = 15f;
	private bool _jump = false;

	[SerializeField]
	private AudioClip[] footSound;
	[SerializeField]
	private AudioClip[] jumpSound;
	[SerializeField]
	private AudioClip[] magicSounds;
	[SerializeField]
	private float _footStepTime = 0.5f;
	private float _nextFootStepTime = 0f;
	private ParticleSystem particles;

	private static Locomotion2D locomotion;
	private static CharacterController instance;

    bool IsGrounded()
    {
        return Physics.Raycast (transform.position, -Vector3.up, _dist_to_ground + 0.1f);
    }

    // Use this for initialization
    void Start()
    {
		instance = this;
		locomotion = new Locomotion2D (this.GetComponent<Animator> ());
		particles = GetComponentInChildren<ParticleSystem> ();
    }

    void FixedUpdate()
    {
        // If position is fixed
        if((Convert.ToInt32(rigidbody.constraints) & Convert.ToInt32(RigidbodyConstraints.FreezePosition)) != 0)
        {
            if (!CastOnClick.IsCasting() && !HelpPickup.IsHelpActive())
            {
                rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            }
        } else
        {
            if (CastOnClick.IsCasting() || HelpPickup.IsHelpActive())
            {
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
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
			this.audio.PlayOneShot(jumpSound[UnityEngine.Random.Range(0, jumpSound.Length)]);
			_jump = false;
		} else {
			if (rigidbody.velocity.magnitude > 0.1f && IsGrounded() && Time.time >= _nextFootStepTime) {
				this.audio.PlayOneShot(footSound[UnityEngine.Random.Range(0, footSound.Length)]);
				_nextFootStepTime = Time.time + _footStepTime;
			}
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

	public static void CastSpell() {
		locomotion.CastSpell ();
		instance.StartCoroutine (instance.spawnParticles ());
	}

	private IEnumerator spawnParticles() {
		this.audio.PlayOneShot(magicSounds[UnityEngine.Random.Range(0, magicSounds.Length)]);
		particles.emissionRate = 10f;
		yield return new WaitForSeconds (1f);
		particles.emissionRate = 0f;
	}
}
