using System.Net;

namespace ReadmeLinkVerifier
{
    static class Utils
    {
        public static bool IsInternetConnected()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
