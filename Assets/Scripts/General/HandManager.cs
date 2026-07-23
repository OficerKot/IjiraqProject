using UnityEngine;
using UnityEngine.XR;

public class HandManager : MonoBehaviour
{
    GameObject handContent = null;
    public static HandManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
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
