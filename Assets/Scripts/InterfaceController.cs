using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Interface element modification class
/// </summary>
public class InterfaceController : MonoBehaviour
{
    [SerializeField] private SettingsController settingsController;
    [SerializeField] private MathController mathController;
    [SerializeField] private Text statText;
    [SerializeField] private GameObject evtHorCrossedInfoPanel;
    [SerializeField] private GameObject turnOffGravityInfoPanel;

    private void Update()
    {
        if (mathController.isInEventHorizon)
        {
            evtHorCrossedInfoPanel.SetActive(true);
            turnOffGravityInfoPanel.SetActive(settingsController.gravityIsOn);
        }
        else
        {
            evtHorCrossedInfoPanel.SetActive(false);
            turnOffGravityInfoPanel.SetActive(false);
        }

        statText.text = "Расстояние (r): " + mathController.distance.ToString("F0") + " м" +
            "\nРадиус Шварцшильда (Rs): " + mathController.schwarzschildRadius.ToString("F3") + " м" +
            "\nРасстояние / радиус Шварцшильда (r/Rs): " + mathController.distanceBySchwarzschildRadius.ToString("F4") +
            "\nМасса чёрной дыры (M): " + FormatBigNumber(MathController.M) + " кг" +
            "\nГравитационное ускорение (g): " + FormatBigNumber(mathController.gravityAcceleration) + " м/с²";
    }

    /// <summary>
    /// Function for formatting large numbers in a more readable form
    /// </summary>
    string FormatBigNumber(double number)
    {
        if (number == 0) return "0";

        int exponent = (int)Math.Log10(Math.Abs(number));

        if (exponent < 6) // For small numbers, we just use the usual format
        {
            return number.ToString("F2");
        }

        double mantissa = number / Math.Pow(10, exponent);

        // Converting power number to the superscript unicode format
        string superscriptExponent = ConvertToSuperscript(exponent.ToString());
        return mantissa.ToString("F2") + " × 10" + superscriptExponent;
    }

    /// <summary>
    /// Auxiliary function for converting regular digits to superscripts
    /// </summary>
    string ConvertToSuperscript(string number)
    {
        Dictionary<char, char> superscriptMap = new Dictionary<char, char>
        {
            {'0', '⁰'},
            {'1', '¹'},
            {'2', '²'},
            {'3', '³'},
            {'4', '⁴'},
            {'5', '⁵'},
            {'6', '⁶'},
            {'7', '⁷'},
            {'8', '⁸'},
            {'9', '⁹'},
            {'-', '⁻'}
        };

        string result = "";
        foreach (char c in number)
        {
            if (superscriptMap.ContainsKey(c))
                result += superscriptMap[c];
            else
                result += c; // If the symbol is not found, leave it as it is
        }

        return result;
    }
}