using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Hellecopter hellicopter;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Effectuate()
    {
        hellicopter.StartTakeOff();
    }
}
