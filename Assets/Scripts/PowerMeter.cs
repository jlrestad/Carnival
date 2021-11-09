using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PowerMeter : MonoBehaviour
{
    public float amount; // How much to increment/decrement by
    public float wait; // Time to wait till increment/decrement
    public float maxPower; // What the start count is
    public float currentPower; // The Current Count

    private void Start()
    {
        StartCoroutine(IncreasePower());
    }

    IEnumerator IncreasePower()
    {
        if (currentPower < maxPower)
        {
            yield return new WaitForSeconds(wait); // Wait for a set amount of seconds
            currentPower += amount;    // Increment
            StartCoroutine(IncreasePower()); // Re-Play
        }
        else if (currentPower > 0f)
        {
            StartCoroutine(DecreasePower());
        }
    }

    IEnumerator DecreasePower()
    {
        yield return new WaitForSeconds(wait); // Wait for a set amount of seconds
        currentPower -= amount;    // Decrement
        StartCoroutine(DecreasePower()); // Re-Play
    }
}


