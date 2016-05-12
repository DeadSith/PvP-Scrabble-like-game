using UnityEngine;

//Used for navigation in help submenus
public class HelpSubmenusBehaviour : MonoBehaviour
{
    public GameObject PreviousMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnReturn();
    }

    public void OnReturn()
    {
        PreviousMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}