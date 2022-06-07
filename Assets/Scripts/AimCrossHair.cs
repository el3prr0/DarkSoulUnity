using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimCrossHair : MonoBehaviour
{
    public Image crossHair;
    public void Enable(){
        crossHair.enabled = true;
    }

    public void Disable(){
        crossHair.enabled = false;
    }
}
