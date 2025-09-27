using UnityEngine;
using UnityEngine.UI;

namespace Reels
{
    public abstract class Reel : MonoBehaviour
    {
        [SerializeField] private Texture2D[] textures;

        public void Initialize()
        {
            if (textures.Length <= 0) return;
            
            int index = Random.Range(0, textures.Length);
            Texture2D texture = textures[index];

            Image imageComponent = GetComponentInParent<Image>();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            
            if (imageComponent != null) imageComponent.sprite = sprite;
        }

        public abstract void OnActivate();
    }
}
