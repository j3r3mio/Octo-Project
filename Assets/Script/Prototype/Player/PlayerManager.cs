﻿using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public ColorManager colorMgr;
    private PlayerController controller;

    void Start()
    {
        // Initialize the color manager with the mesh renderer
        colorMgr.Initialize(GetComponent<MeshRenderer>());
        controller = GetComponent<PlayerController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider col = collision.collider;
        if (col.CompareTag("ColorSwap"))
        {
            ColorChanger changer = collision.collider.GetComponent<ColorChanger>();
            changer?.SwapColors(colorMgr); // Check if the changer exists and swap colors
        }
        else if (col.CompareTag("ControllableHeavy"))
        {
            ObjectController obj = col.GetComponent<ObjectController>();
            if (obj.colorMgr.GetCurrentColor() == colorMgr.GetCurrentColor()) // If the player has the same color than the object
            {
                controller.ControlObject(obj, true, controller);
            }
        }
        else if (col.CompareTag("ControllableLightweight"))
        {
            ObjectController obj = col.GetComponent<ObjectController>();
            if (obj.colorMgr.GetCurrentColor() == colorMgr.GetCurrentColor()) // If the player has the same color than the object
            {
                controller.ControlObject(obj, true, controller);
            }
        }
        else if (collision.gameObject.CompareTag("Barrier"))
        {
            BarrierController barrier = col.GetComponent<BarrierController>();
            BoxCollider barrierCollider = col.GetComponent<BoxCollider>();
            if (barrier.colorMgr.GetCurrentColor() == colorMgr.GetCurrentColor())
            {
                barrierCollider.enabled = false;
            }
            else
            {
                barrierCollider.enabled = true;
            }
        }
        else
        {
            return;
        }
    }
}