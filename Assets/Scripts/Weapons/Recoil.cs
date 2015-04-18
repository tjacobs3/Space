using UnityEngine;
using System.Collections;

public class Recoil : MonoBehaviour {
	private Vector3 originalPosition;
	private Vector3 originalRotation;

	public Vector3 recoilPositionMax;
	public Vector3 recoilPositionMin;
	public Vector3 recoilRotationMax;
	public Vector3 recoilRotationMin;

    public Vector3 recoilPositionMaxADS;
    public Vector3 recoilPositionMinADS;
    public Vector3 recoilRotationMaxADS;
    public Vector3 recoilRotationMinADS;

	private Vector3 targetPosition;
	private Vector3 targetRotation;

	private Vector3 lastPosition;
	private Vector3 lastRotation;

	public float recoverTime = 0.1f;
	private float recovering = 0.21f;

	// Use this for initialization
	void Start () {
		originalPosition = this.transform.localPosition;
		originalRotation = this.transform.localRotation.eulerAngles;
		recovering = recoverTime + 1;
	}
	
	// Update is called once per frame
	void Update () {
		if(recovering <= recoverTime) {
			this.transform.localPosition = GetValue(recovering / recoverTime, lastPosition, targetPosition, originalPosition);
			this.transform.localEulerAngles = GetValue(recovering / recoverTime, lastRotation, targetRotation, originalRotation);

			recovering += Time.deltaTime;

			if(recovering >= recoverTime) {
				this.transform.localPosition = originalPosition;
				this.transform.localEulerAngles = originalRotation;
			}
		}
	}

	public void Fire(bool isADS) {
        Vector3 positionMin = isADS ? recoilPositionMinADS : recoilPositionMin;
        Vector3 positionMax = isADS ? recoilPositionMaxADS : recoilPositionMax;
        Vector3 rotationMin = isADS ? recoilRotationMinADS : recoilRotationMin;
        Vector3 rotationMax = isADS ? recoilRotationMaxADS : recoilRotationMax;

        float newX = Random.Range(positionMin.x, positionMax.x);
        float newY = Random.Range(positionMin.y, positionMax.y);
        float newZ = Random.Range(positionMin.z, positionMax.z);

        float newRX = Random.Range(rotationMin.x, rotationMax.x);
        float newRY = Random.Range(rotationMin.y, rotationMax.y);
        float newRZ = Random.Range(rotationMin.z, rotationMax.z);

		this.lastPosition = this.transform.localPosition;
		this.lastRotation = VectorUtility.normalize180(this.transform.localRotation.eulerAngles);
		this.targetPosition = new Vector3(this.lastPosition.x + newX, this.lastPosition.y + newY, this.lastPosition.z + newZ);
		this.targetRotation = new Vector3(this.lastRotation.x + newRX, this.lastRotation.y + newRY, this.lastRotation.z + newRZ);
		this.recovering = 0.0f;
	}

	private Vector3 GetValue (float t, Vector3 current, Vector3 target, Vector3 origin) {
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			oneMinusT * oneMinusT * current +
				2f * oneMinusT * t * target +
				t * t * origin;
	}
}
