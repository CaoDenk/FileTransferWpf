

using p2pchat.ExeTask;

namespace p2pchat.Utils
{
    class Utils
    {
        public static string GetIpv6WithPortStr()
        {
            if (ListenTask.localIpv6Address != null)
            {
                return $"[{ListenTask.localIpv6Address.ToString()}]:{ListenTask.port}";
            }
            else
                return null;

        }

        public static bool CmpBytes(byte[] bytes1, byte[] bytes2)
        {

            if (BitConverter.ToInt64(bytes1) == BitConverter.ToInt64(bytes2))
            {
                return BitConverter.ToInt64(bytes1, 8) == BitConverter.ToInt64(bytes2, 8);
            }
            else
                return false;
        }

    }
}
