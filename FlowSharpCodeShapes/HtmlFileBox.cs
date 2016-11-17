﻿/* 
* Copyright (c) Marc Clifton
* The Code Project Open License (CPOL) 1.02
* http://www.codeproject.com/info/cpol10.aspx
*/

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;

using FlowSharpLib;
using FlowSharpCodeShapeInterfaces;

namespace FlowSharpCodeShapes
{
    public class HtmlFileBox : Box, IFileBox
    {
        public string Filename { get; set; }

        public HtmlFileBox(Canvas canvas) : base(canvas)
        {
            Text = ".html";
            TextFont.Dispose();
            TextFont = new Font(FontFamily.GenericSansSerif, 6);
            TextAlign = ContentAlignment.TopCenter;
        }

        public void UpdateCodeBehind()
        {
            string data = "";

            if (File.Exists(Filename))
            {
                data = File.ReadAllText(Filename);
            }

            Json["Code"] = data;
        }

        public override GraphicElement CloneDefault(Canvas canvas)
        {
            GraphicElement el = base.CloneDefault(canvas);
            el.TextFont.Dispose();
            el.TextFont = new Font(FontFamily.GenericSansSerif, 10);
            el.TextAlign = ContentAlignment.MiddleCenter;

            return el;
        }

        public override GraphicElement CloneDefault(Canvas canvas, Point offset)
        {
            GraphicElement el = base.CloneDefault(canvas, offset);
            el.TextFont.Dispose();
            el.TextFont = new Font(FontFamily.GenericSansSerif, 10);
            el.TextAlign = ContentAlignment.MiddleCenter;

            return el;
        }

        public override ElementProperties CreateProperties()
        {
            return new HtmlFileBoxProperties(this);
        }

        public override void Serialize(ElementPropertyBag epb, IEnumerable<GraphicElement> elementsBeingSerialized)
        {
            // TODO: Use JSON dictionary instead.
            epb.ExtraData = Filename;
            base.Serialize(epb, elementsBeingSerialized);

            // Also update the backing file.
            if (Json.ContainsKey("Code"))
            {
                File.WriteAllText(Filename, Json["Code"]);
            }
        }

        public override void Deserialize(ElementPropertyBag epb)
        {
            // TODO: Use JSON dictionary instead.
            Filename = epb.ExtraData;
            base.Deserialize(epb);
        }
    }

    public class HtmlFileBoxProperties : ElementProperties
    {
        [Category("Assembly")]
        public string Filename { get; set; }

        public HtmlFileBoxProperties(HtmlFileBox el) : base(el)
        {
            Filename = el.Filename;
        }

        public override void Update(GraphicElement el, string label)
        {
            HtmlFileBox box = (HtmlFileBox)el;

            (label == "Filename").If(() =>
            {
                box.Filename = Filename;
                box.UpdateCodeBehind();
                box.Text = string.IsNullOrEmpty(Filename) ? "?.html" : (Path.GetFileName(Filename));
            });

            base.Update(el, label);
        }
    }
}
