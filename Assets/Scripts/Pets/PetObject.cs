using UnityEngine;

public enum PetType
{
    Penguin,
    Cat,
    Cow,
    Longhorn
}

[CreateAssetMenu(fileName = "Pet", menuName = "ScriptableObjects/Pets", order = 1)]
public class PetObject : ScriptableObject
{
    [SerializeField] private Sprite _happyFace;
    [SerializeField] private Sprite _mildFace;
    [SerializeField] private Sprite _sadFace;
    public PetType petType;

    public Sprite GetFace(float percent)
    {
        if (percent > 0.66f)
        {
            return _happyFace;
        }
        else if (percent > 0.33f)
        {
            return _mildFace;
        }
        else
        {
            return _sadFace;
        }
    }
}
