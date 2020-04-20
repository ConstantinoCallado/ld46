using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Plate plateLeft;
    public Plate plateRight;
    public CrossFader crossFader;
    public Transform onboardingText1;
    public Transform onboardingText2;
    public Transform onboardingText3;

    public Table table;
    public GameObject startingDiskPrefab;
    public GameObject initialCustomerPrefab;

    private DancerMood moodInitialCustomer;

    void Start()
    {
        plateLeft.gameObject.GetComponent<Collider>().enabled = false;
        plateRight.gameObject.GetComponent<Collider>().enabled = false;
        crossFader.gameObject.GetComponent<Collider>().enabled = false;

        Stage0();
    }


    void Stage0()
    {
        DancerManager.dancerManagerRef.tutorialActive = true;
        DancerManager.dancerManagerRef.SpawnDancerAtDestiny(new Vector3(0, 1, 5.5f), initialCustomerPrefab);
        moodInitialCustomer = DancerManager.dancerManagerRef.dancers[0].GetComponent<DancerMood>();

        StartCoroutine(Stage1());
    }
    
    // Put record on right
    IEnumerator Stage1()
    {
        plateRight.gameObject.GetComponent<Collider>().enabled = true;
        onboardingText1.gameObject.SetActive(true);

        while (true)
        {
            if (plateRight.disk != null)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(Stage2());
    }

    // Fader
    IEnumerator Stage2()
    {
        onboardingText1.gameObject.SetActive(false);
        plateRight.gameObject.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(0.2f);
        onboardingText2.gameObject.SetActive(true);
        crossFader.gameObject.GetComponent<Collider>().enabled = true;

        while (true)
        {
            if (!crossFader.isLeft)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(Stage3());
    }

    // Put record on left
    IEnumerator Stage3()
    {
        onboardingText2.gameObject.SetActive(false);
        crossFader.gameObject.GetComponent<Collider>().enabled = false;
        plateRight.gameObject.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(1f);
        plateLeft.gameObject.GetComponent<Collider>().enabled = true;
        onboardingText3.gameObject.SetActive(true);

        while (true)
        {
            if (plateLeft.disk != null)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(Stage4());
    }

    IEnumerator Stage4()
    {
        onboardingText3.gameObject.SetActive(false);
        plateLeft.gameObject.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(0.2f);
        onboardingText2.gameObject.SetActive(true);
        crossFader.gameObject.GetComponent<Collider>().enabled = true;

        while (true)
        {
            if (crossFader.isLeft)
            {
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(Stage5());
    }

    IEnumerator Stage5()
    {
        onboardingText2.gameObject.SetActive(false);
        plateLeft.gameObject.GetComponent<Collider>().enabled = true;
        plateRight.gameObject.GetComponent<Collider>().enabled = true;
        crossFader.gameObject.GetComponent<Collider>().enabled = true;

        while (moodInitialCustomer.currentMood != GameEnums.MoodStates.OnFire)
        {
            yield return new WaitForEndOfFrame();
        }

        DancerManager.dancerManagerRef.SetTutorialActive(false);
    }
}
