using UnityEngine;

namespace Magazine
{
    [CreateAssetMenu(menuName = "DataBase/Magazine/Magazine_Data")]
    public class MagazineData : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        public Sprite Icon => icon;
    }
}
