
(function($) {
    $.cache = $.cache || {};

    $.extend($.fn, {
        render: function(data) {
            if (this.size() == 0) throw new Error("Zero element selected!");

            // Convert the template into pure JavaScript
            var script = "var p=[], print=function(){ p.push.apply(p,arguments); }; " +
            "dataItem = dataItem || [];" +
            "with(dataItem){" +
            "   p.push('" + // Introduce the data as local variables using with(){}
                this.html()
                    .split("&lt;").join("<")
                    .split("%3C").join("<")
                    .split("&gt;").join(">")
                    .split("%3E").join(">")
                    .replace("<!--", " ")
                    .replace("-->", " ")
                    .replace(/'/g, "\\'")
                    .replace(/[\r\t\n]/g, " ")

                   .replace(/'(?=[^$]*$>)/g, "\t")

                   .split("\t").join("'")
                   .replace(/<\$=(.+?)\$>/g, "',$1,'")

                   .split("<$").join("');")
                   .split("$>").join("p.push('")

          + "');}return p.join('');";

            try {
                // Generate a reusable function that will serve as a template
                // generator (and which will be cached).
                //var fn = !/\W/.test(this.id) ? cache[this.id] = (cache[this.id] || $(this).template()) :
                var fn = $.cache[this.selector] || ($.cache[this.selector] = new Function("dataItem", script));

                var html = "";
                if ($.isArray(data)) {
                    for (var i = 0; i < data.length; i++) html += fn(data[i]);
                } else {
                    html += fn(data);
                }

            } catch (err) {
                alert("The template is mal-formed!\n\n" + err + "\n\n" + script);
            }


            // Provide some basic currying to the user
            //var html = fn(data);
            //this.html(html);
            return data ? html : fn;


        }
    });


})(jQuery);