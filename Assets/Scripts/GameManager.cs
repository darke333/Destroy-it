using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    [SerializeField] Transform BuildingSpawnPoint;
    [SerializeField] List<GameObject> Buildings = new List<GameObject>();

    public Cannon cannon;
    public UI ui;    
    public CameraManager cameraManager;
    public enum Condition {shooting, loading}
    public enum EndGameCondition { Won, Lost }


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Buildings[Random.Range(0, Buildings.Count)], BuildingSpawnPoint.position, BuildingSpawnPoint.rotation);
        gameManager = this;        
    }

    public void SetCondition(Condition condition)
    {
        cameraManager.ChangeCameraPos();
        StartCoroutine(ChangeCondition(condition));
    }

    IEnumerator ChangeCondition(Condition condition)
    {
        yield return new WaitForSeconds(cameraManager.ChangePosTime);
        switch (condition)
        {
            case Condition.shooting:
                ui.EnableShooting(true);
                cannon.Reloading = false;
                break;

            case Condition.loading:
                ui.EnableShooting(false);
                cannon.Reloading = true;
                break;
        }
    }

    public void EndGame(EndGameCondition endGameCondition)
    {
        ui.ShowGameEnd(endGameCondition);

        ui.EnableShooting(false);
        cannon.Reloading = false;

    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
