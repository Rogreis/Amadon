
function jumpToAnchor(anchorLink) {
   console.info("jumpToAnchor function: " + anchorLink);
   var anchors = document.getElementsByName(anchorLink);
   if (anchors.length > 0) {
      // Return the first element with the specified name
      cellElement = anchors[0];
   } else {
      console.info('Anchor with name ' + anchorLink + ' not found.');
      return null;
   }

   if (cellElement) {
      try {
         cellElement.scrollIntoView({ behavior: 'smooth' });
      } catch (error) {
         console.info('Could not jump to anchor with name ' + anchorLink);
      }
   }
}

function deferJumpToAnchor(anchorLink) {
   console.info("deferJumpToAnchor function: " + anchorLink);
   requestAnimationFrame(() => jumpToAnchor(anchorLink));
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

// For the text context menu
window.registerClickOutsideHandler = (dotnetHelper) => {
   const hideContextMenu = (event) => {
      dotnetHelper.invokeMethodAsync('HideContextMenu');
      document.removeEventListener('click', hideContextMenu);
   };

   document.addEventListener('click', hideContextMenu);
};

function selectText(id) {
   var element = document.getElementById(id);
   if (element) {
      var range = document.createRange();
      range.selectNodeContents(element);
      var selection = window.getSelection();
      selection.removeAllRanges();
      selection.addRange(range);
   }
}

function changeClass(id, newClass) {
   var element = document.getElementById(id);
   if (element) {
      element.className = newClass;
   }
}

function getSelectedText(id) {

   var selection = window.getSelection();
   var selectedText = selection.toString();
   return selectedText;

//   var element = document.getElementById(id);
//   if (element) {
//      var selection = window.getSelection();
//      var selectedText = selection.toString();
//      return selectedText;
//   }
//   return "";
}

function changeDivClassInTable(tableId, newClass) {
   // Get the table element by its ID
   var table = document.getElementById(tableId);

   // Check if the table element exists
   if (!table) {
      console.info('Table with ID ' + tableId + ' not found.');
      return;
   }
   console.info('Table with ID ' + tableId + ' found.');

   // Get all div elements inside the table
   var divs = table.getElementsByTagName('div');

   // Loop through all div elements and change their class
   for (var i = 0; i < divs.length; i++) {
      divs[i].className = newClass;
   }
}




    

