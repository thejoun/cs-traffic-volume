using TrafficVolume.Managers;

namespace TrafficVolume.Extensions
{
    public static class ArrayExtensions
    {
        public static bool TryGetValue<T>(this Array16<T> array, int index, out T value)
        {
            if (index >= array.m_size)
            {
                Manager.Log.WriteLog($"Array of '{typeof(T).Name}' - index out of range! ({index} / {array.m_size})");
                value = default;
                return false;
            }

            value = array.m_buffer[index];
            
            if (value == null)
            {
                Manager.Log.WriteLog($"Array of '{typeof(T).Name}' - value at {index} is null!)");
                return false;
            }
            
            return true;
        }
        
        public static bool TryGetValue<T>(this Array32<T> array, int index, out T value)
        {
            if (index >= array.m_size)
            {
                Manager.Log.WriteLog($"Array of '{typeof(T).Name}' - index out of range! ({index} / {array.m_size})");
                value = default;
                return false;
            }

            value = array.m_buffer[index];

            if (value == null)
            {
                Manager.Log.WriteLog($"Array of '{typeof(T).Name}' - value at {index} is null!)");
                return false;
            }
            
            return true;
        }
    }
}