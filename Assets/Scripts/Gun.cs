using System.Collections;
using UnityEngine;
//using UnityEngine.Experimental.GlobalIllumination;

public class Gun : MonoBehaviour
{

    [SerializeField] int damage = 10;
    [SerializeField] float range = 100f;

    public Camera fpsCam;
    //public GameObject firePoint;
    public ParticleSystem muzzleFlash;
    public GameObject muzzleLight;
    public GameObject impactEffect;
    [SerializeField] int rateOfFire = 10;

    public AudioSource shootAudio;

    private void Awake()
    {
        shootAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //Burst shot
            for (int i = 0; i < rateOfFire; i++)
            {
                Shoot();
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            muzzleLight.GetComponent<Light>().enabled = false;
        }
    }

    // Turn off the light if the fire button is held down.
    IEnumerator TurnOffMuzzleLight()
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

            StartCoroutine(TurnOffMuzzleLight());

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1f);
        }

    }
}
