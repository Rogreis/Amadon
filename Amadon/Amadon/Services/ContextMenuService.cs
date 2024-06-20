using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amadon.Services
{
    public class ContextMenuService
    {
        public async Task<bool> ShowContextMenu(MouseEventArgs e)
        {
            // Implement context menu logic here (refer to step 2)
            return await Task.FromResult(true); // Or false to prevent default behavior
        }
    }
}
