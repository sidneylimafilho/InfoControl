(function($) {


    test("VAZIO: Deve ser possivel renderizar sem dados", function() {
        var html = $("<div><p><a command=\"click\" href=\"index.html\"></a></P></div>");
        html.render(null);
       
    });

    test("VAZIO: Deve ser possivel renderizar sem dados", function() {
        var html = $("<div><p><a command=\"click\" href=\"index.html\"></a></P></div>");
        html.render({ "Data": [{}] });
        
    });



})(jQuery);