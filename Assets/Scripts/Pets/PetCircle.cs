using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetCircle : MonoBehaviour
{
    [SerializeField] private Image _petFace;
    [SerializeField] private List<PetObject> _pets;
    private PetType _savedType;
    private PetObject _petObject;

    private void Awake()
    {
        GameObject loadingScreenObject = GameObject.Find("LoadingScreenCanvas");
        LoadingScreen loadingScreen = loadingScreenObject.GetComponent<LoadingScreen>();
        _savedType = loadingScreen.petType;
        //get enum eventually
        foreach (PetObject pet in _pets)
        {
            if (_savedType == pet.petType)
            {
                _petObject = pet;
                break;
            }
        }
    }

    private void Update()
    {
        if (_petObject == null || !GameManager.Instance || !PlayerManager.Instance) { return; }
        var hpPercent = PlayerManager.Instance.GetCurHPPercent();
        _petFace.sprite = _petObject.GetFace(hpPercent);
    }
}
