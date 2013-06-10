using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using SpareParts.Lib;

namespace SpareParts
{
	/// <summary>
	/// Interaction logic for WindowDetails.xaml
	/// </summary>
	public partial class WindowDetails : Window
	{
	    private SparePartsEntities _entities=new SparePartsEntities();

		public WindowDetails()
		{
			this.InitializeComponent();
			
			// Insert code required on object creation below this point.
		}

	    private PartWithNotify _part;

	    public PartWithNotify Part
	    {
	        get { return _part; }
	        set { _part = value; }
	    }

	    public SparePartsEntities Entities
	    {
	        get { return _entities; }
	        set { _entities = value; }
	    }

	    public void Calculate()
        {
            int countPartNo=0;
            int countResolutionPartNo=0;
            int countPartNoOriginal=0;
            int countPartName=0;

            if (Part.PartNo.Length > 0)
            {
                countPartNo = Entities.Parts.Count(x => x.PartNo == Part.PartNo);    
            }

            if (!string.IsNullOrEmpty(Part.ResolutionPartNo))
            {
                countResolutionPartNo = Entities.Parts.Count(x => x.ResolutionPartNo == Part.ResolutionPartNo);    
            }

            if (!string.IsNullOrEmpty(Part.PartNoOrignal))
            {
                countPartNoOriginal = Entities.Parts.Count(x => x.PartNoOrignal == Part.PartNoOrignal);    
            }
	        
	        if (!string.IsNullOrEmpty(Part.PartName))
	        {
                countPartName = Entities.Parts.Count(x => x.PartName == Part.PartName);    
	        }


	        TextBlockCountResolutionPartNo.Text = countResolutionPartNo.ToString();
	        TextBlockCountPartNo.Text = countPartNo.ToString();
	        TextBlockCountOriginalPartNo.Text = countPartNoOriginal.ToString();
	        TextBlockCountPartName.Text = countPartName.ToString();
        }

	    private void WindowDetails_OnLoaded(object sender, RoutedEventArgs e)
	    {
	        
	    }
	}
}