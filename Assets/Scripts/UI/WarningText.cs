using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WarningText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI warningText;

    /// <summary>
    /// Sets the warning text to the given text for the given duration.
    /// If duration is negative, the text will stay
    /// </summary>
    public void SetWarningText(string text, float duration=5, bool warningColor=true)
    {
        warningText.text = text;
        if(warningColor)
        {
            warningText.color = Color.red;
        }
        else
        {
            warningText.color = Color.black;
        }
        if(duration < 0)
        {
            return;
        }
        StartCoroutine(ClearWarningText(duration));
    }

    private IEnumerator ClearWarningText(float duration)
    {
        yield return new WaitForSeconds(duration);
        warningText.text = "";
    }
}
