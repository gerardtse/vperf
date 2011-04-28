using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vPref.Remoting
{
    public interface IVPrefServiceProvider
    {
        int returnInterCount();
        void reportIO(String uuid, Dictionary<String, String> data);
    }
}
