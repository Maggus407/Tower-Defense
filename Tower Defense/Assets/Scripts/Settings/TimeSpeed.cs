using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpeed : MonoBehaviour
{
    // Update is called once per frame
    private float normalSpeed;
    private float doubleSpeed;
    private float madnessSpeed;

    void Start()
    {
        normalSpeed = 1f;
        doubleSpeed = 2f;
        madnessSpeed = 4f;
    }

    public void NormalSpeed()
    {
        Time.timeScale = normalSpeed;
    }

    public void DoubleSpeed()
    {
        Time.timeScale = doubleSpeed;
    }

    public void MadnessSpeed()
    {
        Time.timeScale = madnessSpeed;
    }

}
