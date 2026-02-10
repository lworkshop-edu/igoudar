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
    void Start()
    {
        bookopen.SetActive(false);
        ideaopen.SetActive(false);
        congrats.SetActive(false);
        cathelp.SetActive(true);
        catbtn.SetActive(false);
        catwrong.SetActive(false);
        catrcorect.SetActive(false);
       leftObj.SetActive(false); 
        if (rightObj != null) rightObj.SetActive(true);
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
            cathelpbtntest(catrcorect);
        }
        else
        {
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
}

