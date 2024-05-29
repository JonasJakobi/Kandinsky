using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WarningText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI warningText;
    [SerializeField] float warningTextDuration = 4f;


    public void SetWarningText(string text)
    {
        warningText.text = text;
        StartCoroutine(ClearWarningText());
    }

    private IEnumerator ClearWarningText()
    {
        yield return new WaitForSeconds(warningTextDuration);
        warningText.text = "";
    }
}
