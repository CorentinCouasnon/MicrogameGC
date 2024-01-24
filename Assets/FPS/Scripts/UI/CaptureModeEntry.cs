using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.FPS.UI
{
    public class CaptureModeEntry : MonoBehaviour
    {
        [SerializeField] Image _background;
        [SerializeField] TextMeshProUGUI _affiliationText;
        [SerializeField] TextMeshProUGUI _scoreText;

        public void SetBackground(Color color)
        {
            _background.color = color;
        }
        
        public void SetAffiliation(int affiliation)
        {
            _affiliationText.text = $"Team {affiliation}";
        }
        
        public void SetScore(float score)
        {
            _scoreText.text = $"{score:.##}";
        }
    }
}