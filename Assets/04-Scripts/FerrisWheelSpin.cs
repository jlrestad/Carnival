using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheelSpin : MonoBehaviour
{
    [SerializeField] private int degrees = 20;
    public GameObject pivot;

    private void FixedUpdate()
    {
        transform.RotateAround(pivot.transform.position, Vector3.forward, degrees * Time.deltaTime);
    }

    protected void LateUpdate()
    {
        //Lock x and y rotation
        transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);
    }
}
