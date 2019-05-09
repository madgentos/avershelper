using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Path = System.IO.Path;
using File = System.IO.File;

namespace avershelper
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string Format = "ddMMyyyy";
		private const string FormatForHtml = "dd.MM.yyyy";


		string filePath, filePathDir, keyPath, fileKeyDir, fileEx, keyEx, newFilePath, newKeyPath, prefix;

		private void Reg_Zvit_RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			prefix = "rz";
		}

		private void Ne_Reg_Zvit_RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			prefix = "nz";
		}

		private void Dodat_RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			prefix = "d";
		}

		private void Protocol_RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			prefix = "pr";
		}

		private void Rez_Per_RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			prefix = "rp";
		}

		private void Povid_Pro_Zbor_RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			prefix = "p";
		}


		protected override void OnActivated(EventArgs e)
		{
			base.OnActivated(e);
		}

		private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			var datePicker = sender as DatePicker;

			DateTime? selectedDate = datePicker.SelectedDate;

			Debug.WriteLine(selectedDate.Value.ToString(Format));
		}

		public MainWindow()
		{
			InitializeComponent();
			datepicer.SelectedDate = DateTime.Now;
		}


		private void Button_Click_File(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			//openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";

			if (openFileDialog.ShowDialog() == true)
			{
				filePath = openFileDialog.FileName;
				fileEx = Path.GetExtension(filePath);
				filePathDir = Path.GetDirectoryName(filePath) + "\\";

			}
		}

		private void Button_Click_Key(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "p7s Files (*.p7s)|*.p7s";

			if (openFileDialog.ShowDialog() == true)
			{
				keyPath = openFileDialog.FileName;
				keyEx = Path.GetExtension(keyPath);
				fileKeyDir = Path.GetDirectoryName(filePath) + "\\";

			}

		}

		private void Button_Click_Rename(object sender, RoutedEventArgs e)
		{
			string dateNow = datepicer.SelectedDate.Value.ToString(Format);
			DateTime dateTimeNow = (DateTime) datepicer.SelectedDate.Value;
			int i = 0;

			List<String> HtmlList = new List<String>();

			do
			{
				i++;

				if (filePath != null)
				{
					newFilePath = filePathDir;
					newFilePath += prefix;
					newFilePath += dateNow;
					newFilePath += "_" + i.ToString();
					newFilePath += Path.GetExtension(filePath);

					if (File.Exists(newFilePath))
					{
						continue;
					}
					//Debug.WriteLine(File.Exists(newFilePath));
					File.Move(filePath, newFilePath);
					//Debug.WriteLine(newFilePath);

					HtmlList.Add(GenHtmlCode(dateTimeNow, newFilePath));


				}


				if (keyPath != null)
				{
					newKeyPath = fileKeyDir;
					newKeyPath += prefix;
					newKeyPath += dateNow;
					newKeyPath += "_" + i.ToString();
					newKeyPath += fileEx;
					newKeyPath += keyEx;
					File.Move(keyPath, newKeyPath);

					HtmlList.Add(GenHtmlCode(dateTimeNow, newKeyPath));


				}
				break;

			}
			while (File.Exists(newFilePath));

			String HtmlClip = null;
			HtmlList.Reverse();
			String last = HtmlList.Last();
			foreach (string item in HtmlList)
			{

				if (item.Equals(last))
				{
					richbox.AppendText(item);
					HtmlClip += item;
				}
				else
				{
				richbox.AppendText(item+"\r");
					HtmlClip += item+"\r";

				}
			}
			Clipboard.SetText(HtmlClip);
			MessageBox.Show("Готово \n Вывод скопирован в буфер обмена");
			
			

		}

		private String GenHtmlCode(DateTime dateTimeNow, String FilePath)
		{
			String title = titlebox.Text;
			String exten = Path.GetExtension(FilePath);
			String filename = Path.GetFileName(FilePath);
			String folder = dateTimeNow.Year.ToString();
			String ico = GetIcoExten(exten);
			String Datehml = dateTimeNow.ToString(FormatForHtml);

			String Html = $"<p><img src=\"{ico}\" align=\"absmiddle\" style=\"border-width: 0px\"> {Datehml} <a href=\"{folder}/{filename}\">{title}</a></p>";

			return Html;
			//richbox.AppendText("\n");

			//MessageBox.Show(Html);
		}

		private String GetIcoExten(String exten)
		{
			String ico;
			switch (exten)
			{
				case ".doc" : {
						ico = "doc.jpg";
						break;
					}
				case ".docx" : {
						ico = "doc.jpg";
						break;
					}
				case ".pdf" : {
						ico = "pdf.jpg";
						break;
					}
				case ".p7s": {
						ico = "file.png";
						break;
					}
				default:
					ico = "file.png";

					break;
			}

			return "images/" + ico;
		}
	}
}
