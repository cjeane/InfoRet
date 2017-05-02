using System;
using System.Collections.Generic;
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
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Data;

namespace InfoRet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Brush defBrush = (Brush)new BrushConverter().ConvertFromString("#FFE5E5E5");
        Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
        string fileName;
        Query myQuery = new Query();

        public void buildandBindTable(System.Windows.Controls.DataGrid grid, List<string> headers, List<List<String>> data, string seper)
        {
            grid.AutoGenerateColumns = false;
            grid.Columns.Clear();
            int i = 0;
            headers.Insert(0, seper);
            foreach (string header in headers)
            {
                DataGridTextColumn c = new DataGridTextColumn();
                c.Header = header;
                c.Binding = new System.Windows.Data.Binding("[" + i + "]");
                grid.Columns.Add(c);
                i++;
            }

            List<object> rows = new List<object>();
            string[] value;

            foreach (var docRow in data)
            {
                value = new string[headers.Count];
                for (i = 0; i < headers.Count; i++)
                {
                    value[i] = docRow.ElementAt(i);
                }
                rows.Add(value);
            }

            grid.ItemsSource = rows;
            grid.IsReadOnly = true;
            headers.RemoveAt(0);
        }
        bool isNum(string s)
        {
            return s.All(char.IsDigit);
        }
        public MainWindow()
        {
            InitializeComponent();
            /*
            BitmapImage image = new BitmapImage();
            String imagePath = Directory.GetCurrentDirectory() + "\\..\\..\\..\\img\\logo.png";
            image.BeginInit();
            image.UriSource = new Uri(imagePath);
            image.EndInit();
            imageLogo.Source = image;
            */
            filepathGen.Text = Generator.path;
        }
        private void loadDefCollectionButton_Click(object sender, RoutedEventArgs e)
        {
            DocCollection.cleanCollection();
            DocCollection.setupDocCollection();
        }
        private void clearColButton_Click(object sender, RoutedEventArgs e)
        {
            DocCollection.cleanCollection();
            dataGrid1.Visibility = Visibility.Collapsed;
            docWriter.deleteAllFiles(filepathGen.Text);
            ProgressLabel.Content = "Data collection cleared";
        }
        private void genRanDocColButton_Click(object sender, RoutedEventArgs e)
        {
            filepathGen.Text = Generator.path;
            genGrid.Visibility = Visibility.Visible;
            if (queryGrid.Visibility == Visibility.Visible)
            {
                queryGrid.Visibility = Visibility.Collapsed;
            }
            creategrid.Visibility = Visibility.Collapsed;
        }
        private void confirmGenButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isNum(numDocGentextBox.Text) && numDocGentextBox.Text.Length > 0)
            {
                numDocGentextBox.Background = Brushes.Tomato;
                confirmGenButton.IsEnabled = false;
                return;
            }
            if (!isNum(numGenTermtextBox.Text) && numGenTermtextBox.Text.Length > 0)
            {
                numGenTermtextBox.Background = Brushes.Tomato;
                confirmGenButton.IsEnabled = false;
                return;
            }

            Generator.generateRanDocs(0, int.Parse(numDocGentextBox.Text), 0, int.Parse(numGenTermtextBox.Text), filepathGen.Text);
            genGrid.Visibility = Visibility.Collapsed;

            string[] files = Directory.GetFiles(filepathGen.Text);
            foreach (var fileName in files)
                docReader.parseDoc(fileName);
            ProgressLabel.Content = "Generation done";
        }
        private void numDocGentextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (isNum(numDocGentextBox.Text) && numDocGentextBox.Text.Length > 0)
            {
                numDocGentextBox.Background = defBrush;
                confirmGenButton.IsEnabled = true;
            }
        }
        private void numGenTermtextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isNum(numGenTermtextBox.Text) && numGenTermtextBox.Text.Length > 0)
            {
                numGenTermtextBox.Background = defBrush;
                confirmGenButton.IsEnabled = true;
            }
        }
        private void addFilebutton_Click(object sender, RoutedEventArgs e)
        {
            fileName = "";
            ofd.ShowDialog();
            fileName = ofd.FileName;
            if (fileName.Length > 0 && File.Exists(fileName))
                docReader.parseDoc(fileName);
            ProgressLabel.Content = "File added";
        }
        private void addFolderbutton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                string[] files = Directory.GetFiles(fbd.SelectedPath);
                foreach (var fileName in files)
                    docReader.parseDoc(fileName);
            }
            ProgressLabel.Content = "Folder added";
        }
        private void makeQueryButton_Click(object sender, RoutedEventArgs e)
        {
            queryGrid.Visibility = Visibility.Visible;
            if (genGrid.Visibility == Visibility.Visible)
            {
                genGrid.Visibility = Visibility.Collapsed;
            }
            creategrid.Visibility = Visibility.Collapsed;
        }
        private void submitQueryButton_Click(object sender, RoutedEventArgs e)
        {
            Results ans;
            myQuery.setQuery(queryTextBox.Text);
            ans = new Results(myQuery);
            queryGrid.Visibility = Visibility.Collapsed;
            dataGrid.ItemsSource = ans.results;
            if (ans.results.Count > 0)
            {
                tabResults.IsSelected = true;
                ProgressLabel.Content = "Query found matches.";
                queryTextBox.Text = "Enter Query here.";
            }
            else
                ProgressLabel.Content = "Query found no matches please try a new query.";



        }
        private void testStem_Click(object sender, RoutedEventArgs e)
        {
            if (textStem.Text.Length > 0)
            {
                labelStemRes.Content = Stemmer.stem(textStem.Text);
            }
        }
        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            docCountlabel.Content = DocCollection.numOfDocs;
            termCountlabel.Content = DocCollection.termsDocCount.Count;
        }
        private void tabAdmin_GotFocus(object sender, RoutedEventArgs e)
        {
            docCountlabel.Content = DocCollection.numOfDocs;
            termCountlabel.Content = DocCollection.termsDocCount.Count;
        }
        private void refineQueryButton_Click(object sender, RoutedEventArgs e)
        {
            queryExtra.Visibility = Visibility.Visible;
            queryExtra.ItemsSource = myQuery.qTerms;
            dataGrid.Visibility = Visibility.Collapsed;
            topResBox.Visibility = Visibility.Collapsed;
            confirmNewQuery.Visibility = Visibility.Visible;
            resLabel.Visibility = Visibility.Collapsed;
            extraQueryLabel.Visibility = Visibility.Visible;
        }
        private void confirmNewQuery_Click(object sender, RoutedEventArgs e)
        {
            Results ans;
            queryExtra.Visibility = Visibility.Collapsed;
            extraQueryLabel.Visibility = Visibility.Collapsed;
            confirmNewQuery.Visibility = Visibility.Collapsed;
            dataGrid.Visibility = Visibility.Visible;
            topResBox.Visibility = Visibility.Visible;
            resLabel.Visibility = Visibility.Visible;
            ans = new Results(myQuery);
            dataGrid.ItemsSource = ans.results;
        }
        private void resultLimitButton_Click(object sender, RoutedEventArgs e)
        {

            if (isNum(topResBox.Text))
            {
                Results ans;
                int num;
                num = int.Parse(topResBox.Text);
                ans = new Results(myQuery, num);
                dataGrid.ItemsSource = ans.results;
            }

        }
        private void loadDocTest_Click(object sender, RoutedEventArgs e)
        {
            var str = testDocLoadtextBox.Text;
            List<string> headers;
            List<string> freq;
            List<string> idf;
            List<string> weight;

            List<List<string>> freqCon;
            if (DocCollection.findDoc(str) != null)
            {
                headers = new List<string>();
                freq = new List<string>();
                freqCon = new List<List<string>>();
                idf = new List<string>();
                weight = new List<string>();

                foreach (var term in DocCollection.findDoc(str).dict())
                {
                    headers.Add(term.Key);
                    freq.Add(term.Value.ToString());
                    idf.Add(Math.Round(DocCollection.calcIDF(term.Key), 4).ToString());
                    weight.Add(Math.Round((DocCollection.calcIDF(term.Key) * term.Value), 4).ToString());
                }

                freq.Insert(0, "Freq");
                idf.Insert(0, "IDF");
                weight.Insert(0, "Weight");

                freqCon.Add(freq);
                freqCon.Add(idf);
                freqCon.Add(weight);

                buildandBindTable(dataGrid2, headers, freqCon, "Term");
                docLoadLabel.Content = DocCollection.findDoc(str).getFullName();
                docLoadTermsLabel.Content = DocCollection.findDoc(str).dict().Count;
            }
        }
        private void removeDoc_Click(object sender, RoutedEventArgs e)
        {
            var str = testDocLoadtextBox.Text;

            if (DocCollection.findDoc(str) != null)
            {
                DocCollection.removeDoc(str);
                docLoadLabel.Content = "None";
                docLoadTermsLabel.Content = "0";
                dataGrid2.ItemsSource = null;
                dataGrid2.Items.Refresh();
            }
        }
        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            Value row;
            if (dataGrid.SelectedIndex > -1)
            {
                row = dataGrid.SelectedItem as Value;
                var fName = row.term;
                DocCollection.openDoc(fName);
            }
        }
        private void getMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            DocCollection.calcMatrixForDisplay();
            buildandBindTable(dataGrid1, DocCollection.termList, DocCollection.weightMatrix, "Filename\\Term");
            dataGrid1.Visibility = Visibility.Visible;
            ProgressLabel.Content = "Default data set loaded.";
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            Query newQ = new Query();
            var name = testDocLoadtextBox.Text;
            if (DocCollection.findDoc(name) == null)
            {
                testCVLabel.Content = "Document not\nloaded for testing.";
            }
            else if (newQ.setQuery(testQTtextBox.Text, testQWtextBox.Text))
            {

                testCVLabel.Content = Math.Round(Similarity.cosineWeight(newQ, DocCollection.findDoc(name)), 6);
            }
        }
        private void createFileButton_Click(object sender, RoutedEventArgs e)
        {
            creategrid.Visibility = Visibility.Visible;
            queryGrid.Visibility = Visibility.Collapsed;
            genGrid.Visibility = Visibility.Collapsed;
        }
        private void confirmCreateButton_Click(object sender, RoutedEventArgs e)
        {
            docWriter.createDoc(filepathGen.Text, createFileNametextBox.Text, createFileTextBlock.Text);
            docReader.parseDoc(filepathGen.Text + createFileNametextBox.Text + ".txt");
            creategrid.Visibility = Visibility.Collapsed;
        }
        private void tabResults_GotFocus(object sender, RoutedEventArgs e)
        {
            Results ans;
            queryExtra.Visibility = Visibility.Collapsed;
            extraQueryLabel.Visibility = Visibility.Collapsed;
            confirmNewQuery.Visibility = Visibility.Collapsed;
            dataGrid.Visibility = Visibility.Visible;
            topResBox.Visibility = Visibility.Visible;
            resLabel.Visibility = Visibility.Visible;
            ans = new Results(myQuery);
            dataGrid.ItemsSource = ans.results;
        }
    }
}
