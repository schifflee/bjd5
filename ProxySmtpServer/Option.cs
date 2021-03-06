﻿using System.Collections.Generic;
using Bjd;
using Bjd.ctrl;
using Bjd.net;
using Bjd.option;

namespace ProxySmtpServer {
    class Option : OneOption {

        public override char Mnemonic { get { return 'S'; } }

        public Option(Kernel kernel, string path, string nameTag)
            : base(kernel.IsJp(), path, nameTag) {

                var key = "useServer";
            Add(new OneVal(key, false, Crlf.Nextline, new CtrlCheckBox(Lang.Value(key))));

            var pageList = new List<OnePage>();
            key = "Basic";
            pageList.Add(Page1(key, Lang.Value(key), kernel));
            key = "Expansion";
            pageList.Add(Page2(key, Lang.Value(key), kernel));
            pageList.Add(PageAcl());
            Add(new OneVal("tab", null, Crlf.Nextline, new CtrlTabPage("tabPage", pageList)));

            Read(kernel.IniDb); //　レジストリからの読み込み
        }

        private OnePage Page1(string name, string title, Kernel kernel) {
            var onePage = new OnePage(name, title);

            onePage.Add(CreateServerOption(ProtocolKind.Tcp, 8025, 60, 10)); //サーバ基本設定
            var key = "targetPort";
            onePage.Add(new OneVal(key, 25, Crlf.Nextline, new CtrlInt(Lang.Value(key), 5)));
            key = "targetServer";
            onePage.Add(new OneVal(key, "", Crlf.Nextline, new CtrlTextBox(Lang.Value(key), 50)));
            key = "idleTime";
            onePage.Add(new OneVal(key, 1, Crlf.Nextline, new CtrlInt(Lang.Value(key), 5)));

            return onePage;
        }
        private OnePage Page2(string name, string title, Kernel kernel){
            var onePage = new OnePage(name, title);
            var l = new ListVal();
            var key = "mail";
            l.Add(new OneVal(key, "", Crlf.Nextline,new CtrlTextBox(Lang.Value(key), 30)));
            key = "server";
            l.Add(new OneVal(key, "", Crlf.Contonie, new CtrlTextBox(Lang.Value(key), 30)));
            key = "dstPort";
            l.Add(new OneVal(key, 25, Crlf.Nextline, new CtrlInt(Lang.Value(key), 5)));
            key = "address";
            l.Add(new OneVal(key, "", Crlf.Nextline,new CtrlTextBox(Lang.Value(key), 30)));
            key = "specialUser";
            onePage.Add(new OneVal(key, null, Crlf.Nextline, new CtrlDat(Lang.Value(key), l, 360, Lang.LangKind)));

            return onePage;
        }

        //コントロールの変化
        override public void OnChange() {
            var b = (bool)GetCtrl("useServer").Read();
            GetCtrl("tab").SetEnable(b);

        }
    }
}



