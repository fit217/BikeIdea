﻿using System.Collections;

using System.Collections.Generic;
using UnityEngine;


// TODO: Make inherit from Pawn class
/// <summary>Class <c>BikeScript</c> A Unity Component which holds the logic for the Player movement.</summary>
/// Expects there to be an LMG spawned in-place on the bike's location
public class BikeScript : MonoBehaviour
{
    private float distanceToHP; // The current distance to the healthpool.
    private float consecutiveDistanceToHP; // The distance to the healthpool the first time we raycast hit it.

    public Gun currentGun;
    public TurretScript turret;

    public BikeMovementComponent movementComponent;
    private EmmissiveBikeScript emissiveBike;

    private int healthPoolLayer = 6;
    private int healthPoolLayerMask; // A mask that that represents the HealthPool layer

    #region Resets
    /// <summary>This method resets bike health to starting health</summary>
    public void ResetBikeHealth(){
        movementComponent.resetHealth();
    }
    /// <summary>This method resets bike motion to starting motion</summary>
    public void ResetBikeMotion(){
        movementComponent.resetMotion();
    }
    #endregion
    #region Accessors
    public float Energy
    {
        get => movementComponent.HitPoints;
    }

    // TODO: see if we can make attribute
    /// <summary>This method gets the direction the bike's mesh is currently facing in world coordinates.</summary>
    /// <returns>A Vector3 of the bike's forward vector in world coordinates. The Vector's x represents the x direction 
    /// in world coordinates and the vector's y represents the z direction in world coordinates.</returns>
    public Vector3 ForwardVector()
    {
        return movementComponent.ForwardVector();
    }

    #region Camera

    // The empty object the camera follows
    public GameObject cameraFollower;

    // Height of the empty object the camera follows
    public float FollowerHeight 
    {
        get 
        {
            return cameraFollower.transform.position.y;
        }
        set 
        {
            cameraFollower.transform.position = new Vector3(cameraFollower.transform.position.x, value, cameraFollower.transform.position.z);
        }
    }

    // Returns the transform of the empty object the camera follows
    public Transform CameraFollower 
    {
        get 
        {
            return cameraFollower.transform;
        }
    }
    #endregion
    #endregion
    #region Monobehavior
    private void Awake()
    {
        Init();

        // TODO: make this less gross. Update gun/equip code
        InitializeStartingGun();
    }


    private void Update()
    {
        UpdateBikeEmission();
    }


    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            currentGun.Shoot(movementComponent.rb.velocity);
            if(turret!= null)
            {
                turret.Shoot(movementComponent.rb.velocity);
            }
            
        }
    }

    /// <summary>Initialize this class's variables. A replacement for a constructor.</summary>
    public void Init()
    {
        emissiveBike = GetComponentInChildren<EmmissiveBikeScript>();
        movementComponent = GetComponent<BikeMovementComponent>();


        //Initializes Turret 
        if(GetComponentInChildren<TurretScript>() != null) { 
            turret = GetComponentInChildren<TurretScript>();
            turret.Init();
            turret.BulletShot += movementComponent.bl_ProcessCompleted;
        }
        
        healthPoolLayerMask = (1 << healthPoolLayer);
        consecutiveDistanceToHP = 0;
    }

    #endregion

    #region GunCode
    /// <summary>Equips the gun to the bike.</summary>
    /// <param name="gunToEquip">The gun which will be hooked up to the bike's bl_ProcessCompleted. Will be set as a 
    /// child of bikeMeshParent.</param>
    public void EquipGun(Gun gunToEquip) 
    {
        if (currentGun != null) 
        {
            // Remove event handled from current gun
            currentGun.BulletShot -= movementComponent.bl_ProcessCompleted;
        }

        this.currentGun = gunToEquip;
        // Make gun child of TracerMeshParent
        currentGun.transform.parent = movementComponent.bikeMeshParent.transform;

        // Hook up event
        currentGun.BulletShot += movementComponent.bl_ProcessCompleted;
    }

    /// <summary>Initialize the gun for the player to start with.</summary>
    private void InitializeStartingGun()
    {
        // TODO: Make this better
        DoubleBarrelLMG[] guns = Object.FindObjectsOfType<DoubleBarrelLMG>();
        if (guns.Length <= 0)
        {
            Debug.LogError("BikeScript did not find any DoubleBarrelLMGs in scene");
        }
        else
        {
            EquipGun(guns[0]);
        }
    }
    #endregion

    #region Health Related
    /// <summary>Checks to see if a HealthPool is in front of the bike.</summary>
    /// <returns>True when a HealthPool is in front of the bike.</returns>
    private bool HealthPoolCheck()
    {
        Ray ray = new Ray(transform.position, ForwardVector());
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, Mathf.Infinity,  healthPoolLayerMask))
        {
            //Debug.Log("Hit something: " + hitData.collider.gameObject.name);
            distanceToHP = hitData.distance;
            if (consecutiveDistanceToHP == 0) 
            {
                consecutiveDistanceToHP = hitData.distance;
            }
            return true;
        }
        else
        {
            consecutiveDistanceToHP = 0;
            return false;
        }
    }


    /// <summary>Sets the bike's emission material color to a specific color if the bike is or is not pointing at a 
    /// healthpool.</summary>
    private void UpdateBikeEmission()
    {
        if (HealthPoolCheck()) 
        {
            emissiveBike.SetHPDistance(distanceToHP, consecutiveDistanceToHP);
        }
        else 
        {
            emissiveBike.SetNotAheadColor();
        }
    }
    #endregion
}
