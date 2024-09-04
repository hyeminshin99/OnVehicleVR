using UnityEngine;

namespace KUsystem.Utils
{
    public class Poolable : MonoBehaviour
    {
        private bool mIsUsing;
        public bool IsUsing
        {
            get
            {
                return mIsUsing;
            }

            set
            {
                mIsUsing = value;
            }
        }
    }
}