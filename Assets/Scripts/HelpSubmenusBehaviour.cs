using UnityEngine;
using System.Collections;

public class HelpSubmenusBehaviour : MonoBehaviour
{

    public GameObject PreviousMenu;
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.Escape))
            OnReturn();
	}

    public void OnReturn()
    {
        PreviousMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
