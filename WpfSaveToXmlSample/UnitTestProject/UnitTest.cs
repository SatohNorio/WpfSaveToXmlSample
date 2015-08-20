using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfSaveToXmlSample;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void MainWindowBoundsTest()
        {
            Window w = new Window();
            w.Name = "FMainWindow";
            w.Left = 50;
            w.Top = 60;
            w.Width = 70;
            w.Height = 80;
            Setting.SetMainWindowBounds(w);
            var r = Setting.MainWindowBounds;
            Assert.IsTrue(50 == r.Left);
            Assert.IsTrue(60 == r.Top);
            Assert.IsTrue(70 == r.Width);
            Assert.IsTrue(80 == r.Height);
        }

        [TestMethod]
        public void DataGridColumnsTest()
        {
            var dg = new DataGrid();
            dg.Name = "FDataGrid";
            var c1 = new ColumnSetting() { DisplayIndex = 0, Width = -1 };
            var c2 = new ColumnSetting() { DisplayIndex = 1, Width = 100 };
            var c3 = new ColumnSetting() { DisplayIndex = 2, Width = 200 };
            dg.Columns.Add(new DataGridTemplateColumn() { DisplayIndex = c1.DisplayIndex, Width = c1.Width });
            dg.Columns.Add(new DataGridTemplateColumn() { DisplayIndex = c2.DisplayIndex, Width = c2.Width });
            dg.Columns.Add(new DataGridTemplateColumn() { DisplayIndex = c3.DisplayIndex, Width = c3.Width });

            Setting.SetDataGridColumns(dg);
            var cols = Setting.DataGridColumns;
            var c = cols[0];
            Assert.IsTrue(c1.Equals(c));
            c = cols[1];
            Assert.IsTrue(c2.Equals(c));
            c = cols[2];
            Assert.IsTrue(c3.Equals(c));
        }

        [TestMethod]
        public void DoubleToDataGridLengthConverterTest()
        {
            var cvt = new DoubleToDataGridLengthConverter(); 
            double v = 1000000;
            DataGridLength l = (DataGridLength)cvt.Convert(v, typeof(int), null, System.Globalization.CultureInfo.CurrentCulture);
            Assert.IsTrue(l.Value == v);

            v = 0;
            l = (DataGridLength)cvt.Convert(v, typeof(int), null, System.Globalization.CultureInfo.CurrentCulture);
            Assert.IsTrue(l.Value == v);

            v = -1;
            l = (DataGridLength)cvt.Convert(v, typeof(int), null, System.Globalization.CultureInfo.CurrentCulture);
            Assert.IsTrue(DataGridLength.Auto == l);

        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void DoubleToDataGridLengthConverterConvertBackTest()
        {
            var cvt = new DoubleToDataGridLengthConverter();
            DataGridLength l = DataGridLength.Auto;

            double v = (double)cvt.ConvertBack(l, typeof(int), null, System.Globalization.CultureInfo.CurrentCulture);
        }

        [TestMethod]
        public void SavingWindowStateActionTest()
        {
            var trigger = new System.Windows.Interactivity.EventTrigger("Closed");
            var trg = new SavingWindowStateAction();
            trigger.Actions.Add(trg);

            var w = new Window();
            w.Name = "FMainWindow";
            w.Left = 100;
            w.Top = 110;
            w.Width = 320;
            w.Height = 240;

            Interaction.GetTriggers(w).Add(trigger);
            trg.Parameter = w;

            w.ShowDialog();

            var r = Setting.MainWindowBounds;
            Assert.IsTrue(w.Left == r.Left);
            Assert.IsTrue(w.Top == r.Top);
            Assert.IsTrue(w.Width == r.Width);
            Assert.IsTrue(w.Height == r.Height);
        }

        [TestMethod]
        public void SavingDataGridStateActionTest()
        {
            var trigger = new System.Windows.Interactivity.EventTrigger("Closed");
            var trg = new SavingDataGridStateAction();
            trigger.Actions.Add(trg);

            var w = new Window();
            Interaction.GetTriggers(w).Add(trigger);

            var dg = new DataGrid();
            dg.Name = "FDataGrid";
            var c1 = new ColumnSetting() { DisplayIndex = 0, Width = -2 };
            var c2 = new ColumnSetting() { DisplayIndex = 1, Width = 100 };
            var c3 = new ColumnSetting() { DisplayIndex = 2, Width = 200 };
            var c4 = new ColumnSetting() { DisplayIndex = 3, Width = -1 };
            var c5 = new ColumnSetting() { DisplayIndex = 4, Width = 0 };
            dg.Columns.Add(new DataGridTemplateColumn() { DisplayIndex = c1.DisplayIndex, Width = c1.Width });
            dg.Columns.Add(new DataGridTemplateColumn() { DisplayIndex = c2.DisplayIndex, Width = c2.Width });
            dg.Columns.Add(new DataGridTemplateColumn() { DisplayIndex = c3.DisplayIndex, Width = c3.Width });
            dg.Columns.Add(new DataGridTemplateColumn() { DisplayIndex = c4.DisplayIndex, Width = c4.Width });
            dg.Columns.Add(new DataGridTemplateColumn() { DisplayIndex = c5.DisplayIndex, Width = c5.Width });
            trg.Parameter = dg;

            w.ShowDialog();

            var cols = Setting.DataGridColumns;
            var c = cols[0];
            Assert.IsTrue(c1.Equals(c));
            c = cols[1];
            Assert.IsTrue(c2.Equals(c));
            c = cols[2];
            Assert.IsTrue(c3.Equals(c));
            c = cols[3];
            Assert.IsTrue(c4.Equals(c));
            c = cols[4];
            Assert.IsTrue(c5.Equals(c));

            Setting.Save();
        }

        [TestMethod]
        public void SaveTest()
        {
            // Window情報
            var w = new Window();
            w.Name = "FMainWindow";
            w.Left = 100;
            w.Top = 110;
            w.Width = 320;
            w.Height = 240;

            var r = Setting.MainWindowBounds;
            Assert.IsTrue(w.Left == r.Left);
            Assert.IsTrue(w.Top == r.Top);
            Assert.IsTrue(w.Width == r.Width);
            Assert.IsTrue(w.Height == r.Height);

            // DataGridColumns情報
            var c1 = new ColumnSetting() { DisplayIndex = 0, Width = -2 };
            var c2 = new ColumnSetting() { DisplayIndex = 1, Width = 100 };
            var c3 = new ColumnSetting() { DisplayIndex = 2, Width = 200 };
            var c4 = new ColumnSetting() { DisplayIndex = 3, Width = -1 };
            var c5 = new ColumnSetting() { DisplayIndex = 4, Width = 0 };

            var cols = Setting.DataGridColumns;
            var c = cols[0];
            Assert.IsTrue(c1.Equals(c));
            c = cols[1];
            Assert.IsTrue(c2.Equals(c));
            c = cols[2];
            Assert.IsTrue(c3.Equals(c));
            c = cols[3];
            Assert.IsTrue(c4.Equals(c));
            c = cols[4];
            Assert.IsTrue(c5.Equals(c));

        }
    }
}
