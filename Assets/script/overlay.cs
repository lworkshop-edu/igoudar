using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class overlay : MonoBehaviour
{
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject obj4;
    public GameObject bookopen;
    public GameObject catwisper;
    public GameObject cathelp;
    public GameObject catbtn;

   
    public UnityEngine.UI.Slider sliderBarcanfiance;
    public UnityEngine.UI.Slider sliderBarreserves;


    public TMPro.TextMeshProUGUI percentageTexcanfiance;
    public TMPro.TextMeshProUGUI percentageTextreserves;

    [Range(0,100)]
    public float changereserves;
    [Range(0,100)]
    public float changecanfiance;

    public GameObject midlebookopen;



    void Start()
    {
        if (catwisper != null)
            catwisper.SetActive(true);
        bookopen.SetActive(false);
        cathelp.SetActive(false);

        // Load saved slider value
        changecanfiance = PlayerPrefs.GetFloat("changecanfiance", changecanfiance);
    }


    void Update()
    {

        if (sliderBarcanfiance != null && percentageTexcanfiance != null)
        {
            float displayValue = changecanfiance < 3f ? 0.03f : changecanfiance / 100f;
            sliderBarcanfiance.value = displayValue;
            int percent = changecanfiance < 3f ? 0 : Mathf.RoundToInt(sliderBarcanfiance.value * 100f);
            percentageTexcanfiance.text = changecanfiance + "%";
        }
        if (sliderBarreserves != null && percentageTextreserves != null)
        {
            float displayValue = changereserves < 3f ? 0.03f : changereserves / 100f;
            sliderBarreserves.value = displayValue;
            int percent = changereserves < 3f ? 0 : Mathf.RoundToInt(sliderBarreserves.value * 100f);
            percentageTextreserves.text = changereserves + "%";
        }
    }

            public void SetSliderBarcanfiance(float value)
            {
                if (sliderBarcanfiance != null)
                {
                    sliderBarcanfiance.value = Mathf.Clamp01(value);
                }
            }

            public void SetSliderBarreserves(float value)
            {
                if (sliderBarreserves != null)
                {
                    sliderBarreserves.value = Mathf.Clamp01(value);
                }
            }
        public void ActivateObj1()
    {
        obj1.SetActive(true);
        obj2.SetActive(false);
        obj3.SetActive(false);
        obj4.SetActive(false);
    }

            public void closebok()
    {
        LeanTween.scale(midlebookopen, Vector3.zero, 0.2f).setOnComplete(() => {
                                   
                                obj1.SetActive(true);
        obj2.SetActive(false);
        obj3.SetActive(false);
        obj4.SetActive(false);
        bookopen.SetActive(false);                  
                                    
                            });

    }
            public void openbook()
    {
        obj1.SetActive(true);
        obj2.SetActive(false);
        obj3.SetActive(false);
        obj4.SetActive(false);
        bookopen.SetActive(true);
        midlebookopen.transform.localScale = Vector3.zero;

                LeanTween.scale(midlebookopen, Vector3.one, 0.2f);

    }
         public void continiuerbegin()
    {
                catwisper.transform.GetChild(1).transform.localScale = Vector3.one;//secondone is the midle
                LeanTween.scale(catwisper.transform.GetChild(1).gameObject, Vector3.zero, 0.2f).setOnComplete(() => {
                       catwisper.SetActive(false);
                    });
        
    }
         public void continiuer()
    {
        
                    if (catbtn != null)
        {
            RawImage rawImg = catbtn.GetComponent<RawImage>();
            if (rawImg != null)
            {
                Color c = rawImg.color;
                c.a = 0f;
                rawImg.color = c;
                catbtn.SetActive(true);
                LeanTween.value(catbtn, 0f, 1f, 0.2f)
                    .setOnUpdate((float val) => {
                        Color nc = rawImg.color;
                        nc.a = val;
                        rawImg.color = nc;
                    })
                    .setOnComplete(() => {
                       rawImg.color = new Color(rawImg.color.r, rawImg.color.g, rawImg.color.b, 1f);
                    });
            }
        }
               if (cathelp.transform.GetChild(0).gameObject != null)
        {
            RawImage rawImgcathelp = cathelp.transform.GetChild(0).GetComponent<RawImage>();
            if (rawImgcathelp != null)
            {
                
                Color c = rawImgcathelp.color;
                c.a = 1f;
                rawImgcathelp.color = c;
                       cathelp.transform.GetChild(1).gameObject.SetActive(false);
                       cathelp.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);


                LeanTween.value(cathelp.transform.GetChild(0).gameObject, 1f, 0f, 0.2f)
                    .setOnUpdate((float val) => {
                        Color nc = rawImgcathelp.color;
                        nc.a = val;
                        rawImgcathelp.color = nc;
                    })
                    .setOnComplete(() => {
                       cathelp.transform.GetChild(1).gameObject.SetActive(true);
                       cathelp.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                cathelp.SetActive(false);

                    });
            }
        }
       
    }
         public void cathelpbtn()
    {
            if (catbtn != null)
        {
            RawImage rawImg = catbtn.GetComponent<RawImage>();
            if (rawImg != null)
            {
                Color c = rawImg.color;
                c.a = 1f;
                rawImg.color = c;
                catbtn.SetActive(true);
                LeanTween.value(catbtn, 1f, 0f, 0.3f)
                    .setOnUpdate((float val) => {
                        Color nc = rawImg.color;
                        nc.a = val;
                        rawImg.color = nc;
                    })
                    .setOnComplete(() => {
                       catbtn.SetActive(false);
                       rawImg.color = new Color(rawImg.color.r, rawImg.color.g, rawImg.color.b, 1f);
                    });
            }
        }
                cathelp.SetActive(true);
               if (cathelp.transform.GetChild(0).gameObject != null)
        {
            RawImage rawImgcathelp = cathelp.transform.GetChild(0).GetComponent<RawImage>();
            if (rawImgcathelp != null)
            {
                
                Color c = rawImgcathelp.color;
                c.a = 0f;
                rawImgcathelp.color = c;
                       cathelp.transform.GetChild(1).gameObject.SetActive(false);
                       cathelp.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);


                LeanTween.value(cathelp.transform.GetChild(0).gameObject, 0f, 1f, 0.3f)
                    .setOnUpdate((float val) => {
                        Color nc = rawImgcathelp.color;
                        nc.a = val;
                        rawImgcathelp.color = nc;
                    })
                    .setOnComplete(() => {
                       cathelp.transform.GetChild(1).gameObject.SetActive(true);
                       cathelp.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);

                    });
            }
        }
       
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

        public void returntointro()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
