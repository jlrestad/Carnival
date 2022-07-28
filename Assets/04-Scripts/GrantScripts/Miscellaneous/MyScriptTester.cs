using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScriptTester : MonoBehaviour
{
    public HudManager myHUD;
    private void OnEnable()
    {

        myHUD.GameOverCheck();
    }
}
