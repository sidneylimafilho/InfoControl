
(function($) {
    $.cache = $.cache || {};

    $.extend($.fn, {
        render: function(data) {
            if (this.size() == 0) throw new Error("Zero element selected!");

            var template = this.html();

            // Convert the template into pure JavaScript
            var script = "var p=[], print=function(){ p.push.apply(p,arguments); }; " +
            "dataItem = dataItem || [];" +
            "with(dataItem){" +
            "   p.push('" + template // Introduce the data as local variables using with(){}
                    .split("&lt;").join("<")
                    .split("%3C").join("<")
                    .split("&gt;").join(">")
                    .split("%3E").join(">")
                    .split("&quot;").join('"')
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
                var fn = $.cache[template] || ($.cache[template] = new Function("dataItem", script));

                var html = "";
                if ($.isArray(data)) {
                    for (var i = 0; i < data.length; i++) html += fn(data[i]);
                } else {
                    html += fn(data);
                }

            } catch (err) {
                $("<pre class='error' />").text("The template is mal-formed, because " + err + "\n\n" + script).appendTo("body");
            }


            // Provide some basic currying to the user
            //var html = fn(data);
            //this.html(html);
            return data ? html : fn;


        }
    });


})(jQuery);