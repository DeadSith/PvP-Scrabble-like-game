using UnityEngine;

public class HelpSubmenusBehaviour : MonoBehaviour
{
    public GameObject PreviousMenu;

    // Update is called once per frame
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