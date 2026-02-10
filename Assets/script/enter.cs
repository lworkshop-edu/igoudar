using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class enter : MonoBehaviour
{
 
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject obj4;
    public GameObject home;
    public GameObject faq;
    public GameObject background;
    public GameObject background1;
    public GameObject midle;



    void Start()
    {

        home.SetActive(true);

    }

    void Update()
    {
       

    }

        public void ActivateFAQPage()
    {
        if (background1 != null && background != null && midle != null)
        {
            Vector3 bgpos = background1.transform.position;
            background1.transform.position = background.transform.position;

            midle.transform.localScale = Vector3.zero;

            LeanTween.move(background1, bgpos, 0.2f).setOnComplete(() =>
            {
                LeanTween.scale(midle, Vector3.one, 0.3f);
            });
        }
    }

    public void ActivateObj1()
    {
        obj1.SetActive(true);
        obj2.SetActive(false);
        obj3.SetActive(false);
        obj4.SetActive(false);
    }

 
    public void ActivateObj2()
    {
        obj1.SetActive(false);
        obj2.SetActive(true);
        obj3.SetActive(false);
        obj4.SetActive(false);
    }


    public void ActivateObj3()
    {
        obj1.SetActive(false);
        obj2.SetActive(false);
        obj3.SetActive(true);
        obj4.SetActive(false);
    }


    public void ActivateObj4()
    {
        obj1.SetActive(false);
        obj2.SetActive(false);
        obj3.SetActive(false);
        obj4.SetActive(true);
    }
        public void homef()
    {
                if (background1 != null && background != null && midle != null)
        {
            Vector3 bgpos = background1.transform.position;
            midle.transform.localScale = Vector3.zero;

            LeanTween.move(background1, background.transform.position, 0.2f).setOnComplete(() =>
            {
                LeanTween.scale(midle, Vector3.zero, 0.1f).setOnComplete(() =>
                {
                    home.SetActive(true);
                    faq.SetActive(false);
                    background1.transform.position = bgpos;
                });
            });
        }


    }
            public void faqf()
    {
        home.SetActive(false);
        faq.SetActive(true);

    }


        public void overlayscen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

}
