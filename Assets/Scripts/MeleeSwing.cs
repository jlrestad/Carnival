using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSwing : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject body;
    [SerializeField] BoxCollider collider;

    private void Start()
    {
        //collider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            MeleeAttack();
        }    
        if (Input.GetButtonUp("Fire1"))
        {
            Return();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider)
        {
            body.SetActive(false);
        }
    }

    public void MeleeAttack()
    {
        transform.Rotate(Vector3.right, 87f);
    }

    public void Return()
    {
        transform.Rotate(Vector3.right, 36f);
    }

    protected void LateUpdate()
    {
        //Lock x and y rotation
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0f, 0f);
    }
}
