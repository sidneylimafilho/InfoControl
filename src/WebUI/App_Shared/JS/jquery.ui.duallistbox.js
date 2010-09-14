function DualListBox_FireEvent(htmlImage) {
    var option;
    var action = htmlImage.id.substring(htmlImage.id.indexOf(':') + 1);
    var elementPrefix = htmlImage.id.substring(0, htmlImage.id.indexOf(':'));
    var htmlLeftSelect = document.getElementById(elementPrefix + ':leftSelect');
    var htmlRightSelect = document.getElementById(elementPrefix + ':rightSelect');
    var htmlInputHidden = document.getElementById(elementPrefix + ':hidden');


    switch (action) {
        case 'InsertAll':
            for (var i = htmlLeftSelect.options.length - 1; i >= 0; i--) {
                var option = new Option(htmlLeftSelect.options[i].text, htmlLeftSelect.options[i].value);
                htmlRightSelect.options.add(option);
                htmlLeftSelect.options[i] = null;
            }
            break;

        case 'Insert':
            for (var i = htmlLeftSelect.options.length - 1; i >= 0; i--) {
                if (htmlLeftSelect.options[i].selected) {
                    var option = new Option(htmlLeftSelect.options[i].text, htmlLeftSelect.options[i].value);
                    htmlRightSelect.options.add(option);
                    htmlLeftSelect.options[i] = null;
                }
            }
            break;

        case 'Remove':
            for (var i = htmlRightSelect.options.length - 1; i >= 0; i--) {
                if (htmlRightSelect.options[i].selected) {
                    var option = new Option(htmlRightSelect.options[i].text, htmlRightSelect.options[i].value);
                    htmlLeftSelect.options.add(option);
                    htmlRightSelect.options[i] = null;
                }
            }
            break;

        case 'RemoveAll':
            for (var i = htmlRightSelect.options.length - 1; i >= 0; i--) {
                var option = new Option(htmlRightSelect.options[i].text, htmlRightSelect.options[i].value);
                htmlLeftSelect.options.add(option);
                htmlRightSelect.options[i] = null;
            }
            break;
    }

    DualListBox_FillHtmlInputHidden(htmlRightSelect, htmlInputHidden);
}

function DualListBox_FillHtmlInputHidden(selectElement, inputHidden) {
    var buffer = '';
    for (var i = 0; i < selectElement.length; i++) {
        buffer += selectElement[i].value + ':' + selectElement[i].text + ', ';
    }

    buffer = buffer.substring(0, buffer.lastIndexOf(','));
    inputHidden.value = buffer;
} 
