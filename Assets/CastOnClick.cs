using UnityEngine;
using System.Collections;

public class CastOnClick : MonoBehaviour {
    bool _spell = true;
    Vector3 _initial_postion;

	// Use this for initialization
	void Start () {
        _initial_postion = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))        {
            Debug.Log("Spell switch.");
            _spell = !_spell;
        }
    }

    // 
    void OnMouseDown() {
        Debug.Log("spell casted on " + this.name);

        if (_spell)
            transform.Translate(new Vector3(2f, 0, 0));
        else
            transform.RotateAround(_initial_postion, new Vector3(0f, 0f, 1f), 90f);
    }
}
