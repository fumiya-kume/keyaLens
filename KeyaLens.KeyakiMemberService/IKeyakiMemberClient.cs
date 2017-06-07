using Reactive.Bindings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeyaLens.KeyakiMemberService
{
    public interface IKeyakiMemberClient
    {
        ReactiveCollection<KeyakiMemberInfo> MemberCollection { get; set; }
        void GetMemberNameAsync();
    }
}