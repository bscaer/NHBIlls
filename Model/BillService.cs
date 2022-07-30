using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Legiscan.Model
{
    // The BillService uses an API to fetch bills from LegiScan. See https://legiscan.com/
    internal class BillService
    {
        const string LEGISCAN_ENDPOINT = @"https://api.legiscan.com/?key=";

        public static async Task<Bill[]> GetBillsAsync()
        {
            const string masterListUrl = LEGISCAN_ENDPOINT + @"&op=getMasterList&state=NH";
            IList<Bill> bills = new List<Bill>();
            //const string billUrl = LEGISCAN_ENDPOINT + @"&op=getBill&id=1545878";

            try
            {
                using HttpClient client = new();
                using HttpResponseMessage res = await client.GetAsync(masterListUrl);
                using HttpContent content = res.Content;
                var data = await content.ReadAsStringAsync();
                if (data != null)
                {
                    dynamic masterListData = JObject.Parse(data);
                    dynamic masterlist = masterListData.masterlist;
                    int index = 0;
                    while (masterlist[index.ToString()] != null)
                    {
                        dynamic bill = masterlist[index.ToString()];
                        string number = bill.number;
                        string title = bill.title;
                        Bill cf = new(number, title);
                        bills.Add(cf);
                        index++;
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception Hit------------");
                Console.WriteLine(exception);
            }

            return bills.ToArray();
        }
    }
}
