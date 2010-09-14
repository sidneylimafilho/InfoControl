namespace InfoControl.Text.HtmlToRtf
{
    using System;

    public class CSS_styles
    {
        public ByteClass ablaze = new ByteClass(20);
        public byte background_color = 0;
        public byte color = 0;
        public CSS_tag_type css_tag_type = CSS_tag_type.UNKNOWN_CSS;
        public byte font = 0;
        public byte font_family = 0;
        public byte font_size = 0;
        public byte font_style = 0;
        public byte font_weight = 0;
        public int left = 0;
        public int list_style_type = 0;
        public int margin_bottom = 0;
        public int margin_top = 0;
        public ByteClass name = new ByteClass();
        public int page_break_after = 0;
        public int page_break_before = 0;
        public byte text_align = 0;
        public int text_decoration = 0;
        public int top = 0;
        public int vertical_align = 0;
        public int width = 0;
        public bool width_in_percent = false;

        public void CopyCSS_styles(CSS_styles dest)
        {
            dest.name.Clear();
            for (int i = 0; i < 20; i++)
            {
                dest.ablaze.Add(this.ablaze.b[i]);
            }
            dest.css_tag_type = CSS_tag_type.UNKNOWN_CSS;
            dest.font_family = this.font_family;
            dest.color = this.color;
            dest.font_weight = this.font_weight;
            dest.font_size = this.font_size;
            dest.text_align = this.text_align;
            dest.font_style = this.font_style;
            dest.margin_top = this.margin_top;
            dest.background_color = this.background_color;
            dest.font = this.font;
            dest.top = this.top;
            dest.left = this.left;
            dest.width = this.width;
            dest.text_decoration = this.text_decoration;
            dest.vertical_align = this.vertical_align;
            dest.page_break_before = this.page_break_before;
            dest.page_break_after = this.page_break_after;
            dest.list_style_type = this.list_style_type;
            dest.margin_bottom = this.margin_bottom;
            dest.width_in_percent = this.width_in_percent;
        }
    }
}

