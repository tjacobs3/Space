using UnityEngine;
using System.Collections;

public class RayGun : MonoBehaviour, IFireableGun {
    public ParticleSystem hitSparks;
    public float damage = 0.0f;
    public float range = 10.0f;

    public void Fire()
    {
        Debug.DrawLine(transform.position, transform.position + (transform.TransformDirection(Vector3.forward) * 10), Color.white, 0.2f);
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd, out hit, range)) {
			Debug.Log(hit.collider.gameObject.name);
            if (hitSparks)
            {
                ParticleSystem sparkClone = (ParticleSystem)Instantiate(hitSparks, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(sparkClone.gameObject, sparkClone.duration);
            }
        }
    }
}
