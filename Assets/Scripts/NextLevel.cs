using UnityEngine;
using System.Collections;
using System;

public class NextLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            string loaded_level_name = Application.loadedLevelName.Substring(5);
            int loaded_level_no = Int32.Parse(loaded_level_name);
            Application.LoadLevel("Level" + (loaded_level_no + 1));
        }
    }

        

}
