﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfSaveToXmlSample.Properties
{
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase
    {

        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100,100,500,400")]
        public global::System.Windows.Rect MainWindow_Bounds
        {
            get
            {
                return ((global::System.Windows.Rect)(this["MainWindow_Bounds"]));
            }
            set
            {
                this["MainWindow_Bounds"] = value;
            }
        }

        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfColumnSetting xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
<ColumnSetting>
<DisplayIndex>0</DisplayIndex>
<Width>-1</Width>
</ColumnSetting>
<ColumnSetting>
<DisplayIndex>1</DisplayIndex>
<Width>-1</Width>
</ColumnSetting>
<ColumnSetting>
<DisplayIndex>2</DisplayIndex>
<Width>-1</Width>
</ColumnSetting>
<ColumnSetting>
<DisplayIndex>3</DisplayIndex>
<Width>-1</Width>
</ColumnSetting>
</ArrayOfColumnSetting>")]
        public WpfSaveToXmlSample.ColumnSettingCollection DataGrid_Columns
        {
            get
            {
                return ((WpfSaveToXmlSample.ColumnSettingCollection)(this["DataGrid_Columns"]));
            }
            set
            {
                this["DataGrid_Columns"] = value;
            }
        }
    }
}