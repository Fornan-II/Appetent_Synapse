using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndZone : MonoBehaviour
{
    public LayerMask triggerableLayers;
    public Animator fadingCanvasAnimator;
    public float timeUntilMenuReturn = 3.0f;

    private bool _endTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        //Thanks https://answers.unity.com/questions/50279/check-if-layer-is-in-layermask.html
        if (triggerableLayers == (triggerableLayers.value | (1 << other.gameObject.layer)) && !_endTriggered)
        {
            _endTriggered = true;

            fadingCanvasAnimator.SetBool("ShowScreen", false);
            StartCoroutine(ReturnToMenu());
        }
    }

    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(timeUntilMenuReturn);
        //Change scene to menu
        LookScript ls = FindObjectOfType<LookScript>();
        if (ls)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
