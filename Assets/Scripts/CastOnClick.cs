using UnityEngine;
using System.Collections;

public class CastOnClick : MonoBehaviour
{
    bool _casting = false; ///< Set to true for the period of spell being active
    float _CAST_TIME = 1f;

    ObjectCollision object_collision;

    // Use this for initialization
    void Start()
    {
        object_collision = GetComponent<ObjectCollision>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // 
    void OnMouseDown()
    {
        if (!_casting) {
            Debug.Log("spell casted on " + this.name);
        }
    }

    public bool IsCasting()
    {
        return _casting;
    }

    void Cast(Matrix4x4 spell)
    {
        StartCoroutine(ApplyMatrix(spell));
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

    IEnumerator ApplyMatrix(Matrix4x4 spell)
    {
        _casting = true;
        Rigidbody rigidbody = this.gameObject.AddComponent<Rigidbody>();
        rigidbody.useGravity = false;

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
        for (float time = 0; time < _CAST_TIME && !object_collision.isDestroying(); time += Time.deltaTime)
        {
            float progress = time / _CAST_TIME;
            transform.position = Vector3.Lerp(old_position, new_position, progress);
            transform.localScale = Vector3.Lerp(old_scale, new_scale, progress);
            transform.up = Vector3.Slerp(old_up, new_up, progress);
            yield return null;
        }

        // Adjust after the coroutine has finished
        if (!object_collision.isDestroying())
        {
            transform.position = Vector3.Lerp(old_position, new_position, 1f);
            transform.localScale = Vector3.Lerp(old_scale, new_scale, 1f);
            transform.up = Vector3.Slerp(old_up, new_up, 1f);
        }

        Destroy(rigidbody);
        _casting = false;
    }
}
