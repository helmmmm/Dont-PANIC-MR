using UnityEngine;

namespace Tests
{
    public class NumberDoubler : MonoBehaviour
    {
        public int number = 1;  // Initial number

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DoubleNumber();
            }
        }

        public void DoubleNumber()
        {
            number *= 2;
        }
    }
}
