﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::System.Data.Objects.DataClasses.EdmSchemaAttribute()]

// Original file name:
// Generation date: 4/30/2009 7:28:06 PM
namespace northwndModel2
{
    
    /// <summary>
    /// There are no comments for northwndEntities7 in the schema.
    /// </summary>
    public partial class northwndEntities7 : global::System.Data.Objects.ObjectContext
    {
        /// <summary>
        /// Initializes a new northwndEntities7 object using the connection string found in the 'northwndEntities7' section of the application configuration file.
        /// </summary>
        public northwndEntities7() : 
                base("name=northwndEntities7", "northwndEntities7")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// Initialize a new northwndEntities7 object.
        /// </summary>
        public northwndEntities7(string connectionString) : 
                base(connectionString, "northwndEntities7")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// Initialize a new northwndEntities7 object.
        /// </summary>
        public northwndEntities7(global::System.Data.EntityClient.EntityConnection connection) : 
                base(connection, "northwndEntities7")
        {
            this.OnContextCreated();
        }
        partial void OnContextCreated();
        /// <summary>
        /// There are no comments for Account in the schema.
        /// </summary>
        public global::System.Data.Objects.ObjectQuery<Account> Account
        {
            get
            {
                if ((this._Account == null))
                {
                    this._Account = base.CreateQuery<Account>("[Account]");
                }
                return this._Account;
            }
        }
        private global::System.Data.Objects.ObjectQuery<Account> _Account;
        /// <summary>
        /// There are no comments for Account in the schema.
        /// </summary>
        public void AddToAccount(Account account)
        {
            base.AddObject("Account", account);
        }
    }
    /// <summary>
    /// There are no comments for northwndModel2.Account in the schema.
    /// </summary>
    /// <KeyProperties>
    /// AccountNumber
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="northwndModel2", Name="Account")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::northwndModel2.Loan))]
    public partial class Account : global::System.Data.Objects.DataClasses.EntityObject
    {
        /// <summary>
        /// Create a new Account object.
        /// </summary>
        /// <param name="accountNumber">Initial value of AccountNumber.</param>
        /// <param name="accountType">Initial value of AccountType.</param>
        public static Account CreateAccount(int accountNumber, string accountType)
        {
            Account account = new Account();
            account.AccountNumber = accountNumber;
            account.AccountType = accountType;
            return account;
        }
        /// <summary>
        /// There are no comments for Property AccountNumber in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public int AccountNumber
        {
            get
            {
                return this._AccountNumber;
            }
            set
            {
                this.OnAccountNumberChanging(value);
                this.ReportPropertyChanging("AccountNumber");
                this._AccountNumber = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("AccountNumber");
                this.OnAccountNumberChanged();
            }
        }
        private int _AccountNumber;
        partial void OnAccountNumberChanging(int value);
        partial void OnAccountNumberChanged();
        /// <summary>
        /// There are no comments for Property AccountType in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string AccountType
        {
            get
            {
                return this._AccountType;
            }
            set
            {
                this.OnAccountTypeChanging(value);
                this.ReportPropertyChanging("AccountType");
                this._AccountType = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("AccountType");
                this.OnAccountTypeChanged();
            }
        }
        private string _AccountType;
        partial void OnAccountTypeChanging(string value);
        partial void OnAccountTypeChanged();
        /// <summary>
        /// There are no comments for Property Balance in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public global::System.Nullable<int> Balance
        {
            get
            {
                return this._Balance;
            }
            set
            {
                this.OnBalanceChanging(value);
                this.ReportPropertyChanging("Balance");
                this._Balance = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Balance");
                this.OnBalanceChanged();
            }
        }
        private global::System.Nullable<int> _Balance;
        partial void OnBalanceChanging(global::System.Nullable<int> value);
        partial void OnBalanceChanged();
    }
    /// <summary>
    /// There are no comments for northwndModel2.Loan in the schema.
    /// </summary>
    /// <KeyProperties>
    /// AccountNumber
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="northwndModel2", Name="Loan")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    [global::System.Runtime.Serialization.KnownTypeAttribute(typeof(global::northwndModel2.Morgage))]
    public partial class Loan : Account
    {
        /// <summary>
        /// Create a new Loan object.
        /// </summary>
        /// <param name="accountNumber">Initial value of AccountNumber.</param>
        /// <param name="accountType">Initial value of AccountType.</param>
        public static Loan CreateLoan(int accountNumber, string accountType)
        {
            Loan loan = new Loan();
            loan.AccountNumber = accountNumber;
            loan.AccountType = accountType;
            return loan;
        }
        /// <summary>
        /// There are no comments for Property Term in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public global::System.Nullable<int> Term
        {
            get
            {
                return this._Term;
            }
            set
            {
                this.OnTermChanging(value);
                this.ReportPropertyChanging("Term");
                this._Term = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Term");
                this.OnTermChanged();
            }
        }
        private global::System.Nullable<int> _Term;
        partial void OnTermChanging(global::System.Nullable<int> value);
        partial void OnTermChanged();
        /// <summary>
        /// There are no comments for Property Principal in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public global::System.Nullable<int> Principal
        {
            get
            {
                return this._Principal;
            }
            set
            {
                this.OnPrincipalChanging(value);
                this.ReportPropertyChanging("Principal");
                this._Principal = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Principal");
                this.OnPrincipalChanged();
            }
        }
        private global::System.Nullable<int> _Principal;
        partial void OnPrincipalChanging(global::System.Nullable<int> value);
        partial void OnPrincipalChanged();
    }
    /// <summary>
    /// There are no comments for northwndModel2.Morgage in the schema.
    /// </summary>
    /// <KeyProperties>
    /// AccountNumber
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="northwndModel2", Name="Morgage")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    public partial class Morgage : Loan
    {
        /// <summary>
        /// Create a new Morgage object.
        /// </summary>
        /// <param name="accountNumber">Initial value of AccountNumber.</param>
        /// <param name="accountType">Initial value of AccountType.</param>
        public static Morgage CreateMorgage(int accountNumber, string accountType)
        {
            Morgage morgage = new Morgage();
            morgage.AccountNumber = accountNumber;
            morgage.AccountType = accountType;
            return morgage;
        }
        /// <summary>
        /// There are no comments for Property Tax in the schema.
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public global::System.Nullable<int> Tax
        {
            get
            {
                return this._Tax;
            }
            set
            {
                this.OnTaxChanging(value);
                this.ReportPropertyChanging("Tax");
                this._Tax = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("Tax");
                this.OnTaxChanged();
            }
        }
        private global::System.Nullable<int> _Tax;
        partial void OnTaxChanging(global::System.Nullable<int> value);
        partial void OnTaxChanged();
    }
}
