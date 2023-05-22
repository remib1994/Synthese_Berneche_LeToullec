using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Warrior : Player
{


    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        MouvementsJoueur();
        Attack1();
        Attack2();
        Attack3();
        Attack4();
    }
}