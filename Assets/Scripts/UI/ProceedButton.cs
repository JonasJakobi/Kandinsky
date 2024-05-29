using System.Collections;
using System.Security;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProceedButton : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Color defaultColor;
    [SerializeField] Color readyToGoColor;
    bool isButtonSelected = false;
    ShapeRule selectedRule;
    void Update()
    {
        // Get the currently selected GameObject
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        //The user selected a rule
        if (selectedObject?.GetComponent<UIRule>() != null)
        {
            if(!isButtonSelected){ // first time selecting the button, update state
                image.color = readyToGoColor;
                selectedRule = selectedObject.GetComponent<UIRule>().rule;
            }
            isButtonSelected = true;
        }
        else{
            StartCoroutine(tinyDelayUntilButtonIsSelected());
        }

    }
    /// <summary>
    /// Necessary to prevent the proceed button from being selected, therefore unselecting the rule
    /// </summary>
    /// <returns></returns>
    private IEnumerator tinyDelayUntilButtonIsSelected()
    {
        yield return new WaitForSeconds(0.1f);
        isButtonSelected = false;
        image.color = defaultColor;
    }

    public void TryProceeding()
    {
        if(!isButtonSelected)
        {
            Debug.Log("Please select a rule to proceed");
            FindObjectOfType<WarningText>().SetWarningText("Please select a rule to proceed.");
            return;
        }
        FindObjectOfType<CanvasManager>().SetNewRules(new ShapeRule[]{selectedRule});
        this.transform.root.gameObject.SetActive(false);

        

    }
}