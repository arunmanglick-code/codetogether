namespace WindowsApplication1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnValueStatic = new System.Windows.Forms.Button();
            this.btnValueVirtual = new System.Windows.Forms.Button();
            this.btnValueReference = new System.Windows.Forms.Button();
            this.btnValueEquality = new System.Windows.Forms.Button();
            this.btnShadowMyth = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnReferenceSetNull = new System.Windows.Forms.Button();
            this.btnReferenceReset = new System.Windows.Forms.Button();
            this.btnReferenceClone = new System.Windows.Forms.Button();
            this.btnReferenceStatic = new System.Windows.Forms.Button();
            this.btnReferenceVirtual = new System.Windows.Forms.Button();
            this.btnReferenceReference = new System.Windows.Forms.Button();
            this.btnReferenceEquality = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnStringStatic = new System.Windows.Forms.Button();
            this.btnrStringVirtual = new System.Windows.Forms.Button();
            this.btnStringReference = new System.Windows.Forms.Button();
            this.btnStringEquality = new System.Windows.Forms.Button();
            this.btnSingleton = new System.Windows.Forms.Button();
            this.btnSwap = new System.Windows.Forms.Button();
            this.btnCollectionMyth = new System.Windows.Forms.Button();
            this.btnArraylistMyth = new System.Windows.Forms.Button();
            this.btnEnum = new System.Windows.Forms.Button();
            this.btnWithoutThreadPool = new System.Windows.Forms.Button();
            this.btnShallowCopy = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button10 = new System.Windows.Forms.Button();
            this.btn_MultiInheritanceMyth = new System.Windows.Forms.Button();
            this.btn_PublicPrivateInterfaceInheritanceMyth = new System.Windows.Forms.Button();
            this.btn_UsingMyth = new System.Windows.Forms.Button();
            this.btn_CopyConstructorMyth = new System.Windows.Forms.Button();
            this.btn_TrialCode = new System.Windows.Forms.Button();
            this.btn_IDisposableMyth = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btn_IteratorArray = new System.Windows.Forms.Button();
            this.btn_Iterator = new System.Windows.Forms.Button();
            this.btn_IEnumerableArray = new System.Windows.Forms.Button();
            this.btn_ICollectionArray = new System.Windows.Forms.Button();
            this.btn_IEnumerable = new System.Windows.Forms.Button();
            this.btn_CollectionBase = new System.Windows.Forms.Button();
            this.btn_ICollection = new System.Windows.Forms.Button();
            this.btn_StackTrace = new System.Windows.Forms.Button();
            this.btn_InnerException = new System.Windows.Forms.Button();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnReadAssemblyInfo = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.btn_InheritanceMyth = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnValueStatic);
            this.groupBox1.Controls.Add(this.btnValueVirtual);
            this.groupBox1.Controls.Add(this.btnValueReference);
            this.groupBox1.Controls.Add(this.btnValueEquality);
            this.groupBox1.Location = new System.Drawing.Point(12, 128);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(210, 180);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ValueTypes Equality";
            // 
            // btnValueStatic
            // 
            this.btnValueStatic.Location = new System.Drawing.Point(30, 121);
            this.btnValueStatic.Name = "btnValueStatic";
            this.btnValueStatic.Size = new System.Drawing.Size(157, 23);
            this.btnValueStatic.TabIndex = 3;
            this.btnValueStatic.Text = "StaticEquals(x,y)";
            this.btnValueStatic.UseVisualStyleBackColor = true;
            this.btnValueStatic.Click += new System.EventHandler(this.btnValueStatic_Click);
            // 
            // btnValueVirtual
            // 
            this.btnValueVirtual.Location = new System.Drawing.Point(30, 92);
            this.btnValueVirtual.Name = "btnValueVirtual";
            this.btnValueVirtual.Size = new System.Drawing.Size(157, 23);
            this.btnValueVirtual.TabIndex = 2;
            this.btnValueVirtual.Text = "VirtualEquals(x)";
            this.btnValueVirtual.UseVisualStyleBackColor = true;
            this.btnValueVirtual.Click += new System.EventHandler(this.btnValueVirtual_Click);
            // 
            // btnValueReference
            // 
            this.btnValueReference.Location = new System.Drawing.Point(30, 63);
            this.btnValueReference.Name = "btnValueReference";
            this.btnValueReference.Size = new System.Drawing.Size(157, 23);
            this.btnValueReference.TabIndex = 1;
            this.btnValueReference.Text = "ReferenceEquals(x,y)";
            this.btnValueReference.UseVisualStyleBackColor = true;
            this.btnValueReference.Click += new System.EventHandler(this.btnValueReference_Click);
            // 
            // btnValueEquality
            // 
            this.btnValueEquality.Location = new System.Drawing.Point(30, 34);
            this.btnValueEquality.Name = "btnValueEquality";
            this.btnValueEquality.Size = new System.Drawing.Size(157, 23);
            this.btnValueEquality.TabIndex = 0;
            this.btnValueEquality.Text = "==";
            this.btnValueEquality.UseVisualStyleBackColor = true;
            this.btnValueEquality.Click += new System.EventHandler(this.btnValueEquality_Click);
            // 
            // btnShadowMyth
            // 
            this.btnShadowMyth.BackColor = System.Drawing.Color.Brown;
            this.btnShadowMyth.ForeColor = System.Drawing.Color.White;
            this.btnShadowMyth.Location = new System.Drawing.Point(12, 12);
            this.btnShadowMyth.Name = "btnShadowMyth";
            this.btnShadowMyth.Size = new System.Drawing.Size(97, 23);
            this.btnShadowMyth.TabIndex = 1;
            this.btnShadowMyth.Text = "Shadow Myth";
            this.btnShadowMyth.UseVisualStyleBackColor = false;
            this.btnShadowMyth.Click += new System.EventHandler(this.btnShadowMyth_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnReferenceSetNull);
            this.groupBox2.Controls.Add(this.btnReferenceReset);
            this.groupBox2.Controls.Add(this.btnReferenceClone);
            this.groupBox2.Controls.Add(this.btnReferenceStatic);
            this.groupBox2.Controls.Add(this.btnReferenceVirtual);
            this.groupBox2.Controls.Add(this.btnReferenceReference);
            this.groupBox2.Controls.Add(this.btnReferenceEquality);
            this.groupBox2.Location = new System.Drawing.Point(228, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(381, 180);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ReferenceTypes Equality";
            // 
            // btnReferenceSetNull
            // 
            this.btnReferenceSetNull.Location = new System.Drawing.Point(196, 63);
            this.btnReferenceSetNull.Name = "btnReferenceSetNull";
            this.btnReferenceSetNull.Size = new System.Drawing.Size(157, 23);
            this.btnReferenceSetNull.TabIndex = 6;
            this.btnReferenceSetNull.Text = "Set Null & Check";
            this.btnReferenceSetNull.UseVisualStyleBackColor = true;
            this.btnReferenceSetNull.Click += new System.EventHandler(this.btnReferenceSetNull_Click);
            // 
            // btnReferenceReset
            // 
            this.btnReferenceReset.Location = new System.Drawing.Point(196, 92);
            this.btnReferenceReset.Name = "btnReferenceReset";
            this.btnReferenceReset.Size = new System.Drawing.Size(157, 23);
            this.btnReferenceReset.TabIndex = 5;
            this.btnReferenceReset.Text = "Re Instantiate";
            this.btnReferenceReset.UseVisualStyleBackColor = true;
            this.btnReferenceReset.Click += new System.EventHandler(this.btnReferenceRemoveClone_Click);
            // 
            // btnReferenceClone
            // 
            this.btnReferenceClone.Location = new System.Drawing.Point(196, 34);
            this.btnReferenceClone.Name = "btnReferenceClone";
            this.btnReferenceClone.Size = new System.Drawing.Size(157, 23);
            this.btnReferenceClone.TabIndex = 4;
            this.btnReferenceClone.Text = "Make Clone";
            this.btnReferenceClone.UseVisualStyleBackColor = true;
            this.btnReferenceClone.Click += new System.EventHandler(this.btnReferenceClone_Click);
            // 
            // btnReferenceStatic
            // 
            this.btnReferenceStatic.Location = new System.Drawing.Point(30, 121);
            this.btnReferenceStatic.Name = "btnReferenceStatic";
            this.btnReferenceStatic.Size = new System.Drawing.Size(157, 23);
            this.btnReferenceStatic.TabIndex = 3;
            this.btnReferenceStatic.Text = "StaticEquals(x,y)";
            this.btnReferenceStatic.UseVisualStyleBackColor = true;
            this.btnReferenceStatic.Click += new System.EventHandler(this.btnReferenceStatic_Click);
            // 
            // btnReferenceVirtual
            // 
            this.btnReferenceVirtual.Location = new System.Drawing.Point(30, 92);
            this.btnReferenceVirtual.Name = "btnReferenceVirtual";
            this.btnReferenceVirtual.Size = new System.Drawing.Size(157, 23);
            this.btnReferenceVirtual.TabIndex = 2;
            this.btnReferenceVirtual.Text = "VirtualEquals(x)";
            this.btnReferenceVirtual.UseVisualStyleBackColor = true;
            this.btnReferenceVirtual.Click += new System.EventHandler(this.btnReferenceVirtual_Click);
            // 
            // btnReferenceReference
            // 
            this.btnReferenceReference.Location = new System.Drawing.Point(30, 63);
            this.btnReferenceReference.Name = "btnReferenceReference";
            this.btnReferenceReference.Size = new System.Drawing.Size(157, 23);
            this.btnReferenceReference.TabIndex = 1;
            this.btnReferenceReference.Text = "ReferenceEquals(x,y)";
            this.btnReferenceReference.UseVisualStyleBackColor = true;
            this.btnReferenceReference.Click += new System.EventHandler(this.btnReferenceReference_Click);
            // 
            // btnReferenceEquality
            // 
            this.btnReferenceEquality.Location = new System.Drawing.Point(30, 34);
            this.btnReferenceEquality.Name = "btnReferenceEquality";
            this.btnReferenceEquality.Size = new System.Drawing.Size(157, 23);
            this.btnReferenceEquality.TabIndex = 0;
            this.btnReferenceEquality.Text = "==";
            this.btnReferenceEquality.UseVisualStyleBackColor = true;
            this.btnReferenceEquality.Click += new System.EventHandler(this.btnReferenceEquality_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.groupBox3.Controls.Add(this.btnStringStatic);
            this.groupBox3.Controls.Add(this.btnrStringVirtual);
            this.groupBox3.Controls.Add(this.btnStringReference);
            this.groupBox3.Controls.Add(this.btnStringEquality);
            this.groupBox3.Location = new System.Drawing.Point(615, 128);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(218, 180);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "String Types Equality";
            // 
            // btnStringStatic
            // 
            this.btnStringStatic.Location = new System.Drawing.Point(30, 121);
            this.btnStringStatic.Name = "btnStringStatic";
            this.btnStringStatic.Size = new System.Drawing.Size(157, 23);
            this.btnStringStatic.TabIndex = 3;
            this.btnStringStatic.Text = "StaticEquals(x,y)";
            this.btnStringStatic.UseVisualStyleBackColor = true;
            this.btnStringStatic.Click += new System.EventHandler(this.btnStringStatic_Click);
            // 
            // btnrStringVirtual
            // 
            this.btnrStringVirtual.Location = new System.Drawing.Point(30, 92);
            this.btnrStringVirtual.Name = "btnrStringVirtual";
            this.btnrStringVirtual.Size = new System.Drawing.Size(157, 23);
            this.btnrStringVirtual.TabIndex = 2;
            this.btnrStringVirtual.Text = "VirtualEquals(x)";
            this.btnrStringVirtual.UseVisualStyleBackColor = true;
            this.btnrStringVirtual.Click += new System.EventHandler(this.btnrStringVirtual_Click);
            // 
            // btnStringReference
            // 
            this.btnStringReference.Location = new System.Drawing.Point(30, 63);
            this.btnStringReference.Name = "btnStringReference";
            this.btnStringReference.Size = new System.Drawing.Size(157, 23);
            this.btnStringReference.TabIndex = 1;
            this.btnStringReference.Text = "ReferenceEquals(x,y)";
            this.btnStringReference.UseVisualStyleBackColor = true;
            this.btnStringReference.Click += new System.EventHandler(this.btnStringReference_Click);
            // 
            // btnStringEquality
            // 
            this.btnStringEquality.Location = new System.Drawing.Point(30, 34);
            this.btnStringEquality.Name = "btnStringEquality";
            this.btnStringEquality.Size = new System.Drawing.Size(157, 23);
            this.btnStringEquality.TabIndex = 0;
            this.btnStringEquality.Text = "==";
            this.btnStringEquality.UseVisualStyleBackColor = true;
            this.btnStringEquality.Click += new System.EventHandler(this.button7_Click);
            // 
            // btnSingleton
            // 
            this.btnSingleton.Location = new System.Drawing.Point(124, 12);
            this.btnSingleton.Name = "btnSingleton";
            this.btnSingleton.Size = new System.Drawing.Size(192, 23);
            this.btnSingleton.TabIndex = 8;
            this.btnSingleton.Text = "ThreadSafe Singleton";
            this.btnSingleton.UseVisualStyleBackColor = true;
            this.btnSingleton.Click += new System.EventHandler(this.btnSingleton_Click);
            // 
            // btnSwap
            // 
            this.btnSwap.BackColor = System.Drawing.Color.Brown;
            this.btnSwap.ForeColor = System.Drawing.Color.White;
            this.btnSwap.Location = new System.Drawing.Point(340, 12);
            this.btnSwap.Name = "btnSwap";
            this.btnSwap.Size = new System.Drawing.Size(75, 23);
            this.btnSwap.TabIndex = 9;
            this.btnSwap.Text = "Swap Myth";
            this.btnSwap.UseVisualStyleBackColor = false;
            this.btnSwap.Click += new System.EventHandler(this.btnSwap_Click);
            // 
            // btnCollectionMyth
            // 
            this.btnCollectionMyth.Location = new System.Drawing.Point(502, 12);
            this.btnCollectionMyth.Name = "btnCollectionMyth";
            this.btnCollectionMyth.Size = new System.Drawing.Size(196, 23);
            this.btnCollectionMyth.TabIndex = 10;
            this.btnCollectionMyth.Text = "Runtime Modify HashTable Myth";
            this.btnCollectionMyth.UseVisualStyleBackColor = true;
            this.btnCollectionMyth.Click += new System.EventHandler(this.btnCollectionMyth_Click);
            // 
            // btnArraylistMyth
            // 
            this.btnArraylistMyth.Location = new System.Drawing.Point(502, 70);
            this.btnArraylistMyth.Name = "btnArraylistMyth";
            this.btnArraylistMyth.Size = new System.Drawing.Size(196, 23);
            this.btnArraylistMyth.TabIndex = 11;
            this.btnArraylistMyth.Text = "Runtime Modify ArrayList Myth";
            this.btnArraylistMyth.UseVisualStyleBackColor = true;
            this.btnArraylistMyth.Click += new System.EventHandler(this.btnArraylistMyth_Click);
            // 
            // btnEnum
            // 
            this.btnEnum.Location = new System.Drawing.Point(12, 41);
            this.btnEnum.Name = "btnEnum";
            this.btnEnum.Size = new System.Drawing.Size(97, 23);
            this.btnEnum.TabIndex = 12;
            this.btnEnum.Text = "Enum Myth";
            this.btnEnum.UseVisualStyleBackColor = true;
            this.btnEnum.Click += new System.EventHandler(this.btnEnum_Click);
            // 
            // btnWithoutThreadPool
            // 
            this.btnWithoutThreadPool.Location = new System.Drawing.Point(124, 41);
            this.btnWithoutThreadPool.Name = "btnWithoutThreadPool";
            this.btnWithoutThreadPool.Size = new System.Drawing.Size(192, 23);
            this.btnWithoutThreadPool.TabIndex = 13;
            this.btnWithoutThreadPool.Text = "MultiThreading w/o Thread Pool";
            this.btnWithoutThreadPool.UseVisualStyleBackColor = true;
            this.btnWithoutThreadPool.Click += new System.EventHandler(this.btnWithoutThreadPool_Click);
            // 
            // btnShallowCopy
            // 
            this.btnShallowCopy.BackColor = System.Drawing.Color.Brown;
            this.btnShallowCopy.ForeColor = System.Drawing.Color.White;
            this.btnShallowCopy.Location = new System.Drawing.Point(340, 41);
            this.btnShallowCopy.Name = "btnShallowCopy";
            this.btnShallowCopy.Size = new System.Drawing.Size(122, 23);
            this.btnShallowCopy.TabIndex = 14;
            this.btnShallowCopy.Text = "Shallow Copy Myth";
            this.btnShallowCopy.UseVisualStyleBackColor = false;
            this.btnShallowCopy.Click += new System.EventHandler(this.btnShallowCopy_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button6);
            this.groupBox4.Controls.Add(this.button5);
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Controls.Add(this.button3);
            this.groupBox4.Controls.Add(this.button4);
            this.groupBox4.Location = new System.Drawing.Point(12, 326);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(617, 135);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "CLR Profile";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(412, 34);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(157, 23);
            this.button6.TabIndex = 5;
            this.button6.Text = "Profiled Code - Using String";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(216, 63);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(157, 23);
            this.button5.TabIndex = 4;
            this.button5.Text = "Profiled Code - Brush";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(216, 34);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(157, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Profiled Code - String";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(30, 92);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(157, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Code - Stream";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(30, 63);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(157, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "Code - Brush";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(30, 34);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(157, 23);
            this.button4.TabIndex = 0;
            this.button4.Text = "Code - String";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button10);
            this.groupBox5.Location = new System.Drawing.Point(645, 326);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(390, 135);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "GC Myth";
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(30, 34);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(157, 23);
            this.button10.TabIndex = 0;
            this.button10.Text = "GC Call";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // btn_MultiInheritanceMyth
            // 
            this.btn_MultiInheritanceMyth.BackColor = System.Drawing.Color.Brown;
            this.btn_MultiInheritanceMyth.ForeColor = System.Drawing.Color.White;
            this.btn_MultiInheritanceMyth.Location = new System.Drawing.Point(912, 12);
            this.btn_MultiInheritanceMyth.Name = "btn_MultiInheritanceMyth";
            this.btn_MultiInheritanceMyth.Size = new System.Drawing.Size(192, 23);
            this.btn_MultiInheritanceMyth.TabIndex = 15;
            this.btn_MultiInheritanceMyth.Text = "MultiInheritanceMyth";
            this.btn_MultiInheritanceMyth.UseVisualStyleBackColor = false;
            this.btn_MultiInheritanceMyth.Click += new System.EventHandler(this.btn_MultiInheritanceMyth_Click);
            // 
            // btn_PublicPrivateInterfaceInheritanceMyth
            // 
            this.btn_PublicPrivateInterfaceInheritanceMyth.Location = new System.Drawing.Point(704, 12);
            this.btn_PublicPrivateInterfaceInheritanceMyth.Name = "btn_PublicPrivateInterfaceInheritanceMyth";
            this.btn_PublicPrivateInterfaceInheritanceMyth.Size = new System.Drawing.Size(192, 23);
            this.btn_PublicPrivateInterfaceInheritanceMyth.TabIndex = 16;
            this.btn_PublicPrivateInterfaceInheritanceMyth.Text = "PublicPrivateInterfaceInheritanceMyth";
            this.btn_PublicPrivateInterfaceInheritanceMyth.UseVisualStyleBackColor = true;
            this.btn_PublicPrivateInterfaceInheritanceMyth.Click += new System.EventHandler(this.btn_PublicPrivateInterfaceInheritanceMyth_Click);
            // 
            // btn_UsingMyth
            // 
            this.btn_UsingMyth.BackColor = System.Drawing.Color.Brown;
            this.btn_UsingMyth.ForeColor = System.Drawing.Color.White;
            this.btn_UsingMyth.Location = new System.Drawing.Point(12, 70);
            this.btn_UsingMyth.Name = "btn_UsingMyth";
            this.btn_UsingMyth.Size = new System.Drawing.Size(97, 23);
            this.btn_UsingMyth.TabIndex = 17;
            this.btn_UsingMyth.Text = "Using Myth";
            this.btn_UsingMyth.UseVisualStyleBackColor = false;
            this.btn_UsingMyth.Click += new System.EventHandler(this.btn_UsingMyth_Click);
            // 
            // btn_CopyConstructorMyth
            // 
            this.btn_CopyConstructorMyth.BackColor = System.Drawing.Color.Brown;
            this.btn_CopyConstructorMyth.ForeColor = System.Drawing.Color.White;
            this.btn_CopyConstructorMyth.Location = new System.Drawing.Point(125, 70);
            this.btn_CopyConstructorMyth.Name = "btn_CopyConstructorMyth";
            this.btn_CopyConstructorMyth.Size = new System.Drawing.Size(150, 23);
            this.btn_CopyConstructorMyth.TabIndex = 18;
            this.btn_CopyConstructorMyth.Text = "Copy Constructor Myth";
            this.btn_CopyConstructorMyth.UseVisualStyleBackColor = false;
            this.btn_CopyConstructorMyth.Click += new System.EventHandler(this.btn_CopyConstructorMyth_Click);
            // 
            // btn_TrialCode
            // 
            this.btn_TrialCode.Location = new System.Drawing.Point(730, 70);
            this.btn_TrialCode.Name = "btn_TrialCode";
            this.btn_TrialCode.Size = new System.Drawing.Size(150, 23);
            this.btn_TrialCode.TabIndex = 19;
            this.btn_TrialCode.Text = "Trial Code";
            this.btn_TrialCode.UseVisualStyleBackColor = true;
            this.btn_TrialCode.Click += new System.EventHandler(this.btn_TrialCode_Click);
            // 
            // btn_IDisposableMyth
            // 
            this.btn_IDisposableMyth.BackColor = System.Drawing.Color.Brown;
            this.btn_IDisposableMyth.ForeColor = System.Drawing.Color.White;
            this.btn_IDisposableMyth.Location = new System.Drawing.Point(340, 99);
            this.btn_IDisposableMyth.Name = "btn_IDisposableMyth";
            this.btn_IDisposableMyth.Size = new System.Drawing.Size(150, 23);
            this.btn_IDisposableMyth.TabIndex = 20;
            this.btn_IDisposableMyth.Text = "IDisposable Myth";
            this.btn_IDisposableMyth.UseVisualStyleBackColor = false;
            this.btn_IDisposableMyth.Click += new System.EventHandler(this.btn_IDisposableMyth_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btn_IteratorArray);
            this.groupBox6.Controls.Add(this.btn_Iterator);
            this.groupBox6.Controls.Add(this.btn_IEnumerableArray);
            this.groupBox6.Controls.Add(this.btn_ICollectionArray);
            this.groupBox6.Controls.Add(this.btn_IEnumerable);
            this.groupBox6.Controls.Add(this.btn_CollectionBase);
            this.groupBox6.Controls.Add(this.btn_ICollection);
            this.groupBox6.Location = new System.Drawing.Point(12, 467);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(250, 170);
            this.groupBox6.TabIndex = 6;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Collection Myth";
            // 
            // btn_IteratorArray
            // 
            this.btn_IteratorArray.Location = new System.Drawing.Point(119, 106);
            this.btn_IteratorArray.Name = "btn_IteratorArray";
            this.btn_IteratorArray.Size = new System.Drawing.Size(91, 23);
            this.btn_IteratorArray.TabIndex = 6;
            this.btn_IteratorArray.Text = "Iterator on Array";
            this.btn_IteratorArray.UseVisualStyleBackColor = true;
            this.btn_IteratorArray.Click += new System.EventHandler(this.btn_IteratorArray_Click);
            // 
            // btn_Iterator
            // 
            this.btn_Iterator.Location = new System.Drawing.Point(6, 106);
            this.btn_Iterator.Name = "btn_Iterator";
            this.btn_Iterator.Size = new System.Drawing.Size(91, 23);
            this.btn_Iterator.TabIndex = 5;
            this.btn_Iterator.Text = "Iterator";
            this.btn_Iterator.UseVisualStyleBackColor = true;
            this.btn_Iterator.Click += new System.EventHandler(this.btn_Iterator_Click);
            // 
            // btn_IEnumerableArray
            // 
            this.btn_IEnumerableArray.Location = new System.Drawing.Point(113, 19);
            this.btn_IEnumerableArray.Name = "btn_IEnumerableArray";
            this.btn_IEnumerableArray.Size = new System.Drawing.Size(118, 23);
            this.btn_IEnumerableArray.TabIndex = 4;
            this.btn_IEnumerableArray.Text = "IEnumerable on Array";
            this.btn_IEnumerableArray.UseVisualStyleBackColor = true;
            this.btn_IEnumerableArray.Click += new System.EventHandler(this.btn_IEnumerableArray_Click);
            // 
            // btn_ICollectionArray
            // 
            this.btn_ICollectionArray.Location = new System.Drawing.Point(113, 48);
            this.btn_ICollectionArray.Name = "btn_ICollectionArray";
            this.btn_ICollectionArray.Size = new System.Drawing.Size(118, 23);
            this.btn_ICollectionArray.TabIndex = 3;
            this.btn_ICollectionArray.Text = "ICollection on Array";
            this.btn_ICollectionArray.UseVisualStyleBackColor = true;
            this.btn_ICollectionArray.Click += new System.EventHandler(this.btn_ICollectionArray_Click);
            // 
            // btn_IEnumerable
            // 
            this.btn_IEnumerable.Location = new System.Drawing.Point(6, 19);
            this.btn_IEnumerable.Name = "btn_IEnumerable";
            this.btn_IEnumerable.Size = new System.Drawing.Size(91, 23);
            this.btn_IEnumerable.TabIndex = 2;
            this.btn_IEnumerable.Text = "IEnumerable";
            this.btn_IEnumerable.UseVisualStyleBackColor = true;
            this.btn_IEnumerable.Click += new System.EventHandler(this.btn_IEnumerable_Click);
            // 
            // btn_CollectionBase
            // 
            this.btn_CollectionBase.Location = new System.Drawing.Point(6, 77);
            this.btn_CollectionBase.Name = "btn_CollectionBase";
            this.btn_CollectionBase.Size = new System.Drawing.Size(91, 23);
            this.btn_CollectionBase.TabIndex = 1;
            this.btn_CollectionBase.Text = "CollectionBase";
            this.btn_CollectionBase.UseVisualStyleBackColor = true;
            this.btn_CollectionBase.Click += new System.EventHandler(this.btn_CollectionBase_Click);
            // 
            // btn_ICollection
            // 
            this.btn_ICollection.Location = new System.Drawing.Point(6, 48);
            this.btn_ICollection.Name = "btn_ICollection";
            this.btn_ICollection.Size = new System.Drawing.Size(91, 23);
            this.btn_ICollection.TabIndex = 0;
            this.btn_ICollection.Text = "ICollection";
            this.btn_ICollection.UseVisualStyleBackColor = true;
            this.btn_ICollection.Click += new System.EventHandler(this.btn_ICollection_Click);
            // 
            // btn_StackTrace
            // 
            this.btn_StackTrace.BackColor = System.Drawing.Color.Brown;
            this.btn_StackTrace.ForeColor = System.Drawing.Color.White;
            this.btn_StackTrace.Location = new System.Drawing.Point(17, 29);
            this.btn_StackTrace.Name = "btn_StackTrace";
            this.btn_StackTrace.Size = new System.Drawing.Size(150, 23);
            this.btn_StackTrace.TabIndex = 21;
            this.btn_StackTrace.Text = "StackTrace Myth";
            this.btn_StackTrace.UseVisualStyleBackColor = false;
            this.btn_StackTrace.Click += new System.EventHandler(this.btn_StackTrace_Click);
            // 
            // btn_InnerException
            // 
            this.btn_InnerException.BackColor = System.Drawing.Color.Brown;
            this.btn_InnerException.ForeColor = System.Drawing.Color.White;
            this.btn_InnerException.Location = new System.Drawing.Point(17, 68);
            this.btn_InnerException.Name = "btn_InnerException";
            this.btn_InnerException.Size = new System.Drawing.Size(150, 23);
            this.btn_InnerException.TabIndex = 22;
            this.btn_InnerException.Text = "InnerException Myth";
            this.btn_InnerException.UseVisualStyleBackColor = false;
            this.btn_InnerException.Click += new System.EventHandler(this.btn_InnerException_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.button8);
            this.groupBox7.Controls.Add(this.button7);
            this.groupBox7.Controls.Add(this.btn_InnerException);
            this.groupBox7.Controls.Add(this.btn_StackTrace);
            this.groupBox7.Location = new System.Drawing.Point(280, 476);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(390, 135);
            this.groupBox7.TabIndex = 6;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "GC Myth";
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(179, 68);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(150, 23);
            this.button8.TabIndex = 23;
            this.button8.Text = "Generics Myth";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(179, 29);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(150, 23);
            this.button7.TabIndex = 22;
            this.button7.Text = "Generics Myth";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click_1);
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.DropDownWidth = 200;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBox1.Items.AddRange(new object[] {
            "AAAAAAAAAAAAAAAAAAAA",
            "BBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB",
            "CCCCCCCCCCC"});
            this.comboBox1.Location = new System.Drawing.Point(680, 505);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(84, 21);
            this.comboBox1.TabIndex = 21;
            // 
            // btnReadAssemblyInfo
            // 
            this.btnReadAssemblyInfo.Location = new System.Drawing.Point(704, 41);
            this.btnReadAssemblyInfo.Name = "btnReadAssemblyInfo";
            this.btnReadAssemblyInfo.Size = new System.Drawing.Size(187, 23);
            this.btnReadAssemblyInfo.TabIndex = 22;
            this.btnReadAssemblyInfo.Text = "Read AssemblyInfo";
            this.btnReadAssemblyInfo.UseVisualStyleBackColor = true;
            this.btnReadAssemblyInfo.Click += new System.EventHandler(this.btnReadAssemblyInfo_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(502, 41);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(150, 23);
            this.button9.TabIndex = 23;
            this.button9.Text = "Enum";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.Color.Brown;
            this.button11.ForeColor = System.Drawing.Color.White;
            this.button11.Location = new System.Drawing.Point(912, 70);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(150, 23);
            this.button11.TabIndex = 24;
            this.button11.Text = "Abstract Myth";
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // btn_InheritanceMyth
            // 
            this.btn_InheritanceMyth.BackColor = System.Drawing.Color.Brown;
            this.btn_InheritanceMyth.ForeColor = System.Drawing.Color.White;
            this.btn_InheritanceMyth.Location = new System.Drawing.Point(912, 41);
            this.btn_InheritanceMyth.Name = "btn_InheritanceMyth";
            this.btn_InheritanceMyth.Size = new System.Drawing.Size(192, 23);
            this.btn_InheritanceMyth.TabIndex = 25;
            this.btn_InheritanceMyth.Text = "InheritanceMyth";
            this.btn_InheritanceMyth.UseVisualStyleBackColor = false;
            this.btn_InheritanceMyth.Click += new System.EventHandler(this.btn_InheritanceMyth_Click);
            // 
            // button12
            // 
            this.button12.BackColor = System.Drawing.Color.Brown;
            this.button12.ForeColor = System.Drawing.Color.White;
            this.button12.Location = new System.Drawing.Point(340, 70);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(122, 23);
            this.button12.TabIndex = 26;
            this.button12.Text = "Shallow Copy Myth";
            this.button12.UseVisualStyleBackColor = false;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 727);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.btn_InheritanceMyth);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.btnReadAssemblyInfo);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.btn_IDisposableMyth);
            this.Controls.Add(this.btn_TrialCode);
            this.Controls.Add(this.btn_CopyConstructorMyth);
            this.Controls.Add(this.btn_UsingMyth);
            this.Controls.Add(this.btn_PublicPrivateInterfaceInheritanceMyth);
            this.Controls.Add(this.btn_MultiInheritanceMyth);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.btnShallowCopy);
            this.Controls.Add(this.btnWithoutThreadPool);
            this.Controls.Add(this.btnEnum);
            this.Controls.Add(this.btnArraylistMyth);
            this.Controls.Add(this.btnCollectionMyth);
            this.Controls.Add(this.btnSwap);
            this.Controls.Add(this.btnSingleton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnShadowMyth);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnValueStatic;
        private System.Windows.Forms.Button btnValueVirtual;
        private System.Windows.Forms.Button btnValueReference;
        private System.Windows.Forms.Button btnValueEquality;
        private System.Windows.Forms.Button btnShadowMyth;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnReferenceStatic;
        private System.Windows.Forms.Button btnReferenceVirtual;
        private System.Windows.Forms.Button btnReferenceReference;
        private System.Windows.Forms.Button btnReferenceEquality;
        private System.Windows.Forms.Button btnReferenceClone;
        private System.Windows.Forms.Button btnReferenceReset;
        private System.Windows.Forms.Button btnReferenceSetNull;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnStringStatic;
        private System.Windows.Forms.Button btnrStringVirtual;
        private System.Windows.Forms.Button btnStringReference;
        private System.Windows.Forms.Button btnStringEquality;
        private System.Windows.Forms.Button btnSingleton;
        private System.Windows.Forms.Button btnSwap;
        private System.Windows.Forms.Button btnCollectionMyth;
        private System.Windows.Forms.Button btnArraylistMyth;
        private System.Windows.Forms.Button btnEnum;
        private System.Windows.Forms.Button btnWithoutThreadPool;
        private System.Windows.Forms.Button btnShallowCopy;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btn_MultiInheritanceMyth;
        private System.Windows.Forms.Button btn_PublicPrivateInterfaceInheritanceMyth;
        private System.Windows.Forms.Button btn_UsingMyth;
        private System.Windows.Forms.Button btn_CopyConstructorMyth;
        private System.Windows.Forms.Button btn_TrialCode;
        private System.Windows.Forms.Button btn_IDisposableMyth;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btn_ICollection;
        private System.Windows.Forms.Button btn_CollectionBase;
        private System.Windows.Forms.Button btn_IEnumerable;
        private System.Windows.Forms.Button btn_ICollectionArray;
        private System.Windows.Forms.Button btn_IEnumerableArray;
        private System.Windows.Forms.Button btn_Iterator;
        private System.Windows.Forms.Button btn_IteratorArray;
        private System.Windows.Forms.Button btn_StackTrace;
        private System.Windows.Forms.Button btn_InnerException;
        private System.Windows.Forms.GroupBox groupBox7;
        protected internal System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button btnReadAssemblyInfo;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button btn_InheritanceMyth;
        private System.Windows.Forms.Button button12;

    }
}

