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
        //get enum eventually
        foreach (PetObject pet in _pets)
        {
            if (pet.petType == PetType.Penguin)
            {
                _petObject = pet;
                break;
            }
        }
    }

    private void Update()
    {
        if (_petObject == null || !GameManager.Instance || GameManager.Instance.RoundOver || !PlayerManager.Instance) { return; }
        var hpPercent = PlayerManager.Instance.GetCurHPPercent();
        _petFace.sprite = _petObject.GetFace(hpPercent);
    }
}
