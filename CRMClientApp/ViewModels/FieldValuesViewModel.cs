using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMClientApp.ViewModels
{
    public class FieldValuesViewModel
    {
        public string? ProjectMenuItemName { get; set; }
        public string? ServiceMenuItemName { get; set; }
        public string? BlogMenuItemName { get; set; }
        public string? ContactsMenuItemName { get; set; }
        public string? PrecedingFormText { get; set; }
    }
}
