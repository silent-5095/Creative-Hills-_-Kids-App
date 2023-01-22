using UnityEngine;

namespace Magazine
{
    [CreateAssetMenu(menuName = "DataBase/Magazine/Magazine_Data")]
    public class MagazineData : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private string summary;
        public string Summary => summary;
        public Sprite Icon => icon;
    }
}
