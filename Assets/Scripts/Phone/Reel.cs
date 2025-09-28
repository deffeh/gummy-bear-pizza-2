using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Phone
{
    public abstract class Reel : MonoBehaviour
    {
        [SerializeField] private Texture2D[] textures;
        [SerializeField] public static int timeLostPerReel = 3;
        public string ActivateText = "";
        public int index = 3;
        public void Initialize()
        {
            if (textures.Length <= 0) return;
            
            int index = Random.Range(0, textures.Length);
            Texture2D texture = textures[index];

            Image imageComponent = GetComponentInParent<Image>();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            
            if (imageComponent != null) imageComponent.sprite = sprite;
        }

        public virtual void OnActivate()
        {
            if (PhoneManager.Instance.TextPrefab)
            {
                TextMeshProUGUI newText = Instantiate(PhoneManager.Instance.TextPrefab, transform.parent);
                newText.text = ActivateText;
                
            }

            PhoneManager.PlaySound(index);

        }
    }
}
