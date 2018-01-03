using UnityEngine;

namespace Utils {
    public class Utils : MonoBehaviour {
        public static void DestroyElement(string name)
        {
            DestroyImmediate(GameObject.Find(name));
        }
    }

    public static class FloatArrayExtensions
    {
        public static float Sample( this float[] fArr, float t)
        {
            int count = fArr.Length;
            if (count == 0)
            {
                Debug.LogError("Unable to sample array - it has no elements.");
                return 0;
            }
    
            if (count == 1) return fArr[0];
    
            float f = t * (count - 1);
            int idLower = Mathf.FloorToInt(f);
            int idUpper = Mathf.FloorToInt(f + 1);
    
            if (idUpper >= count) return fArr[count - 1];
            if (idLower < 0) return fArr[0];
    
            return Mathf.Lerp(fArr[idLower], fArr[idUpper], f - idLower);
        }
    }
}