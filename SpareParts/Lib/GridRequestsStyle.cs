using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SpareParts.Lib
{
    public class GridRequestsStyle : StyleSelector
    {
        public Style ExcelStyle { get; set; }

        public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
        {
            RequestWithNotify request = item as RequestWithNotify;

            if (request.Export)
            {
                return ExcelStyle;
            }

            return null;
        }
    }
}
