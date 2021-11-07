using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEvents : MonoBehaviour
{
    public GameObject previousSelected;
    public List<GameObject> selectors = new List<GameObject>();
    public float offset;

    public void Start()
    {
        Visibility(true);
        Selector(EventSystem.current.firstSelectedGameObject);
    }

    public void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            Selector(EventSystem.current.currentSelectedGameObject);
        }
    }

    public void Selector(GameObject selected)
    {
        switch (selected.tag)
        {
            case "Button":
                if (selected != previousSelected)
                {
                    RectTransform rect = selected.GetComponent<RectTransform>();
                    float rectPosition = rect.sizeDelta.x / 2 + offset;

                    Visibility(true);

                    selectors[0].transform.position = new Vector2(selected.transform.position.x + rectPosition, selected.transform.position.y);
                    selectors[1].transform.position = new Vector2(selected.transform.position.x - rectPosition, selected.transform.position.y);

                    previousSelected = selected;
                }
                break;
        }
    }

    public void Visibility(bool active)
    {
        for (int i = 0; i < selectors.Count; i++)
        {
            selectors[i].SetActive(active);
        }
    }
}
