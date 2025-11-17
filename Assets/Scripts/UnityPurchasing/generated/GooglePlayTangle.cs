// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("0ANXE8+Mxnx/9tCumGj4utNDUoXiRZZMHTFv3hh6DFaR7BmUIiJIHKZ2WEr2AeUYVoYJs6ekHwtTgR+TagXS2ueipz1mFnrkittN2TG499h/JVY7CM/+jjR+LjDoh0dQBRabkzXfJtr8W0Dhh3sVS4x7CtOcvPOTRVS9nyaOcmD7+NYplEZAVO4bR/NmUDDckUKlVupWKdnomfqDIe4pJAl8gvrCtB3UEN+BO+lov73byvWrObq0u4s5urG5Obq6uwzgrjHCX42LObqZi7a9spE98z1Mtrq6ur67uEEfpEtNJ94TKF67owd6T4yBY+ec8v00VyUhStbHuk1FkbJQuduSMlqon4a7nQyRTn6eiI6OqpU3w3zMDCjU4mIsB61gfrm4uru6");
        private static int[] order = new int[] { 5,12,3,4,4,11,7,10,9,12,11,11,13,13,14 };
        private static int key = 187;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
