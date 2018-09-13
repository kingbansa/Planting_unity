using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantExpPanel : MonoBehaviour {

    public Slider waterExpSlider;
    public Slider sunExpSlider;
    public Slider fertilizerExpSlider;
    public Dropdown plantListDropdown;
    private int maxVal = 10; 
    List<float> waterExp = new List<float>();                          //디비에서 받아온 식물 경험치
    List<float> sunExp = new List<float>();
    List<float> fertilizerExp = new List<float>();
    List<int> Lv = new List<int>();
    private float maxWaterExp = 10, maxSunExp = 10, maxFertilizerExp = 10;

    
    public Text currentWaterExpText, currentSunExpText, currentFertilizerExpText;

    float[] waterLvUpExp = { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };
    float[] sunLvUpExp = { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };
    float[] fertilizerLvUpExp = { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };


    private static PlantExpPanel instance;
    public static PlantExpPanel Instance
    {
        get
        {
            return instance;
        }
    }
    // Use this for initialization
    void Start() {
        StartCoroutine(ExpList());
    }

    public void CallExpList()
    {
        ExpList();
    }

    IEnumerator ExpList()
    {
        ClearList();
        yield return new WaitForSeconds(0.1f);
        loginScript.Instance.SelectQuery("*", "PlantList_");
        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i <= loginScript.plantPos.Count - 1; i++)
        {
            waterExp.Add(loginScript.waterEXP[i]);
            sunExp.Add(loginScript.sunEXP[i]);
            fertilizerExp.Add(loginScript.fertilizerEXP[i]);
            Lv.Add(loginScript.Lv[i]);
        }

        int _index = plantListDropdown.value;
        CallCurrentExp_IndexChanged(_index);
    }

    public void CallCurrentExp_IndexChanged(int index)
    {
        CurrentExp_IndexChanged(index);
    }

    IEnumerator CurrentExp_IndexChanged(int index)
    {
        ExpList();

        yield return new WaitForSeconds(0.5f);

        maxWaterExp = waterLvUpExp[ Lv[index-1] - 1];/////////////////////////////이부분 에러 일단 고쳣?는데 (클라우드레코트랙커블  142번줄 참고) ExpList호출순서 고치기
        maxSunExp = sunLvUpExp[Lv[index - 1] - 1];
        maxFertilizerExp = fertilizerLvUpExp[Lv[index - 1] - 1];

        currentWaterExpText.text = waterExp[index - 1] + " / " + maxWaterExp;
        currentSunExpText.text = sunExp[index - 1] + " / " + maxSunExp;
        currentFertilizerExpText.text = fertilizerExp[index - 1] + " / " + maxFertilizerExp;
        ChangeExpValue(index);
    }

    public void ChangeExpValue(int index)
    {
        if(index == 0)
        {
            waterExpSlider.value = 0.0f;
            sunExpSlider.value = 0.0f;
            fertilizerExpSlider.value = 0.0f;
            Debug.Log(index);
        }
        else if(index > 0)
        {
            waterExpSlider.value = waterExp[index-1] / waterLvUpExp[Lv[index - 1] - 1];
            sunExpSlider.value = sunExp[index - 1] / sunLvUpExp[Lv[index - 1] - 1];
            fertilizerExpSlider.value = fertilizerExp[index - 1] / fertilizerLvUpExp[Lv[index - 1] - 1];
        }
    }

    public void ClearList()
    {
        loginScript.plantName.Clear();
        loginScript.plantPos.Clear();
        loginScript.Lv.Clear();
        loginScript.plantID.Clear();
        loginScript.waterEXP.Clear();
        loginScript.sunEXP.Clear();
        loginScript.fertilizerEXP.Clear();
        waterExp.Clear();
        sunExp.Clear();
        fertilizerExp.Clear();
    }
}
