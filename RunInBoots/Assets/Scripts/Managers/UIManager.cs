using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UIManager : MonoSingleton<UIManager>
{
    private TextMeshProUGUI _timerText;

    private List<GameObject> _catnipIcons = new List<GameObject>();
    
    public GameObject catnipIconPrefab;
    public Transform catnipIconContainer;

    private int _totalCatnipCount;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬 로드 시 이벤트 등록
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // UIManager가 파괴될 때 이벤트 등록 해제
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindTimerText();
        FindCatnipIconContainer();
        PlaceCatnipIcons();

    }

    private void FindTimerText()
    {
        GameObject timerObject = GameObject.FindGameObjectWithTag("TimerText");
        if (timerObject != null)
        {
            _timerText = timerObject.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogWarning("TimerText UI 요소를 찾을 수 없습니다.");
        }
    }

    public void UpdateTimerUI(float remainingTime)
    {
        if (_timerText != null)
        {
            _timerText.text = $"{Mathf.CeilToInt(remainingTime)}";
        }
        else
        {
            Debug.LogWarning("TimerText UI 요소가 설정되지 않았습니다.");
        }
    }

    private void FindCatnipIconContainer()
    {
        if (catnipIconContainer == null)
        {
            GameObject containerObject = GameObject.FindGameObjectWithTag("CatnipIconContainer");
            if (containerObject != null)
            {
                catnipIconContainer = containerObject.transform;
            }
            else
            {
                Debug.LogWarning("CatnipIconContainer를 찾을 수 없습니다.");
            }
        }
    }

    public void InitializeCatnipUI(int count)
    {
        _totalCatnipCount = count;
        PlaceCatnipIcons();
    }

    private void PlaceCatnipIcons()
    {
        ClearCatnipUI();

        for (int i = 0; i < _totalCatnipCount; i++)
        {
            GameObject icon = Instantiate(catnipIconPrefab, catnipIconContainer);
            _catnipIcons.Add(icon);
            SetCatnipIconState(icon, GameManager.Instance._catnipCollectedStates[i]);
        }
    }

    public void UpdateCatnipUI(int catnipID)
    {
        if (catnipID > 0 && catnipID <= _catnipIcons.Count)
        {
            GameManager.Instance._catnipCollectedStates[catnipID - 1] = true;
            SetCatnipIconState(_catnipIcons[catnipID - 1], true);
        }
        else
        {
            Debug.LogWarning("잘못된 catnipID: " + catnipID);
        }
    }

    

    public void ClearCatnipUI()
    {
        foreach (GameObject icon in _catnipIcons)
        {
            Destroy(icon);
        }
        _catnipIcons.Clear();
    }

    private void SetCatnipIconState(GameObject icon, bool isActive)
    {
        Color iconColor = icon.GetComponent<UnityEngine.UI.Image>().color;
        iconColor.a = isActive ? 1.0f : 0.5f; // 선명도 조정  
        icon.GetComponent<UnityEngine.UI.Image>().color = iconColor;
    }
}
