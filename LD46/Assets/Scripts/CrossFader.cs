using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFader : MonoBehaviour
{
    private float maxKnobRangeX = 0.00155f;
    private Table djTable;
    public bool isLeft = true;
    public Transform knobTransform;

    public void Awake()
    {
        djTable = transform.parent.GetComponent<Table>();
    }

    public void Start()
    {
        MoveKnob();
    }

    public void Interact()
    {
        isLeft = !isLeft;

        djTable.DoCrossFade();

        MoveKnob();

        AudioManager.audioManagerRef.PlaySound("sfx_crossfader");
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
