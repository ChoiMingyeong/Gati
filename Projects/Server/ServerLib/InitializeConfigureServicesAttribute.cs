using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLib
{
    // Api 클래스 라이브러리에서 각종 initalize 세팅할때 이 커스텀 어트리뷰트 사용
    // Program.cs  ReflectionUtil.ExecuteStaticMethod<InitializeAfterStartServerAttribute>(host.Services);
    // 해당 함수로 InitializeAfterStartServerAttribute 가 적용된 함수들을 가져와서 호출
    // 그냥 Attribute로 사용하면 시스템 내에 Attrubute를 사용하는 쓸모없는 애들까지 가져와서 처리하려고 하기 때문에
    // 커스텀 어트리뷰트를 사용하는거.
    public class InitializeConfigureServicesAttribute : Attribute
    {

    }
}
