using UnityEngine;
using System.Collections;

public class CastOnClick : MonoBehaviour
{
    static bool _casting = false; ///< Set to true for the period of spell being active
    float _CAST_TIME = 1f;
    float _time = 0;

    ObjectCollision _object_collision;
    SpellManager _spell_manager;
	ParticleSystem _particles;

    bool _colided = false;

    // Use this for initialization
    void Start()
    {
        _spell_manager = GameObject.Find("SpellManager").GetComponent<SpellManager>();
        _object_collision = GetComponent<ObjectCollision>();
		_particles = GetComponentInChildren<ParticleSystem> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (_casting && _time > 0)
        {
            _time -= Time.deltaTime;
            if (_time <= 0)
                _casting = false;
        }
    }

    // 
    void OnMouseDown()
    {
        if (!_casting) {
            Matrix4x4 spell = _spell_manager.GetSpell();
            if (spell != Matrix4x4.identity)
            {
                StartCoroutine(ApplyMatrix(spell));
                Debug.Log("spell " + spell.ToString() + " casted on " + this.name);
            }
        }
    }

    public static bool IsCasting()
    {
        return _casting;
    }

    float GetAngle()
    {
        // Compute the angle of object
        float angle = Vector2.Angle(transform.up, Vector3.up);
        Vector3 cross = Vector3.Cross(transform.up, Vector3.up);
        if (cross.z > 0)
            angle = 360 - angle;
        return angle;
    }

    // Compute the matrix (move to the origin and reset rotation before)
    Matrix4x4 putToOrigin(Matrix4x4 spell)
    {

        float angle = GetAngle();
        return
            Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one) *
            Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, angle), Vector3.one) *
            spell *
            Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -angle), Vector3.one) *
            Matrix4x4.TRS(-transform.position, Quaternion.identity, Vector3.one);
    }

    void OnCollisionEnter(Collision collision)
    {
        _colided = true;
    }

    IEnumerator ApplyMatrix(Matrix4x4 spell)
    {
        _time = _CAST_TIME;
        _casting = true;
		_particles.emissionRate = 30f;
        Rigidbody rigidbody = this.gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;

		CharacterController.CastSpell ();

        spell = putToOrigin(spell);

        // Apply rotation
        Vector3 old_up = transform.up;
        Vector3 new_up = spell.MultiplyVector(old_up);

        // Apply translation
        Vector3 old_position = transform.position;
        Vector3 new_position = spell.MultiplyPoint(old_position);

        // Apply scaling
        Vector3 old_scale = transform.localScale;
        Vector3 scale_pos_x = spell.MultiplyPoint(old_position + new Vector3(old_scale.x, 0, 0));
        Vector3 scale_pos_y = spell.MultiplyPoint(old_position + new Vector3(0, old_scale.y, 0));
        Vector3 scale_pos_z = spell.MultiplyPoint(old_position + new Vector3(0, 0, old_scale.z));
        Vector3 new_scale = new Vector3(Vector3.Distance(new_position, scale_pos_x), Vector3.Distance(new_position, scale_pos_y), Vector3.Distance(new_position, scale_pos_z));

        // Lerp in the coroutine for the CAST TIME
        float time;
        for (time = 0; time < _CAST_TIME && !_colided; time += Time.deltaTime)
        {
            float progress = time / _CAST_TIME;
            transform.position = Vector3.Lerp(old_position, new_position, progress);
            transform.localScale = Vector3.Lerp(old_scale, new_scale, progress);
            transform.up = Vector3.Slerp(old_up, new_up, progress);
            yield return null;
        }

        // Adjust after the coroutine has finished
        if (!_colided)
        {
            transform.position = Vector3.Lerp(old_position, new_position, 1f);
            transform.localScale = Vector3.Lerp(old_scale, new_scale, 1f);
            transform.up = Vector3.Slerp(old_up, new_up, 1f);
        } else
        {
            float progress = (time - 2* Time.deltaTime) / _CAST_TIME; 
            transform.position = Vector3.Lerp(old_position, new_position, progress);
            transform.localScale = Vector3.Lerp(old_scale, new_scale, progress);
            transform.up = Vector3.Slerp(old_up, new_up, progress);
        }

		_particles.emissionRate = 0f;

        Destroy(rigidbody);
        _casting = false;
        _colided = false;
    }
}
