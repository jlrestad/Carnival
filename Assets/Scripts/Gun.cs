using System.Collections;
using UnityEngine;
//using UnityEngine.Experimental.GlobalIllumination;

public class Gun : MonoBehaviour
{

    [SerializeField] int damage = 10;
    [SerializeField] float range = 100f;

    public Camera fpsCam;
    //public GameObject firePoint;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject muzzleLight;
    [SerializeField] GameObject impactEffect;
    [SerializeField] float burstAmount = 3f;
    [SerializeField] float delayFire = 1f;
    [SerializeField] float rateOfFire = 125f;
    [SerializeField] float coolDown = 0.1f;

    [SerializeField] bool canShoot = true;

    public AudioSource shootAudio;

    private void Awake()
    {
        shootAudio = GetComponent<AudioSource>();
        canShoot = true;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot == true)
        {
            StartCoroutine(BurstFire()); //cooldown isn't working
        }

        if (Input.GetButtonUp("Fire1"))
        {
            muzzleLight.GetComponent<Light>().enabled = false;
        }
    }

    IEnumerator BurstFire()
    {
        delayFire = 60 / rateOfFire;

        //Create a burst of fire
        for (int i = 0; i < burstAmount; i++)
        {
            Shoot();
            yield return new WaitForSeconds(delayFire); //amount of time between bullet fire
        }
        canShoot = false;
        yield return new WaitForSeconds(coolDown); //don't allow firing of gun for a cool down period

        canShoot = true;
        yield return null;
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
