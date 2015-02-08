using UnityEngine;
using System.Collections;



public class SpellPickup : MonoBehaviour {
    public Vector3 _translation = Vector3.zero;
    public Vector3 _rotation = Vector3.zero;
    public Vector3 _scaling = Vector3.one;

	public AudioClip[] _pickupSounds;
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
			StartCoroutine(DestroyAndPlaySound());
        }
    }

	IEnumerator DestroyAndPlaySound() {
		this.collider.enabled = false;
		this.audio.PlayOneShot (_pickupSounds [UnityEngine.Random.Range (0, _pickupSounds.Length)]);
		for (int i = 0; i < 30; i++) {
			this.transform.localScale *= 0.1f;
			yield return null;
		}
		GameObject.Destroy(this.gameObject);
	}
}
