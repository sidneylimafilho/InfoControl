namespace InfoControl.Text.HtmlToRtf
{
    using System;

    public class nest_tables
    {
        public int cell;
        public int change_width;
        private int MAX_COLUMNS = 50;
        public string nestTblDescription;
        public bool table = false;
        public int[,] table_array;
        public bool table_border_visible;
        public int[,] table_colbg;
        public int[,] table_colspan;
        public int[,] table_images;
        public int table_level;
        public int[,] table_map;
        public table_params table_p;
        public int[,] table_rowspan;
        public int[,] table_symbols;
        public int[,] table_valign;
        public int[,] table_width;
        public int tblen;
        public int td;
        public string td_align;
        public int td_bg;
        public bool td_open;
        public int[] td_percent_width;
        public int td_up_columns;
        public int td_up_curcol;
        public int td_up_width;
        public int[] td_width;
        public byte[] tdAlignColgroup;
        public string tr_align;
        public int tr_bg;
        public int tr_cur;
        public bool tr_open;

        public nest_tables()
        {
            this.td_width = new int[this.MAX_COLUMNS];
            this.td_percent_width = new int[this.MAX_COLUMNS];
            this.tdAlignColgroup = new byte[this.MAX_COLUMNS];
            this.table_p = new table_params();
            this.nestTblDescription = "";
        }
    }
}

