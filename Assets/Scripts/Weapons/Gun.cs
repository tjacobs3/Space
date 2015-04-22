using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	public int totalRounds = 30;
	public float fireRate = 0.1f;
	public float reloadTime = 1f;
	public float muzzleLightDuration = 0.03f;
	public bool fullAuto = true;
	public ParticleEmitter muzzleFlash;
	public Light muzzleLight;
    public float adsTime = 0.5f;
    public Transform model;
    public Transform adsPosition;
    public Transform hipPosition;

	public Transform ejectionPort;
	public Rigidbody shell;
	public Vector3 shellVelocityMin;
	public Vector3 shellVelocityMax;
	public Vector3 shellRotation;

	private int rounds = 0;
	private Recoil recoil;
	private float nextFire;
	private float muzzleLightIntensity;
	private float muzzleLightTime = 0f;
	private float shellLiveTime = 2f;
    private bool isADS = false;
    private IFireableGun fireMechanism;

	// Use this for initialization
	void Start () {
		rounds = totalRounds;
		recoil = gameObject.GetComponent<Recoil>();
        fireMechanism = gameObject.GetComponent<IFireableGun>();
		nextFire = Time.time;

		if(muzzleLight != null) {
			muzzleLight.enabled = false;
			muzzleLightIntensity = muzzleLight.intensity;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Reload")) {
			Reload();
		}

		if (rounds > 0 && Input.GetButton("Fire1") && Time.time > nextFire) {
			Fire();
		}

        if (Input.GetButtonDown("Fire2"))
        {
            ToggleADS();
        }

		if(muzzleLight != null) {
			if(muzzleLightTime > 0) {
				muzzleLightTime -= Time.deltaTime;
			} else if(muzzleLight.enabled) {
				muzzleLight.enabled = false;
			}
		}
	}

	void Fire() {
		nextFire = Time.time + fireRate;
		recoil.Fire(isADS);

		if(muzzleFlash != null) {
			muzzleFlash.Emit();
		}

		if(muzzleLight != null) {
			muzzleLight.enabled = true;
			muzzleLightTime = muzzleLightDuration;
		}

		if(shell != null && ejectionPort != null) {
			Rigidbody shellClone = Instantiate(shell, ejectionPort.position, ejectionPort.rotation) as Rigidbody;
			shellClone.velocity = transform.TransformDirection(VectorUtility.RandVector(shellVelocityMin, shellVelocityMax));
			Physics.IgnoreCollision( shellClone.GetComponent<Collider>(), transform.root.GetComponent<Collider>() );
			shellClone.AddTorque(VectorUtility.RandVector(shellRotation / 2, shellRotation), ForceMode.VelocityChange);
			Destroy(shellClone.gameObject, shellLiveTime);
		}

        if (fireMechanism != null)
        {
            fireMechanism.Fire();
        }

		--rounds;
	}

	void Reload() {
		nextFire = Time.time + reloadTime;
		rounds = totalRounds;
	}

    void ToggleADS()
    {
        isADS = !isADS;
        if (model != null && adsPosition != null && hipPosition != null)
        {
            StartCoroutine(ToggleModelPosition());
        }
    }

    IEnumerator ToggleModelPosition()
    {
        float elapsedTime = 0f;
        while (elapsedTime < adsTime)
        {
            elapsedTime += Time.deltaTime;
            if (isADS)
            {
                model.transform.position = Vector3.Lerp(hipPosition.position, adsPosition.position, elapsedTime / adsTime);
                model.transform.localRotation = Quaternion.Lerp(hipPosition.localRotation, adsPosition.localRotation, elapsedTime / adsTime);
            }
            else
            {
                model.transform.position = Vector3.Lerp(adsPosition.position, hipPosition.position, elapsedTime / adsTime);
                model.transform.localRotation = Quaternion.Lerp(adsPosition.localRotation, hipPosition.localRotation, elapsedTime / adsTime);
            }
            yield return null; 
        }
    }
}
