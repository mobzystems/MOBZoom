using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MOBZystems.Zoom
{
	/// <summary>
	/// Properties Form.
	/// </summary>
	public class PropertiesForm : System.Windows.Forms.Form
	{
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBoxWidth;
    private System.Windows.Forms.TextBox textBoxHeight;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.RadioButton radioButtonOriginalSize;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

    private int imageWidth;
    private System.Windows.Forms.RadioButton radioButtonFrozenSize;
    private GroupBox groupBox1;
    private GroupBox groupBox2;
    private int imageHeight;

		public PropertiesForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
      this.label1 = new System.Windows.Forms.Label();
      this.textBoxWidth = new System.Windows.Forms.TextBox();
      this.textBoxHeight = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.radioButtonFrozenSize = new System.Windows.Forms.RadioButton();
      this.radioButtonOriginalSize = new System.Windows.Forms.RadioButton();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(13, 18);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(80, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "&Width:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.label1.Click += new System.EventHandler(this.label1_Click);
      // 
      // textBoxWidth
      // 
      this.textBoxWidth.Location = new System.Drawing.Point(95, 17);
      this.textBoxWidth.MaxLength = 4;
      this.textBoxWidth.Name = "textBoxWidth";
      this.textBoxWidth.Size = new System.Drawing.Size(67, 21);
      this.textBoxWidth.TabIndex = 1;
      this.textBoxWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // textBoxHeight
      // 
      this.textBoxHeight.Location = new System.Drawing.Point(95, 41);
      this.textBoxHeight.MaxLength = 4;
      this.textBoxHeight.Name = "textBoxHeight";
      this.textBoxHeight.Size = new System.Drawing.Size(67, 21);
      this.textBoxHeight.TabIndex = 4;
      this.textBoxHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(13, 42);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(80, 16);
      this.label2.TabIndex = 3;
      this.label2.Text = "&Height:";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.label2.Click += new System.EventHandler(this.label2_Click);
      // 
      // radioButtonFrozenSize
      // 
      this.radioButtonFrozenSize.AutoSize = true;
      this.radioButtonFrozenSize.Checked = true;
      this.radioButtonFrozenSize.Location = new System.Drawing.Point(95, 20);
      this.radioButtonFrozenSize.Name = "radioButtonFrozenSize";
      this.radioButtonFrozenSize.Size = new System.Drawing.Size(89, 17);
      this.radioButtonFrozenSize.TabIndex = 0;
      this.radioButtonFrozenSize.TabStop = true;
      this.radioButtonFrozenSize.Text = "&Frozen image";
      // 
      // radioButtonOriginalSize
      // 
      this.radioButtonOriginalSize.AutoSize = true;
      this.radioButtonOriginalSize.Location = new System.Drawing.Point(95, 43);
      this.radioButtonOriginalSize.Name = "radioButtonOriginalSize";
      this.radioButtonOriginalSize.Size = new System.Drawing.Size(92, 17);
      this.radioButtonOriginalSize.TabIndex = 1;
      this.radioButtonOriginalSize.Text = "&Original image";
      // 
      // label3
      // 
      this.label3.Location = new System.Drawing.Point(163, 18);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(80, 16);
      this.label3.TabIndex = 2;
      this.label3.Text = "pixels";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // label4
      // 
      this.label4.Location = new System.Drawing.Point(163, 42);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(80, 16);
      this.label4.TabIndex = 5;
      this.label4.Text = "pixels";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // buttonOK
      // 
      this.buttonOK.Location = new System.Drawing.Point(100, 170);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(91, 24);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(197, 170);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(91, 25);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.radioButtonOriginalSize);
      this.groupBox1.Controls.Add(this.radioButtonFrozenSize);
      this.groupBox1.Location = new System.Drawing.Point(12, 91);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(276, 69);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Set size of...";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Controls.Add(this.textBoxHeight);
      this.groupBox2.Controls.Add(this.textBoxWidth);
      this.groupBox2.Controls.Add(this.label2);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Location = new System.Drawing.Point(12, 12);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(276, 73);
      this.groupBox2.TabIndex = 0;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Image size:";
      // 
      // PropertiesForm
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(300, 200);
      this.Controls.Add(this.groupBox2);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOK);
      this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PropertiesForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Image Properties";
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);

    }
		#endregion

    private bool IsValidInteger(string s)
    {
      try
      {
        int.Parse(s);
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    private void buttonOK_Click(object sender, System.EventArgs e)
    {
      try
      {
        if (!IsValidInteger(textBoxWidth.Text))
        {
          MessageBox.Show(this, "Width is not valid", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        if (!IsValidInteger(textBoxHeight.Text))
        {
          MessageBox.Show(this, "Height is not valid", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
        this.imageWidth = int.Parse(textBoxWidth.Text);
        this.imageHeight = int.Parse(textBoxHeight.Text);
        this.DialogResult = DialogResult.OK;
      }
      catch (Exception ex)
      {
        MessageBox.Show(this, ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }

    /// <summary>
    /// Property ImageWidth (int)
    /// </summary>
    public int ImageWidth
    {
      get
      {
        return this.imageWidth;
      }
      set
      {
        this.imageWidth = value;
        textBoxWidth.Text = this.imageWidth.ToString();
      }
    }

    /// <summary>
    /// Property ImageHeight (int)
    /// </summary>
    public int ImageHeight
    {
      get
      {
        return this.imageHeight;
      }
      set
      {
        this.imageHeight = value;
        textBoxHeight.Text = this.imageHeight.ToString();
      }
    }

    /// <summary>
    /// Is this the original size of the image? Else frozen size
    /// </summary>
    public bool IsOriginalSize
    {
      get
      {
        return radioButtonOriginalSize.Checked;
      }
      set 
      {
        radioButtonOriginalSize.Checked = value;
      }
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {

    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void label2_Click(object sender, EventArgs e)
    {

    }
	}
}
