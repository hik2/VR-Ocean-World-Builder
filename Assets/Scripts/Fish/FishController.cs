using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour {

    public float speedMin;
    public float speedMax;
    private float speed;
    public float rotationSpeed;
    public float neighbourDistance;
    public float avoidanceDistance;
    public float terrainAvoidanceDistance;
    public float terrainAvoidAngleMin;
    public float terrainAvoidAngleMax;
    public float previousTurnTimeoutDuration;
    public LayerMask avoidanceMask;

    public FlockManager flock;

    //Used for turning to avoid terrain
    RaycastHit forwardHit;
    private Quaternion turnDirection;
    private float terrainAvoidanceAngle;
    private bool terrainAvoidanceMode;
    //Terrain avoidance turn combinations
    private Quaternion previousTurn;
    private float previousTurnTimeout;

    //Animaion control
    private Animator fishAnimator;

	// Use this for initialization
	void Start () {
        speed = Random.Range(speedMin, speedMax);
        fishAnimator = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update() {
        fishAnimator.speed = speed / 5;
        previousTurnTimeout -= Time.deltaTime;
        if (terrainAvoidanceMode)
        {
            TerrainAvoidanceTurn();
        }
        else if(Physics.Raycast(transform.position, transform.forward, out forwardHit, terrainAvoidanceDistance, avoidanceMask))
        {
            //Debug.Log("");
            turnDirection = Quaternion.LookRotation(forwardHit.normal, Vector3.up);
            if(previousTurnTimeout >= 0)
            {
                //Averaged the previous and current terran normals
                Vector3 averagedDirections = (previousTurn * Vector3.forward + forwardHit.normal) / 2;
                Quaternion inBetween = Quaternion.LookRotation(averagedDirections);
                //Quaternion inBetween = Quaternion.Slerp(previousTurn, turnDirection, 0.5f);
               // Debug.DrawRay(forwardHit.point, previousTurn * Vector3.forward, Color.cyan, 1f);
                //Debug.DrawRay(forwardHit.point, inBetween * Vector3.forward, Color.green, 1f);
                previousTurnTimeout = previousTurnTimeoutDuration;
                previousTurn = turnDirection;
                turnDirection = inBetween;
            }
            else
            {
                previousTurnTimeout = previousTurnTimeoutDuration;
                previousTurn = turnDirection;
            }
            //Debug.DrawRay(forwardHit.point, forwardHit.normal, Color.red, 1f);
           // Debug.DrawRay(forwardHit.point, turnDirection * Vector3.forward, Color.blue, 1f);
            //Debug.Break();
            float angleToTurn = Quaternion.Angle(transform.rotation, turnDirection);
            terrainAvoidanceAngle = Random.Range(terrainAvoidAngleMin, terrainAvoidAngleMax) * angleToTurn;
           // Debug.Log("Terrain Avoidance Angle: " + terrainAvoidanceAngle + " Angle to turn: " + angleToTurn + " Turn Direction: " + turnDirection);
            terrainAvoidanceMode = true;
            TerrainAvoidanceTurn();
        }
        //if (Vector3.Distance(flock.transform.position, transform.position) > flock.boundary)
        //{
        //    Vector3 direction = flock.transform.position - transform.position;
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        //    speed = Random.Range(speedMin, speedMax);
        //}
        else
        {
            if (Random.Range(0, 100) < 20)
            {
                ApplyRules();
            }
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
	}

    void TerrainAvoidanceTurn()
    {
        float turnSpeed = rotationSpeed * Time.deltaTime;
        Quaternion oldRotation = transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, turnDirection, turnSpeed);
        speed = Random.Range(speedMin, speedMax);
        float angleTurned = Quaternion.Angle(oldRotation, transform.rotation);
        terrainAvoidanceAngle -= angleTurned;
       // Debug.Log("Angle turned: " + angleTurned + " Angle remaining: " + terrainAvoidanceAngle);
        if(terrainAvoidanceAngle <= 0)
        {
            terrainAvoidanceMode = false;
        }
    }

    void ApplyRules()
    {
        List<GameObject> allFish = flock.fish;

        Vector3 vectorAvoid = Vector3.zero;
        Vector3 groupCentre = this.transform.position;
        Vector3 groupFacing = this.transform.forward;
        float groupSpeed = this.speed + 0.1f;

        int groupSize = 1;
        foreach(GameObject otherFish in allFish)
        {
            if(otherFish != this)
            {
                float seperation = Vector3.Distance(this.transform.position, otherFish.transform.position);
                //If the other fish is close enough to be in flock
                if(seperation <= neighbourDistance)
                {
                    groupCentre += otherFish.transform.position;
                    groupFacing += otherFish.transform.forward;
                    groupSize++;

                    //If other fish is too close then avoid
                    if(seperation <= avoidanceDistance)
                    {
                        vectorAvoid -= otherFish.transform.position - this.transform.position;
                    }
                    groupSpeed += otherFish.GetComponent<FishController>().speed;
                }
            }
        }

        //If this fish is in a flock apply flock rules
        if(groupSize > 1)
        {
            speed = groupSpeed / groupSize;
            groupFacing /= groupSize;

            groupCentre /= groupSize;
            Vector3 direction = groupCentre + vectorAvoid + groupFacing - this.transform.position;
            //If there is bait then head towards that as well
            if(flock.fishBait != null)
            {
                direction += (flock.fishBait.transform.position - transform.position);
            }
            if(direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
            }
        }
    }
}
