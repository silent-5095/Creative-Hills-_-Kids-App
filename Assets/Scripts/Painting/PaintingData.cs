using UnityEngine;

namespace Painting
{
    [CreateAssetMenu(menuName = "DataBase/Painting/SectionData", fileName = "SectionData_")]
    public class PaintingData : ScriptableObject
    {
        public GameObject section;

        public void SetSection(GameObject sec)
        {
            section = sec;
        }
        public int BrushIndex
        {
            get => PlayerPrefs.GetInt(nameof(BrushIndex) + section.GetInstanceID());
            set => PlayerPrefs.SetInt(nameof(BrushIndex) + section.GetInstanceID(), value);
        }
        public int TextureIndex
        {
            get => PlayerPrefs.GetInt(nameof(TextureIndex) + section.GetInstanceID());
            set => PlayerPrefs.SetInt(nameof(TextureIndex) + section.GetInstanceID(), value);
        }
        public int ColorIndex
        {
            get => PlayerPrefs.GetInt(nameof(ColorIndex) + section.GetInstanceID());
            set => PlayerPrefs.SetInt(nameof(ColorIndex) + section.GetInstanceID(), value);
        }
    }
}