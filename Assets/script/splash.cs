using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class splash : MonoBehaviour
{
    
 
    public GameObject logofade;





    void Start()
    {

        HideSplashAndShowHome();
    
    }


    void Update()
    {
       

    }

   
    public void HideSplashAndShowHome()
    {


        if (logofade != null)
       {
            RawImage rawImg = logofade.GetComponent<RawImage>();
            if (rawImg != null)
            {
                
                Color c = rawImg.color;
                c.a = 0f;
                rawImg.color = c;
                logofade.SetActive(true);
                LeanTween.value(logofade, 0f, 1f, 1f)
                    .setOnUpdate((float val) => {
                        Color nc = rawImg.color;
                        nc.a = val;
                        rawImg.color = nc;
                    })
                    .setOnComplete(() => {
                        LeanTween.value(logofade, 1f, 0f, 1f)
                            .setOnUpdate((float val2) => {
                                Color nc2 = rawImg.color;
                                nc2.a = val2;
                                rawImg.color = nc2;
                            })
                            .setOnComplete(() => {
                                   
                                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                                    
                            });
                    });
            }
        }


    }
   
 
 
}
