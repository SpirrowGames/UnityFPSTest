using UnityEngine;
// ReSharper disable PossibleLossOfFraction

namespace FpsTest
{
    /// <summary>
    /// レティクルの表示
    /// </summary>
    public class Reticle : MonoBehaviour
    {
        public int size = 10;
        public int width = 2;
        public int spread = 20;
        public CharacterMovement character;
        
        private CharacterStatus _status;

        private void Awake()
        {
            _status = character.GetComponent<CharacterStatus>();
        }

        private void OnGUI()
        {
            if (_status.isDead) return;

            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.gray);
            texture.wrapMode = TextureWrapMode.Repeat;
            texture.Apply();
            
            GUI.DrawTexture(
                new Rect(Screen.width / 2 - width / 2, (Screen.height / 2 - size / 2) + spread / 2, width, size),
                texture);

            GUI.DrawTexture(
                new Rect(Screen.width / 2 - width / 2, (Screen.height / 2 - size / 2) - spread / 2, width, size),
                texture);

            GUI.DrawTexture(
                new Rect((Screen.width / 2 - size / 2) + spread / 2, Screen.height / 2 - width / 2, size, width),
                texture);

            GUI.DrawTexture(
                new Rect((Screen.width / 2 - size / 2) - spread / 2, Screen.height / 2 - width / 2, size, width),
                texture);
        }
    }
}