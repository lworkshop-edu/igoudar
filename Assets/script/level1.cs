using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class level1 : MonoBehaviour
{
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject obj4;
    public GameObject bookopen;
    public GameObject ideaopen;
    public GameObject congrats;
    public GameObject cathelp;
    public GameObject cathelp2;
    public GameObject catbtn;

    [System.Serializable]
    public class KeyValue
    {
        public GameObject key; // has corect img, hovered, wrong as children
        public string value;
    }

    public List<KeyValue> keys;

    private int currentKeyIndex = 0;


  
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
  
    public GameObject midlebookopen;
    public GameObject midleideaopen;
    public GameObject midlecongrats;


    public GameObject catwrong;//have close btn on secondchild
    public GameObject catrcorect;//have continue btn on secondchild

    public List<GameObject> doorsObjs; // have corect , wrong as child 

    public List<string> corectext;
    public List<string> wrongtext;
    
    public GameObject tutor1;
    public GameObject tutor2;
    public GameObject tutor3;
     public GameObject tutor4;
     public GameObject tutor5;

    private bool isTutorialActive;
    private bool tutorialScalesCached;
    private Vector3 tutor1OriginalScale;
    private Vector3 tutor2OriginalScale;
    private Vector3 tutor3OriginalScale;
    private Vector3 tutor4OriginalScale;
    private Vector3 tutor5OriginalScale;


    void Start()
    {
        bookopen.SetActive(false);
        ideaopen.SetActive(false);
        congrats.SetActive(false);
        cathelp.SetActive(true);
        catwrong.SetActive(false);
        catrcorect.SetActive(false);

        // Load saved slider value
        changecanfiance = PlayerPrefs.GetFloat("changecanfiance", changecanfiance);

        if (keys != null && keys.Count > 0 && keys[0].key != null)
        {
            SetKeyChildActive(keys[0].key, "hovered");
        }

        StartTutorial();
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
   public void doorcklick(int doornumber)
    {
        if (isTutorialActive)
            return;

        if ((catwrong != null && catwrong.activeSelf) || (catrcorect != null && catrcorect.activeSelf))
            return;
        if (keys == null || currentKeyIndex >= keys.Count) return;
        var key = keys[currentKeyIndex];
        if (key == null || key.key == null) return;


        int doorIndex = doornumber - 1;
       
        for (int i = 0; i < doorsObjs.Count; i++)
        {
            if (doorsObjs[i] != null)
            {
                Transform correctChild = null, wrongChild = null;
                for (int j = 0; j < doorsObjs[i].transform.childCount; j++)
                {
                    var child = doorsObjs[i].transform.GetChild(j);
                    if (child.name.ToLower().Contains("corect"))
                        correctChild = child;
                    else if (child.name.ToLower().Contains("wrong"))
                        wrongChild = child;
                }
                if (i == doorIndex)
                {
                    if (key.value == doornumber.ToString())
                    {
                        if (correctChild != null) correctChild.gameObject.SetActive(true);
                        if (wrongChild != null) wrongChild.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (correctChild != null) correctChild.gameObject.SetActive(false);
                        if (wrongChild != null) wrongChild.gameObject.SetActive(true);
                    }
                }
                else
                {
                    if (correctChild != null) correctChild.gameObject.SetActive(false);
                    if (wrongChild != null) wrongChild.gameObject.SetActive(false);
                }
            }
        }

        if (key.value == doornumber.ToString())
        {
            SetKeyChildActive(key.key, "corect");
         
            cathelpbtntest(catrcorect);
            catrcorect.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = corectext[currentKeyIndex];
            catbtn.SetActive(true);
            cathelp.SetActive(false);
            catwrong.SetActive(false);
        }
        else
        {
            SetKeyChildActive(key.key, "wrong");
            cathelpbtntest(catwrong);
            catwrong.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = wrongtext[currentKeyIndex];
           // catwrong.SetActive(true);
            cathelp.SetActive(false);
            catbtn.SetActive(true);
            catrcorect.SetActive(false);
        }
    }

    public void OnWrongCloseClicked()
    {
        // catwrong.SetActive(false);
        continiuertest(catwrong);

        
        for (int i = 0; i < doorsObjs.Count; i++)
        {
            if (doorsObjs[i] != null)
            {
                for (int j = 0; j < doorsObjs[i].transform.childCount; j++)
                {
                    var child = doorsObjs[i].transform.GetChild(j);
                    if (child.name.ToLower().Contains("corect") || child.name.ToLower().Contains("wrong"))
                        child.gameObject.SetActive(false);
                }
            }
        }
        if (keys != null && currentKeyIndex < keys.Count && keys[currentKeyIndex].key != null)
        {
            SetKeyChildActive(keys[currentKeyIndex].key, "hovered");
        }
    }

    public void OnContinueClicked()
    {
        // catrcorect.SetActive(false);
        continiuertest(catrcorect);
       
        for (int i = 0; i < doorsObjs.Count; i++)
        {
            if (doorsObjs[i] != null)
            {
                for (int j = 0; j < doorsObjs[i].transform.childCount; j++)
                {
                    var child = doorsObjs[i].transform.GetChild(j);
                    if (child.name.ToLower().Contains("corect") || child.name.ToLower().Contains("wrong"))
                        child.gameObject.SetActive(false);
                }
            }
        }
        currentKeyIndex++;
        if (keys != null && currentKeyIndex < keys.Count)
        {
      
            if (keys[currentKeyIndex].key != null)
                SetKeyChildActive(keys[currentKeyIndex].key, "hovered");
        }
        else
        {
            opencongrats();
        }
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

    private void StartTutorial()
    {
        isTutorialActive = true;
        CacheTutorOriginalScales();
        if (cathelp != null) cathelp.SetActive(false);
        if (catbtn != null) catbtn.SetActive(false);
        ShowTutor(1);
    }

    private void CacheTutorOriginalScales()
    {
        if (tutorialScalesCached) return;

        if (tutor1 != null) tutor1OriginalScale = tutor1.transform.localScale;
        if (tutor2 != null) tutor2OriginalScale = tutor2.transform.localScale;
        if (tutor3 != null) tutor3OriginalScale = tutor3.transform.localScale;
        if (tutor4 != null) tutor4OriginalScale = tutor4.transform.localScale;
        if (tutor5 != null) tutor5OriginalScale = tutor5.transform.localScale;

        tutorialScalesCached = true;
    }

    private void EndTutorial()
    {
        isTutorialActive = false;
        if (tutor1 != null) tutor1.SetActive(false);
        if (tutor2 != null) tutor2.SetActive(false);
        if (tutor3 != null) tutor3.SetActive(false);
        if (tutor4 != null) tutor4.SetActive(false);
        if (tutor5 != null) tutor5.SetActive(false);
        if (cathelp != null) cathelp.SetActive(true);
    }

    private void ShowTutor(int tutorIndex)
    {
        if (tutor1 != null) tutor1.SetActive(false);
        if (tutor2 != null) tutor2.SetActive(false);
        if (tutor3 != null) tutor3.SetActive(false);
        if (tutor4 != null) tutor4.SetActive(false);
        if (tutor5 != null) tutor5.SetActive(false);

        GameObject tutorToShow = null;
        if (tutorIndex == 1) tutorToShow = tutor1;
        else if (tutorIndex == 2) tutorToShow = tutor2;
        else if (tutorIndex == 3) tutorToShow = tutor3;
        else if (tutorIndex == 4) tutorToShow = tutor4;
        else if (tutorIndex == 5) tutorToShow = tutor5;

        AnimateTutorIn(tutorToShow);
    }

    private void AnimateTutorIn(GameObject tutorObj)
    {
        if (tutorObj == null) return;

        tutorObj.SetActive(true);
        LeanTween.cancel(tutorObj);
        Vector3 targetScale = GetTutorOriginalScale(tutorObj);

        CanvasGroup canvasGroup = tutorObj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = tutorObj.AddComponent<CanvasGroup>();
        }

        tutorObj.transform.localScale = targetScale * 0.95f;
        canvasGroup.alpha = 0f;

        LeanTween.scale(tutorObj, targetScale, 0.2f).setEaseOutQuad();
        LeanTween.value(tutorObj, 0f, 1f, 0.2f)
            .setOnUpdate((float val) => {
                canvasGroup.alpha = val;
            })
            .setOnComplete(() => {
                RefreshTutorTextMeshes(tutorObj);
            });
    }

    private void AnimateTutorOut(GameObject tutorObj, System.Action onComplete)
    {
        if (tutorObj == null)
        {
            onComplete?.Invoke();
            return;
        }

        LeanTween.cancel(tutorObj);
        Vector3 targetScale = GetTutorOriginalScale(tutorObj);

        CanvasGroup canvasGroup = tutorObj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = tutorObj.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 1f;

        LeanTween.scale(tutorObj, targetScale * 0.95f, 0.15f).setEaseInQuad();
        LeanTween.value(tutorObj, 1f, 0f, 0.15f)
            .setOnUpdate((float val) => {
                canvasGroup.alpha = val;
            })
            .setOnComplete(() => {
                tutorObj.SetActive(false);
                tutorObj.transform.localScale = targetScale;
                canvasGroup.alpha = 1f;
                onComplete?.Invoke();
            });
    }

    private Vector3 GetTutorOriginalScale(GameObject tutorObj)
    {
        if (tutorObj == tutor1) return tutor1OriginalScale;
        if (tutorObj == tutor2) return tutor2OriginalScale;
        if (tutorObj == tutor3) return tutor3OriginalScale;
        if (tutorObj == tutor4) return tutor4OriginalScale;
        if (tutorObj == tutor5) return tutor5OriginalScale;
        return tutorObj.transform.localScale;
    }

    private void RefreshTutorTextMeshes(GameObject tutorObj)
    {
        if (tutorObj == null) return;

        Canvas.ForceUpdateCanvases();
        var texts = tutorObj.GetComponentsInChildren<TMP_Text>(true);
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].ForceMeshUpdate();
        }
    }

    private GameObject GetActiveTutor()
    {
        if (tutor1 != null && tutor1.activeSelf) return tutor1;
        if (tutor2 != null && tutor2.activeSelf) return tutor2;
        if (tutor3 != null && tutor3.activeSelf) return tutor3;
        if (tutor4 != null && tutor4.activeSelf) return tutor4;
        if (tutor5 != null && tutor5.activeSelf) return tutor5;
        return null;
    }

    public void Tutor1Next()
    {
        ShowTutor(2);
    }

    public void Tutor2Prev()
    {
        ShowTutor(1);
    }

    public void Tutor2Next()
    {
        ShowTutor(3);
    }

    public void Tutor3Prev()
    {
        ShowTutor(2);
    }

    public void Tutor3Next()
    {
        ShowTutor(4);
    }

    public void Tutor4Prev()
    {
        ShowTutor(3);
    }

    public void Tutor4Next()
    {
        ShowTutor(5);
    }

    public void Tutor5Prev()
    {
        ShowTutor(4);
    }

    public void TutorClose()
    {
        AnimateTutorOut(GetActiveTutor(), EndTutorial);
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

}

