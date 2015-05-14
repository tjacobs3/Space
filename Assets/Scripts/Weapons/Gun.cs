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
    public Light light;

	public Transform ejectionPort;
	public Rigidbody shell;
	public Vector3 shellVelocityMin;
	public Vector3 shellVelocityMax;
	public Vector3 shellRotation;
    public Inventory.AmmoType ammoType = Inventory.AmmoType.Pistol;

    public float cameraShakeMagnitude = 0.2f;
    public float cameraShakeDuration = 0.2f;
    public float cameraShakeSpeed = 10f;

	private int rounds = 0;
	private Recoil recoil;
	private float nextFire;
	private float muzzleLightIntensity;
	private float muzzleLightTime = 0f;
	private float shellLiveTime = 2f;
    private bool isADS = false;
    private IFireableGun fireMechanism;
    private CharacterController parentRigidBody;
    private Inventory ammoSource;

	// Use this for initialization
	void Start () {
		recoil = gameObject.GetComponent<Recoil>();
        fireMechanism = gameObject.GetComponent<IFireableGun>();
        ammoSource = transform.root.GetComponentInChildren<Inventory>();
		nextFire = Time.time;
        parentRigidBody = findParentRigidBody();
        reloadRounds();

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

        if (Input.GetButtonDown("Toggle Light"))
        {
            ToggleLight();
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
            if (parentRigidBody != null)
            {
                shellClone.velocity = shellClone.velocity + parentRigidBody.velocity;
            }
			Destroy(shellClone.gameObject, shellLiveTime);
		}

        if (fireMechanism != null)
        {
            fireMechanism.Fire();
        }

        SendMessageUpwards("ShakeCamera", new Vector3(cameraShakeMagnitude, cameraShakeDuration, cameraShakeSpeed));
		--rounds;
        ammoSource.useAmmo(ammoType);
	}

	void Reload() {
		nextFire = Time.time + reloadTime;
        reloadRounds();
	}

    void reloadRounds()
    {
        rounds = Mathf.Min(totalRounds, ammoSource.totalAmmo(ammoType));
    }

    void ToggleADS()
    {
        isADS = !isADS;
        if (model != null && adsPosition != null && hipPosition != null)
        {
            StartCoroutine(ToggleModelPosition());
        }
    }

    void ToggleLight()
    {
        if (light != null)
        {
            light.enabled = !light.enabled;
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

    private CharacterController findParentRigidBody()
    {
        Transform o = transform.parent;
        while (o != null)
        {
            CharacterController r = o.GetComponent<CharacterController>();
            if (r != null) return r;
            o = o.parent;
        }
        return null;
    }
}
