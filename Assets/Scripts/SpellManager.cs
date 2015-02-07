using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class SpellManager : MonoBehaviour {
    public Transform _spell_button_prefab;
    int _active_spell = -1;
    List<Matrix4x4> spells = new List<Matrix4x4>();
    List<GameObject> buttons = new List<GameObject>(); 

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

    void SpellClicked(int spell_no)
    {
        Debug.Log("spell " + spell_no);
        if (_active_spell == -1)
        {
            _active_spell = spell_no;
        } else
        {
            spells[spell_no] *= spells[_active_spell];
            spells.RemoveAt(_active_spell);
            _active_spell = -1;
            DisplaySpells();
        }
    }

    void DisplaySpells()
    {
        foreach (GameObject button in buttons)
        {
            GameObject.Destroy(button);
        }
        buttons.Clear();

        foreach (Matrix4x4 spell in spells)
        {
            int button_no = buttons.Count;

            Transform button_transform = (Transform) Instantiate(_spell_button_prefab, Vector3.zero, Quaternion.identity);
            button_transform.SetParent(transform, false);
            buttons.Add(button_transform.gameObject);

            Button button = button_transform.GetComponent<Button>();
            button.onClick.AddListener(() => { SpellClicked( button_no); });

            button_transform.GetComponentInChildren<Text>().text = GetSpellText(spell);
        }
    }

    string GetSpellText(Matrix4x4 mat)
    {
        string result = "";
        foreach (int x in Enumerable.Range(0, 4))
        {
            foreach (int y in Enumerable.Range(0, 4))
            {
                result += (mat[x,y] >= 0f ?  " " : "") + mat[x, y].ToString("0.00") + " ";
            }
            result = result.Remove(result.Length -1) + "\n";
        }
        return result.Remove(result.Length - 1);
    }

    public Matrix4x4 getSpell()
    {
        if (_active_spell == -1)
            return Matrix4x4.identity;
        else
            return spells[_active_spell];
    }
}
