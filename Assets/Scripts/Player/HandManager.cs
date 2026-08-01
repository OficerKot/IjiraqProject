using UnityEngine;
using UnityEngine.XR;

public class HandManager 
{
    GameObject handContent = null;
  
    public bool IsHandFree()
    {
        return handContent = null;
    }

    public GameObject WhatInHand()
    {
        return handContent;
    }
    public void PutInHand(GameObject obj)
    {
        handContent = obj;
    }
}
