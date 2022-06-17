using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Reset_To_Title : MonoBehaviour
{
    public void ResetToTitle()
    {
        SceneManager.LoadScene(0);
    }
}
