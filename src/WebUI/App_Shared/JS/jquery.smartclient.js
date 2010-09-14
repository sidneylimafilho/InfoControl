/// <reference path="jquery.debug.js" />
/// <summary>
/// SmartClient 
/// </summary> 
(function($) {

    /***************************************************************************************************
    Extend jQuery
    ***************************************************************************************************/
    $.fn.extend({
        hasControl: function(bool) {
            if (bool)
                this[0].control = bool;

            return this[0].control != undefined;
        },
        attrUp: function(name) {
            if (this.length > 0)
                return this.attr(name) || this.parent().attrUp(name);
            return undefined;
        },
        outerHtml: function(html) {
            return html ?
                this.before(html).remove() :
                jQuery("<p>").append(this.eq(0).clone()).html();
        },
        attachHtmlInTarget: function(html, t, m) {
            // Get target tag
            var target = t || this.attrUp("target") || this;

            if ($(target).size() == 0)
                throw TargetMissingException(this);

            var mode = m || this.attrUp("mode");

            if (mode === "after") {
                $(target).after(html);
                $(target).parent().initializeControls();
            } else {
                $(target).html(html).initializeControls();
            }

        },
        ajaxIframe: function(url, ctrl, onsucess) {
            var iframe = $("#ajaxIFrame");

            if ($("#ajaxIFrame").size() == 0) {
                iframe = $(document.body).prepend("<IFRAME id=\"ajaxIFrame\">").find("#ajaxIFrame");
            }

            iframe.unbind("load").bind("load", function() {
                var html = $("body", iframe.contents()).html();
                ctrl.attachHtmlInTarget(html);
                if (onsucess)
                    onsucess(html, "notmodified", null);
                iframe.unbind("load");
            });
            iframe.hide().attr("src", url);

        },
        warnCapsLockIsOn: function(callback) {
            this.each(function(i, elem) {

                var myKeyCode = e.keyCode || e.which;

                if (e.shiftKey) {
                    // Lower case letters are seen while depressing the Shift key, therefore Caps Lock is on
                    if ((myKeyCode >= 97 && myKeyCode <= 122))
                        callback();
                }
                else {
                    // Upper case letters are seen without depressing the Shift key, therefore Caps Lock is on
                    if ((myKeyCode >= 65 && myKeyCode <= 90))
                        callback();
                }

            });
        },
        getAddress: function() {
            // Prepare the url
            var url = this.attrUp("href") || this.attrUp("source");

            if (this.attr("action")) {
                url = this.attrUp("controller");
                if (this.attrUp("action"))
                    url += this.attrUp("action");
            }
            return url;
        },
        dataBind: function(options) {
            for (var i = 0, l = this.length; i < l; i++) {
                options = options || {};
                var self = $(this[i]);

                if (self.attr("onbinding")) eval(self.attr("onbinding"));
                if (options.onbinding) options.onbinding();

                var formData = {};

                var form = self.closest("[asForm]") || self.closest("FORM");

                // Get All html form controls
                var fields = form.find(":text, select, textarea, :checked, :password, [type=hidden]")
                        .map(function(i, elem) { return '"' + elem.name + '": "' + elem.value + '"'; });

                options.params = options.params || {};
                if (self.attrUp("params") && self.attrUp("params") != "") {
                    $.extend(options.params, eval("(" + self.attrUp("params") + ")"));
                }

                // Makes the comparison "options.data || {}" because options.data can be filled, when trigger 
                // is fired otherwise prepares the data Request Payload
                if (!options.data)
                    options.data = ('{"Params":<0>, "FormData":{<1>}}')
                                        .replace("<0>", $.toJSON(options.params))
                                        .replace("<1>", fields.get().join(","));

                // Allow fire DataBinding in controls that has TRIGGER atribute
                if (self.attrUp("trigger")) {
                    self.closest("[trigger]").find("[param]").each(function() {
                        options.params[self.attr("param")] = self.val();
                    });

                    $(self.attrUp("trigger")).dataBind(options);
                    return;
                }

                // save the control that is fire dataBind, because closure "sucess" dont access
                var ctrl = self;

                var type = self.attrUp("method") || "POST";

                // Prepare the url
                var url = self.getAddress();

                // Only fires ajax if there are url
                if (url)
                    $.ajax({
                        type: type,
                        url: url,
                        data: options.data,
                        contentType: "application/json",
                        ifModified: true,
                        success: function(result, status, request) {


                            // If Not Modified then get cached content file by iframe
                            if (request.status == 304) {
                                ctrl.ajaxIframe(url, ctrl, options.onsucess);
                            } else {
                                // If Http Status 200 then OK, process JSON because data should be transform on html
                                var html = result;
                                var target = ctrl.attrUp("target");

                                if (request.responseText != "") {
                                    if (request.getResponseHeader("Content-type").indexOf("json") > -1) {

                                        if (result.Errors) {
                                            if ($.isArray(result.Errors)) {
                                                for (var item in result.Errors)
                                                    alert(item);
                                            }
                                        }

                                        if (result.Data) {
                                            if (result.Data.length == 0 && $(ctrl.attrUp("emptytemplate")).size() > 0) {
                                                result.isEmpty = true;
                                                html = $(ctrl.attrUp("emptytemplate")).html();
                                                target = ctrl.attrUp("emptytarget");
                                            } else {
                                                // Get template tag     
                                                tpl = ctrl;
                                                if ($(ctrl.attrUp("template")).size() > 0)
                                                    tpl = $(ctrl.attrUp("template"));

                                                html = tpl.render(result.Data);
                                            }
                                        }
                                    }

                                    ctrl.attachHtmlInTarget(html, target);

                                }

                                if (options.onsucess) options.onsucess(result, status, request);

                            }

                            if (ctrl.attr("onsucess")) eval(ctrl.attr("onsucess"));

                            if (ctrl.attrUp("once")) ctrl.unbind(ctrl.attr("command"));


                        },
                        error: function(result, status, event) {
                            eval(ctrl.attrUp("onerror"));
                            if (result.status == "404")
                                throw PageNotFoundException(ctrl);
                        }
                    });

            }
            return this;
        }, /* End DataBind*/

        /***************************************************************************************************
        Live controls, this allow load html with plugins and load it dynamically
        ***************************************************************************************************/

        initializeControls: function() {

            this._initializeCommandControls();

            this._initializeCalendars();

            this._initializeHtmlBox();

            this._initializeAutocomplete();

            return this;


        },
        _initializeCommandControls: function() {

            // enable link fire dataBinding            
            $("[href]", this).attr("method", "GET");

            $("[command]", this).each(function(i, ctrl) {
                if (!$(ctrl).hasControl()) {
                    $(ctrl).hasControl(true);

                    var eventType = $(ctrl).attr("command") || "click";
                    if (eventType === "load") {
                        $(ctrl).dataBind();
                    } else {
                        $(ctrl).bind(eventType, function(event) {
                            $(ctrl).dataBind();
                            event.stopPropagation();
                            return false;
                        });
                    }
                }
            });
        },
        _initializeCalendars: function() {
            $("[plugin*=calendar]", this).each(function() {

                if (!$(this).hasControl()) {
                    $(this).hasControl(true);

                    $(this).datepicker({
                        inline: false,
                        showOn: "button",
                        changeMonth: true,
                        changeYear: true,
                        constrainInput: false,
                        showOtherMonths: true,
                        nextText: "Próximo",
                        prevText: "Anterior",
                        dateFormat: "dd/mm/yy",
                        monthNamesShort: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho',
                                          'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'],
                        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado']
                    });

                    $(this).setMask({ mask: "39/19/9999" });
                }
            });


        },

        _initializeHtmlBox: function() {

            /***************************************************************************************************
            HTML Box
            ***************************************************************************************************/
            if ($.fn.htmlbox) {
                $("[plugin=htmlbox]", this).each(function() {
                    if (!$(this).hasControl()) {
                        $(this).hasControl(true);

                        var height = $(this).css("height") || "100px";
                        var weight = $(this).css("weight") || "610px";
                        $(this).css("height", height).css("weight", weight).htmlbox({
                            toolbars: [
                        	    [
                                "separator", "cut", "copy", "paste",
                                "separator", "undo", "redo",
                                "separator", "bold", "italic", "underline", "strike", "sup", "sub",
                                "separator", "justify", "left", "center", "right",
                                "separator", "ol", "ul", "indent", "outdent",
                                "separator", "link", "unlink", "image",
                            //Strip tags
                        		"separator", "removeformat", "striptags", "hr", "paragraph"
                            // Styles, Source code syntax buttons
                            //, "separator", "quote", "styles", "syntax"
                        		],
                        		[
                            // Formats, Font size, Font family, Font color, Font, Background
                                "separator", "formats", "fontsize", "fontfamily",
                        		"separator", "fontcolor", "highlight",
                            // Show code
                        		"separator", "code"
                        		]
                        	],
                            idir: "/App_themes/glasscyan/controls/Editor/",
                            icons: "default",  // Icon set
                            about: false,
                            skin: "silver",  // Skin, silver
                            output: "xhtml",  // Output
                            toolbar_height: 24, // Toolbar height
                            tool_height: 16,   // Tools height
                            tool_width: 16,    // Tools width
                            tool_image_height: 16,  // Tools image height
                            tool_image_width: 16,  // Tools image width
                            css: "body{margin:3px;font-family:verdana;font-size:11px; background-image:none;}p{margin:0px;}",
                            success: function(data) { alert(data); }, // AJAX on success
                            error: function(a, b, c) { return this; }   // AJAX on error
                        });
                    }
                });
            }

        },

        _initializeAutocomplete: function() {
            /***************************************************************************************************
            Autocomplete
            ***************************************************************************************************/
            $("[plugin=autocomplete]", this).each(function() {
                if (!$(this).hasControl()) {
                    $(this).hasControl(true);

                    $(this).autocomplete({
                        url: $(this).attrUp("controller") || $(this).attr("servicepath"),
                        minChars: $(this).attr("minChars") || 1,
                        parse: function(response) {
                            var parsed = [];
                            if (response) {
                                var data = response.Data;
                                for (var i = 0; i < data.length; i++) {
                                    parsed[parsed.length] = {
                                        data: data[i],
                                        value: data[i].Name,
                                        result: data[i].Name
                                    };
                                }
                            }

                            return parsed;
                        },
                        formatItem: function(item) {
                            return item.Name; // +' (' + item.mail + ')';
                        }
                    });
                }
            });
        }


    }); // End Initialize Controls



    /***************************************************************************************************
    Format Numbers
    ***************************************************************************************************/

    Number.prototype.format = function(format) {
        if (format && format != "") {
            var i = parseFloat(this);
            if (isNaN(i)) i = 0.00;
            i = parseInt((Math.abs(i) + .005) * 100) / 100;


            var s = new String(i);
            //if (s.indexOf('.') < 0) s += ',00';
            if (s.indexOf('.') == (s.length - 2)) s += '0';

            s = (i < 0 ? '-' : '') + s;
            return s.replace(".", ",");
        }
        return this;
    }



    $(document).initializeControls();


})(jQuery);


function Exception(msg) {
    msg = " Module: SmartClient \n Function: " + arguments.callee + " \n" + msg;
};

function PageNotFoundException(sender) {
    Exception(" A página '" + sender.getAddress() + "' não foi encontrada!");
}

function TargetMissingException(sender) {
    Exception(" Não foi encontrado o elemento html '" + sender.attrUp("target") + "'! \n\n Html Trace: " + sender.outerHtml());
}





