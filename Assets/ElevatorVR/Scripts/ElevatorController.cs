using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorController : MonoBehaviour
{
    public static ElevatorController Elevator;

    public Animator animator;
    public List<string> SceneOrder = new List<string>();

    [ReadOnlyField] public int Level = 0;

	void Start ()
    {
        Elevator = this;

        if (SceneOrder.Count <= 0) return;

        DontDestroyOnLoad(gameObject);

        SceneManager.LoadSceneAsync(SceneOrder[Level]);

        animator.SetBool("DoorOpen", true);

        //Play the door open sound
	}
	
    void LoadNextScene()
    {
        //Play the door closing sound

        animator.SetBool("DoorOpen", false);
        StartCoroutine(DoneLoadingTimer());
    }

    private IEnumerator DoneLoadingTimer()
    {
        yield return SceneManager.LoadSceneAsync(SceneOrder[Level]);

        //Play the ding sound

        yield return new WaitForSeconds(3.0f);

        //Play the opening door sound

        animator.SetBool("DoorOpen", true);

        print("Scene " + Level + " loaded");
    }
}