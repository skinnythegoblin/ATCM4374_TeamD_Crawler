using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    public void ButtonTouchAnim()
    {
        GetComponent<Animation>().Play();
    }
    
}
