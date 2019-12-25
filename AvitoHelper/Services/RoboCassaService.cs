using AvitoHelper.Helpers;
using AvitoHelper.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvitoHelper.Services
{

    public enum BuyedItem
    {
        subscribe,
        push
    }
    public class Purchace : BaseType
    {
        public int uId { get; set; }
        public int PayId { get; set; }
        public int Count { get; set; }
        public decimal outSumm { get; set; }
        public string type { get; set; }
    }
    public class RoboKassaService
    {
        private string MerchantLogin = "product_finder";
        private string passOne = "lsXEyJ89YmMHU4bCS7E3";
        private string passTwo = "uhOctKL71e4xvF2pUI6M";
        public string Description = "";
        public string GetHash(Purchace purchace)
        {
            var summ = purchace.outSumm.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            return new Crypto().CalculateMD5Hash(MerchantLogin + ":" + summ + ":" + purchace.PayId + ":" + passOne + ":type=" + purchace.type + "uId=" + purchace.uId + "count=" + purchace.Count);
        }

        public bool CheckHash(string outSumm, string InvId, string Type, string count, string uId, string Hash)
        {
            return string.Equals(new Crypto().CalculateMD5Hash($"{outSumm}:{InvId}:{passTwo}:type={Type}:uId={uId}:count={count}"), Hash, StringComparison.InvariantCultureIgnoreCase);
        }
        public string GetLink(Purchace purchace)
        {
            var summ = purchace.outSumm.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            return $"https://auth.robokassa.ru/Merchant/Index.aspx?MerchantLogin=product_finder&InvId={purchace.PayId}&Culture=ru&Encoding=utf-8&OutSum={summ}&type={purchace.type}&SignatureValue={GetHash(purchace)}";
        }
    }
}
