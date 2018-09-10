namespace Assembly
{
    public class InstanceBase<T> where T : new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (null == _instance)
                    _instance = new T();
                return _instance;
            }
        }
    }
}