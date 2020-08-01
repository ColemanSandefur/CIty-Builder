using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : TextMeshProUGUI
{
    private bool update = false;
    protected string _text;
    public string Text {
        get {return _text;}
        set {
            _text = value;
            update = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (update) {
            update = false;
            SetText(_text);
        }
    }
}
