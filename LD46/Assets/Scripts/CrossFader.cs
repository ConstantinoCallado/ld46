using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFader : MonoBehaviour
{
    private float maxKnobRangeX = 0.12f;
    public bool isLeft = true;
    public Transform knobTransform;

    public void Start()
    {
        MoveKnob();
    }

    public void Interact()
    {
        isLeft = !isLeft;

        MoveKnob();
    }

    void MoveKnob()
    {
        float newX = maxKnobRangeX;

        if (isLeft)
        {
            newX = -maxKnobRangeX;
        }

        knobTransform.localPosition = new Vector3(newX, knobTransform.localPosition.y, knobTransform.localPosition.z);
    }
}
