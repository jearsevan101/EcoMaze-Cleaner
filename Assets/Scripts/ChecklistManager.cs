using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChecklistManager: MonoBehaviour
{
    public TextMeshProUGUI step1;
    public TextMeshProUGUI step2;
    public TextMeshProUGUI step3;
    public TextMeshProUGUI step4;
    public TextMeshProUGUI step5;
    public TextMeshProUGUI checkliststep1;
    public TextMeshProUGUI checkliststep2;
    public TextMeshProUGUI checkliststep3;


    public Image step4Image;
    public Image checklistStep4Image;

    public GameObject step1obj;
    public GameObject step2obj;
    public GameObject step3obj;
    public GameObject step4obj;

    private int currentStep = 0;

    private void Start()
    {
        InitializeStep();
    }

    private void InitializeStep()
    {
        step1obj.SetActive(true);
        //step1.gameObject.SetActive(true);
        //step1Image.gameObject.SetActive(true);
        //checkliststep1.gameObject.SetActive(false);
        //checklistStep1Image.gameObject.SetActive(false);
        //step2.gameObject.SetActive(false);
        //step2Image.gameObject.SetActive(false);
        //checkliststep2.gameObject.SetActive(false);
        //checklistStep2Image.gameObject.SetActive(false);
        //step3.gameObject.SetActive(false);
        //step3Image.gameObject.SetActive(false);
        //checkliststep3.gameObject.SetActive(false);
        //checklistStep3Image.gameObject.SetActive(false);
    }

    public void CompleteCurrentStep()
    {
        currentStep++;

        switch(currentStep)
            {
            case 1:
                Debug.Log("Step 1 completed!");
                //step1.gameObject.SetActive(false);
                //step1Image.gameObject.SetActive(false);
                //checkliststep1.gameObject.SetActive(true);
                //checklistStep1Image.gameObject.SetActive(true);
                //step2.gameObject.SetActive(true);
                //step2Image.gameObject.SetActive(true);
                step1obj.SetActive(false);
                step2obj.SetActive(true);
                break;
            case 2:
                Debug.Log("Step 2 Completed!");
                step2obj.SetActive(false);
                step3obj.SetActive(true);
                break;
            case 3:
                Debug.Log("Step 3 Completed!");
                step3obj.SetActive(false);
                step4obj.SetActive(true);
                checklistStep4Image.gameObject.SetActive(false);
                break;
            }
    }
}