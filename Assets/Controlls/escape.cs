using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;


public class escape : MonoBehaviour
{
    public Transform canvas;
    public Transform Player;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause();
                }
    
    }
    public void pause()
        {
           
            if (canvas.gameObject.activeInHierarchy == false)
            {
                canvas.gameObject.SetActive(true);
                Time.timeScale = 0;
            
              
            } else
            {
                canvas.gameObject.SetActive(false);
                Time.timeScale = 1;
            


        }

    }
}
