using UnityEngine;
using System.Collections;

public class Scene : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevelName);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            Application.LoadLevel(Application.loadedLevelName);

    }
}
