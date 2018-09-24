using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicRaycaster : ArcRaycaster {

    Transform activeController;
    public Transform leftController, rightController;

	public LineRenderer lineRenderer;
	public GameObject teleportTarget;
	public LineRenderer pointerOrigin;
	public Transform pointerCursor;

	[Tooltip("Initial velocity of projectile")]
	public float velocity = 10;
	[Tooltip("Initial acceleration of projectile")]
	public float acceleration = 9.8f;
	[Tooltip("Additional flight time allows parabola to dip below origin")]
	public float additionalFlightTime = 0.5f;
	[Tooltip("Number of segments used to check for collision\nSuggested to be 10% of visual parabola")]
	public int segments = 30;

	// Points of parabola
	protected List<Vector3> parabola = new List<Vector3> ();

	public float Angle {
		get {
			return Vector3.Angle (Forward, ControllerForward);
		}
	}

	public Vector3 Velocity {
		get {
			return ControllerForward * velocity;
		}
	}

	public Vector3 Acceleration {
		get {
			return Up * -1.0f * acceleration;
		}
	}

	public float FlightTime {
		get {
			float flightTime = 2.0f * velocity * Mathf.Sin (Angle * Mathf.Deg2Rad) / acceleration;
			return flightTime + additionalFlightTime;
		}
	}

	public float FlightDistance {
		get {
			return (velocity * velocity) * Mathf.Sin ((2.0f * Angle) * Mathf.Deg2Rad) / acceleration;
		}
	}

	public float HitTime { get; protected set; }

	public Vector3 SampleCurve(Vector3 position, Vector3 velocity, Vector3 acceleration, float t) {
		Vector3 result = new Vector3 ();
		result.x = position.x + velocity.x * t + 0.5f * acceleration.x * t * t;
		result.y = position.y + velocity.y * t + 0.5f * acceleration.y * t * t;
		result.z = position.z + velocity.z * t + 0.5f * acceleration.z * t * t;
		return result;
	}

	void Awake() {
		if (trackingSpace == null && OVRManager.instance != null) {
			GameObject cameraObject = OVRManager.instance.gameObject;
			trackingSpace = cameraObject.transform.Find ("TrackingSpace");
			Debug.LogWarning ("Tracking space not set for ParabolicRaycaster");
		}
		if (trackingSpace == null) {
			Debug.LogError ("Tracking MUST BE set for ParabolicRaycaster");
		}
		activeController = rightController;
	}

	void Update()
	{
		for(int i = 0; i < Input.GetJoystickNames().Length; i++){
            if(Input.GetJoystickNames()[i].EndsWith("Left") && activeController == rightController){
                activeController = leftController;
                break;
            } else if(Input.GetJoystickNames()[i].EndsWith("Right") && activeController == leftController){
				activeController = rightController;
				break;
			}
        }

		Vector3 start = activeController.position + activeController.forward * 0.022f;
		pointerOrigin.SetPosition(0, start);
		pointerOrigin.SetPosition(1, activeController.position + activeController.forward * 0.5f);

		MakingContact = false;

		parabola.Clear();
		parabola.Add(start);

		Vector3 last = start;
		Vector3 velocity = Velocity;
		Vector3 acceleration = Acceleration;

		RaycastHit hit;
		float recip = 1.0f / (float)(segments - 1);
		lineRenderer.positionCount = 1;
		lineRenderer.SetPosition(0, start);
		for(int i = 1; i < segments; i++) {
			float t = (float)i * recip;
			t *= FlightTime;

			Vector3 next = SampleCurve(start, velocity, acceleration, t);

			if (Physics.Linecast (last, next, out hit, ~excludeLayers)) {
				parabola.Add (hit.point);
				Normal = hit.normal;
				HitPoint = hit.point;

				lineRenderer.positionCount++;
				lineRenderer.SetPosition(i, hit.point);

				float angle = Vector3.Angle(Vector3.up, hit.normal);
				if (angle < surfaceAngle) {
					HitTime = t;
					pointerCursor.position = HitPoint;
					MakingContact = true;
				} else {
					if(Physics.Raycast(hit.point + hit.normal * 0.1f, Vector3.down, out hit)){
						angle = Vector3.Angle(Vector3.up, hit.normal);
						if(angle < surfaceAngle){
							pointerCursor.position = HitPoint;
							lineRenderer.positionCount++;
							lineRenderer.SetPosition(i + 1, HitPoint + Vector3.down * hit.distance);
							Normal = hit.normal;
							HitPoint = hit.point;
							MakingContact = true;
						}
					}
				}
				if(MakingContact){
					teleportTarget.SetActive(true);
					teleportTarget.transform.position = HitPoint;
				} else {
					teleportTarget.SetActive(false);
				}
				break;
			} else {
				parabola.Add (next);
				lineRenderer.positionCount++;
				lineRenderer.SetPosition(i, next);
				pointerCursor.position = next;
			}

			last = next;
		}
	}
}