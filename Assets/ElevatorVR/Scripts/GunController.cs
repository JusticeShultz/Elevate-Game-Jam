/// <summary>
/// 
/// Simple script that handles weapon firing, etc.
/// 
/// Created by Justice Shultz.
/// 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [Header("Input Fields")]

    [Tooltip("Which hand do we fire from?")]
        public SteamVR_Input_Sources HandType;

    [Tooltip("What event makes us shoot?")]
        public SteamVR_Action_Boolean GrabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");

    [Tooltip("What event makes us cooldown the weapon?")]
        public SteamVR_Action_Boolean GrabGripAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");

    [Header("Gun Related")]

    [Tooltip("The delay time before the next shot may be fired.")]
        public float FireSpeed = 0.5f;

    [Tooltip("What is the max value our cooldown cap can reach before needing to cooldown for a moment.")]
        public float CooldownCap = 5.0f;

    [Tooltip("How much heat will be output with each shot.")]
        public float CooldownGain = 0.25f;

    [Tooltip("How long does the manual cooldown take?")]
        public float ManualCooldownTime = 1.0f;

    [Tooltip("How long does the automatic cooldown take?")]
        public float AutoCooldownTime = 2.0f;

    [Tooltip("How much damage our bullet will do.")]
        public float Damage = 1.0f;

    [Tooltip("The rate at which the bullet flies.")]
        public float BulletFlySpeed = 10.5f;

    [Header("Objects")]

    [Tooltip("Which prefab will be used as our object.")]
        public GameObject BulletObj;

    [Tooltip("Which object will hold our bullets.")]
        public GameObject BulletPool;

    [Tooltip("The particle system that will play on reload.")]
        public ParticleSystem ReloadParticle;

    [Tooltip("Which animator will pick up the fire trigger.")]
        public Animator GunAnimator;

    [Tooltip("The image that will render the cooldown.")]
        public Image FillImage;

    //This will be the behind the scenes pool handling our bullets.
    [HideInInspector] public List<GameObject> bulletPool = new List<GameObject>();

    //This will hold how long before the next bullet is.
    float local_FireCD = 0;

    //This will be where our current cooldown rate is at.
        float local_CooldownCharge = 0;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start ()
    {
        DontDestroyOnLoad(BulletPool);

        //Instantiate our bullet objects.
		for(int i = 0; i < 100; ++i)
            bulletPool.Add(Instantiate(BulletObj));

        //Update all the created objects to be inactive.
        foreach(GameObject obj in bulletPool)
        {
            obj.GetComponent<TrailRenderer>().enabled = false;
            obj.transform.parent = BulletPool.transform;
            obj.SetActive(false);
            obj.transform.position = Vector3.zero;
        }
	}

    void Update()
    {
        FillImage.fillAmount = Mathf.Lerp(FillImage.fillAmount, local_CooldownCharge / CooldownCap, 0.05f);

        if (local_FireCD > 0)
        {
            local_FireCD -= Time.deltaTime;
            return;
        }

        if (GrabPinchAction.GetStateDown(HandType) && local_CooldownCharge < CooldownCap)
        {
            local_FireCD = FireSpeed;
            local_CooldownCharge += CooldownGain;

            GunAnimator.SetTrigger("Shoot");

            foreach (GameObject obj in bulletPool)
            {
                if (!obj.activeSelf)
                {
                    obj.transform.position = transform.position;
                    obj.SetActive(true);
                    obj.GetComponent<Rigidbody>().velocity = transform.forward * BulletFlySpeed;
                    obj.GetComponent<BulletController>().Damage = Damage;
                    break;
                }
            }
        }

        if (GrabGripAction.GetStateDown(HandType) && local_CooldownCharge > 0)
        {
            local_FireCD = ManualCooldownTime;
            local_CooldownCharge = 0;
            ReloadParticle.Play();
        }
        
        if (local_CooldownCharge >= CooldownCap)
        {
            local_FireCD = AutoCooldownTime;
            local_CooldownCharge = 0;
            ReloadParticle.Play();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Update all the created objects to be inactive.
        foreach (GameObject obj in bulletPool)
        {
            obj.SetActive(false);
            obj.transform.position = Vector3.zero;
        }
    }
}
