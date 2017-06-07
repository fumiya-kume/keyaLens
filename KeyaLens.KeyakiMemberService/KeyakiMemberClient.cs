using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KeyaLens.KeyakiMemberService
{
    public class KeyakiMemberClient : IKeyakiMemberClient
    {
        public ReactiveCollection<KeyakiMemberInfo> MemberCollection { get; set; } = new ReactiveCollection<KeyakiMemberInfo>();

        public KeyakiMemberClient()
        {
            GetMemberNameAsync();
        }

        public async void GetMemberNameAsync()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetAsync("http://www.keyakizaka46.com/s/k46o/search/artist?ima=0000");
                var htmlText = await result.Content.ReadAsStringAsync();

                var AgilityPack = new HtmlAgilityPack.HtmlDocument();
                AgilityPack.LoadHtml(htmlText);

                var MemberNodes = AgilityPack.DocumentNode
                    .Descendants().Where(node => node.GetAttributeValue("class", "") == "box-member")
                    .Select(Mothernode => Mothernode.Descendants().Where(node => node.Name == "ul").First()
                    .Descendants().Where(node => node.Name == "li"))
                    .Aggregate((node, NextNode) => node.Concat(NextNode));

                var memberList = MemberNodes.Select(node =>
                 {
                     var name = node.Descendants("p").Where(ChildNode => ChildNode.GetAttributeValue("class", "") == "name").First().InnerText.Replace("\n", "").Replace(" ", "");

                     var MemberID = node.GetAttributeValue("data-member", "");
                     var MemberPageURL = $"http://www.keyakizaka46.com/s/k46o/artist/{MemberID}";

                     var ProfileImageURL = node.Descendants("img").First().GetAttributeValue("src", "");

                     return new KeyakiMemberInfo() { Name = name, memberPageURL = MemberPageURL, ProfileImageURL = ProfileImageURL };
                 })
               .Distinct()
               .ToList();

                MemberCollection.AddRangeOnScheduler(memberList);
            }
        }
    }
}
