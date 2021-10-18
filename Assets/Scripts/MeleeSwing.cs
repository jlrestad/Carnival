using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSwing : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject body;
    //[SerializeField] BoxCollider collider;

    private void Start()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            body.SetActive(false);
            target.GetComponent<Rigidbody>().isKinematic = false;
            target.GetComponent<Rigidbody>().useGravity = true;
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
