using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClumsyWizard.Utilities
{
    public static class CWString
    {
        public static void NormalizeString(ref string normalize)
        {
            normalize = normalize.Replace(" ", "").Replace("\'", "");
        }

        public static void NormalizeString(ref string[] normalize)
        {
            for (int i = 0; i < normalize.Length; i++)
            {
                normalize[i] = normalize[i].Replace(" ", "");
            }
        }

        public static void NormalizeString(ref List<string> normalize)
        {
            for (int i = 0; i < normalize.Count; i++)
            {
                normalize[i] = normalize[i].Replace(" ", "");
            }
        }
    }
}