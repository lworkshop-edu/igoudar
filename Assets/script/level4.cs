using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class level4 : MonoBehaviour
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
        public GameObject key;
        public bool value;
    }


    public GameObject leftObj;
    public GameObject rightObj;
    private int[] jarCounts = new int[4];
    [System.Serializable]
    public class RightBtnInfo
    {
        public GameObject btnObj;
        [System.Serializable]
        public class WeekInfo
        {
            public int requiredJars;
        }
        public List<WeekInfo> weeks = new List<WeekInfo>();
    }
    public List<RightBtnInfo> rightBtnInfos;
    private int selectedRightBtn = -1;
    private int rightBtnClickCount = 0;
    private int currentWeek = 0;


    public UnityEngine.UI.Slider sliderBarcanfiance;
    public UnityEngine.UI.Slider sliderBarreserves;


    public TMPro.TextMeshProUGUI percentageTexcanfiance;
    public TMPro.TextMeshProUGUI percentageTextreserves;

    [Range(0,100)]
    public float changereserves;
    [Range(0,100)]
    public float changecanfiance;
    public GameObject midlebookopen;
    public GameObject midleideaopen;
    public GameObject midlecongrats;


    public GameObject catwrong;
    public GameObject catrcorect;


    public GameObject continueRightBtn;

    public TMPro.TextMeshProUGUI weektext;

    public GameObject textfloat;
    public GameObject textfloatplace;
 
    public TMPro.TextMeshProUGUI cathelptext;
    public TMPro.TextMeshProUGUI cathelptexttop;
    public List<string> corectext;
    public List<string> wrongtext;
    public GameObject topimagerighthide;
    private int initialSequenceStep = 0;

    public GameObject introobj;
    private int currentIntroIndex = 0;
    private bool showingFixText = true;

    [System.Serializable]
    public class textcangedwithweek
    {
        public string texttop;
       
        public string textbottom;
    }
    public List<textcangedwithweek> textchnagedwithweeks ;

    public string toptextfix;
    public string bottomtextfix;

    public GameObject ideabtn;

    
    void Start()
    {
        bookopen.SetActive(false);
        ideaopen.SetActive(false);
        congrats.SetActive(false);
        cathelp.SetActive(false);
        catbtn.SetActive(false);
        catwrong.SetActive(false);
        catrcorect.SetActive(false);
        leftObj.SetActive(false);
        ideabtn.SetActive(false);
        
        changecanfiance = PlayerPrefs.GetFloat("changecanfiance", changecanfiance);
        
        if (rightObj != null) rightObj.SetActive(false);
        for (int i = 0; i < rightBtnInfos.Count; i++)
        {
            Transform indicator = rightBtnInfos[i].btnObj.transform.Find("indicator level");
            if (indicator != null)
                indicator.gameObject.SetActive(false);

            Transform jars = rightBtnInfos[i].btnObj.transform.Find("jars");
            if (jars != null)
            {
                foreach (Transform jar in jars)
                    jar.gameObject.SetActive(false);
            }

            jarCounts[i] = 0;
        }
        selectedRightBtn = -1;
        currentWeek = 0;
        if (continueRightBtn != null)
            continueRightBtn.SetActive(false);
        UpdateWeekText();
        initialSequenceStep = 0;
        
        if (introobj != null)
        {
            for (int i = 0; i < introobj.transform.childCount; i++)
            {
                introobj.transform.GetChild(i).gameObject.SetActive(false);
            }
            
            if (introobj.transform.childCount > 0)
            {
                currentIntroIndex = 0;
                introobj.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    private void UpdateJarText(int btnIndex)
    {
        if (btnIndex < 0 || btnIndex >= rightBtnInfos.Count) return;
        GameObject btnObj = rightBtnInfos[btnIndex].btnObj;
        if (btnObj == null) return;
        Transform textChild = btnObj.transform.Find("Text (TMP) (1)");
        if (textChild != null)
        {
            TMP_Text tmp = textChild.GetComponent<TMP_Text>();
            if (tmp != null)
            {
                int count = jarCounts[btnIndex];
                string jarreWord = count == 1 ? "jarre" : "jarres";
                tmp.text = $"Besoin: {count} {jarreWord}";
            }
        }
    }

    public void OnJarClicked(int btnIndex)
    {
        if (btnIndex < 0 || btnIndex >= rightBtnInfos.Count) return;
        GameObject btnObj = rightBtnInfos[btnIndex].btnObj;
        Transform jars = btnObj.transform.Find("jars");
        if (jars == null) return;

        foreach (Transform jar in jars)
        {
            if (jar.gameObject.activeSelf)
            {
                jar.gameObject.SetActive(false);
                jarCounts[btnIndex] = Mathf.Max(0, jarCounts[btnIndex] - 1);
                UpdateJarText(btnIndex);
                break;
            }
        }

        if (leftObj != null)
        {
            foreach (Transform horizon in leftObj.transform)
            {
                foreach (Transform img in horizon)
                {
                    if (!img.gameObject.activeSelf)
                    {
                        img.gameObject.SetActive(true);
                        return;
                    }
                }
            }
        }
    }
  public void OnLeftObjClicked()
    {
        if (leftObj == null) return;
        if (selectedRightBtn == -1 || selectedRightBtn >= rightBtnInfos.Count) return;
        if (jarCounts[selectedRightBtn] >= 3) return;
        bool imageDeactivated = false;
        foreach (Transform horizon in leftObj.transform)
        {
            foreach (Transform img in horizon)
            {
                if (img.gameObject.activeSelf)
                {
                    img.gameObject.SetActive(false);
                    imageDeactivated = true;
                    break;
                }
            }
            if (imageDeactivated) break;
        }
        if (imageDeactivated)
        {
            AddJarToSelectedRightBtn();
        }
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
    }

    public void OnCorectBtnClicked()
    {
        int maxWeeks = 0;
        foreach (var info in rightBtnInfos)
        {
            if (info.weeks != null && info.weeks.Count > maxWeeks)
                maxWeeks = info.weeks.Count;
        }
        rightBtnClickCount = 0;
        foreach (var info in rightBtnInfos)
        {
            if (info.btnObj != null)
                info.btnObj.tag = "Untagged";
        }
        if (continueRightBtn != null)
            continueRightBtn.SetActive(false);
        if (currentWeek + 1 < maxWeeks)
        {
            currentWeek++;
            ResetAllJars();
            ResetAllLeftObjImages();
            UpdateWeekText();
            
            showingFixText = true;
            if (cathelptexttop != null)
            {
                cathelptexttop.text = toptextfix;
            }
            if (cathelptext != null)
            {
                cathelptext.text = bottomtextfix;
            }
            
            cathelpbtn();
            leftObj.SetActive(false);
            
            continiuertest(catrcorect);
                   if (catwrong != null)
            catwrong.SetActive(false);
               foreach (var info in rightBtnInfos)
        {
            if (info.btnObj != null)
                info.btnObj.tag = "Untagged";
            if (info.btnObj != null)
            {
                var indicators = info.btnObj.GetComponentsInChildren<Transform>(true);
                foreach (var t in indicators)
                {
                    if (t.name.ToLower().Contains("indicator") && t.gameObject.activeSelf)
                    {
                        rightBtnClickCount++;
                        t.gameObject.transform.parent.gameObject.tag = "Clicked";
                    }
                }
            }
        }
        }
        else
        {
            opencongrats();
        }

    }


    public void OnWrongBtnClicked()
    {

        continiuertest(catwrong);
               foreach (var info in rightBtnInfos)
        {
            if (info.btnObj != null)
                info.btnObj.tag = "Untagged";
        }

        rightBtnClickCount = 0;
        if (continueRightBtn != null)
            continueRightBtn.SetActive(false);

        ResetAllJars();
        ResetAllLeftObjImages();
        UpdateWeekText();
        if (catwrong != null)
            catwrong.SetActive(false);
               foreach (var info in rightBtnInfos)
        {
            if (info.btnObj != null)
                info.btnObj.tag = "Untagged";
            if (info.btnObj != null)
            {
                var indicators = info.btnObj.GetComponentsInChildren<Transform>(true);
                foreach (var t in indicators)
                {
                    if (t.name.ToLower().Contains("indicator") && t.gameObject.activeSelf)

                    {
                        rightBtnClickCount++;
                        t.gameObject.transform.parent.gameObject.tag = "Clicked";
                    }
                }
            }
        }
    }

   
    private void SetKeyIndicator(GameObject keyObj, bool active)
    {
    }
    private void SetKeyNumberOn(GameObject keyObj)
    {
    }

    public void OnWrongCloseClicked()
    {
    }

    public void OnContinueClicked()
    {
        bool allCorrect = true;
        for (int i = 0; i < rightBtnInfos.Count; i++)
        {
            int required = 0;
            if (rightBtnInfos[i].weeks != null && currentWeek < rightBtnInfos[i].weeks.Count)
                required = rightBtnInfos[i].weeks[currentWeek].requiredJars;
            if (jarCounts[i] != required)
            {
                allCorrect = false;
                break;
            }
        }
        if (allCorrect)
        {
            SetCatText(catrcorect, corectext, currentWeek);
            cathelpbtntest(catrcorect);
        }
        else
        {
            SetCatText(catwrong, wrongtext, currentWeek);
            cathelpbtntest(catwrong);
        }
        }

    private void ResetAllJars()
    {
        for (int i = 0; i < rightBtnInfos.Count; i++)
        {
            ResetJars(i);
        }
    }

    private void ResetAllLeftObjImages()
    {
        if (leftObj == null) return;
        foreach (Transform horizon in leftObj.transform)
        {
            foreach (Transform img in horizon)
            {
                img.gameObject.SetActive(true);
            }
        }
    }

    private void UpdateWeekText()
    {
        if (weektext != null)
        {
            weektext.text = $"Semaine {currentWeek + 1}";
        }
    }
    
    private void SetKeyChildActive(GameObject keyObj, string childName)
    {
    }

    public void OnRightBtnClicked(int btnIndex)
    {
        if (btnIndex < 0 || btnIndex >= rightBtnInfos.Count) return;
        selectedRightBtn = btnIndex;
        if (leftObj != null && !leftObj.activeSelf)
        {
            leftObj.SetActive(true);
            continiuer();
        }
        for (int i = 0; i < rightBtnInfos.Count; i++)
        {
            var btnObj = rightBtnInfos[i].btnObj;
            if (btnObj != null)
            {
                var indicators = btnObj.GetComponentsInChildren<Transform>(true);
                foreach (var t in indicators)
                {
                    if (t.name.ToLower().Contains("indicator"))
                        t.gameObject.SetActive(false);
                }
            }
        }
        var selectedBtnObj = rightBtnInfos[btnIndex].btnObj;
        if (selectedBtnObj != null)
        {
            var indicators = selectedBtnObj.GetComponentsInChildren<Transform>(true);
            foreach (var t in indicators)
            {
                if (t.name.ToLower().Contains("indicator"))
                    t.gameObject.SetActive(true);
            }
        }
        if (!rightBtnInfos[btnIndex].btnObj.CompareTag("Clicked"))
        {
            rightBtnClickCount++;
            rightBtnInfos[btnIndex].btnObj.tag = "Clicked";
        }
        if (continueRightBtn != null && rightBtnClickCount >= 4)
            continueRightBtn.SetActive(true);
    }

    public void AddJarToSelectedRightBtn()
    {
        if (selectedRightBtn == -1 || selectedRightBtn >= rightBtnInfos.Count) return;
        GameObject btnObj = rightBtnInfos[selectedRightBtn].btnObj;
        Transform jars = btnObj.transform.Find("jars");
        if (jars == null) return;

        int activeCount = 0;
        foreach (Transform child in jars)
            if (child.gameObject.activeSelf) activeCount++;

        if (activeCount < 3)
        {
            foreach (Transform child in jars)
            {
                if (!child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(true);
                    jarCounts[selectedRightBtn]++;
                    UpdateJarText(selectedRightBtn);
                    break;
                }
            }
        }
    }

    public void OnLeftImageClicked(GameObject imageObj)
    {
        if (selectedRightBtn == -1 || selectedRightBtn >= rightBtnInfos.Count) return;
        if (jarCounts[selectedRightBtn] >= 3) return;
        imageObj.SetActive(false);
        AddJarToSelectedRightBtn();
    }

    public void ResetJars(int btnIndex)
    {
        if (btnIndex < 0 || btnIndex >= rightBtnInfos.Count) return;
        jarCounts[btnIndex] = 0;
        Transform jars = rightBtnInfos[btnIndex].btnObj.transform.Find("jars");
        if (jars == null) return;
        foreach (Transform child in jars)
        {
            child.gameObject.SetActive(false);
        }
        UpdateJarText(btnIndex);
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
        if (initialSequenceStep == 0)
        {
            initialSequenceStep = 1;
            showingFixText = false;
            
            if (textchnagedwithweeks != null && currentWeek < textchnagedwithweeks.Count)
            {
                if (cathelptexttop != null)
                {
                    cathelptexttop.text = textchnagedwithweeks[currentWeek].texttop;
                }
                if (cathelptext != null)
                {
                    cathelptext.text = textchnagedwithweeks[currentWeek].textbottom;
                }
            }
            
            cathelpbtn();
            if (rightObj != null)
            {
                rightObj.SetActive(true);
            }
            return;
        }
        else if (initialSequenceStep == 1)
        {
            initialSequenceStep = 2;
        }
        
        if (showingFixText)
        {
            showingFixText = false;
            
            if (textchnagedwithweeks != null && currentWeek < textchnagedwithweeks.Count)
            {
                if (cathelptexttop != null)
                {
                    cathelptexttop.text = textchnagedwithweeks[currentWeek].texttop;
                }
                if (cathelptext != null)
                {
                    cathelptext.text = textchnagedwithweeks[currentWeek].textbottom;
                }
            }
            
            cathelpbtn();
            if (rightObj != null && !rightObj.activeSelf)
            {
                rightObj.SetActive(true);
            }
            return;
        }
        
        if (topimagerighthide.activeSelf)
        {
            topimagerighthide.SetActive(false);
        }
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

    public void CloseIntroObj()
    {
        if (introobj == null) return;
        
        if (currentIntroIndex < introobj.transform.childCount)
        {
            introobj.transform.GetChild(currentIntroIndex).gameObject.SetActive(false);
        }
        
        currentIntroIndex++;
    
        if (currentIntroIndex < introobj.transform.childCount)
        {
            introobj.transform.GetChild(currentIntroIndex).gameObject.SetActive(true);
        }
        else
        {
            if (cathelp != null)
            {

                showingFixText = true;
                if (cathelptexttop != null)
                {
                    cathelptexttop.text = toptextfix;
                }
                if (cathelptext != null)
                {
                    cathelptext.text = bottomtextfix;
                }
                ideabtn.SetActive(true);
                cathelp.SetActive(true);
            }
        }
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
        Transform background = GetCatBackground(obj);
        Transform midle = GetCatMiddle(obj);
        if (background != null)
        {
            RawImage rawImgcathelp = background.GetComponent<RawImage>();
            if (rawImgcathelp != null)
            {
                Color c = rawImgcathelp.color;
                c.a = 0f;
                rawImgcathelp.color = c;
                if (midle != null)
                {
                    midle.gameObject.SetActive(false);
                }
                if (background.childCount > 0)
                {
                    background.GetChild(0).gameObject.SetActive(false);
                }
                LeanTween.value(background.gameObject, 0f, 1f, 0.3f)
                    .setOnUpdate((float val) => {
                        Color nc = rawImgcathelp.color;
                        nc.a = val;
                        rawImgcathelp.color = nc;
                    })
                    .setOnComplete(() => {
                        if (midle != null)
                        {
                            midle.gameObject.SetActive(true);
                        }
                        if (background.childCount > 0)
                        {
                            background.GetChild(0).gameObject.SetActive(true);
                        }
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
        Transform background = GetCatBackground(obj);
        Transform midle = GetCatMiddle(obj);
        if (background != null)
        {
            RawImage rawImgcathelp = background.GetComponent<RawImage>();
            if (rawImgcathelp != null)
            {
                Color c = rawImgcathelp.color;
                c.a = 1f;
                rawImgcathelp.color = c;
                if (midle != null)
                {
                    midle.gameObject.SetActive(false);
                }
                if (background.childCount > 0)
                {
                    background.GetChild(0).gameObject.SetActive(false);
                }
                LeanTween.value(background.gameObject, 1f, 0f, 0.2f)
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

    private bool AllBtnsClicked()
    {
        for (int i = 0; i < rightBtnInfos.Count; i++)
        {
            var btnObj = rightBtnInfos[i].btnObj;
            bool found = false;
            if (btnObj != null)
            {
                var indicators = btnObj.GetComponentsInChildren<Transform>(true);
                foreach (var t in indicators)
                {
                    if (t.name.ToLower().Contains("indicator") && t.gameObject.activeSelf)
                    {
                        found = true;
                        break;
                    }
                }
            }
            if (!found) return false;
        }
        return true;
    }

    private void SetCatText(GameObject catObj, List<string> texts, int index)
    {
        if (catObj == null || texts == null || texts.Count == 0) return;
        int safeIndex = Mathf.Clamp(index, 0, texts.Count - 1);
        Transform midle = GetCatMiddle(catObj);
        TextMeshProUGUI textObj = null;
        if (midle != null)
        {
            textObj = midle.GetComponentInChildren<TextMeshProUGUI>(true);
        }
        if (textObj == null)
        {
            textObj = catObj.GetComponentInChildren<TextMeshProUGUI>(true);
        }
        if (textObj != null)
        {
            string value = texts[safeIndex];
            if (texts == corectext)
            {
                value = value.Replace("Feedback positif", "Feedback positif\n");
            }
            else if (texts == wrongtext)
            {
                value = value.Replace("Feedback négatif", "Feedback négatif\n");
            }
            textObj.text = value;
        }
    }

    private Transform GetCatBackground(GameObject catObj)
    {
        if (catObj == null) return null;
        Transform background = catObj.transform.Find("background");
        if (background != null) return background;
        if (catObj.transform.childCount > 0) return catObj.transform.GetChild(0);
        return null;
    }

    private Transform GetCatMiddle(GameObject catObj)
    {
        if (catObj == null) return null;
        Transform midle = catObj.transform.Find("midle");
        if (midle != null) return midle;
        if (catObj.transform.childCount > 1) return catObj.transform.GetChild(1);
        return null;
    }
}

