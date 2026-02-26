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

    public List<GameObject> leveles ; 
    public bool saveUnlockedLevels = true;
    public List<Texture2D> levelButtonImages ;

    private void PlayClickSfx()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }

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
            Invoke(nameof(ShowCatHelpFromScript), 1f);
        }
        else
        {
            cathelp.SetActive(false);
        }

        changecanfiance = PlayerPrefs.GetFloat("changecanfiance", changecanfiance);

        LevelProgression.Configure(false, saveUnlockedLevels);
        RefreshLevelButtons();

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
        PlayClickSfx();
        obj1.SetActive(true);
        obj2.SetActive(false);
        obj3.SetActive(false);
        obj4.SetActive(false);
    }

            public void closebok()
    {
        PlayClickSfx();
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
        PlayClickSfx();
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
        PlayClickSfx();
                catwisper.transform.GetChild(1).transform.localScale = Vector3.one;//secondone is the midle
                LeanTween.scale(catwisper.transform.GetChild(1).gameObject, Vector3.zero, 0.2f).setOnComplete(() => {
                       catwisper.SetActive(false);
                       
                       // Show cathelp after catwisper is hidden
                       ShowCatHelpPanel();
                    });
        
    }
         public void continiuer()
    {
        PlayClickSfx();
        
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
            PlayClickSfx();
            ShowCatHelpPanel();
    }

    private void ShowCatHelpFromScript()
    {
        ShowCatHelpPanel();
    }

    private void ShowCatHelpPanel()
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
        PlayClickSfx();
        obj1.SetActive(false);
        obj2.SetActive(true);
        obj3.SetActive(false);
        obj4.SetActive(false);
    }


    public void ActivateObj3()
    {
        PlayClickSfx();
        obj1.SetActive(false);
        obj2.SetActive(false);
        obj3.SetActive(true);
        obj4.SetActive(false);
    }


    public void ActivateObj4()
    {
        PlayClickSfx();
        obj1.SetActive(false);
        obj2.SetActive(false);
        obj3.SetActive(false);
        obj4.SetActive(true);
    }

        public void returntointro()
    {
        PlayClickSfx();
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
        PlayClickSfx();
        if (textPieces.Count == 0 || currentPageIndex <= 0) return;
        
        currentPageIndex--;
        UpdateMidleText();
    }
    
 
    public void next()
    {
        PlayClickSfx();
        if (textPieces.Count == 0 || currentPageIndex >= textPieces.Count - 1) return;
        
        currentPageIndex++;
        UpdateMidleText();
    }

    public void OnLevelHoverEnter(GameObject levelBtn)
    {
        if (!IsLevelUnlocked(levelBtn)) return;
        if (levelBtn == null || levelBtn == selectedLevel) return;
        
        Transform hover = levelBtn.transform.Find("hover");
        Transform arrow = levelBtn.transform.Find("arrow");
        
        if (hover != null) hover.gameObject.SetActive(true);
        if (arrow != null) arrow.gameObject.SetActive(true);
    }
    
    public void OnLevelHoverExit(GameObject levelBtn)
    {
        if (!IsLevelUnlocked(levelBtn)) return;
        if (levelBtn == null || levelBtn == selectedLevel) return;
        
        Transform hover = levelBtn.transform.Find("hover");
        Transform arrow = levelBtn.transform.Find("arrow");
        
        if (hover != null) hover.gameObject.SetActive(false);
        if (arrow != null) arrow.gameObject.SetActive(false);
    }
    
    public void OnLevelSelect(GameObject levelBtn)
    {
        
        if (!IsLevelUnlocked(levelBtn)) return;
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

    private void RefreshLevelButtons()
    {
        if (leveles == null || leveles.Count == 0)
        {
            return;
        }

        int unlockedLevelsCount = LevelProgression.GetUnlockedLevelsCount(leveles.Count);
        for (int i = 0; i < leveles.Count; i++)
        {
            GameObject levelObj = leveles[i];
            if (levelObj == null)
            {
                continue;
            }

            bool isUnlocked = i < unlockedLevelsCount;
            Button levelButton = levelObj.GetComponent<Button>();
            if (levelButton != null)
            {
                levelButton.interactable = isUnlocked;
            }

            if (isUnlocked && i < levelButtonImages.Count)
            {
                Texture sprite = levelButtonImages[i];
                if (sprite != null)
                {
                    RawImage targetImage = levelObj.GetComponent<RawImage>();


                    if (targetImage != null)
                    {
                        targetImage.texture = sprite;
                    }
                }
            }

            if (!isUnlocked)
            {
                Transform hover = levelObj.transform.Find("hover");
                Transform arrow = levelObj.transform.Find("arrow");
                Transform select = levelObj.transform.Find("select");

                if (hover != null) hover.gameObject.SetActive(false);
                if (arrow != null) arrow.gameObject.SetActive(false);
                if (select != null) select.gameObject.SetActive(false);

                if (selectedLevel == levelObj)
                {
                    selectedLevel = null;
                }
            }
        }
    }

    private bool IsLevelUnlocked(GameObject levelBtn)
    {
        if (levelBtn == null || leveles == null || leveles.Count == 0)
        {
            return false;
        }

        int index = leveles.IndexOf(levelBtn);
        if (index < 0)
        {
            return true;
        }

        int unlockedLevelsCount = LevelProgression.GetUnlockedLevelsCount(leveles.Count);
        return index < unlockedLevelsCount;
    }

    public void ResetProgress()
    {
        LevelProgression.ResetProgress();
        selectedLevel = null;
        RefreshLevelButtons();
    }

    public void ApplyLevelImagesFromList()
    {
        if (leveles == null || levelButtonImages == null)
        {
            return;
        }

        int unlockedLevelsCount = LevelProgression.GetUnlockedLevelsCount(leveles.Count);
        int count = Mathf.Min(unlockedLevelsCount, levelButtonImages.Count);
        for (int i = 0; i < count; i++)
        {
            GameObject levelObj = leveles[i];
            Texture2D sprite = levelButtonImages[i];
            if (levelObj == null || sprite == null)
            {
                continue;
            }

            RawImage targetImage = levelObj.GetComponent<RawImage>();
         

            if (targetImage != null)
            {
                targetImage.texture = sprite;
            }
        }
    }
}
