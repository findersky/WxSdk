using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pioneer.WxSdk.Api
{
    [Apipath("创建自定义菜单", "/api/MenuCreate")]
    public class MenuCreateRequest : Request<Response>
    {
        public MenuData MenuData
        {
            get;
            set;
        }

        public override string Url
        {
            get { return string.Format(" https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", this.AccessToken); }
        }


        protected override Tuple<bool, string> DoValidate()
        {
            if (this.MenuData == null)
            {
                return Tuple.Create(false, "MenuData is null");
            }

            if (this.MenuData.button.Count == 0 || this.MenuData.button.Count > 3)
            {
                return Tuple.Create(false, "主菜单必须是1-3个");
            }

            foreach (var x in this.MenuData.button)
            {
                if (x.sub_button.Count > 5)
                {
                    return Tuple.Create(false, "二级菜单数量不能多于5个 " + x.name);
                }
            }

            return base.DoValidate();
        }

        protected override Response DoResponse()
        {
            string json = this.Serialize(this.MenuData);
            this.logger.Trace(json);
            return this.HttpPost(json);
        }
    }

    public class MenuData
    {
        public MenuData()
        {
            this.button = new List<TopButton>();
        }
        public List<TopButton> button
        {
            get;
            set;
        }
    }

    public class ButtonInfo
    {
        public string key
        {
            get;
            set;
        }
        public string name
        {
            get;
            set;
        }
        public string type
        {
            get;
            set;
        }
        public string url
        {
            get;
            set;
        }

        public virtual void Validate()
        {
            if (this.type == ButtonType.Click)
            {
                if (string.IsNullOrEmpty(this.key))
                {
                    throw new WxException("WM01", "菜单类型为Click Key不能为空");
                }
            }
            else if (this.type == ButtonType.View)
            {
                if (string.IsNullOrEmpty(this.url))
                {
                    throw new WxException("WM02", "菜单类型为View Url不能为空");
                }
            }
            else
            {
                throw new WxException("WM03", "菜单类型不能为空");
            }
        }
    }

    public class TopButton : ButtonInfo
    {
        public TopButton()
        {
            sub_button = new List<SubButton>();
        }

        public override void Validate()
        {
            if (this.sub_button.Count > 0)
                base.Validate();
        }


        public List<SubButton> sub_button
        {
            get;
            set;
        }
    }

    public class ButtonType
    {
        public const string View = "view";
        public const string Click = "click";
        /// <summary>
        /// 扫码推事件
        /// 扫码推事件
        ///用户点击按钮后，微信客户端将调起扫一扫工具，完成扫码操作后显示扫描结果（如果是URL，将进入URL），且会将扫码的结果传给开发者，开发者可以下发消息。

        /// </summary>
        public const string scancode_push = "scancode_push";

        /// <summary>
        /// 扫码推事件且弹出“消息接收中”提示框
        //用户点击按钮后，微信客户端将调起扫一扫工具，完成扫码操作后，将扫码的结果传给开发者，同时收起扫一扫工具，然后弹出“消息接收中”提示框，随后可能会收到开发者下发的消息。

        /// </summary>
        public const string scancode_waitmsg = "scancode_waitmsg";

        /// <summary>
        /// 弹出系统拍照发图
        //用户点击按钮后，微信客户端将调起系统相机，完成拍照操作后，会将拍摄的相片发送给开发者，并推送事件给开发者，同时收起系统相机，随后可能会收到开发者下发的消息。
        /// </summary>
        public const string pic_sysphoto = "pic_sysphoto";

        /// <summary>
        /// 弹出拍照或者相册发图
        ///用户点击按钮后，微信客户端将弹出选择器供用户选择“拍照”或者“从手机相册选择”。用户选择后即走其他两种流程。
        /// </summary>
        public const string pic_photo_or_album = "pic_photo_or_album";

        /// <summary>
        /// 弹出微信相册发图器
        ///用户点击按钮后，微信客户端将调起微信相册，完成选择操作后，将选择的相片发送给开发者的服务器，并推送事件给开发者，同时收起相册，随后可能会收到开发者下发的消息。
        /// </summary>
        public const string pic_weixin = "pic_weixin";
        /// <summary>
        /// 弹出地理位置选择器
        /// 用户点击按钮后，微信客户端将调起地理位置选择工具，完成选择操作后，将选择的地理位置发送给开发者的服务器，同时收起位置选择工具，随后可能会收到开发者下发的消息。
        /// </summary>
        public const string location_select = "location_select";

    }

    public class SubButton : ButtonInfo
    {
    }
}
