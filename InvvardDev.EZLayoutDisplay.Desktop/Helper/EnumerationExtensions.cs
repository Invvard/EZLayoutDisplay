using System;

namespace InvvardDev.EZLayoutDisplay.Desktop.Helper
{
    public static class EnumerationExtensions
    {
        public static bool Has<T>(this Enum type, T value)
        {
            try { return ((int) (object) type & (int) (object) value) == (int) (object) value; }
            catch { return false; }
        }

        public static bool Is<T>(this Enum type, T value)
        {
            try { return (int) (object) type == (int) (object) value; }
            catch { return false; }
        }

        public static T Add<T>(this Enum type, T value)
        {
            try { return (T) (object) ((int) (object) type | (int) (object) value); }
            catch (Exception ex) { throw new ArgumentException($"Could not append value from enumerated type '{typeof(T).Name}'.", ex); }
        }

        public static T Remove<T>(this Enum type, T value)
        {
            try { return (T) (object) ((int) (object) type & ~(int) (object) value); }
            catch (Exception ex)
            {
                throw new ArgumentException($"Could not remove value from enumerated type '{typeof(T).Name}'.", ex);
            }
        }
    }
}