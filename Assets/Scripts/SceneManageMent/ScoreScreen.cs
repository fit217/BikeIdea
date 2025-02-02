using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>Class <c>ScoreScreen</c> A class to manage the UI in the GameOver screen.</summary>
public class ScoreScreen : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI WinText;
    public TextMeshProUGUI LossText;

    void Awake()
    {
        scoreText.text = PlayerDataObject.lastGameScore.ToString();
        if (PlayerDataObject.survivedEvent) 
        {
            // Winscreen message
            WinText.gameObject.SetActive(true);
        }
        else 
        {
            // Loss screen message
            LossText.gameObject.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }
    //To be honest this method doesn't do a whole lot atm
    private void GetInput() 
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            // Restart the game
            // StartCoroutine(LoadYourAsyncScene("TestScene")); 
            ScoreTracker scoreControlPanel=Object.FindObjectOfType<ScoreTracker>();
            scoreControlPanel.resetGame();
            //TODO-kill all enemies, also reseet time and score
                // EnemyManager enemyControlPanel=Object.FindObjectOfType<EnemyManager>();
                // enemyControlPanel.killAllEnemies();
                // enemyControlPanel.Init();
                // BulletPool BulletControlPanel=Object.FindObjectOfType<BulletPool>();
                // BulletControlPanel.DeInit();
                // // BulletControlPanel.Init();
                // BikeMovementComponent BikeMovementControlPanel=Object.FindObjectOfType<BikeMovementComponent>();
                // BikeMovementControlPanel.Init();
                // print(BikeMovementControlPanel.HitPoints);

            //TODO-reset score
                // ScoreTracker scoreControlPanel=Object.FindObjectOfType<ScoreTracker>();
                // scoreControlPanel.Init();
                //reset player health and vectors
            //TODO-location update
            //TODO-camera update
        }
    }

    /// <summary>Loads the scene associated with the string asycronously.</summary>
    /// <param name="scene">The name of the scene to load.</param>
    IEnumerator LoadYourAsyncScene(string scene)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
