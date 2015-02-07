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

    int _STEP_COUNT = 10;

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

    void updateCoordinates(Matrix4x4 matrix)  {
        matrix = _move_from_center * matrix * _move_to_center;
        transform.position = matrix.MultiplyPoint(transform.position);
        transform.up = (matrix.MultiplyVector(transform.up));
        _move_to_center = Matrix4x4.TRS(-transform.position, Quaternion.identity, Vector3.one);
        _move_from_center = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
   } 

    // 
    void OnMouseDown()
    {
        Debug.Log("spell casted on " + this.name);

        if (_spell)
        {
            updateCoordinates(_rotate_mat * _translate_mat);
            // updateCoordinates(_translate_mat);
        }

        else
        {
            updateCoordinates(_translate_mat * _rotate_mat);
        }
    }

    void Cast(Matrix4x4 spell)
    {
        // _current_spell = spell * (1f / _STEP_COUNT);
        StartCoroutine("ApplyMatrix");
    }

    IEnumerator ApplyMatrix()
    {
        for (int step = 0; step < _STEP_COUNT; step++)
        {
            /*Color c = renderer.material.color;
            c.a = f;
            renderer.material.color = c; */
            yield return new WaitForSeconds(.1f);
        }
    }
}
