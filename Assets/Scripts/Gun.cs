using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject muzzleLight;
    public GameObject impactEffect;

    public AudioSource shootAudio;

    void Start()
    {
        shootAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            muzzleLight.GetComponent<Light>().enabled = false;
        }
    }

    // Turn off the light if the fire button is held down.
    IEnumerator OffMuzzleLight()
    {
        yield return new WaitForSeconds(0.05f);

        muzzleLight.GetComponent<Light>().enabled = false;
    }

    void Shoot()
    {
        muzzleLight.GetComponent<Light>().enabled = true;
        muzzleFlash.Play();

        shootAudio.pitch = Random.Range(0.8f, 1.3f);
        shootAudio.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();

            if(target != null)
            {
                target.TakeDamage(damage);
            }

            StartCoroutine(OffMuzzleLight());

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1f);
        }

    }
}
