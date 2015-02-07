using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CastOnClick : MonoBehaviour
{
    int _spell = 0;
    List<Matrix4x4> spells = new List<Matrix4x4>();

    float _CAST_TIME = 1f;

    // Use this for initialization
    void Start()
    {
        spells.Add(Matrix4x4.TRS(new Vector3(0f, 2f, 0f), Quaternion.identity, Vector3.one));
        spells.Add(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 45f), Vector3.one));
        spells.Add(Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 2));
        spells.Add(spells[1] * spells[0]);
        spells.Add(spells[0] * spells[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _spell = (_spell + 1) % spells.Count;
            Debug.Log("Using spell " + _spell);
        }
    }

    // 
    void OnMouseDown()
    {
        Debug.Log("spell casted on " + this.name);

        Cast(spells[_spell]);
    }

    void Cast(Matrix4x4 spell)
    {
        StartCoroutine(ApplyMatrix(spell));
    }

    IEnumerator ApplyMatrix(Matrix4x4 spell)
    {
        // Compute the angle of object
        float ang = Vector2.Angle(transform.up, Vector3.up);
        Vector3 cross = Vector3.Cross(transform.up, Vector3.up);
        if (cross.z > 0)
            ang = 360 - ang;

        // Compute the matrix (move to the origin and reset rotation before)
        spell = 
            Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one) * 
            Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, ang), Vector3.one) *
            spell *
            Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -ang), Vector3.one) *
            Matrix4x4.TRS(-transform.position, Quaternion.identity, Vector3.one);

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
        for (float time = 0; time < _CAST_TIME; time += Time.deltaTime)
        {
            float progress = time / _CAST_TIME;
            transform.position = Vector3.Lerp(old_position, new_position, progress);
            transform.localScale = Vector3.Lerp(old_scale, new_scale, progress);
            transform.up = Vector3.Slerp(old_up, new_up, progress);
            yield return null;
        }

        // Adjust after the coroutine has finished
        transform.position = Vector3.Lerp(old_position, new_position, 1f);
        transform.localScale = Vector3.Lerp(old_scale, new_scale, 1f);
        transform.up = Vector3.Slerp(old_up, new_up, 1f);
    }
}
