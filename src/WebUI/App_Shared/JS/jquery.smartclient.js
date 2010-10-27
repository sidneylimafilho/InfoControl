/// <reference path="jquery-vsdoc.js" />
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
            var url = this.attrUp("source") || this.attrUp("href");

            if (this.attr("action")) {
                url += "/" + this.attrUp("action");
            }
            return url.replace("~", window.applicationPath || "");
        },
        dataBind: function(options) {
            for (var i = 0, l = this.length; i < l; i++)
                $(this[i])._dataBind(options);



            return this;
        }, /* End DataBind*/

        _dataBind: function(options) {
            options = options || {};
            var $this = $(this[0]);

            if ($this.attr("onbinding")) eval($this.attr("onbinding"));
            if (options.onbinding) options.onbinding();

            var formData = {};

            options.params = options.params || {};
            if ($this.attrUp("params") && $this.attrUp("params") != "") {
                $.extend(options.params, eval("(" + $this.attrUp("params") + ")"));
            }

            var form = $this.closest("[asForm]") || $this.closest("FORM");

            // Get All html form controls
            var fields = form.find(":text, select, textarea, :checked, :password, [type=hidden]")
                        .map(function(i, elem) { options.params[elem.name] = elem.value; return true; });

            // Makes the comparison "options.data || {}" because options.data can be filled, when trigger 
            // is fired otherwise prepares the data Request Payload
            if (!options.data)
                options.data = $.toJSON(options.params);

            // Allow fire DataBinding in controls that has TRIGGER atribute
            if ($this.attrUp("trigger")) {
                $this.closest("[trigger]").find("[param]").each(function() {
                    options.params[$this.attr("param")] = $this.val();
                });

                $($this.attrUp("trigger")).dataBind(options);
                return;
            }

            // save the control that is fire dataBind, because closure "sucess" dont access
            //var ctrl = $this;

            var type = $this.attrUp("method") || "POST";

            // Prepare the url
            var url = $this.getAddress();

            // Only fires ajax if there are url
            if (url) {
                $.ajax({
                    type: type,
                    url: url,
                    data: options.data,
                    contentType: "application/json",
                    ifModified: true,
                    success: function(result, status, request) {


                        // If Not Modified then get cached content file by iframe
                        if (request.status == 304) {
                            $this.ajaxIframe(url, $this, options.onsucess);
                        } else {
                            // If Http Status 200 then OK, process JSON because data should be transform on html
                            var html = result;
                            var target = $this.attrUp("target");

                            if (request.responseText != "") {
                                if (request.getResponseHeader("Content-type").indexOf("json") > -1) {

                                    if (result.Errors) {
                                        if ($.isArray(result.Errors)) {
                                            for (var item in result.Errors)
                                                alert(item);
                                        } else {
                                            alert(result.Errors);
                                        }
                                    }

                                    //
                                    // result.Data   = ClientResponse
                                    // result.d      = ASMX/WCF JSON return
                                    // result.d.Data = ASMX/WCF JSON return ClientResponse
                                    // 
                                    var data = result.Data || result.d || result.d.Data || result;

                                    if (data.length == 0 && $($this.attrUp("emptytemplate")).size() > 0) {
                                        result.isEmpty = true;
                                        html = $($this.attrUp("emptytemplate")).html();
                                        target = $this.attrUp("emptytarget");
                                    } else {
                                        // Get template tag
                                        tpl = $this;
                                        if ($($this.attrUp("template")).size() > 0)
                                            tpl = $($this.attrUp("template"));

                                        html = tpl.render(data);
                                    }

                                }

                                $this.attachHtmlInTarget(html, target);

                            }

                            if (options.onsucess) options.onsucess(result, status, request);

                        }

                        if ($this.attr("onsucess")) eval($this.attr("onsucess"));

                        if ($this.attrUp("once")) $this.unbind($this.attr("command"));


                    },
                    error: function(result, status, event) {
                        eval($this.attrUp("onerror"));
                        if (result.status == "404")
                            throw PageNotFoundException($this);
                    }
                });
            }
        },

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

                        var options = {
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
                            idir: "../../App_themes/glasscyan/controls/Editor/",
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
                        };

                        options = $.extend(options, eval("(" + $(this).attr("options") + ")"));

                        var height = $(this).css("height") || "400px";
                        var weight = $(this).css("weight") || "100%";
                        $(this).css("height", height)
                               .css("weight", weight)
                               .htmlbox(options);
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
                        url: $(this).getAddress(),
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





