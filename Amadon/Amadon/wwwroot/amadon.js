function jumpToAnchor(anchorLink) {
    const cellElement = document.getElementById(anchorLink);
    if (cellElement) {
        try {
            cellElement.scrollIntoView();
        } catch (error) {   
            // Handle routing error
            //alert("An error occurred during routing: " + error.message);
        }
    }
 }

window.registerClickEvent = function(dotNetObject) {
    document.querySelector('a').addEventListener('click', function(event) {
        event.preventDefault();
        dotNetObject.invokeMethodAsync('OnLinkClicked');
    });
};

window.setupF1KeyListener = () => {
    document.body.addEventListener('keydown', (event) => {
        if (event.key === 'F1') {
            event.preventDefault(); // To prevent the browser's default F1 action
            DotNet.invokeMethodAsync('Amadon', 'HandleF1KeyPress');
        }
    });
};

window.addDoubleClickListener = function (elementId, dotNetReference) {
    const element = document.getElementById(elementId);
    if (element) {
        element.addEventListener('dblclick', (event) => {
            dotNetReference.invokeMethodAsync('OnRowDoubleClicked', event.target.closest('tr').rowIndex);
        });
    }
}

    

