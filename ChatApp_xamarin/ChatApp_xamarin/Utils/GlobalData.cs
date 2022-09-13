using ChatApp_xamarin.Models;

namespace ChatApp_xamarin.Utils
{
    public class GlobalData
    {
        static private GlobalData _ins;
        static public GlobalData ins
        {
            get
            {
                if (_ins == null)
                    _ins = new GlobalData();
                return _ins;
            }
            set { _ins = value; }
        }


        public User currentUser { get; set; }

        public bool isSilentMode { get; set; }

    }
}
