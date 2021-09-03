using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStates
{
    Playing,Success,Failed
}
public class GameManager : MonoBehaviour
{
    #region SINGLETON
    public static GameManager Instance = null;
    private void Awake()
    {
        if (Instance == null) 
        {
            Application.targetFrameRate = 60;
            Instance = this;
        } else if (Instance != this) 
        {
            Destroy(gameObject);
        }
    }

    #endregion

    private int collectedItem = 0;
    public Transform[] doors;
    public GameStates currentState = GameStates.Playing;
    [Header("UI")] 
    public GameObject successPanel;
    public GameObject failedPanel;
    public void CollectedItem()
    {
        collectedItem++;
        if (collectedItem==1)
        {
            var targetScale = doors[0].localScale;
            targetScale.y = 0;
            StartCoroutine(scaleOverTime(doors[0], targetScale, 1));
        }else if (collectedItem==3)
        {
            
        }else if (collectedItem==5)
        {
            
        }
    }
    
    bool isScaling = false;

    IEnumerator scaleOverTime(Transform objectToScale, Vector3 toScale, float duration)
    {
        //Make sure there is only one instance of this function running
        if (isScaling)
        {
            yield break; ///exit if this is still running
        }
        isScaling = true;

        float counter = 0;

        //Get the current scale of the object to be moved
        Vector3 startScaleSize = objectToScale.localScale;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            objectToScale.localScale = Vector3.Lerp(startScaleSize, toScale, counter / duration);
            yield return null;
        }

        isScaling = false;
    }

    public void Success()
    {
        currentState = GameStates.Success;
        successPanel.SetActive(true);
    }

    public void Failed()
    {
        currentState = GameStates.Failed;
        failedPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
