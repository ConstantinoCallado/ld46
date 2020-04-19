using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DancersOnFireCounter : MonoBehaviour
{
    public static int dancersOnFire;
    Text dancersText;

    void Awake()
    {
        dancersOnFire = 0;
        dancersText = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (dancersOnFire == 0)
        {
            dancersText.text = "";
        }
        else
        {
            dancersText.text = "Dancers on Fire: " + dancersOnFire;
        }
    }
}
