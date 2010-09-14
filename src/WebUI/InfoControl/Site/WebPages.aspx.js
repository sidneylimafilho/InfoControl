function DeletePage(object) {
    var line = object;
    while (line.tagName.toUpperCase() != "LI")
        line = line.parentNode;

    line.style.display = 'none';

    var request = PageMethods.DeletePage(
        $(object).attr("companyId"),
        $(object).attr("pageId"));
}



