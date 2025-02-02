﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class SpellManager : MonoBehaviour {
    public Transform _spell_button_prefab;
    int _active_spell = -1;
    List<Matrix4x4> _spells = new List<Matrix4x4>();
    List<GameObject> _buttons = new List<GameObject>();

    public Color _active_button_color = Color.black;
    Color _inactive_button_color; // Set via the prafab

    // Use this for initialization
    void Start () {
        /*_spells.Add(Matrix4x4.TRS(new Vector3(0f, 2f, 0f), Quaternion.identity, Vector3.one));
        _spells.Add(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 45f), Vector3.one));
        _spells.Add(Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 2));*/
        DisplaySpells();
    }
	
	// Update is called once per frame
	void Update () {
        _inactive_button_color = _spell_button_prefab.GetComponent<Button>().image.color;
    }

    void SpellClicked(int spell_no)
    {
        Debug.Log("spell " + spell_no);
        if (_active_spell == -1)
        {
            _buttons[spell_no].GetComponent<Button>().image.color = _active_button_color;
            _active_spell = spell_no;
        } else if (_active_spell == spell_no)
        {
            _active_spell = -1;
            _buttons[spell_no].GetComponent<Button>().image.color = _inactive_button_color;
        } else
        {
            _spells[spell_no] = _spells[spell_no] * _spells[_active_spell];
            _spells.RemoveAt(_active_spell);

            if (_active_spell > spell_no)
                _active_spell = spell_no;
            else // After removal of a spell that preceded the current one
                _active_spell = spell_no - 1;

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
            button.name = "Spell button " + button_no;

            if (_active_spell == button_no)
                _buttons[button_no].GetComponent<Button>().image.color = _active_button_color;

            button_transform.GetComponentInChildren<Text>().text = GetSpellText(spell);
        }
    }

    string GetSpellText(Matrix4x4 mat)
    {
        string result = "";
        foreach (int x in Enumerable.Range(0, 4))
        {
            if (x == 2) continue;
            foreach (int y in Enumerable.Range(0, 4))
            {
                if (y == 2) continue;
                result += (mat[x,y] >= 0f ?  " " : "") + mat[x, y].ToString("0.00") + " ";
            }
            result = result.Remove(result.Length -1) + "\n";
        }
        return result.Remove(result.Length - 1);
    }

    public Matrix4x4 GetSpell()
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

    public void AddSpell(Matrix4x4 spell)
    {
        _spells.Add(spell);
        DisplaySpells();
    }
}
