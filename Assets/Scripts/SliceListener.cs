using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliceListener : MonoBehaviour
{
    public Slicer slicer;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "SliceableCube" || other.tag == "SliceableSphere")
        {
            slicer.isTouched = true;
        }
    
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Sliceable")
        {
            slicer.isTouched = true;
        }
    }
}
