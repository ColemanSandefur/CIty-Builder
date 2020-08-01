using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityStats : MonoBehaviour
{
    public TextDisplay balanceDisplay;
    [SerializeField] private int _balance = 100;
    public int Balance {
        get {return _balance;}
        set {
            _balance = value;
            balanceDisplay.SetText("Balance: " + _balance);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Balance = _balance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
