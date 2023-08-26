using UnityEngine;
using System.Collections;
using GameScene.Data.Handlers;

public class Lever : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string leverName;
    [SerializeField] private LeverManager leverManager;
    [SerializeField] private GameObject leverSound;

    private IEnumerator Start()
    {
        while (!AssignmentsDataHandler.Instance.IsLoaded())
        {
            yield return null;
        }

        if (AssignmentsDataHandler.Instance.IsCompleted(36))
        {
            animator.SetTrigger("Open");
            gameObject.tag = "Untagged";
            leverManager.OpenDrawer();
        }
    }

    private void Interact()
    {
        leverManager.AddLeverSymbolToSelection(leverName);
        animator.SetTrigger("Open");
        Instantiate(leverSound, transform.position, Quaternion.identity);
        // prevent from be selectionable another time
        gameObject.tag = "Untagged";
    }

    internal IEnumerator ResetLever()
    {
        animator.SetTrigger("Close");
        Instantiate(leverSound, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);

        gameObject.tag = "Interactable";
    }
}