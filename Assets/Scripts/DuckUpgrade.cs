using UnityEngine;
using TMPro;

namespace Assets.Scripts
{
    class DuckUpgrade
    {
        string Name;
        int Price;
        TextMeshProUGUI PriceTxt;
        Sprite UpgradeImg;

        public DuckUpgrade(string _Name, int _Price, TextMeshProUGUI _PriceTxt, Sprite _UpgradeImg)
        {
            Name = _Name;
            Price = _Price;
            PriceTxt = _PriceTxt;
            UpgradeImg = _UpgradeImg;
        }

        public Sprite getImg()
        {
            return UpgradeImg;
        }

        public int getPrice()
        {
            return Price;
        }
    }
}
