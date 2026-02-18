using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class overlay : MonoBehaviour
{
    private static bool hasShownCatWisper;
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject obj4;
    public GameObject bookopen;
    public GameObject catwisper;
    public GameObject cathelp;
    public GameObject catbtn;


    public TMPro.TextMeshProUGUI midleText;
    public string helpText = "";
    private List<string> textPieces = new List<string>();
    private int currentPageIndex = 0;
    

    private GameObject selectedLevel = null;
   
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
        {
            catwisper.SetActive(!hasShownCatWisper);
            hasShownCatWisper = true;
        }
        bookopen.SetActive(false);
        if (catwisper != null && !catwisper.activeSelf)
        {
            Invoke("cathelpbtn", 1f);
        }
        else
        {
            cathelp.SetActive(false);
        }

        changecanfiance = PlayerPrefs.GetFloat("changecanfiance", changecanfiance);

        InitializeHelpText();
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
                       
                       // Show cathelp after catwisper is hidden
                       cathelpbtn();
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

    // Initialize and split help text into pieces
    private void InitializeHelpText()
    {
        textPieces.Clear();
        
        if (string.IsNullOrEmpty(helpText))
        {
            textPieces.Add("No help text available.");
            currentPageIndex = 0;
            UpdateMidleText();
            return;
        }
        
        if (helpText.Length <= 80)
        {
            textPieces.Add(helpText);
        }
        else
        {
            int numPages = Mathf.CeilToInt(helpText.Length / 80f);
            numPages = Mathf.Min(numPages, 5);
            
            int charsPerPage = Mathf.CeilToInt(helpText.Length / (float)numPages);
            for (int i = 0; i < helpText.Length; i += charsPerPage)
            {
                int length = Mathf.Min(charsPerPage, helpText.Length - i);
                textPieces.Add(helpText.Substring(i, length));
            }
        }
        
        currentPageIndex = 0;
        UpdateMidleText();
    }
    private void UpdateMidleText()
    {
        if (midleText != null && textPieces.Count > 0)
        {
            midleText.text = textPieces[currentPageIndex];
        }
        
        UpdateButtonVisibility();
    }
    
    private void UpdateButtonVisibility()
    {
        if (cathelp == null) return;

        Transform prevBtn = FindChildRecursive(cathelp.transform, "prev");
        Transform nextBtn = FindChildRecursive(cathelp.transform, "next");
        
        if (textPieces.Count <= 1)
        {
            SetButtonInteractable(prevBtn, false);
            SetButtonInteractable(nextBtn, false);
            return;
        }
        

        if (prevBtn != null)
        {
            SetButtonInteractable(prevBtn, currentPageIndex > 0);
        }
        

        if (nextBtn != null)
        {
            SetButtonInteractable(nextBtn, currentPageIndex < textPieces.Count - 1);
        }
    }

    private void SetButtonInteractable(Transform buttonTransform, bool interactable)
    {
        if (buttonTransform == null) return;

        if (!buttonTransform.gameObject.activeSelf)
        {
            buttonTransform.gameObject.SetActive(true);
        }

        Button button = buttonTransform.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = interactable;
        }
    }

    private Transform FindChildRecursive(Transform parent, string childName)
    {
        if (parent == null || string.IsNullOrEmpty(childName)) return null;

        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }

            Transform found = FindChildRecursive(child, childName);
            if (found != null)
            {
                return found;
            }
        }

        return null;
    }
    

    public void prev()
    {
        if (textPieces.Count == 0 || currentPageIndex <= 0) return;
        
        currentPageIndex--;
        UpdateMidleText();
    }
    
 
    public void next()
    {
        if (textPieces.Count == 0 || currentPageIndex >= textPieces.Count - 1) return;
        
        currentPageIndex++;
        UpdateMidleText();
    }

    public void OnLevelHoverEnter(GameObject levelBtn)
    {
        if (levelBtn == null || levelBtn == selectedLevel) return;
        
        Transform hover = levelBtn.transform.Find("hover");
        Transform arrow = levelBtn.transform.Find("arrow");
        
        if (hover != null) hover.gameObject.SetActive(true);
        if (arrow != null) arrow.gameObject.SetActive(true);
    }
    
    public void OnLevelHoverExit(GameObject levelBtn)
    {
        if (levelBtn == null || levelBtn == selectedLevel) return;
        
        Transform hover = levelBtn.transform.Find("hover");
        Transform arrow = levelBtn.transform.Find("arrow");
        
        if (hover != null) hover.gameObject.SetActive(false);
        if (arrow != null) arrow.gameObject.SetActive(false);
    }
    
    public void OnLevelSelect(GameObject levelBtn)
    {
        if (levelBtn == null) return;
        
        if (selectedLevel != null)
        {
            Transform prevSelect = selectedLevel.transform.Find("select");
            Transform prevArrow = selectedLevel.transform.Find("arrow");
            Transform prevHover = selectedLevel.transform.Find("hover");
            
            if (prevSelect != null) prevSelect.gameObject.SetActive(false);
            if (prevArrow != null) prevArrow.gameObject.SetActive(false);
            if (prevHover != null) prevHover.gameObject.SetActive(false);
        }
        
        selectedLevel = levelBtn;
        Transform select = levelBtn.transform.Find("select");
        Transform arrow = levelBtn.transform.Find("arrow");
        Transform hover = levelBtn.transform.Find("hover");
        
        if (select != null) select.gameObject.SetActive(true);
        if (hover != null) hover.gameObject.SetActive(true);
        if (arrow != null) arrow.gameObject.SetActive(true);
    }
}
