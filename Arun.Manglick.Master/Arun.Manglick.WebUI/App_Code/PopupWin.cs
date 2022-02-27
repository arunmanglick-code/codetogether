using System;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace Arun.Manglick.UI
{

    /// <summary>
    /// Popup window control
    /// </summary>
    [DefaultProperty("Message"), ToolboxData("<{0}:PopupWin runat=server></{0}:PopupWin>")]
    [Designer(typeof(PopupWinDesigner))]
    public class PopupWin : System.Web.UI.WebControls.WebControl, IPostBackEventHandler
    {
        #region Private variables, constants and constructor

        private string msg, fullmsg, divDesign, cntStyle, sPopup, spopStyle,
          cntStyleN, cntStyleI, aStyle, aCommands, hdrStyle, title, closeHtml;

        private string popupBackground, popupBorderDark, popupBorderLight,
          cntBorderDark, cntBorderLight, cntBackground, gradientStart,
          gradientEnd, textColor, xButton, xButtonOver, sLink, sTarget;

        private PopupColorStyle clrStyle;
        private PopupDocking popDock;
        private PopupAction popAction;

        int xOffset, yOffset, iSpeed = 20;
        Size winSize;
        bool winScroll = true, bAutoShow = true, bDragDrop = false, bShowLink = true;

        int iHide, startTime;

        const string sScript = @"<script type=""text/javascript"">
      //<![CDATA[
        var [id]oldonloadHndlr=window.onload, [id]popupHgt, [id]actualHgt, [id]tmrId=-1, [id]resetTimer;
        var [id]titHgt, [id]cntDelta, [id]tmrHide=-1, [id]hideAfter=[hide], [id]hideAlpha, [id]hasFilters=[ie];
        var [id]nWin, [id]showBy=null, [id]dxTimer=-1, [id]popupBottom, [id]oldLeft;
        var [id]nText,[id]nMsg,[id]nTitle,[id]bChangeTexts=false;
        [id]espopup_winLoad();

        var [id]oldonscrollHndr=window.onscroll;
        window.onscroll=[id]espopup_winScroll;
        [id]nText=""[popup]"";

        function [id]espopup_winScroll()
        {
          if ([id]oldonscrollHndr!=null) [id]oldonscrollHndr();
          if ([id]tmrHide!=-1)
          {
            el=document.getElementById('[id]');
            el.style.display='none'; el.style.display='block';
          }
        }

        function [id]espopup_ShowPopup(show)
        {
          if ([id]dxTimer!=-1) { el.filters.blendTrans.stop(); }

          if (([id]tmrHide!=-1) && ((show!=null) && (show==[id]showBy)))
          {
            clearInterval([id]tmrHide);
            [id]tmrHide=setInterval([id]espopup_tmrHideTimer,[id]hideAfter);
            return;
          }
          if ([id]tmrId!=-1) return;
          [id]showBy=show;

          elCnt=document.getElementById('[id]_content')
          elTit=document.getElementById('[id]_header');
          el=document.getElementById('[id]');
          el.style.left=[id]oldLeft;
          el.style.top='';
          el.style.filter='';

          if ([id]tmrHide!=-1) clearInterval([id]tmrHide); [id]tmrHide=-1;

          document.getElementById('[id]_header').style.display='none';
          document.getElementById('[id]_content').style.display='none';

          if (navigator.userAgent.indexOf('Opera')!=-1)
            el.style.bottom=(document.body.scrollHeight*1-document.body.scrollTop*1
                            -document.body.offsetHeight*1+1*[id]popupBottom)+'px';
          
          if ([id]bChangeTexts)
          {
            [id]bChangeTexts=false;
            document.getElementById('[id]aCnt').innerHTML=[id]nMsg;
            document.getElementById('[id]titleEl').innerHTML=[id]nTitle;
          }

          [id]actualHgt=0; el.style.height=[id]actualHgt+'px';
          el.style.visibility='';
          if (![id]resetTimer) el.style.display='';
          [id]tmrId=setInterval([id]espopup_tmrTimer,([id]resetTimer?[stime]:20));
        }

        function [id]espopup_winLoad()
        {
          if ([id]oldonloadHndlr!=null) [id]oldonloadHndlr();

          elCnt=document.getElementById('[id]_content')
          elTit=document.getElementById('[id]_header');
          el=document.getElementById('[id]');
					[id]oldLeft=el.style.left;
          [id]popupBottom=el.style.bottom.substr(0,el.style.bottom.length-2);
          
          [id]titHgt=elTit.style.height.substr(0,elTit.style.height.length-2);
          [id]popupHgt=el.style.height;
          [id]popupHgt=[id]popupHgt.substr(0,[id]popupHgt.length-2); [id]actualHgt=0;
          [id]cntDelta=[id]popupHgt-(elCnt.style.height.substr(0,elCnt.style.height.length-2));

          if ([autoshow])
          {
            [id]resetTimer=true;
            [id]espopup_ShowPopup(null);
          }
        }

        function [id]espopup_tmrTimer()
        {
          el=document.getElementById('[id]');
          if ([id]resetTimer)
          {
            el.style.display='';
            clearInterval([id]tmrId); [id]resetTimer=false;
            [id]tmrId=setInterval([id]espopup_tmrTimer,[speed]);
          }
          [id]actualHgt+=5;
          if ([id]actualHgt>=[id]popupHgt)
          {
            [id]actualHgt=[id]popupHgt; clearInterval([id]tmrId); [id]tmrId=-1;
            document.getElementById('[id]_content').style.display='';
            if ([id]hideAfter!=-1) [id]tmrHide=setInterval([id]espopup_tmrHideTimer,[id]hideAfter);
          }
          if ([id]titHgt<[id]actualHgt-6)
            document.getElementById('[id]_header').style.display='';
          if (([id]actualHgt-[id]cntDelta)>0)
          {
            elCnt=document.getElementById('[id]_content')
            elCnt.style.display='';
            elCnt.style.height=([id]actualHgt-[id]cntDelta)+'px';
          }
          el.style.height=[id]actualHgt+'px';
        }
        
        function [id]espopup_tmrHideTimer()
        {
          clearInterval([id]tmrHide); [id]tmrHide=-1;
          el=document.getElementById('[id]');
          if ([id]hasFilters)
          {
            backCnt=document.getElementById('[id]_content').innerHTML;
            backTit=document.getElementById('[id]_header').innerHTML;
            document.getElementById('[id]_content').innerHTML='';
            document.getElementById('[id]_header').innerHTML='';
            el.style.filter='blendTrans(duration=1)';
            el.filters.blendTrans.apply();
            el.style.visibility='hidden';
            el.filters.blendTrans.play();
            document.getElementById('[id]_content').innerHTML=backCnt;
            document.getElementById('[id]_header').innerHTML=backTit;
            
            [id]dxTimer=setInterval([id]espopup_dxTimer,1000);
          }
          else el.style.visibility='hidden';
        }
        
        function [id]espopup_dxTimer()
        {
          clearInterval([id]dxTimer); [id]dxTimer=-1;
        }
     
        function [id]espopup_Close()
        {
          if ([id]tmrId==-1)
          {
            el=document.getElementById('[id]');
            el.style.filter='';
            el.style.display='none';
            if ([id]tmrHide!=-1) clearInterval([id]tmrHide); [id]tmrHide=-1;
            [sclose]
          }
        }
    
        function [id]espopup_ShowWindow()
        {
          [slink]
          if ([id]nWin!=null) [id]nWin.close();
          [id]nWin=window.open('','[id]nWin','[winstyle], '+
            'menubar=no, resizable=no, status=no, toolbar=no, location=no');
          [id]nWin.document.write([id]nText);
        }

        var [id]mousemoveBack,[id]mouseupBack;
        var [id]ofsX,[id]ofsY;
        function [id]espopup_DragDrop(e)
        {
          [id]mousemoveBack=document.body.onmousemove;
          [id]mouseupBack=document.body.onmouseup;
          ox=(e.offsetX==null)?e.layerX:e.offsetX;
          oy=(e.offsetY==null)?e.layerY:e.offsetY;
          [id]ofsX=ox; [id]ofsY=oy;

          document.body.onmousemove=[id]espopup_DragDropMove;
          document.body.onmouseup=[id]espopup_DragDropStop;
          if ([id]tmrHide!=-1) clearInterval([id]tmrHide);
        }

        function [id]espopup_DragDropMove(e)
        {
          el=document.getElementById('[id]');          
          if (e==null&&event!=null)
          {
            el.style.left=(event.clientX*1+document.body.scrollLeft-[id]ofsX)+'px';
            el.style.top=(event.clientY*1+document.body.scrollTop-[id]ofsY)+'px';
            event.cancelBubble=true;
          }
          else
          {
            el.style.left=(e.pageX*1-[id]ofsX)+'px';
            el.style.top=(e.pageY*1-[id]ofsY)+'px';
            e.cancelBubble=true;
          }
          if ((event.button&1)==0) [id]espopup_DragDropStop();
        }

        function [id]espopup_DragDropStop()
        {
          document.body.onmousemove=[id]mousemoveBack;
          document.body.onmouseup=[id]mouseupBack;
        }

      //]]>
      </script>";

        /// <summary>
        /// Initialize script, styles and colors
        /// </summary>
        public PopupWin()
        {
            // {0} = E5EDFA - popupBackground  
            // {1} = 455690 - popupBorderDark  
            // {2} = A6B4CF - popupBorderLight 
            // {3} = 728EB8 - cntBorderDark    
            // {4} = B9C9EF - cntBorderLight   
            // {5} = E9EFF9 - cntBackground    
            // {6} = E0E9F8 - gradientStart    
            // {7} = FFFFFF - gradientEnd      
            // {8} = 1F336B - textColor        
            // {9} = 6A87B2 - xButton          
            // {10}= 45638F - xButtonOver      

            divDesign = @"background:#{0}; border-right:1px solid #{1}; border-bottom:1px solid #{1};
                  border-left:1px solid #{2}; border-top:1px solid #{2}; position:absolute;
                  z-index:9999; ";

            cntStyle = @"border-left:1px solid #{3}; border-top:1px solid #{3};
                 border-bottom:1px solid #{4}; border-right:1px solid #{4};
                 background:#{5}; padding:2px; overflow:hidden; text-align:center;
                 filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=0,
                 StartColorStr='#FF{6}', EndColorStr='#FF{7}');";
            cntStyleI = @"position:absolute; left:2px; width:{0}px; top:20px; height:{1}px;";
            cntStyleN = @"position:absolute; left:2px; width:{0}px; top:20px; height:{1}px;";

            aStyle = @"font:12px arial,sans-serif; color:#{8}; text-decoration:none;";
            aCommands = @"onmouseover=""style.textDecoration='underline';""
                  onmouseout=""style.textDecoration='none';""
                  href=""[cmd]""";

            hdrStyle = @"position:absolute; left:2px; width:[wid]px; top:2px; height:14px;
                 filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=0,
                 StartColorStr='#FF{6}', EndColorStr='#FF{7}');";

            title = "Title here";
            msg = "Message to show in popup";
            fullmsg = "Text to display in new window.";

            closeHtml = @"<span style=""position:absolute; right:0px; top:0px; cursor:pointer; color:#{9}; font:bold 12px arial,sans-serif; 
                  position:absolute; right:3px;""
                  onclick=""[id]espopup_Close()""
                  onmousedown=""event.cancelBubble=true;""
                  onmouseover=""style.color='#{10}';""
                  onmouseout=""style.color='#{9}';"">X</span>";

            sPopup = "<head><title>{1}</title><style type=\\\"text/css\\\">{2}</style></head>" +
              "<body><h1>{1}</h1><p>{0}</p></body>";

            spopStyle = "body {" +
              "    background:#[gs]; padding:5px;" +
              "    filter:progid:DXImageTransform.Microsoft.Gradient(" +
              "     GradientType=0,StartColorStr='#FF[gs]', EndColorStr='#FF[ge]');" +
              "  }" +
              "  h1 {" +
              "    font:bold 16px arial,sans-serif; color:#[clr]; " +
              "    text-align:center; margin:0px;" +
              "  }" +
              "  p {" +
              "    font:14px arial,sans-serif; color:#[clr];" +
              "  }";

            ColorStyle = PopupColorStyle.Blue;
            xOffset = yOffset = 15; popDock = PopupDocking.BottomRight;
            iHide = 5000; winSize = new Size(400, 250);
            Width = new Unit("200px");
            Height = new Unit("100px");
            startTime = 1000;
            popAction = PopupAction.MessageWindow;
        }

        #endregion
        #region Properties


        /// <summary>
        /// Get or set type of action
        /// </summary>
        [Category("Action"), Description("Type of action (after link is clicked).")]
        public PopupAction ActionType
        {
            get { return popAction; }
            set { popAction = value; }
        }


        /// <summary>
        /// Get or set text to display in new window 
        /// (when user clicked on popup window)
        /// </summary>
        [Bindable(true), Category("Action"), Description("This text will be displayed in new window.")]
        public string Text
        {
            get { return fullmsg; }
            set { fullmsg = value.Replace("\\", "\\\\").Replace("\"", "\\\""); }
        }


        /// <summary>
        /// Get or set adress/javascript to open when link is clicked
        /// </summary>
        [Bindable(true), Category("Action"), Description("Adress/javascript to open when link is clicked.")]
        public string Link
        {
            get { return sLink; }
            set { sLink = value; }
        }


        /// <summary>
        /// Get or set target for specified link
        /// </summary>
        [Bindable(true), Category("Action"), Description("Target for specified link.")]
        public string LinkTarget
        {
            get { return sTarget; }
            set { sTarget = value; }
        }


        /// <summary>
        /// Get or set predefined color style
        /// </summary>
        [Category("Design"), Description("Predefined color style.")]
        public PopupColorStyle ColorStyle
        {
            get { return clrStyle; }
            set
            {
                clrStyle = value;
                switch (value)
                {
                    case PopupColorStyle.Blue:
                        textColor = "1F336B";
                        xButtonOver = popupBorderDark = "455690";
                        xButton = cntBorderDark = "728EB8";
                        popupBorderLight = cntBorderLight = "B9C9EF";
                        popupBackground = cntBackground = gradientStart = "E0E9F8";
                        gradientEnd = "FFFFFF";
                        break;

                    case PopupColorStyle.Violet:
                        textColor = "200040";
                        xButtonOver = popupBorderDark = "400080";
                        xButton = cntBorderDark = "7D5AA0";
                        popupBorderLight = cntBorderLight = "B9AAC8";
                        popupBackground = cntBackground = gradientStart = "D2C8DC";
                        gradientEnd = "FFFFFF";
                        break;

                    case PopupColorStyle.Green:
                        textColor = "004000";
                        xButtonOver = popupBorderDark = "008000";
                        xButton = cntBorderDark = "5AA05A";
                        popupBorderLight = cntBorderLight = "AAC8AA";
                        popupBackground = cntBackground = gradientStart = "C8DCC8";
                        gradientEnd = "FFFFFF";
                        break;

                    case PopupColorStyle.Red:
                        textColor = "400000";
                        xButtonOver = popupBorderDark = "800000";
                        xButton = cntBorderDark = "A05A5A";
                        popupBorderLight = cntBorderLight = "C8AAAA";
                        popupBackground = cntBackground = gradientStart = "DCC8C8";
                        gradientEnd = "FFFFFF";
                        break;
                }
            }
        }


        /// <summary>
        /// Get or set message to display in popup window
        /// </summary>
        [Bindable(true), Category("Texts"), Description("Message in popup.")]
        public string Message
        {
            get { return msg; }
            set { msg = value; }
        }


        /// <summary>
        /// Get or set title to display in popup window and new window
        /// </summary>
        [Bindable(true), Category("Texts"), Description("Title of popup element and new window.")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }


        /// <summary>
        /// Get or set ligh Gradient color
        /// </summary>
        [Bindable(true), Category("Design"), Description("Ligh Gradient color.")]
        public Color GradientLight
        {
            get { return ColorFromString(gradientEnd); }
            set { ColorStyle = PopupColorStyle.Custom; gradientEnd = ColorToString(value); }
        }


        /// <summary>
        /// Get or set dark gradient color (Background in Mozilla)
        /// </summary>
        [Bindable(true), Category("Design"), Description("Dark gradient color (Background in Mozilla).")]
        public Color GradientDark
        {
            get { return ColorFromString(gradientStart); }
            set { ColorStyle = PopupColorStyle.Custom; popupBackground = cntBackground = gradientStart = ColorToString(value); }
        }


        /// <summary>
        /// Get or set text color
        /// </summary>
        [Bindable(true), Category("Design"), Description("Text color.")]
        public Color TextColor
        {
            get { return ColorFromString(textColor); }
            set { ColorStyle = PopupColorStyle.Custom; textColor = ColorToString(value); }
        }


        /// <summary>
        /// Get or set light shadow color
        /// </summary>
        [Bindable(true), Category("Design"), Description("Light shadow color.")]
        public Color LightShadow
        {
            get { return ColorFromString(popupBorderLight); }
            set { ColorStyle = PopupColorStyle.Custom; popupBorderLight = cntBorderLight = ColorToString(value); }
        }


        /// <summary>
        /// Get or set dark shadow color
        /// </summary>
        [Bindable(true), Category("Design"), Description("Dark shadow color.")]
        public Color DarkShadow
        {
            get { return ColorFromString(xButtonOver); }
            set { ColorStyle = PopupColorStyle.Custom; xButtonOver = popupBorderDark = ColorToString(value); }
        }


        /// <summary>
        /// Get or set shadow color
        /// </summary>
        [Bindable(true), Category("Design"), Description("Shadow color.")]
        public Color Shadow
        {
            get { return ColorFromString(xButton); }
            set { ColorStyle = PopupColorStyle.Custom; xButton = cntBorderDark = ColorToString(value); }
        }


        /// <summary>
        /// Get or set popup window docking
        /// </summary>
        [Category("Layout"), Description("Popup window docking.")]
        public PopupDocking DockMode
        {
            get { return popDock; }
            set { popDock = value; }
        }


        /// <summary>
        /// Get or set x offset
        /// </summary>
        [Category("Layout"), Description("X offset (from left or right)."), DefaultValue(15)]
        public int OffsetX
        {
            get { return xOffset; }
            set { xOffset = value; }
        }


        /// <summary>
        /// Get or set y offset
        /// </summary>
        [Category("Layout"), Description("Y offset from bottom."), DefaultValue(15)]
        public int OffsetY
        {
            get { return yOffset; }
            set { yOffset = value; }
        }


        /// <summary>
        /// Get or set how long will window be displayed
        /// </summary>
        [Bindable(true), Category("Behavior"), DefaultValue(500),
        Description("How long will be window displayed in milliseconds(-1 for infinite).")]
        public int HideAfter
        {
            get { return iHide; }
            set { iHide = value; }
        }


        /// <summary>
        /// Get or set how fast will showing of popup be
        /// </summary>
        [Bindable(true), Category("Behavior"), DefaultValue(20),
        Description("Get or set how fast will showing of popup be.")]
        public int PopupSpeed
        {
            get { return iSpeed; }
            set { iSpeed = value; }
        }


        /// <summary>
        /// Get or set delay before displaying popup control
        /// </summary>
        [Bindable(true), Category("Behavior"), DefaultValue(1000),
        Description("Delay before displaying popup control.")]
        public int ShowAfter
        {
            get { return startTime; }
            set { startTime = value; }
        }


        /// <summary>
        /// Automaticly show popup when page loads (after ShowAfter miliseconds).
        /// </summary>
        [Category("Behavior"), DefaultValue(true)]
        [Description("Automaticly show popup when page loads (after ShowAfter miliseconds).")]
        public bool AutoShow
        {
            get { return bAutoShow; }
            set { bAutoShow = value; }
        }


        /// <summary>
        /// Get or set wether user can move popup element
        /// </summary>
        [Category("Behavior"), DefaultValue(true)]
        [Description("Allow user to move popup element.")]
        public bool DragDrop
        {
            get { return bDragDrop; }
            set { bDragDrop = value; }
        }


        /// <summary>
        /// Get or set window size
        /// </summary>
        [Bindable(true), Category("Window"), Description("Opened window size.")]
        public Size WindowSize
        {
            get { return winSize; }
            set { winSize = value; }
        }


        /// <summary>
        /// Get or set window scrollbars
        /// </summary>
        [Bindable(true), Category("Window"), Description("Display scrollbars in new window."),
            DefaultValue(true)]
        public bool WindowScroll
        {
            get { return winScroll; }
            set { winScroll = value; }
        }


        /// <summary>
        /// Generate link inside popup ?
        /// </summary>
        [Bindable(true), DefaultValue(true),
            Category("Action"), Description("Generate link inside popup and enable action ?")]
        public bool ShowLink
        {
            get { return bShowLink; }
            set { bShowLink = value; }
        }


        #endregion
        #region Methods

        /// <summary>
        /// Convert color to string (no color names !)
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>String (without # prefix)</returns>
        private string ColorToString(Color color)
        {
            return color.R.ToString("x").PadLeft(2, '0') +
              color.G.ToString("x").PadLeft(2, '0') + color.B.ToString("x").PadLeft(2, '0');
        }


        /// <summary>
        /// Convert string in RRGGBB format to Gdi+ color
        /// </summary>
        /// <param name="str">String to convert</param>
        /// <returns>Gdi+ color</returns>
        private Color ColorFromString(string str)
        {
            return ColorTranslator.FromHtml("#" + str);
        }


        /// <summary>
        /// Replace {0}..{10} with colors
        /// </summary>
        /// <param name="html">Html source</param>
        private string PutColors(string html)
        {
            return String.Format(html, popupBackground, popupBorderDark, popupBorderLight,
              cntBorderDark, cntBorderLight, cntBackground, gradientStart,
              gradientEnd, textColor, xButton, xButtonOver);
        }

        /// <summary>
        /// Render this control to the output parameter specified.
        /// </summary>
        /// <param name="output"> The HTML writer to write out to </param>
        protected override void Render(HtmlTextWriter output)
        {
            string br = Page.Request.Browser.Browser;
            string script = sScript, sps = spopStyle;

            string acmd = "";
            switch (popAction)
            {
                case PopupAction.MessageWindow:
                    acmd = aCommands.Replace("[cmd]", "javascript:[id]espopup_ShowWindow();");
                    script = script.Replace("[slink]", "");
                    script = script.Replace("[sclose]", "");
                    break;
                case PopupAction.RaiseEvents:
                    acmd = aCommands.Replace("[cmd]", "javascript:[id]espopup_ShowWindow();");
                    string scriptClick = (LinkClicked == null) ? "" :
                      (Page.GetPostBackClientEvent(this, "C") + "; return;");
                    string scriptClose = (PopupClosed == null) ? "" :
                      (Page.GetPostBackClientEvent(this, "X") + "; return;");
                    script = script.Replace("[slink]", scriptClick);
                    script = script.Replace("[sclose]", scriptClose);
                    break;
                case PopupAction.OpenLink:
                    acmd = aCommands.Replace("[cmd]", sLink);
                    script = script.Replace("[slink]", "");
                    script = script.Replace("[sclose]", "");
                    if (sTarget != "") acmd += " target=\"" + sTarget + "\"";
                    break;
            }

            sps = sps.Replace("[gs]", gradientStart);
            sps = sps.Replace("[ge]", gradientEnd);
            sps = sps.Replace("[clr]", textColor);

            script = script.Replace("[winstyle]", String.Format(
              "width={0},height={1},scrollbars={2}", winSize.Width,
              winSize.Height, (winScroll ? "yes" : "no")));
            script = script.Replace("[hide]", iHide.ToString());
            script = script.Replace("[stime]", startTime.ToString());
            script = script.Replace("[id]", ClientID);
            script = script.Replace("[ie]", (br == "IE" ? "true" : "false"));
            script = script.Replace("[popup]", String.Format(sPopup, fullmsg, title, sps));
            script = script.Replace("[autoshow]", bAutoShow.ToString().ToLower());
            script = script.Replace("[speed]", iSpeed.ToString());

            string divPos = String.Format("width:{0}; height:{1}; ", Width, Height);
            string cntsI = String.Format(cntStyleI, Width.Value - 6, Height.Value - 24);
            string cntsN = String.Format(cntStyleN, Width.Value - 10, Height.Value - 28);

            string sDragDrop = "";
            if (bDragDrop)
                sDragDrop = " onmousedown=\"return " + ClientID + "espopup_DragDrop(event);\" ";
            if (popDock == PopupDocking.BottomLeft) divPos += "left:"; else divPos += "right:";
            divPos += string.Format("{0}px; bottom:{1}px;", xOffset, yOffset);
            output.Write(String.Format("<div id=\"{0}\" " +
              "style=\"display:none; {1} {2}\" onselectstart=\"return false;\" {4}>" +
              "<div id=\"{3}\" style=\"cursor:default; display:none; {5}\">{6}</div>" +
              "<div id=\"{7}\" onmousedown=\"event.cancelBubble=true;\" style=\"display:none; {8}\">" +
              ((bShowLink == true) ?
                "<a style=\"{9}\" {10} id=\"{11}\">{12}</a></div></div>" :
                "<span style=\"{9}\" id=\"{11}\">{12}</span></div></div>"),
              ClientID, PutColors(divDesign), divPos, ClientID + "_header", sDragDrop, PutColors(hdrStyle).
              Replace("[wid]", (Width.Value - 6).ToString()) + PutColors(aStyle),
              "<span id=\"" + ClientID + "titleEl\">" + title + "</span>" + PutColors(closeHtml).Replace("[id]", ClientID), ClientID + "_content",
              PutColors(cntStyle) + ((br != "Netscape" && br != "Mozilla") ? cntsI : cntsN), PutColors(aStyle),
              acmd.Replace("[id]", ClientID), ClientID + "aCnt", msg) + script);
        }


        /// <summary>
        /// Generate html code to show in new window
        /// </summary>
        /// <param name="title">Title</param>
        /// <param name="text">Text</param>
        /// <returns>Html code for new window</returns>
        internal string GetWinText(string title, string text)
        {
            string sps = spopStyle;
            sps = sps.Replace("[gs]", gradientStart);
            sps = sps.Replace("[ge]", gradientEnd);
            sps = sps.Replace("[clr]", textColor);

            return String.Format(sPopup, text.Replace("\\", "\\\\").Replace("\"", "\\\""), title, sps);
        }

        /// <summary>
        /// Returns html code for designer
        /// </summary>
        internal string GetDesignCode()
        {
            string divPos = String.Format("width:{0}; height:{1}; ",
              Width, Height);
            string cntsI = String.Format(cntStyleI, Width.Value - 6, Height.Value - 24);

            return String.Format("<div id=\"{0}\" " +
              "style=\"{1} {2}; left:0px; top:0px; \">" +
              "<div id=\"{3}\" style=\"{4}\">{5}</div>" +
              "<div id=\"{6}\" style=\"{7}\">" +
              "<a style=\"{8}\" {9}>{10}</a></div></div>",
              ClientID, PutColors(divDesign), divPos, ClientID + "_header", PutColors(hdrStyle).
              Replace("[wid]", (Width.Value - 6).ToString()) +
              PutColors(aStyle), title + PutColors(closeHtml).Replace("[id]", ClientID), ClientID + "_content",
              PutColors(cntStyle) + cntsI, PutColors(aStyle), aCommands.Replace("[id]", ClientID), msg);
        }

        #endregion
        #region Events and Event handlers

        /// <summary>
        /// Raise event
        /// </summary>
        /// <param name="eventArgument">Closed or clicked on link ?</param>
        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument == "C") LinkClicked(this, EventArgs.Empty);
            if (eventArgument == "X") PopupClosed(this, EventArgs.Empty);
        }


        /// <summary>
        /// User clicked on link on popup box
        /// </summary>
        [Category("Popup"), Description("User clicked on link on popup box.")]
        public event EventHandler LinkClicked;


        /// <summary>
        /// User clicked on 'X' on popup box.
        /// </summary>
        [Category("Popup"), Description("User clicked on 'X' on popup box.")]
        public event EventHandler PopupClosed;

        #endregion
    }

    /// <summary>
    /// Class for displaying PopupWin in designer
    /// </summary>
    public class PopupWinDesigner : ControlDesigner
    {
        #region Overriden methods

        /// <summary>
        /// Returns HTML code to show in designer
        /// </summary>
        public override string GetDesignTimeHtml()
        {
            try
            {
                return ((PopupWin)Component).GetDesignCode();
            }
            catch (Exception er)
            {
                return GetErrorDesignTimeHtml(er);
            }
        }

        #endregion
    }


    /// <summary>
    /// Predefined color style
    /// </summary>
    public enum PopupColorStyle
    {
        #region Members

        /// <summary> Blue style (default) </summary>
        Blue,
        /// <summary> Red style </summary>
        Red,
        /// <summary> Green style </summary>
        Green,
        /// <summary> Violet style </summary>
        Violet,
        /// <summary> Custom style - defined by colors </summary>
        Custom

        #endregion
    }


    /// <summary>
    /// Popup window docking
    /// </summary>
    public enum PopupDocking
    {
        #region Members

        /// <summary> Control is docked to left and bottom </summary>
        BottomLeft,
        /// <summary> Control is docked to right and bottom </summary>
        BottomRight

        #endregion
    }


    /// <summary>
    /// Action to do after user clicked on link
    /// </summary>
    public enum PopupAction
    {
        #region Members

        /// <summary> Raise server events </summary>
        RaiseEvents,
        /// <summary> Open new browser window with text message </summary>
        MessageWindow,
        /// <summary> Open link or javascript script </summary>
        OpenLink

        #endregion
    }

}
