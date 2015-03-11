using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk
{

    /// <summary>
    /// 登录时微信公众平台网页返回的信息
    /// </summary>
    public class WxLoginRetInfo
    {
        string errMsg;

        public string ErrMsg
        {
            get
            {

                if (string.IsNullOrEmpty(errMsg) && this.Base_resp != null)
                {
                    errMsg = this.Base_resp.Err_msg;
                }

                return errMsg;
            }

            set { errMsg = value; }
        }
        public int ShowVerifyCode { get; set; }



        public string Redirect_url
        {
            get;
            set;
        }

        public BaseResp Base_resp
        {
            get;
            set;
        }

        public bool IsError
        {
            get;
            set;
        }
    }
}
