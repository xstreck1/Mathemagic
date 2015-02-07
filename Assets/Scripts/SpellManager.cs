using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellManager : MonoBehaviour {
    public Transform _spell_button_prefab;
    int _spell = 0;
    List<Matrix4x4> spells = new List<Matrix4x4>();

    // Use this for initialization
    void Start () {
        spells.Add(Matrix4x4.TRS(new Vector3(0f, 2f, 0f), Quaternion.identity, Vector3.one));
        spells.Add(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 45f), Vector3.one));
        spells.Add(Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 2));
        spells.Add(spells[1] * spells[0]);
        spells.Add(spells[0] * spells[1]);
        DisplaySpells();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void DisplaySpells()
    {
        float display_width = GetComponent<RectTransform>().rect.width;
        foreach (Matrix4x4 spell in spells)
        {
            GameObject button = (GameObject) Instantiate(_spell_button_prefab, Vector3.zero, Quaternion.identity);
        }
    }
}
