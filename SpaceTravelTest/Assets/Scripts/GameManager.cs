using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour {
    public AsteroidSpawner asterSpawner;
    public Player playerPrefab;
    public Vector3 playerStartingPosition = Vector3.one * 50;

    public Timer timer;
    public GameObject playPanel;
    public GameObject replayPanel;

    public static Score score { get; set; }



    void Awake()
    {
        replayPanel.SetActive(false);      //make sure that the replay button is not displayed by default
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
    }



    public void StartGame()
    {
        //hide the play
        playPanel.SetActive(false);

        SpawnPlayer();
        timer.StartTimer();

        //spawn the enemies
        EnemySpawner.startSpawning = true;
    }


    void EndGame()
    {
        timer.StopTimer();
        replayPanel.SetActive(true);               //make sure to turn on the replay button
        
        score.TimerBonus(timer.GetTotalTime);
        EnemySpawner.startSpawning = false;         //turn off the spawning of enemies
    }



    public void RestartGame()
    {
        Application.LoadLevel(Application.loadedLevel);     //restart the level
    }



    public void SpawnPlayer()
    {
        Instantiate(playerPrefab, playerStartingPosition, Quaternion.identity);
    }



    public void PlayerDied()
    {
        EndGame();
    }
}