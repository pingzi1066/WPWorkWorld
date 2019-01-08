/****************************************************************************** 
 * 
 * Maintaince Logs: 
 * 2017-02-16     WP      Initial version
 * 
 * *****************************************************************************/


using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// 给Ugui 提供的一系列的方法。
/// </summary>
public static class KMUITools
{
    static public void SetText(Text text, string str)
    {
        if (text)
            text.text = str;
    }

    static public void AddMethodToIntInput(InputField inputF, UnityAction<string> method)
    {
        AddMethodToInput(inputF, method, InputField.CharacterValidation.Integer);
    }

    static public void AddMethodToFloatInput(InputField inputF, UnityAction<string> method)
    {
        AddMethodToInput(inputF, method, InputField.CharacterValidation.Decimal);
    }

    static public void AddMethodToInput(InputField inputF, UnityAction<string> method, InputField.CharacterValidation inputType)
    {
        if (inputF)
        {
            inputF.characterValidation = inputType;
            inputF.onEndEdit.AddListener(method);
        }
    }
}
