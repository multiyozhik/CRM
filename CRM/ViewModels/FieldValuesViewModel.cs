using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CRMSystem.ViewModels
{
    public class FieldValuesViewModel
    {
        [DataType(DataType.Text, ErrorMessage="Данные должны быть в виде строки")]
        public string? ProjectMenuItemName { get; set; }

        [DataType(DataType.Text, ErrorMessage = "Данные должны быть в виде строки")]
        public string? ServiceMenuItemName { get; set; }

        [DataType(DataType.Text, ErrorMessage = "Данные должны быть в виде строки")]
        public string? BlogMenuItemName { get; set; }

        [DataType(DataType.Text, ErrorMessage = "Данные должны быть в виде строки")]
        public string? ContactsMenuItemName { get; set; }

        [DataType(DataType.Text, ErrorMessage = "Данные должны быть в виде строки")]
        public string? PrecedingFormText { get; set; }
    }
}
