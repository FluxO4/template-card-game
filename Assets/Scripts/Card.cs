using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public GameObject visual;
    public Renderer faceMat;

    public int nameValue;

    public int value
    {
        get
        {
            int faceValue = nameValue % 13;
            if (faceValue == 0) faceValue = 13;

            int symbolValue = nameValue / 13;

            return symbolValue + faceValue * 10;
        }
    }

    public int belongsTo = -1;  // -1 for deck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if((transform.position - targetPosition).magnitude > 0.05f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, 0.05f);

                if ((transform.position - targetPosition).magnitude < 2f)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
                }
            }
            else
            {
                moving = false;
            }
        }
        else if (localMoving)
        {
            if ((transform.localPosition - targetPosition).magnitude > 0.05f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, 0.05f);

                if ((transform.localPosition - targetPosition).magnitude < 2f)
                {
                    transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, 0.1f);
                }
            }
            else
            {
                localMoving = false;
            }
        }

    }

    Vector3 targetPosition;
    Quaternion targetRotation;
    bool moving;
    bool localMoving;

    public void moveTo(Vector3 newPosition, Quaternion newRotation)
    {
        targetPosition = newPosition;
        targetRotation = newRotation;
        moving = true;
    }

    public void moveToLocal(Vector3 newPosition, Quaternion newRotation)
    {
        targetPosition = newPosition;
        targetRotation = newRotation;
        localMoving = true;
    }

    public void activateVisual()
    {
        visual.SetActive(true);
    }

    public void deactivateVisualAfter(float seconds = 1)
    {
        StartCoroutine(deactivateCoroutine(seconds));
    }

    IEnumerator deactivateCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        visual.SetActive(false);
    }

    public void throwCard()
    {

    }
}
