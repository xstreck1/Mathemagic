using UnityEngine;
using System.Collections;

public class CastOnClick : MonoBehaviour
{
    bool _spell = true;
    Vector3 _initial_postion;

    Matrix4x4 _move_to_center;
    Matrix4x4 _move_from_center;
    Matrix4x4 _translate_mat;
    Matrix4x4 _rotate_mat;

    Matrix4x4 _current_spell;

    float _CAST_TIME = 1f;

    // Use this for initialization
    void Start()
    {
        _move_to_center = Matrix4x4.TRS(-transform.position, Quaternion.identity, Vector3.one);
        _move_from_center = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
        _initial_postion = transform.position;
        _translate_mat = Matrix4x4.TRS(new Vector3(0f, 2f, 0f), Quaternion.identity, Vector3.one);
        _rotate_mat = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 45f), Vector3.one);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Spell switch.");
            _spell = !_spell;
        }
    }

    // 
    void OnMouseDown()
    {
        Debug.Log("spell casted on " + this.name);

        if (_spell)
        {
            Cast(_translate_mat);
            // updateCoordinates(_translate_mat);
        }

        else
        {
            Cast(_translate_mat * _rotate_mat);
        }
    }

    void Cast(Matrix4x4 spell)
    {
        _current_spell = spell;
        StartCoroutine("ApplyMatrix");
    }

    IEnumerator ApplyMatrix()
    {
        _current_spell = _move_from_center * _current_spell * _move_to_center;
        Vector3 _new_position = _current_spell.MultiplyPoint(transform.position);
        Vector3 _new_up = _current_spell.MultiplyVector(transform.up);
        Vector3 _old_position = transform.position;
        Vector3 _old_up = transform.up;

        for (float time = 0; time < _CAST_TIME; time += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(_old_position, _new_position, time / _CAST_TIME);
            transform.up = Vector3.Slerp(_old_up, _new_up, time / _CAST_TIME);
            yield return null;
        }

        _move_to_center = Matrix4x4.TRS(-transform.position, Quaternion.identity, Vector3.one);
        _move_from_center = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
    }
}
