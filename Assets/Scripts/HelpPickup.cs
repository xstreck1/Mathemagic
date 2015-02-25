using UnityEngine;
using System.Collections;

public class HelpPickup : MonoBehaviour
{
    public Transform help_text;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
            help_text.gameObject.SetActive(true);
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
            help_text.gameObject.SetActive(false);
    }
}