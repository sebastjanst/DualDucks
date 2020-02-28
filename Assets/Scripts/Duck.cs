using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class Duck
    {
        bool DuckEnabled = false;
        Image DuckImg;

        public Duck(bool isEnabled, Image duckImg)
        {
            DuckEnabled = isEnabled;
            DuckImg = duckImg;
        }

        public bool isDuckEnabled()
        {
            return DuckEnabled;
        }

        public void enableDuck(bool isEnabled)
        {
            DuckEnabled = isEnabled;
        }
    }
}
