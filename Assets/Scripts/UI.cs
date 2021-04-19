using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI : MonoBehaviour
{
    public float ShootForce;

    [Header("Slider Attriutes")]
    
    [SerializeField] float changeSpeed = 1;
    [SerializeField] GameObject sliderObj;

    [SerializeField] GameObject Victory;
    [SerializeField] GameObject Defeat;
    [SerializeField] GameObject ReloadButton;

    Slider slider;
    
    bool UP;
    bool shooting;
    float MaxForce;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);
        MaxForce = GameManager.gameManager.cannon.MaxForce;
        slider = sliderObj.GetComponent<Slider>();
        slider.maxValue = MaxForce;
        slider.minValue = -MaxForce;
        slider.value = slider.minValue;
    }
    
    
    void ControllShootingForce()
    {
        if (Input.touchCount > 0)
            switch (Input.touches[0].phase)
            {
                case TouchPhase.Began:
                    slider.value = slider.minValue;
                    sliderObj.SetActive(true);
                    break;

                case TouchPhase.Ended:
                    if (sliderObj.activeSelf)
                    {
                        GameManager.gameManager.cannon.StartShooting();
                        sliderObj.SetActive(false);
                        EnableShooting(false);
                    }
                    
                    break;
            }
    }

    public void ShowGameEnd(GameManager.EndGameCondition endGameCondition)
    {
        switch (endGameCondition)
        {
            case GameManager.EndGameCondition.Won:
                Victory.SetActive(true);
                break;
            case GameManager.EndGameCondition.Lost:
                Defeat.SetActive(true);
                break;
        }
        ReloadButton.SetActive(true);
    }

    public void EnableShooting(bool active)
    {
        shooting = active;        
    }

    void ChangeSaveSliderValue()
    {
        if (slider.value == -MaxForce)
            UP = true;
        if (slider.value == MaxForce)
            UP = false;

        slider.value += UP ? Time.deltaTime * changeSpeed * MaxForce * 2:
            -Time.deltaTime * changeSpeed * MaxForce * 2;

        ShootForce = Mathf.Abs(Mathf.Abs(slider.value) - MaxForce);
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting)
        {
            ChangeSaveSliderValue();
            ControllShootingForce();
        }
        
    }
}
