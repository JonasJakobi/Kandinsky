using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIRuleSelector : MonoBehaviour
{
    [SerializeField]
    GameObject minimumAmountElement;
    [SerializeField]
    GameObject maximumAmountElement;
    [SerializeField]
    GameObject relativePositionElement;

    TMP_Dropdown dropdown;

    public void ChangeRuleElements(Int32 dropdownInput){ // 0,1,2
        if(dropdownInput == 0){
            minimumAmountElement.SetActive(true);
            maximumAmountElement.SetActive(false);
            relativePositionElement.SetActive(false);
        }
        else if(dropdownInput == 1){
           minimumAmountElement.SetActive(false);
            maximumAmountElement.SetActive(true);
            relativePositionElement.SetActive(false);
        }
        else if(dropdownInput == 2){
            minimumAmountElement.SetActive(false);
            maximumAmountElement.SetActive(false);
            relativePositionElement.SetActive(true);
        }

    }
    
}
