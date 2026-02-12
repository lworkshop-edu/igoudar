using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class level2 : MonoBehaviour
{
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject obj4;
    public GameObject bookopen;
    public GameObject ideaopen;
    public GameObject congrats;
    public GameObject cathelp;
    public GameObject catbtn;

    [System.Serializable]
    public class KeyValue
    {
        public GameObject key; // has select, dark numberoff, numberon, indicator as children
        public bool value; // true if correct, false if not
    }


    public List<KeyValue> keys;
    private int currentKeyIndex = 0;
    private bool leftObjActive = false;
    public GameObject leftObj; // assign in inspector: contains corectBtn, wrongBtn, text1-4
    public List<GameObject> leftTexts; // assign in inspector: text1, text2, text3, text4
    private int countdown = 3;
    private Coroutine countdownCoroutine;


    public UnityEngine.UI.Slider sliderBarcanfiance;
    public UnityEngine.UI.Slider sliderBarreserves;


    public TMPro.TextMeshProUGUI percentageTexcanfiance;
    public TMPro.TextMeshProUGUI percentageTextreserves;

    [Range(0,100)]
    public float changereserves;
    [Range(0,100)]
    public float changecanfiance;
    
    public GameObject textfloat;
    public GameObject textfloatplace;
    // Call this method when FAQ is activated
    public GameObject midlebookopen;
    public GameObject midleideaopen;
    public GameObject midlecongrats;


    public GameObject catwrong;//have close btn on secondchild
    public GameObject catrcorect;//have continue btn on secondchild



    public List<string> corectext; // have corect , wrong as child 
    public List<string> wrongtext;

    public TMPro.TextMeshProUGUI contertext;


    void Start()
    {
        bookopen.SetActive(false);
        ideaopen.SetActive(false);
        congrats.SetActive(false);
        cathelp.SetActive(true);
        catbtn.SetActive(false);
        catwrong.SetActive(false);
        catrcorect.SetActive(false);

        // Load saved slider value
        changecanfiance = PlayerPrefs.GetFloat("changecanfiance", changecanfiance);

        SetAllKeysDark();
        if (keys != null && keys.Count > 0 && keys[0].key != null)
        {
            SetKeyVisualState(keys[0].key, true, false, false);
        }
        if (leftObj != null) leftObj.SetActive(false);
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

    public void KeyClicked(int keyIndex)
    {
        if (catwrong != null && catwrong.activeSelf)
        {
            OnWrongCloseClicked();
            return;
        }
        
        if (catrcorect != null && catrcorect.activeSelf)
        {
            OnContinueClicked();
            return;
        }
        
        if (leftObjActive) return;
        continiuer();
        if (keys == null || keyIndex < 0 || keyIndex >= keys.Count) return;
        if (currentKeyIndex != keyIndex) return;
        var key = keys[keyIndex];
        if (key == null || key.key == null) return;

        SetAllKeysDark();
        SetKeyVisualState(key.key, false, true, false);

        if (leftObj != null)
        {
            leftObj.SetActive(true);
            leftObj.transform.localScale = Vector3.zero;
            LeanTween.scale(leftObj, Vector3.one, 0.3f);
            leftObjActive = true;
            for (int i = 0; i < leftTexts.Count; i++)
            {
                leftTexts[i].gameObject.SetActive(i == keyIndex);
            }
            
            countdown = 3;
            if (contertext != null)
            {
                contertext.text = "0" + countdown.ToString();
            }
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
            }
            countdownCoroutine = StartCoroutine(CountdownTimer());
        }
    }

    public void OnCorectBtnClicked()
    {
        if (!leftObjActive) return;
        if (keys == null || currentKeyIndex >= keys.Count) return;
        var key = keys[currentKeyIndex];
        if (key == null || key.key == null) return;

        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }

        if (key.value) 
        {
            SetKeyNumberOn(key.key);
            if (leftObj != null)
            {
                LeanTween.scale(leftObj, Vector3.zero, 0.2f).setOnComplete(() => {
                    leftObj.SetActive(false);
                });
            }
            leftObjActive = false;
         
            SetCatText(catrcorect, corectext, currentKeyIndex);
            cathelpbtntest(catrcorect);
         
            SetKeyVisualState(key.key, false, false, false);
            currentKeyIndex++;
            if (currentKeyIndex < keys.Count)
            {
                SetAllKeysDark();
                SetKeyVisualState(keys[currentKeyIndex].key, true, false, false);
            }
        }
        else
        {
            if (leftObj != null)
            {
                LeanTween.scale(leftObj, Vector3.zero, 0.2f).setOnComplete(() => {
                    leftObj.SetActive(false);
                });
            }
            leftObjActive = false;
            SetCatText(catwrong, wrongtext, currentKeyIndex);
            cathelpbtntest(catwrong);
        }
    }


    public void OnWrongBtnClicked()
    {
        if (!leftObjActive) return;
        if (keys == null || currentKeyIndex >= keys.Count) return;
        var key = keys[currentKeyIndex];
        if (key == null || key.key == null) return;

        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }

        if (!key.value) 
        {

            SetKeyNumberOn(key.key);

            if (leftObj != null)
            {
                LeanTween.scale(leftObj, Vector3.zero, 0.2f).setOnComplete(() => {
                    leftObj.SetActive(false);
                });
            }
            leftObjActive = false;

            SetCatText(catrcorect, corectext, currentKeyIndex);
            cathelpbtntest(catrcorect);

            SetKeyVisualState(key.key, false, false, false);
            currentKeyIndex++;
            if (currentKeyIndex < keys.Count)
            {
                SetAllKeysDark();
                SetKeyVisualState(keys[currentKeyIndex].key, true, false, false);
            }
        }
        else
        {

            if (leftObj != null)
            {
                LeanTween.scale(leftObj, Vector3.zero, 0.2f).setOnComplete(() => {
                    leftObj.SetActive(false);
                });
            }
            leftObjActive = false;
            SetCatText(catwrong, wrongtext, currentKeyIndex);
            cathelpbtntest(catwrong);
        }
    }

   
    private void SetKeyIndicator(GameObject keyObj, bool active)
    {
        if (keyObj == null) return;
        for (int i = 0; i < keyObj.transform.childCount; i++)
        {
            var child = keyObj.transform.GetChild(i);
            if (child.name.ToLower().Contains("indicator"))
                child.gameObject.SetActive(active);
        }
    }
    private void SetKeyNumberOn(GameObject keyObj)
    {
        if (keyObj == null) return;
        for (int i = 0; i < keyObj.transform.childCount; i++)
        {
            var child = keyObj.transform.GetChild(i);
            if (child.name.ToLower().Contains("numberon"))
                child.gameObject.SetActive(true);
            if (child.name.ToLower().Contains("numberoff"))
                child.gameObject.SetActive(false);
            if (child.name.ToLower().Contains("select"))
                child.gameObject.SetActive(false);
            if (child.name.ToLower().Contains("dark"))
                child.gameObject.SetActive(false);
        }
    }

    public void OnWrongCloseClicked()
    {
        continiuertest(catwrong);
 
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }

        if (leftObj != null)
        {
            LeanTween.scale(leftObj, Vector3.zero, 0.2f).setOnComplete(() => {
                leftObj.SetActive(false);
            });
        }
        leftObjActive = false;
        if (keys != null && currentKeyIndex < keys.Count && keys[currentKeyIndex].key != null)
        {
            SetAllKeysDark();
            SetKeyVisualState(keys[currentKeyIndex].key, true, false, false);
        }
    }

    public void OnContinueClicked()
    {
        continiuertest(catrcorect);
      
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }

        if (leftObj != null)
        {
            LeanTween.scale(leftObj, Vector3.zero, 0.2f).setOnComplete(() => {
                leftObj.SetActive(false);
            });
        }
        leftObjActive = false;
    
        if (currentKeyIndex >= keys.Count)
        {
            opencongrats();
        }
    }

            private void SetKeyVisualState(GameObject keyObj, bool indicatorActive, bool selectActive, bool darkActive)
            {
                if (keyObj == null) return;
                for (int i = 0; i < keyObj.transform.childCount; i++)
                {
                    var child = keyObj.transform.GetChild(i);
                    var childName = child.name.ToLower();
                    if (childName.Contains("indicator"))
                    {
                        child.gameObject.SetActive(indicatorActive);
                    }
                    else if (childName.Contains("select"))
                    {
                        child.gameObject.SetActive(selectActive);
                    }
                    else if (childName.Contains("dark"))
                    {
                        child.gameObject.SetActive(darkActive);
                    }
                }
            }

            private void SetAllKeysDark()
            {
                if (keys == null) return;
                for (int i = 0; i < keys.Count; i++)
                {
                    var key = keys[i];
                    if (key != null && key.key != null)
                    {
                        if (IsNumberOnActive(key.key))
                        {
                            SetKeyVisualState(key.key, false, false, false);
                        }
                        else
                        {
                            SetKeyVisualState(key.key, false, false, true);
                        }
                    }
                }
            }

            private bool IsNumberOnActive(GameObject keyObj)
            {
                if (keyObj == null) return false;
                for (int i = 0; i < keyObj.transform.childCount; i++)
                {
                    var child = keyObj.transform.GetChild(i);
                    if (child.name.ToLower().Contains("numberon") && child.gameObject.activeSelf)
                    {
                        return true;
                    }
                }
                return false;
            }
    private void SetKeyChildActive(GameObject keyObj, string childName)
    {
        if (keyObj == null) return;
        for (int i = 0; i < keyObj.transform.childCount -1; i++)
        {
            var child = keyObj.transform.GetChild(i).gameObject;
            child.SetActive(child.name.ToLower().Contains(childName.ToLower()));
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
            ideaopen.SetActive(false);
            congrats.SetActive(false);

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

    public void closeidea()
    {
        LeanTween.scale(midleideaopen, Vector3.zero, 0.2f).setOnComplete(() => {
            obj1.SetActive(true);
            obj2.SetActive(false);
            obj3.SetActive(false);
            obj4.SetActive(false);
            bookopen.SetActive(false);
            ideaopen.SetActive(false);
        });
    }
    

    public void openidea()
    {
        obj1.SetActive(true);
        obj2.SetActive(false);
        obj3.SetActive(false);
        obj4.SetActive(false);
        bookopen.SetActive(false);
        ideaopen.SetActive(true);
        midleideaopen.transform.localScale = Vector3.zero;
        LeanTween.scale(midleideaopen, Vector3.one, 0.2f);
    }
    public void opencongrats()
    {

        bookopen.SetActive(false);
        ideaopen.SetActive(false);
        congrats.SetActive(true);
        midlecongrats.transform.localScale = Vector3.zero;
        LeanTween.scale(midlecongrats, Vector3.one, 0.2f);
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

    private void IncreaseSliderCanfiance()
    {
        changecanfiance = PlayerPrefs.GetFloat("changecanfiance", changecanfiance);
        if (changecanfiance < 100)
        {
            changecanfiance = Mathf.Min(changecanfiance + 1f, 100f);
            PlayerPrefs.SetFloat("changecanfiance", changecanfiance);
            PlayerPrefs.Save();
        }
            ShowSliderChangeText("+1");
    }

    private void DecreaseSliderCanfiance()
    {
        changecanfiance = PlayerPrefs.GetFloat("changecanfiance", changecanfiance);
        if (changecanfiance > 0)
        {
            changecanfiance = Mathf.Max(changecanfiance - 1f, 0f);
            PlayerPrefs.SetFloat("changecanfiance", changecanfiance);
            PlayerPrefs.Save();
        }
            ShowSliderChangeText("-1");
    }

    private void ShowSliderChangeText(string text)
    {
        if (textfloat == null || textfloatplace == null) return;

        GameObject floatingText = Instantiate(textfloat, textfloatplace.transform.parent);
        floatingText.transform.position = textfloatplace.transform.position;
        
        TextMeshProUGUI tmp = floatingText.GetComponent<TextMeshProUGUI>();
        if (tmp == null)
        {
            tmp = floatingText.GetComponentInChildren<TextMeshProUGUI>();
        }
        
        if (tmp != null)
        {
            tmp.text = text;
        }

        Destroy(floatingText, 2f);
    }
    
    public void cathelpbtntest(GameObject obj)
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

        if (obj == catrcorect)
        {
            IncreaseSliderCanfiance();
        }
        else if (obj == catwrong)
        {
            DecreaseSliderCanfiance();
        }
        
        obj.SetActive(true);
        if (obj.transform.GetChild(0).gameObject != null)
        {
            RawImage rawImgcathelp = obj.transform.GetChild(0).GetComponent<RawImage>();
            if (rawImgcathelp != null)
            {
                Color c = rawImgcathelp.color;
                c.a = 0f;
                rawImgcathelp.color = c;
                obj.transform.GetChild(1).gameObject.SetActive(false);
                obj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                LeanTween.value(obj.transform.GetChild(0).gameObject, 0f, 1f, 0.3f)
                    .setOnUpdate((float val) => {
                        Color nc = rawImgcathelp.color;
                        nc.a = val;
                        rawImgcathelp.color = nc;
                    })
                    .setOnComplete(() => {
                        obj.transform.GetChild(1).gameObject.SetActive(true);
                        obj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        for (int i = 0; i < obj.transform.childCount; i++)
                        {
                            var child = obj.transform.GetChild(i).gameObject;
                            child.SetActive(true);
                            if (child.transform.childCount > 0)
                            {
                                child.transform.GetChild(0).gameObject.SetActive(true);
                            }
                        }
                    });
            }
        }
    }


    public void continiuertest(GameObject obj)
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
        if (obj.transform.GetChild(0).gameObject != null)
        {
            RawImage rawImgcathelp = obj.transform.GetChild(0).GetComponent<RawImage>();
            if (rawImgcathelp != null)
            {
                Color c = rawImgcathelp.color;
                c.a = 1f;
                rawImgcathelp.color = c;
                obj.transform.GetChild(1).gameObject.SetActive(false);
                obj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                LeanTween.value(obj.transform.GetChild(0).gameObject, 1f, 0f, 0.2f)
                    .setOnUpdate((float val) => {
                        Color nc = rawImgcathelp.color;
                        nc.a = val;
                        rawImgcathelp.color = nc;
                    })
                    .setOnComplete(() => {
                       
                        for (int i = 0; i < obj.transform.childCount; i++)
                        {
                            var child = obj.transform.GetChild(i).gameObject;
                            child.SetActive(true);
                            if (child.transform.childCount > 0)
                            {
                                child.transform.GetChild(0).gameObject.SetActive(true);
                            }
                        }
                         obj.SetActive(false);
                    });
            }
        }
    }

    private IEnumerator CountdownTimer()
    {
        while (countdown > 0)
        {
            yield return new WaitForSeconds(1f);
            countdown--;
            
            if (contertext != null)
            {
                contertext.text = "0" + countdown.ToString();
            }
            
            if (countdown == 0)
            {
                if (leftObj != null)
                {
                    LeanTween.scale(leftObj, Vector3.zero, 0.2f).setOnComplete(() => {
                        leftObj.SetActive(false);
                    });
                }
                leftObjActive = false;
                SetCatText(catwrong, wrongtext, currentKeyIndex);
                cathelpbtntest(catwrong);
                countdownCoroutine = null;
            }
        }
    }

    private void SetCatText(GameObject catObj, List<string> texts, int index)
    {
        if (catObj == null || texts == null || index < 0 || index >= texts.Count) return;
        var textObj = catObj.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (textObj != null)
        {
            textObj.text = texts[index];
        }
    }

}

