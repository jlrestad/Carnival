using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;
//using UnityEngine.Experimental.GlobalIllumination;

public class Gun : MonoBehaviour
{

    //[SerializeField] int damage = 10;
    [SerializeField] float range = 100f;

    public Camera fpsCam;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject muzzleLight;
    [SerializeField] GameObject impactEffect;
    [SerializeField] float force = 10.0f;
    [SerializeField] float burstAmount = 3.0f;
    [SerializeField] float delayFire = 1.0f;
    [SerializeField] float rateOfFire = 666f;
    [SerializeField] float coolDown = 0.3f;

    [HideInInspector] public RaycastHit hit;

    [SerializeField] bool canShoot = true;

    public GameObject brokenBottle;
    public AudioSource shootAudio;

    private void Awake()
    {
        shootAudio = GetComponent<AudioSource>();
        canShoot = true;
    }

    private void Start()
    {
        canShoot = true;
    }
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot || Input.GetAxis("RtTrigger") > 0 && canShoot)
        {
            StartCoroutine(BurstFire());
            GetTriggerUse();
        }

        if (Input.GetButtonUp("Fire1") || Input.GetAxis("RtTrigger") > 0)
        {
            muzzleLight.GetComponent<Light>().enabled = false;
        }
        else
        {
            return;
        }
    }

    //Used to control Joystick trigger from the ability to spam fire.
    void GetTriggerUse()
    {
        if (Input.GetAxis("RtTrigger") > 0)
        {
            canShoot = false;
        }
        else
        {
            canShoot = true;
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
        //Turn on the light effect
        muzzleLight.GetComponent<Light>().enabled = true;
        muzzleFlash.Play();

        //Play the sound effect
        shootAudio.pitch = Random.Range(0.8f, 1.3f);
        shootAudio.Play();

        //Get raycast hit information and use it to calculate damage
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            //Debug.Log(hit.transform.name);

            Transform target = hit.transform.GetComponent<Transform>();
            TargetSetActive targetScript = hit.transform.GetComponentInChildren<TargetSetActive>();
            BossTarget bossTargetScript = hit.transform.GetComponentInChildren<BossTarget>();

            //FOR SKILLSHOT
            if (target != null && target.CompareTag("MovingTarget") && !targetScript.targetHit)
            {
                targetScript.HitTarget();
            }

            //FOR FREAKSHOW-BOSS
            if (target != null && target.CompareTag("BossTarget") && !bossTargetScript.targetHit)
            {
                StartCoroutine(bossTargetScript.HitTarget());
            }

            //FOR BREAKABLES
            if (target != null && target.CompareTag("BottleBreakable"))
            {
                //Swap unbroken for broken object.
                Instantiate(brokenBottle, target.transform.position, target.transform.rotation);
                Destroy(target.gameObject);
            }
            if (target.CompareTag("BottleBroken"))
            {
                //Add force to the broken object rigidbody.
                hit.rigidbody.AddForce(target.up * force);
            }

            StartCoroutine(TurnOffMuzzleLight());

            //Instantiate the particle effect
            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 1f); //Should this be turned off instead of destroyed for optimization?
        }
    }
}
