// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("pMscFClsafOo2LQqRBWDF/92ORaonv4SX4xrmCSY5xcmVzRN7yDn6seyTDQMetMa3hFP9SemcXMVBDtlZlFIdVPCX4CwUEZAQGRb+Q2yAsKP0WqFg+kQ3eaQdW3JtIFCT60pUvd0enVF93R/d/d0dHXCLmD/DJFDaLiWhDjPK9aYSMd9aWrRxZ1P0V08M/qZ6++EGAl0g4tffJ53FVz8lCyLWILT/6EQ1rTCmF8i11rs7IbS+xHoFDKVji9JtduFQrXEHVJyPV0ezZndAUIIsrE4HmBWpjZ0HY2cS4uac1HoQLyuNTYY51qIjpog1Yk9seuY9cYBMED6sOD+JkmJnsvYVV1F93RXRXhzfF/zPfOCeHR0dHB1duYaLKziyWOusHd2dHV0");
        private static int[] order = new int[] { 4,10,9,8,6,10,8,13,12,11,10,13,13,13,14 };
        private static int key = 117;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
