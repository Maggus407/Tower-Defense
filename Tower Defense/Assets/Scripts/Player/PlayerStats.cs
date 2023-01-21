using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{

    public static int Lives;
    public int startlives = 100;

    public static int Money;
    public int startMoney = 100;

    public static int Rounds;


    // Start is called before the first frame update
    void Start()
    {
        Lives = startlives;
        Money = startMoney;

        Rounds = 0;
    }


}
