using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    /// <summary>
    /// 多客服聊天记录
    /// 
    /// 目前这个接口从wiki文档上看有问题  分页查询  不知道总数 没法查
    /// 暂时不实现
    /// 
    /// </summary>
    [Apipath("多客服聊天记录", "/api/ChartMessage")]
    public class ChartMessageRequest : Request<ChartMessageResponse>
    {
        public override string Url
        {
            get { throw new NotImplementedException(); }
        }

        protected override ChartMessageResponse DoResponse()
        {
            throw new NotImplementedException();
        }
    }
}
