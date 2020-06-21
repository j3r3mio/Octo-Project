﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateController : MonoBehaviour
{
    public ColorManager colorMgr;
    public bool isActive = false;
    public int pressurePlateValue = 0;
    public GameObject diodeColor;
    public GameObject diodeColor1;
    public Material initialMaterial;

    [Header("Sound")]
    public AudioClip PlateActivation;
    public float volume;
    public bool b_HasActivated = false;

    [Header("Dialogue")]
    public bool b_dialogueHappenned = false;
    public DialogueTrigger dialogueTrigger;

    public delegate void OnPressurePlateActivatedDelegate();
    public event OnPressurePlateActivatedDelegate OnPressurePlateActivated;

    public void OnTriggerStay(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            PlayerManager player = collider.GetComponent<PlayerManager>();

            if (player.colorMgr.GetCurrentColor() == colorMgr.GetCurrentColor())
            {
                pressurePlateValue = collider.GetComponent<PlayerController>().weight;

                if (pressurePlateValue == 2)
                {
                    diodeColor.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
                    diodeColor1.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
                    ActivatePressurePlate();

                    //Sound
                    if (!b_HasActivated)
                    {
                        AudioSource.PlayClipAtPoint(PlateActivation, transform.position, volume);
                        Debug.Log("Activated");
                        b_HasActivated = true;
                    }
                }
            }
        }

        if (collider.CompareTag("ControllableHeavy"))
        {
            ObjectController obj = collider.GetComponent<ObjectController>();

            if (obj.colorMgr.GetCurrentColor() == colorMgr.GetCurrentColor())
            {
                pressurePlateValue = collider.GetComponent<ObjectController>().weight;

                if (pressurePlateValue == 2)
                {
                    diodeColor.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
                    diodeColor1.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
                    ActivatePressurePlate();

                    //Sound
                    if (!b_HasActivated)
                    {
                        AudioSource.PlayClipAtPoint(PlateActivation, transform.position, volume);
                        Debug.Log("Activated");
                        b_HasActivated = true;
                    }
                }
            }
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("ControllableLightweight"))
        {
            ObjectController obj = collider.GetComponent<ObjectController>();

            if (obj.colorMgr.GetCurrentColor() == colorMgr.GetCurrentColor())
            {
                pressurePlateValue++;
                Debug.Log(pressurePlateValue);
                diodeColor1.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);

                if (pressurePlateValue == 2)
                {
                    diodeColor.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
                    diodeColor1.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
                    ActivatePressurePlate();

                    //Sound
                    if (!b_HasActivated)
                    {
                        AudioSource.PlayClipAtPoint(PlateActivation, transform.position, volume);
                        Debug.Log("Activated");
                        b_HasActivated = true;
                    }

                    //Dialogue
                    if (!b_dialogueHappenned)
                    {
                        ThisObjectDialogueTrigger();
                    }
                }
            }
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if(collider.CompareTag("ControllableHeavy"))
        {
            isActive = false;
            pressurePlateValue = 0;
            diodeColor.GetComponent<MeshRenderer>().material = initialMaterial;
            diodeColor1.GetComponent<MeshRenderer>().material = initialMaterial;
        }
                
        if (collider.CompareTag("Player"))
        {   
            isActive = false;
            pressurePlateValue = 0;
            diodeColor.GetComponent<MeshRenderer>().material = initialMaterial;
            diodeColor1.GetComponent<MeshRenderer>().material = initialMaterial;
        }

        if (collider.CompareTag("ControllableLightweight"))
        {
            pressurePlateValue--;
            pressurePlateValue = 0;
            isActive = false;
            diodeColor.GetComponent<MeshRenderer>().material = initialMaterial;
            diodeColor1.GetComponent<MeshRenderer>().material = initialMaterial;
        }               
    }

    private void ActivatePressurePlate()
    {
        isActive = true;
        OnPressurePlateActivated?.Invoke();
    }

    public void ThisObjectDialogueTrigger()
    {
        dialogueTrigger.TriggerDialogue();
        b_dialogueHappenned = true;
        Debug.Log(b_dialogueHappenned);
    }
}
