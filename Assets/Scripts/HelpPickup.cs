using UnityEngine;
using System.Collections;

public class HelpPickup : MonoBehaviour {
    public Transform help_text;

    static bool _help_active = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (_help_active)
            Debug.Log("update called");
        if (Input.GetMouseButtonDown(0) && help_text.gameObject.activeSelf)
        {
            _help_active = false ;
            help_text.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject);
        }
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            _help_active = true;
            help_text.gameObject.SetActive(true);
        }
    }

    public static bool IsHelpActive()
    {
        return _help_active;
    }
}
