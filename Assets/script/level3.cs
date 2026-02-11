using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class level3 : MonoBehaviour
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
    public class KeyValueText
    {
        public string toptext;
        public string numberofquest;
        public string quest;
        public string answer1;
        public string answer2;
        public string answer3;
        public string correctAnswer; // "A", "B", or "C"
    }

    public TextMeshProUGUI toptext;
    public TextMeshProUGUI numberofquest;
    public TextMeshProUGUI questText;
    public TextMeshProUGUI answer1;
    public TextMeshProUGUI answer2;
    public TextMeshProUGUI answer3;
         private bool answerLocked = false;
    public List<KeyValueText> questions; // List of questions and answers
    private int currentQuestionIndex = 0;
    public GameObject quest; 



    public Slider sliderBarcanfiance;
    public Slider sliderBarreserves;


    public TextMeshProUGUI percentageTexcanfiance;
    public TextMeshProUGUI percentageTextreserves;

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
        
    public TextMeshProUGUI wrongcatText;
    public TextMeshProUGUI corectcatText;

    [System.Serializable]
    public class ChoiceFeedback
    {
        public string choiceLetter; 
        public string feedbackText; 
    }

    public List<ChoiceFeedback> feedbacks; 


    void Start()
    {
        bookopen.SetActive(false);
        ideaopen.SetActive(false);
        congrats.SetActive(false);
        cathelp.SetActive(true);
        catbtn.SetActive(false);
        catwrong.SetActive(false);
        catrcorect.SetActive(false);
        if (quest != null) quest.SetActive(false);

        // Load saved slider value
        changecanfiance = PlayerPrefs.GetFloat("changecanfiance", changecanfiance);

        if (questions != null && questions.Count > 0)
        {
            if (feedbacks == null) feedbacks = new List<ChoiceFeedback>();
            while (feedbacks.Count < questions.Count)
            {
                if (feedbacks.Count > 0)
                    feedbacks.Add(feedbacks[feedbacks.Count - 1]); 
                else
                    feedbacks.Add(new ChoiceFeedback { choiceLetter = "", feedbackText = "" });
            }
        }
    }

    public void ShowQuestUI()
    {
        if (quest != null)
        {
            quest.SetActive(true);
            DisplayCurrentQuestion();
        }
    }


    private void DisplayCurrentQuestion()
    {
GameObject[] allObjects = FindObjectsOfType<GameObject>(true);
                foreach (var obj in allObjects)
                {
                    if (obj.name == "corect" || obj.name == "wrong")
                    {
                        obj.SetActive(false);
                    }
                }
        if (questions == null || questions.Count == 0 || currentQuestionIndex >= questions.Count) return;
        var q = questions[currentQuestionIndex];
        if (toptext != null)
            toptext.text = q.toptext;
        if (numberofquest != null)
            numberofquest.text = q.numberofquest;
        if (questText != null)
            questText.text = q.quest;
        if (answer1 != null)
            answer1.text = q.answer1;
        if (answer2 != null)
            answer2.text = q.answer2;
        if (answer3 != null)
            answer3.text = q.answer3;
    }


    public void OnAnswerSelected(GameObject answerObj)
    {
        if (answerObj == null) return;
        string answer = answerObj.name;
        if (answerLocked) return;
        if (questions == null || currentQuestionIndex >= questions.Count) return;
        var q = questions[currentQuestionIndex];
        answerLocked = true;
        Transform correctChild = answerObj.transform.Find("corect");
        Transform wrongChild = answerObj.transform.Find("wrong");
        if (correctChild != null) correctChild.gameObject.SetActive(false);
        if (wrongChild != null) wrongChild.gameObject.SetActive(false);


       
        ChoiceFeedback feedback = (feedbacks != null && feedbacks.Count > currentQuestionIndex) ? feedbacks[currentQuestionIndex] : null;
        string feedbackText = "";
        if (feedback != null)
        {
            feedbackText = $"Décision juste : Choix {answer}\n\n{feedback.feedbackText}";
        }
        else
        {
            feedbackText = $"Décision juste : Choix {answer}";
        }
        if (corectcatText != null) corectcatText.text = feedbackText;
        if (wrongcatText != null) wrongcatText.text = feedbackText;

        if (q.correctAnswer == answer)
        {
            if (correctChild != null) correctChild.gameObject.SetActive(true);
            cathelpbtntest(catrcorect);
        }
        else
        {
            cathelpbtntest(catwrong);
            if (wrongChild != null) wrongChild.gameObject.SetActive(true);
        }
    }
 
    public void OnCorectBtnClicked()
    {

        catrcorect.SetActive(false);
        catbtn.SetActive(true);
        answerLocked = false;
        currentQuestionIndex++;
        if (currentQuestionIndex < questions.Count)
        {
            DisplayCurrentQuestion();
        }
        else
        {
            opencongrats();
        }
    }

    public void OnWrongBtnClicked()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true);
                foreach (var obj in allObjects)
                {
                    if (obj.name == "corect" || obj.name == "wrong")
                    {
                        obj.SetActive(false);
                    }
                }
        catwrong.SetActive(false);
        answerLocked = false;
        catbtn.SetActive(true);
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
                        // Show the question UI and allow answer selection
                        if (quest != null)
                        {
                            quest.SetActive(true);
                            DisplayCurrentQuestion();
                        }
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
        
        // Update slider based on correct or wrong answer
        if (obj == catrcorect)
        {
            IncreaseSliderCanfiance();
        }
        else if (obj == catwrong)
        {
            DecreaseSliderCanfiance();
        }
        
        obj.SetActive(true);
        if (obj.transform.GetChild(1).gameObject != null)
        {
            RawImage rawImgcathelp = obj.transform.GetChild(1).GetComponent<RawImage>();
            if (rawImgcathelp != null)
            {
                Color c = rawImgcathelp.color;
                c.a = 0f;
                rawImgcathelp.color = c;
                obj.transform.GetChild(2).gameObject.SetActive(false);
                obj.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                LeanTween.value(obj.transform.GetChild(1).gameObject, 0f, 1f, 0.3f)
                    .setOnUpdate((float val) => {
                        Color nc = rawImgcathelp.color;
                        nc.a = val;
                        rawImgcathelp.color = nc;
                    })
                    .setOnComplete(() => {
                        obj.transform.GetChild(2).gameObject.SetActive(true);
                        obj.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
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

