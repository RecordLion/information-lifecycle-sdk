using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sample
{
    public partial class RecordDetails : Window
    {
        public RecordDetails(string recordsManagerUrl, string recordUri)
        {
            InitializeComponent();

            this.DetailsControl.RecordsManagerUrl = recordsManagerUrl;
            this.DetailsControl.RecordUri = recordUri;
        }
    }
}
