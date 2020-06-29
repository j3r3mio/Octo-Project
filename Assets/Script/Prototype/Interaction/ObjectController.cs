﻿using UnityEngine;

// Extends the player controller class to be controllable
public class ObjectController : PlayerController
{
    public ColorManager colorMgr;
    public Transform PlayerLeavePoint { get; private set; }
    public PlayerController Player { get; set; }
    public GameObject smoke;
    public GameObject yellowSmoke;
    public GameObject magentaSmoke;

    //Introductions DialogueTriggering
    public bool b_dialogueHappenned = false;
    public bool b_dialogueAllowed = false;
    public DialogueTrigger dialogueTrigger;

    //Animation props
    public Animator animator;
    //public Animation propsWalk;

    [Header("Sound")]
    public AudioClip Pos;
    public float volume;

    [Header("Tuto Depos")]
    public GameObject deposTuto;
    public bool hasActivated = false;

    // override the player controller start method
    protected override void Start()
    {
        //Tuto Depos
        hasActivated = false;

        base.Start(); // Call the player controller start
        colorMgr.Initialize(GetComponent<SkinnedMeshRenderer>());
        colorMgr.InitializeObjectRenderer(GetComponent<MeshRenderer>());
        PlayerLeavePoint = transform.GetChild(0);
        isControlled = false; // Disable controls for this object
    }

    protected override void Update()
    {
        if (!isControlled) return;
        base.Update();

        ObjectController obj = this.GetComponent<ObjectController>();

        //DialogueTriggering
        if (!b_dialogueHappenned & b_dialogueAllowed)
        {
            ThisObjectDialogueTrigger();
        }

        //Tuto Depos
        deposTuto.SetActive(true);
        hasActivated = true;

        if (Input.GetKey(KeyCode.E))
        {
            AudioSource.PlayClipAtPoint(Pos, transform.position, volume);

            //Tuto Depos
            if (hasActivated)
            {
                deposTuto.SetActive(false);
            }

            Instantiate(smoke, obj.transform.position, obj.transform.rotation);
            Player.ControlObject(this, false, Player);
            Destroy(GameObject.Find("Rework Smoke(Clone)"), 2f);
            StopPlayFootstepSound();
        }
    }

    //Function to trigger the dialogue of this object only once
    public void ThisObjectDialogueTrigger()
    {
        dialogueTrigger.TriggerDialogue();
        b_dialogueHappenned = true;
        Debug.Log(b_dialogueHappenned);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("ColorSwap"))
        {
            ObjectController obj = this.GetComponent<ObjectController>();

            if (Input.GetKey(KeyCode.A) && isControlled)
            {
                ColorChanger changer = other.GetComponent<ColorChanger>();
                changer?.SwapColors(colorMgr); // Check if the changer exists and swap colors
                Player.GetComponent<PlayerManager>().colorMgr.SetCurrentColor(colorMgr.GetCurrentColor()); // Update the player color

                if (colorMgr.GetCurrentColor() == ObjectColor.YELLOW)
                {
                    Instantiate(yellowSmoke, obj.transform.position, obj.transform.rotation);
                    Destroy(GameObject.Find("Rework Yellow Smoke(Clone)"), 2f);
                }
                else if (colorMgr.GetCurrentColor() == ObjectColor.MAGENTA)
                {
                    Instantiate(magentaSmoke, obj.transform.position, obj.transform.rotation);
                    Destroy(GameObject.Find("Rework Magenta Smoke(Clone)"), 2f);
                }
            } 
        }
        else
        {
            return;
        }
    }
}
