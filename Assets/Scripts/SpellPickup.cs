using UnityEngine;
using System.Collections;



public class SpellPickup : MonoBehaviour {
    public Vector3 _translation = Vector3.zero;
    public Vector3 _rotation = Vector3.zero;
    public Vector3 _scaling = Vector3.one;


    SpellManager _spell_manager;

    // Use this for initialization
    void Start () {
        _spell_manager = GameObject.Find("SpellManager").GetComponent<SpellManager>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            _spell_manager.AddSpell(Matrix4x4.TRS(_translation, Quaternion.Euler(_rotation), _scaling));
            GameObject.Destroy(this.gameObject);
        }
    }
}
