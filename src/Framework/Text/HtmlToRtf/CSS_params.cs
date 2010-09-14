namespace InfoControl.Text.HtmlToRtf
{
    using System;
    using System.Collections;

    public class CSS_params
    {
        public ByteClass ablaze = new ByteClass(0x20);
        public bool body_tag;
        public int body_tag_index;
        public int bold;
        public ByteClass buf = new ByteClass();
        public int buf_pos;
        public int buf_size;
        public int color_list_num;
        public ArrayList CSS_style = new ArrayList();
        public CSS_styles CSS_style_default = new CSS_styles();
        public ArrayList CSS_style_str_stack = new ArrayList();
        public int div;
        public bool em_tag;
        public int em_tag_open;
        public ByteClass file_name = new ByteClass();
        public int font_list_num;
        public bool font_tag;
        public int font_tag_open;
        public bool found;
        public CSS_tag_type hNumber = CSS_tag_type.UNKNOWN_CSS;
        public int italic;
        public ArrayList names = new ArrayList();
        public bool p_tag;
        public bool p_tag_only;
        public int p_tag_only_index;
        public int p_tag_only_open;
        public int p_tag_open;
        public position pos = new position();
        public int span;
        public ArrayList stack = new ArrayList();
        public int stack_num;
        public ArrayList stack_tag = new ArrayList();
        public int stack_tag_num;
        public ByteClass style_name = new ByteClass(0x40);
        public int style_str_stack_num;
        public int styles;
        public bool td_tag_only;
        public int td_tag_only_index;
        public int td_tag_only_open;
        public ByteClass tmp = new ByteClass();
        public int underline;
        public bool use;
        public int vertical_align;

        public void CSS_reset(CSS_params CSS_param)
        {
            CSS_param.names.Add("font-family");
            CSS_param.names.Add("color");
            CSS_param.names.Add("font-weight");
            CSS_param.names.Add("font-size");
            CSS_param.names.Add("text-align");
            CSS_param.names.Add("font-style");
            CSS_param.names.Add("margin-top");
            CSS_param.names.Add("background-color");
            CSS_param.names.Add("font");
            CSS_param.names.Add("top");
            CSS_param.names.Add("left");
            CSS_param.names.Add("width");
            CSS_param.names.Add("text-decoration");
            CSS_param.names.Add("vertical-align");
            CSS_param.names.Add("page-break-before");
            CSS_param.names.Add("page-break-after");
            CSS_param.names.Add("list-style-type");
            CSS_param.names.Add("margin-bottom");
        }
    }
}

