using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class SpellManager : MonoBehaviour {
    public Transform _spell_button_prefab;
    int _active_spell = -1;
    List<Matrix4x4> _spells = new List<Matrix4x4>();
    List<GameObject> _buttons = new List<GameObject>(); 

    // Use this for initialization
    void Start () {
        _spells.Add(Matrix4x4.TRS(new Vector3(0f, 2f, 0f), Quaternion.identity, Vector3.one));
        _spells.Add(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 45f), Vector3.one));
        _spells.Add(Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 2));
        _spells.Add(_spells[1] * _spells[0]);
        _spells.Add(_spells[0] * _spells[1]);
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
            _spells[spell_no] *= _spells[_active_spell];
            _spells.RemoveAt(_active_spell);
            _active_spell = -1;
            DisplaySpells();
        }
    }

    void DisplaySpells()
    {
        foreach (GameObject button in _buttons)
        {
            GameObject.Destroy(button);
        }
        _buttons.Clear();

        foreach (Matrix4x4 spell in _spells)
        {
            int button_no = _buttons.Count;

            Transform button_transform = (Transform) Instantiate(_spell_button_prefab, Vector3.zero, Quaternion.identity);
            button_transform.SetParent(transform, false);
            _buttons.Add(button_transform.gameObject);

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
        Matrix4x4 spell;
        if (_active_spell == -1)
            spell= Matrix4x4.identity;
        else
        {
            spell = _spells[_active_spell];

            _spells.RemoveAt(_active_spell);
            _active_spell = -1;
            DisplaySpells();
        }
        return spell;
    }
}
