using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuUIController : MonoBehaviour
{
    public UIDocument MainMenuDocument;

    void OnEnable()
    {
        MainMenuDocument = GetComponent<UIDocument>();
    }
}
