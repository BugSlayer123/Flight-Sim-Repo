using UnityEngine;
using System;

[ExecuteInEditMode]
public class AirplaneGUI : MonoBehaviour
{
    public bool debugMode = false;

    public Rect positionSPEED = new Rect(10, 40, 300, 20);
    public Rect positionTHROTTLE = new Rect(10, 60, 300, 20);
    public Rect positionALTITUDE = new Rect(10, 80, 300, 20);
	public Rect positionGLOAD = new Rect(10, 100, 300, 20);
	public Rect positionBox1 = new Rect(10, 40, 300, 20);
	public Rect positionBox2 = new Rect(10, 60, 300, 20);
    public Rect positionBox3 = new Rect(10, 80, 300, 20);
	public Rect positionBox4 = new Rect(10, 80, 300, 20);
	public GUISkin skin = null;
	public Texture BoxTexture;

	public Rigidbody Rigidbody { get; internal set; }
	private WorldManager world = null;
	private float throttle = 1.21f;


	private void Awake()
	{
		Rigidbody = GetComponent<Rigidbody>();
		world = GameObject.FindObjectOfType<WorldManager>();
	}

	private float CalculatePitchG()
	{
		// Angular velocity is in radians per second.
		Vector3 localVelocity = transform.InverseTransformDirection(Rigidbody.velocity);
		Vector3 localAngularVel = transform.InverseTransformDirection(Rigidbody.angularVelocity);

		// Local pitch velocity (X) is positive when pitching down.

		// Radius of turn = velocity / angular velocity
		float radius = (Mathf.Approximately(localAngularVel.x, 0.0f)) ? float.MaxValue : localVelocity.z / localAngularVel.x;

		// The radius of the turn will be negative when in a pitching down turn.

		// Force is mass * radius * angular velocity^2
		float verticalForce = (Mathf.Approximately(radius, 0.0f)) ? 0.0f : (localVelocity.z * localVelocity.z) / radius;

		// Express in G (Always relative to Earth G)
		float verticalG = verticalForce / -9.81f;

		// Add the planet's gravity in. When the up is facing directly up, then the full
		// force of gravity will be felt in the vertical.
		verticalG += transform.up.y * (Physics.gravity.y / -9.81f);

		return verticalG;
	}

	private void OnGUI()
	{
		const float msToKnots = 1.94384f;
		GUI.skin = skin;
		GUI.Label(positionSPEED, string.Format("SPEED: {0:0.0} KTS", Rigidbody.velocity.magnitude * msToKnots));
		GUI.Label(positionTHROTTLE, string.Format("THROTTLE: {0:0}%", Rigidbody.velocity.magnitude * throttle));
        GUI.Label(positionGLOAD, string.Format("G LOAD: {0:0.0} G", CalculatePitchG()));
		GUI.Label(positionALTITUDE, string.Format("ALTITUDE: {0:0} FT", Mathf.Abs(world.transform.position.x - Rigidbody.position.y) * 1.0f));
		GUI.Box(positionBox1, BoxTexture);
		GUI.Box(positionBox2, BoxTexture);
        GUI.Box(positionBox3, BoxTexture);
		GUI.Box(positionBox4, BoxTexture);
	}
}
