﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OptionsTradeWell.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("OTW")]
        public string ServerName {
            get {
                return ((string)(this["ServerName"]));
            }
            set {
                this["ServerName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("FUT")]
        public string FuturesTableName {
            get {
                return ((string)(this["FuturesTableName"]));
            }
            set {
                this["FuturesTableName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("OPT")]
        public string OptionsTableName {
            get {
                return ((string)(this["OptionsTableName"]));
            }
            set {
                this["OptionsTableName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int UniqueIndexInDdeDataArray {
            get {
                return ((int)(this["UniqueIndexInDdeDataArray"]));
            }
            set {
                this["UniqueIndexInDdeDataArray"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("18")]
        public int NumberOfTrackingOptions {
            get {
                return ((int)(this["NumberOfTrackingOptions"]));
            }
            set {
                this["NumberOfTrackingOptions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("365")]
        public double DaysInYear {
            get {
                return ((double)(this["DaysInYear"]));
            }
            set {
                this["DaysInYear"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4")]
        public int RoundTo {
            get {
                return ((int)(this["RoundTo"]));
            }
            set {
                this["RoundTo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3")]
        public double MaxValueOfImplVol {
            get {
                return ((double)(this["MaxValueOfImplVol"]));
            }
            set {
                this["MaxValueOfImplVol"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("-10")]
        public double ChartsMinYValue {
            get {
                return ((double)(this["ChartsMinYValue"]));
            }
            set {
                this["ChartsMinYValue"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("70")]
        public double ChartsMaxYValue {
            get {
                return ((double)(this["ChartsMaxYValue"]));
            }
            set {
                this["ChartsMaxYValue"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public double ChartsStepYValue {
            get {
                return ((double)(this["ChartsStepYValue"]));
            }
            set {
                this["ChartsStepYValue"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("70000")]
        public double MaxOptionStrikeInQuikDesk {
            get {
                return ((double)(this["MaxOptionStrikeInQuikDesk"]));
            }
            set {
                this["MaxOptionStrikeInQuikDesk"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("250")]
        public double StrikeStep {
            get {
                return ((double)(this["StrikeStep"]));
            }
            set {
                this["StrikeStep"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("POS")]
        public string PositionTableName {
            get {
                return ((string)(this["PositionTableName"]));
            }
            set {
                this["PositionTableName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("SPBFUT26WILL")]
        public string Account {
            get {
                return ((string)(this["Account"]));
            }
            set {
                this["Account"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool checkBoxMaxFutQ {
            get {
                return ((bool)(this["checkBoxMaxFutQ"]));
            }
            set {
                this["checkBoxMaxFutQ"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool checkBoxDeltaStep {
            get {
                return ((bool)(this["checkBoxDeltaStep"]));
            }
            set {
                this["checkBoxDeltaStep"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool checkBoxPriceLevels {
            get {
                return ((bool)(this["checkBoxPriceLevels"]));
            }
            set {
                this["checkBoxPriceLevels"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool checkBosClosePos {
            get {
                return ((bool)(this["checkBosClosePos"]));
            }
            set {
                this["checkBosClosePos"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool checkBoxPriceCon {
            get {
                return ((bool)(this["checkBoxPriceCon"]));
            }
            set {
                this["checkBoxPriceCon"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool checkBoxPnLCon {
            get {
                return ((bool)(this["checkBoxPnLCon"]));
            }
            set {
                this["checkBoxPnLCon"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string checkBoxMaxFutQValue {
            get {
                return ((string)(this["checkBoxMaxFutQValue"]));
            }
            set {
                this["checkBoxMaxFutQValue"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string checkBoxDeltaStepValue {
            get {
                return ((string)(this["checkBoxDeltaStepValue"]));
            }
            set {
                this["checkBoxDeltaStepValue"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string checkBoxPriceLevelsValue {
            get {
                return ((string)(this["checkBoxPriceLevelsValue"]));
            }
            set {
                this["checkBoxPriceLevelsValue"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string checkBosCloseWhatPosWillBeCloseValue {
            get {
                return ((string)(this["checkBosCloseWhatPosWillBeCloseValue"]));
            }
            set {
                this["checkBosCloseWhatPosWillBeCloseValue"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string checkBosCloseTrackingInstrValue {
            get {
                return ((string)(this["checkBosCloseTrackingInstrValue"]));
            }
            set {
                this["checkBosCloseTrackingInstrValue"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string checkBoxPricePriceConSignValue {
            get {
                return ((string)(this["checkBoxPricePriceConSignValue"]));
            }
            set {
                this["checkBoxPricePriceConSignValue"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string checkBoxPricePriceConValue {
            get {
                return ((string)(this["checkBoxPricePriceConValue"]));
            }
            set {
                this["checkBoxPricePriceConValue"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string checkBoxPnLValue {
            get {
                return ((string)(this["checkBoxPnLValue"]));
            }
            set {
                this["checkBoxPnLValue"] = value;
            }
        }
    }
}
