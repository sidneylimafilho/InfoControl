(function($) {



    test("NULL: Deve ser possivel renderizar sem dados", function() {
        var html = "<div><p><a command=\"click\" href=\"index.html\"></a></p></div>";
        var div = $(html);
        div.render(null);
        ok(true);
    });

    test("BIND: Deve ser possivel renderizar pelo menos 1 dado", function() {
        var html = $("<div><p><a command=\"click\" href=\"index.html\"></a></P></div>");
        var result = html.render([{}, {}]);
        equal($(result).length, 2);
    });

    test("BIND: Deve ser possivel renderizar pelo menos 5 dados", function() {
        var html = $("<div><p><a command=\"click\" href=\"index.html\"></a></P></div>");
        var result = html.render([{}, {}, {}, {}, {}]);
        equal($(result).length, 5);
    });



    test("TAGS ESPECIAIS: Tags especiais do template, com conteudo javascript ", function() {
        window.i = 0;
        var html = $("<div><p><$ window.i=1; $></P></div>");
        var result = html.render([{}]);
        equal(window.i, 1);
    });

    test("TAGS ESPECIAIS: Tags especiais do template, document.write", function() {
        var html = $("<div><p><$= \"teste\" $></P></div>");
        var result = html.render({});
        ok($(result).outerHtml().indexOf("teste") > -1);
        equal($(result).outerHtml().indexOf("teste"), 3);
    });

    test("TAG ESPECIAIS INVÁLIDA: Deve indicar que o template está inválido!", function() {
        var html = $("<div><p><$ if({) $></P></div>");
        html.render(null);
        ok(true);
    });


})(jQuery);