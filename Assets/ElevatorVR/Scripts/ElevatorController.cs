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
    [ReadOnlyField]

    public bool isSceneTransitioning;
	void Start ()
    {
        Elevator = this;

        isSceneTransitioning = false;

        //Ray added this line
        //animator = GetComponent<Animator>();

        if (SceneOrder.Count <= 0) return;

        DontDestroyOnLoad(gameObject);

        //SceneManager.LoadSceneAsync(SceneOrder[Level]);

        animator.SetTrigger("Open_trigger");
            
        //Play the door open sound
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        if (isSceneTransitioning == false)
        {
            //Play the door closing sound
            isSceneTransitioning = true;
            StartCoroutine(DoneLoadingTimer());
        }


    }

    private IEnumerator DoneLoadingTimer()
    {
        Level++;
        animator.SetTrigger("Close_trigger");

        yield return new WaitForSeconds(3.0f);


        yield return SceneManager.LoadSceneAsync(SceneOrder[Level]);

        //Play the ding sound

        yield return new WaitForSeconds(3.0f);

        //Play the opening door sound

        animator.SetTrigger("Open_trigger");

        print("Scene " + Level + " loaded");
    }
}