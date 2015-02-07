using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CastOnClick : MonoBehaviour
{
    int _spell = 0;
    int _SPELL_COUNT = 3;

    Matrix4x4 _move_to_center;
    Matrix4x4 _move_from_center;
    List<Matrix4x4> spells = new List<Matrix4x4>();
    Matrix4x4 _translate_mat;
    Matrix4x4 _rotate_mat;
    Matrix4x4 _scale_mat;

    float _CAST_TIME = 1f;

    // Use this for initialization
    void Start()
    {
        _move_to_center = Matrix4x4.TRS(-transform.position, Quaternion.identity, Vector3.one);
        _move_from_center = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
        spells.Add(Matrix4x4.TRS(new Vector3(0f, 2f, 0f), Quaternion.identity, Vector3.one));
        spells.Add(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 45f), Vector3.one));
        spells.Add(Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 2));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _spell = (_spell + 1) % _SPELL_COUNT;
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
        spell = _move_from_center * spell * _move_to_center;
        Vector3 old_up = transform.up;
        Vector3 new_up = spell.MultiplyVector(old_up);

        Vector3 old_position = transform.position;
        Vector4 affine_pos = old_position;
        affine_pos.w = 1;
        Vector4 new_affine_pos = spell * (affine_pos);
        Vector3 new_position = (new_affine_pos) / new_affine_pos.w;

        Vector3 old_scale = transform.localScale;
        Vector3 scale_pos_x = spell.MultiplyPoint(old_position + new Vector3(old_scale.x, 0, 0));
        Vector3 scale_pos_y = spell.MultiplyPoint(old_position + new Vector3(0, old_scale.y, 0));
        Vector3 scale_pos_z = spell.MultiplyPoint(old_position + new Vector3(0, 0, old_scale.z));
        Vector3 new_scale = new Vector3(Vector3.Distance(new_position, scale_pos_x), Vector3.Distance(new_position, scale_pos_y), Vector3.Distance(new_position, scale_pos_z));

        for (float time = 0; time < _CAST_TIME; time += Time.deltaTime)
        {
            float progress = time / _CAST_TIME;
            transform.position = Vector3.Lerp(old_position, new_position, progress);
            transform.localScale = Vector3.Lerp(old_scale, new_scale, progress);
            transform.up = Vector3.Slerp(old_up, new_up, progress);
            yield return null;
        }

        transform.position = Vector3.Lerp(old_position, new_position, 1f);
        transform.localScale = Vector3.Lerp(old_scale, new_scale, 1f);
        transform.up = Vector3.Slerp(old_up, new_up, 1f);

        _move_to_center = Matrix4x4.TRS(-transform.position, Quaternion.identity, Vector3.one);
        _move_from_center = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
    }
}
