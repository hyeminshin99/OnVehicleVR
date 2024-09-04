using KUsystem.Manager;
using System.Collections.Generic;
using System.Collections;
using System.Drawing.Text;
using UnityEngine;

namespace KUsystem.Vection
{
    public class ReplaceCube: MonoBehaviour
    {
        private Vector3 mDestroyPosition = Vector3.zero;

        public void OnTriggerEnter(Collider other)
        {
            gameObject.transform.position = new Vector3(
                gameObject.transform.position.x - 500.0f,
                gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
}