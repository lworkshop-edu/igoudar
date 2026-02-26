using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

    private int selectedDoorNumber = -1;
    private HashSet<int> solvedDoorNumbers = new HashSet<int>();
    private HashSet<GameObject> knownKeyObjects = new HashSet<GameObject>();
    private HashSet<GameObject> solvedKeyObjects = new HashSet<GameObject>();


  
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

    public List<GameObject> doorsObjs;

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

    private void PlayClickSfx()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }


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

        solvedDoorNumbers.Clear();
        knownKeyObjects.Clear();
        solvedKeyObjects.Clear();
        ClearAllDoorStates();
        RefreshKeyVisualStates();

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
        PlayClickSfx();
        if (isTutorialActive)
            return;

        if ((catwrong != null && catwrong.activeSelf) || (catrcorect != null && catrcorect.activeSelf))
            return;

        if (doorsObjs == null || doorsObjs.Count == 0) return;

        int normalizedDoorNumber = NormalizeToDoorNumber(doornumber);
        if (normalizedDoorNumber < 1) return;

        selectedDoorNumber = normalizedDoorNumber;
        UpdateDoorSelectionVisuals(selectedDoorNumber - 1);
    }
    public void KeyClicked(GameObject keyObj)
    {
        int keyNumber;
        if (!TryGetKeyNumberFromObject(keyObj, out keyNumber))
        {
            return;
        }

        HandleKeyClicked(keyObj, keyNumber);
    }

    public void KeyClicked(int keyNumber)
    {
        HandleKeyClicked(GetClickedKeyObjectFromEvent(), keyNumber);
    }

    private void HandleKeyClicked(GameObject keyObj, int keyNumber)
    {
        PlayClickSfx();
        if (isTutorialActive)
            return;

        if ((catwrong != null && catwrong.activeSelf) || (catrcorect != null && catrcorect.activeSelf))
            return;

        if (selectedDoorNumber < 1)
            return;

        int normalizedKeyDoorNumber = NormalizeToDoorNumber(keyNumber);
        if (normalizedKeyDoorNumber < 1)
            return;

        GameObject clickedKeyObj = ResolveKeyVisualObject(keyObj);
        if (clickedKeyObj == null)
        {
            return;
        }

        knownKeyObjects.Add(clickedKeyObj);

        HighlightSelectedKey(clickedKeyObj);

        bool isCorrect = normalizedKeyDoorNumber == selectedDoorNumber;
        ShowDoorResult(selectedDoorNumber - 1, isCorrect);

        int feedbackIndex = selectedDoorNumber - 1;

        if (isCorrect)
        {
            solvedDoorNumbers.Add(selectedDoorNumber);
            if (clickedKeyObj != null)
            {
                solvedKeyObjects.Add(clickedKeyObj);
                SetKeyChildActive(clickedKeyObj, "corect");
                SetKeyInteractable(clickedKeyObj, false);
            }
            if (AudioManager.Instance != null) AudioManager.Instance.PlayCorrect();

            cathelpbtntest(catrcorect);
            SetCatDialogText(catrcorect, corectext, feedbackIndex);
            catbtn.SetActive(true);
            cathelp.SetActive(false);
            catwrong.SetActive(false);
        }
        else
        {
            if (clickedKeyObj != null)
            {
                SetKeyChildActive(clickedKeyObj, "wrong");
            }
            if (AudioManager.Instance != null) AudioManager.Instance.PlayWrong();

            cathelpbtntest(catwrong);
            SetCatDialogText(catwrong, wrongtext, feedbackIndex);
            cathelp.SetActive(false);
            catbtn.SetActive(true);
            catrcorect.SetActive(false);
        }
    }

    private bool TryGetKeyNumberFromObject(GameObject keyObj, out int keyNumber)
    {
        keyNumber = -1;
        if (keyObj == null)
            return false;

        if (TryParseFirstInteger(keyObj.name, out keyNumber))
            return true;

        TMP_Text tmpText = keyObj.GetComponentInChildren<TMP_Text>(true);
        if (tmpText != null && TryParseFirstInteger(tmpText.text, out keyNumber))
            return true;

        Text legacyText = keyObj.GetComponentInChildren<Text>(true);
        if (legacyText != null && TryParseFirstInteger(legacyText.text, out keyNumber))
            return true;

        return false;
    }

    private bool TryParseFirstInteger(string value, out int number)
    {
        number = -1;
        if (string.IsNullOrEmpty(value))
            return false;

        int start = -1;
        int length = 0;
        for (int i = 0; i < value.Length; i++)
        {
            if (char.IsDigit(value[i]))
            {
                if (start < 0)
                {
                    start = i;
                }
                length++;
            }
            else if (start >= 0)
            {
                break;
            }
        }

        if (start < 0 || length <= 0)
            return false;

        string numberText = value.Substring(start, length);
        return int.TryParse(numberText, out number);
    }

    public void OnWrongCloseClicked()
    {
        PlayClickSfx();
        // catwrong.SetActive(false);
        continiuertest(catwrong);

        
        ClearAllDoorStates();
        RefreshKeyVisualStates();
        selectedDoorNumber = -1;
    }

    public void OnContinueClicked()
    {
        PlayClickSfx();
        // catrcorect.SetActive(false);
        continiuertest(catrcorect);
       
        ClearAllDoorStates();
        RefreshKeyVisualStates();
        selectedDoorNumber = -1;

        if (AreAllKeysSolved())
        {
            opencongrats();
        }
    }
    private void SetKeyChildActive(GameObject keyObj, string childName)
    {
        if (keyObj == null) return;
        bool disableAll = string.IsNullOrEmpty(childName);
        for (int i = 0; i < keyObj.transform.childCount; i++)
        {
            var child = keyObj.transform.GetChild(i).gameObject;
            string childLower = child.name.ToLower();
            bool isStateChild = childLower.Contains("corect") || childLower.Contains("wrong") || childLower.Contains("hovered") || childLower.Contains("select");
            if (!isStateChild)
            {
                continue;
            }

            if (disableAll)
            {
                child.SetActive(false);
            }
            else
            {
                child.SetActive(childLower.Contains(childName.ToLower()));
            }
        }
    }

    private void SetCatDialogText(GameObject catObj, List<string> texts, int index)
    {
        if (catObj == null || texts == null || index < 0 || index >= texts.Count)
            return;

        Transform textRoot = catObj.transform.GetChild(0).transform.GetChild(0);
        if (textRoot == null)
            return;

        TextMeshProUGUI textComponent = textRoot.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = texts[index];
        }
    }

    private void HighlightSelectedKey(GameObject selectedKeyObj)
    {
        if (knownKeyObjects == null) return;

        foreach (var keyObj in knownKeyObjects)
        {
            if (keyObj == null) continue;

            if (solvedKeyObjects.Contains(keyObj))
            {
                SetKeyChildActive(keyObj, "corect");
            }
            else if (keyObj == selectedKeyObj)
            {
                SetKeyChildActive(keyObj, "hovered");
            }
            else
            {
                SetKeyChildActive(keyObj, null);
            }
        }
    }

    private void ClearAllKeyStates()
    {
        if (keys == null) return;

        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i] == null || keys[i].key == null) continue;
            SetKeyChildActive(keys[i].key, null);
        }
    }

    private void RefreshKeyVisualStates()
    {
        if (knownKeyObjects == null) return;

        foreach (var keyObj in knownKeyObjects)
        {
            if (keyObj == null) continue;

            if (solvedKeyObjects.Contains(keyObj))
            {
                SetKeyChildActive(keyObj, "corect");
            }
            else
            {
                SetKeyChildActive(keyObj, null);
            }
        }
    }

    private bool AreAllKeysSolved()
    {
        if (doorsObjs == null || doorsObjs.Count == 0) return false;
        return solvedDoorNumbers.Count >= doorsObjs.Count;
    }

    private int NormalizeToDoorNumber(int rawNumber)
    {
        if (doorsObjs == null || doorsObjs.Count == 0) return -1;

        if (rawNumber >= 1 && rawNumber <= doorsObjs.Count)
        {
            return rawNumber;
        }

        if (rawNumber >= 0 && rawNumber < doorsObjs.Count)
        {
            return rawNumber + 1;
        }

        return -1;
    }

    private GameObject GetClickedKeyObjectFromEvent()
    {
        if (EventSystem.current == null)
        {
            return null;
        }

        return ResolveKeyVisualObject(EventSystem.current.currentSelectedGameObject);
    }

    private GameObject ResolveKeyVisualObject(GameObject inputObj)
    {
        if (inputObj == null)
        {
            return null;
        }

        if (HasKeyStateChild(inputObj.transform))
        {
            return inputObj;
        }

        for (int i = 0; i < inputObj.transform.childCount; i++)
        {
            Transform child = inputObj.transform.GetChild(i);
            if (HasKeyStateChild(child))
            {
                return child.gameObject;
            }
        }

        Transform parent = inputObj.transform.parent;
        while (parent != null)
        {
            if (HasKeyStateChild(parent))
            {
                return parent.gameObject;
            }
            parent = parent.parent;
        }

        return null;
    }

    private bool HasKeyStateChild(Transform root)
    {
        if (root == null)
        {
            return false;
        }

        for (int i = 0; i < root.childCount; i++)
        {
            string childName = root.GetChild(i).name.ToLower();
            if (childName.Contains("corect") || childName.Contains("wrong") || childName.Contains("hovered") || childName.Contains("select"))
            {
                return true;
            }
        }

        return false;
    }

    private void SetKeyInteractable(GameObject keyObj, bool isInteractable)
    {
        if (keyObj == null) return;

        var parentButton = keyObj.transform.parent != null ? keyObj.transform.parent.GetComponent<Button>() : null;
        if (parentButton != null)
        {
            parentButton.interactable = isInteractable;
        }

        var button = keyObj.GetComponent<Button>();
        if (button != null)
        {
            button.interactable = isInteractable;
        }

        var childButtons = keyObj.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < childButtons.Length; i++)
        {
            childButtons[i].interactable = isInteractable;
        }
    }

    private void UpdateDoorSelectionVisuals(int selectedDoorIndex)
    {
        if (doorsObjs == null) return;

        for (int i = 0; i < doorsObjs.Count; i++)
        {
            var door = doorsObjs[i];
            if (door == null) continue;

            bool isSelected = i == selectedDoorIndex;
            SetDoorChildrenState(door, false, false, isSelected);
        }
    }

    private void ShowDoorResult(int doorIndex, bool isCorrect)
    {
        if (doorsObjs == null) return;

        for (int i = 0; i < doorsObjs.Count; i++)
        {
            var door = doorsObjs[i];
            if (door == null) continue;

            if (i == doorIndex)
            {
                SetDoorChildrenState(door, isCorrect, !isCorrect, false);
            }
            else
            {
                SetDoorChildrenState(door, false, false, false);
            }
        }
    }

    private void ClearAllDoorStates()
    {
        if (doorsObjs == null) return;

        for (int i = 0; i < doorsObjs.Count; i++)
        {
            var door = doorsObjs[i];
            if (door == null) continue;
            SetDoorChildrenState(door, false, false, false);
        }
    }

    private void SetDoorChildrenState(GameObject door, bool showCorrect, bool showWrong, bool showSelect)
    {
        if (door == null) return;

        for (int i = 0; i < door.transform.childCount; i++)
        {
            var child = door.transform.GetChild(i);
            string childName = child.name.ToLower();

            if (childName.Contains("corect"))
            {
                child.gameObject.SetActive(showCorrect);
            }
            else if (childName.Contains("wrong"))
            {
                child.gameObject.SetActive(showWrong);
            }
            else if (childName.Contains("select"))
            {
                child.gameObject.SetActive(showSelect);
            }
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
        PlayClickSfx();
        ShowTutor(2);
    }

    public void Tutor2Prev()
    {
        PlayClickSfx();
        ShowTutor(1);
    }

    public void Tutor2Next()
    {
        PlayClickSfx();
        ShowTutor(3);
    }

    public void Tutor3Prev()
    {
        PlayClickSfx();
        ShowTutor(2);
    }

    public void Tutor3Next()
    {
        PlayClickSfx();
        ShowTutor(4);
    }

    public void Tutor4Prev()
    {
        PlayClickSfx();
        ShowTutor(3);
    }

    public void Tutor4Next()
    {
        PlayClickSfx();
        ShowTutor(5);
    }

    public void Tutor5Prev()
    {
        PlayClickSfx();
        ShowTutor(4);
    }

    public void TutorClose()
    {
        PlayClickSfx();
        AnimateTutorOut(GetActiveTutor(), EndTutorial);
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
            ideaopen.SetActive(false);
            congrats.SetActive(false);

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

    public void closeidea()
    {
        PlayClickSfx();
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
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySpark();
        }
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
        LevelProgression.UnlockNextLevel(1, 4);
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayCongrats();
            }
        bookopen.SetActive(false);
        ideaopen.SetActive(false);
        congrats.SetActive(true);
        midlecongrats.transform.localScale = Vector3.zero;
        LeanTween.scale(midlecongrats, Vector3.one, 0.2f);
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

