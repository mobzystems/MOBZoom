using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

// TODO: suggest better file name on second and later saves
// TODO: crop bitmap better (now uses picturebox size, should be bitmap size)

namespace MOBZystems.Zoom
{
  /// <summary>
  /// The main form of MOBZoom
  /// </summary>
  public class ZoomForm : System.Windows.Forms.Form
  {
    [DllImport("User32")]
    private static extern IntPtr GetDesktopWindow();

    [DllImportAttribute("gdi32.dll")]
    private static extern bool BitBlt(
      IntPtr hdcDest,       // handle to destination DC 
      int nXDest,           // x-coord of destination upper-left corner 
      int nYDest,           // y-coord of destination upper-left corner 
      int nWidth,           // width of destination rectangle 
      int nHeight,          // height of destination rectangle 
      IntPtr hdcSrc,        // handle to source DC 
      int nXSrc,            // x-coordinate of source upper-left corner 
      int nYSrc,            // y-coordinate of source upper-left corner 
      System.Int32 dwRop    // raster operation code 
      );

    [DllImportAttribute("gdi32.dll")]
    private static extern bool StretchBlt(
      IntPtr hdcDest,       // handle to destination DC
      int nXOriginDest,     // x-coord of destination upper-left corner
      int nYOriginDest,     // y-coord of destination upper-left corner
      int nWidthDest,       // width of destination rectangle
      int nHeightDest,      // height of destination rectangle
      IntPtr hdcSrc,        // handle to source DC
      int nXOriginSrc,      // x-coord of source upper-left corner
      int nYOriginSrc,      // y-coord of source upper-left corner
      int nWidthSrc,        // width of source rectangle
      int nHeightSrc,       // height of source rectangle
      System.Int32 dwRop    // raster operation code
      );

    [DllImport("user32.dll")]
    private static extern IntPtr WindowFromPoint(System.Drawing.Point point);

    private System.Windows.Forms.MainMenu mainMenu;
    private System.Windows.Forms.MenuItem menuItemViewmainStatusStrip;
    private System.Windows.Forms.MenuItem menuItemViewAlwaysOnTop;
    private System.Windows.Forms.MenuItem menuItemView;
    private System.Windows.Forms.MenuItem menuItemEdit;
    private System.Windows.Forms.MenuItem menuItemZoom;
    private System.Windows.Forms.MenuItem menuItemZoom1x;
    private System.Windows.Forms.MenuItem menuItemZoom2x;
    private System.Windows.Forms.MenuItem menuItemZoom4x;
    private System.Windows.Forms.MenuItem menuItemZoom8x;
    private System.Windows.Forms.MenuItem menuItemZoom16x;
    private System.Windows.Forms.MenuItem menuItemFile;
    private System.Windows.Forms.MenuItem menuItemFileSaveAs;
    private System.Windows.Forms.MenuItem menuItemFileSep1;
    private System.Windows.Forms.MenuItem menuItemFileExit;
    private System.Windows.Forms.MenuItem menuItemEditFreeze;
    private System.Windows.Forms.MenuItem menuItemEditUnfreeze;
    private System.Windows.Forms.MenuItem menuItemEditSep1;
    private System.Windows.Forms.MenuItem menuItemEditCopy;
    private System.Windows.Forms.MenuItem menuItemHelp;
    private System.Windows.Forms.MenuItem menuItemHelpAbout;
    private System.Windows.Forms.MenuItem menuItemViewShrinkToFit;
    private System.Windows.Forms.MenuItem menuItemViewSep1;
    private System.Windows.Forms.MenuItem menuItemEditSep2;
    private System.Windows.Forms.MenuItem menuItemEditCrop;
    private System.Windows.Forms.MenuItem menuItemEditProperties;
    private System.Windows.Forms.MenuItem menuItemEditSep3;
    private StatusStrip mainStatusStrip;
    private ToolStripStatusLabel toolStripStatusLabelPrompt;
    private ToolStripStatusLabel toolStripStatusLabelPos;
    private ToolStripStatusLabel toolStripStatusLabelSize;
    private ToolStripStatusLabel toolStripStatusLabelLink;

    private const Int32 SRCCOPY = 0xCC0020;
    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
      public int left;
      public int top;
      public int right;
      public int bottom;
    }
    [DllImport("user32.dll")]
    private static extern int
      GetWindowRect(IntPtr hwnd, out RECT rc);

    private int zoom = 2;
    private Bitmap bitmap;
    private bool frozen = false;
    private int lastSaveFilterIndex;
    private string lastSaveDir = null;

    private System.Timers.Timer timerMouse;
    private System.Windows.Forms.PictureBox pictureBox;
    private IContainer components;

    public ZoomForm()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();

      Version version = new Version(Application.ProductVersion);
      this.toolStripStatusLabelLink.Text = "MOBZoom v" + version.Major.ToString() + "." + version.Minor.ToString() + "." + version.Build.ToString() + " by MOBZystems";
    }

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZoomForm));
      this.timerMouse = new System.Timers.Timer();
      this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
      this.menuItemFile = new System.Windows.Forms.MenuItem();
      this.menuItemFileSaveAs = new System.Windows.Forms.MenuItem();
      this.menuItemFileSep1 = new System.Windows.Forms.MenuItem();
      this.menuItemFileExit = new System.Windows.Forms.MenuItem();
      this.menuItemEdit = new System.Windows.Forms.MenuItem();
      this.menuItemEditFreeze = new System.Windows.Forms.MenuItem();
      this.menuItemEditUnfreeze = new System.Windows.Forms.MenuItem();
      this.menuItemEditSep1 = new System.Windows.Forms.MenuItem();
      this.menuItemEditCrop = new System.Windows.Forms.MenuItem();
      this.menuItemEditSep2 = new System.Windows.Forms.MenuItem();
      this.menuItemEditCopy = new System.Windows.Forms.MenuItem();
      this.menuItemEditSep3 = new System.Windows.Forms.MenuItem();
      this.menuItemEditProperties = new System.Windows.Forms.MenuItem();
      this.menuItemView = new System.Windows.Forms.MenuItem();
      this.menuItemViewShrinkToFit = new System.Windows.Forms.MenuItem();
      this.menuItemViewSep1 = new System.Windows.Forms.MenuItem();
      this.menuItemViewmainStatusStrip = new System.Windows.Forms.MenuItem();
      this.menuItemViewAlwaysOnTop = new System.Windows.Forms.MenuItem();
      this.menuItemZoom = new System.Windows.Forms.MenuItem();
      this.menuItemZoom1x = new System.Windows.Forms.MenuItem();
      this.menuItemZoom2x = new System.Windows.Forms.MenuItem();
      this.menuItemZoom4x = new System.Windows.Forms.MenuItem();
      this.menuItemZoom8x = new System.Windows.Forms.MenuItem();
      this.menuItemZoom16x = new System.Windows.Forms.MenuItem();
      this.menuItemHelp = new System.Windows.Forms.MenuItem();
      this.menuItemHelpAbout = new System.Windows.Forms.MenuItem();
      this.pictureBox = new System.Windows.Forms.PictureBox();
      this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabelPrompt = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabelPos = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabelSize = new System.Windows.Forms.ToolStripStatusLabel();
      this.toolStripStatusLabelLink = new System.Windows.Forms.ToolStripStatusLabel();
      ((System.ComponentModel.ISupportInitialize)(this.timerMouse)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
      this.mainStatusStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // timerMouse
      // 
      this.timerMouse.Enabled = true;
      this.timerMouse.Interval = 200;
      this.timerMouse.SynchronizingObject = this;
      this.timerMouse.Elapsed += new System.Timers.ElapsedEventHandler(this.timerMouse_Elapsed);
      // 
      // mainMenu
      // 
      this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFile,
            this.menuItemEdit,
            this.menuItemView,
            this.menuItemZoom,
            this.menuItemHelp});
      // 
      // menuItemFile
      // 
      this.menuItemFile.Index = 0;
      this.menuItemFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemFileSaveAs,
            this.menuItemFileSep1,
            this.menuItemFileExit});
      this.menuItemFile.Text = "&File";
      // 
      // menuItemFileSaveAs
      // 
      this.menuItemFileSaveAs.Enabled = false;
      this.menuItemFileSaveAs.Index = 0;
      this.menuItemFileSaveAs.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
      this.menuItemFileSaveAs.Text = "&Save image as...";
      this.menuItemFileSaveAs.Click += new System.EventHandler(this.menuItemFileSaveAs_Click);
      // 
      // menuItemFileSep1
      // 
      this.menuItemFileSep1.Index = 1;
      this.menuItemFileSep1.Text = "-";
      // 
      // menuItemFileExit
      // 
      this.menuItemFileExit.Index = 2;
      this.menuItemFileExit.Text = "E&xit";
      this.menuItemFileExit.Click += new System.EventHandler(this.menuItemFileExit_Click);
      // 
      // menuItemEdit
      // 
      this.menuItemEdit.Index = 1;
      this.menuItemEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemEditFreeze,
            this.menuItemEditUnfreeze,
            this.menuItemEditSep1,
            this.menuItemEditCopy,
            this.menuItemEditSep2,
            this.menuItemEditCrop,
            this.menuItemEditSep3,
            this.menuItemEditProperties});
      this.menuItemEdit.Text = "&Edit";
      // 
      // menuItemEditFreeze
      // 
      this.menuItemEditFreeze.Index = 0;
      this.menuItemEditFreeze.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
      this.menuItemEditFreeze.Text = "&Freeze image";
      this.menuItemEditFreeze.Click += new System.EventHandler(this.menuItemEditFreeze_Click);
      // 
      // menuItemEditUnfreeze
      // 
      this.menuItemEditUnfreeze.Enabled = false;
      this.menuItemEditUnfreeze.Index = 1;
      this.menuItemEditUnfreeze.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
      this.menuItemEditUnfreeze.Text = "&Unfreeze image";
      this.menuItemEditUnfreeze.Click += new System.EventHandler(this.menuItemEditUnfreeze_Click);
      // 
      // menuItemEditSep1
      // 
      this.menuItemEditSep1.Index = 2;
      this.menuItemEditSep1.Text = "-";
      // 
      // menuItemEditCrop
      // 
      this.menuItemEditCrop.Enabled = false;
      this.menuItemEditCrop.Index = 5;
      this.menuItemEditCrop.Text = "C&rop image to window size";
      this.menuItemEditCrop.Click += new System.EventHandler(this.menuItemEditCrop_Click);
      // 
      // menuItemEditSep2
      // 
      this.menuItemEditSep2.Index = 4;
      this.menuItemEditSep2.Text = "-";
      // 
      // menuItemEditCopy
      // 
      this.menuItemEditCopy.Index = 3;
      this.menuItemEditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
      this.menuItemEditCopy.Text = "&Copy image to clipboard";
      this.menuItemEditCopy.Click += new System.EventHandler(this.menuItemEditCopy_Click);
      // 
      // menuItemEditSep3
      // 
      this.menuItemEditSep3.Index = 6;
      this.menuItemEditSep3.Text = "-";
      // 
      // menuItemEditProperties
      // 
      this.menuItemEditProperties.Index = 7;
      this.menuItemEditProperties.Text = "&Properties...";
      this.menuItemEditProperties.Click += new System.EventHandler(this.menuItemEditProperties_Click);
      // 
      // menuItemView
      // 
      this.menuItemView.Index = 2;
      this.menuItemView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemViewShrinkToFit,
            this.menuItemViewSep1,
            this.menuItemViewmainStatusStrip,
            this.menuItemViewAlwaysOnTop});
      this.menuItemView.Text = "&View";
      // 
      // menuItemViewShrinkToFit
      // 
      this.menuItemViewShrinkToFit.Enabled = false;
      this.menuItemViewShrinkToFit.Index = 0;
      this.menuItemViewShrinkToFit.Text = "Size window to &fit image";
      this.menuItemViewShrinkToFit.Click += new System.EventHandler(this.menuItemViewShrinkToFit_Click);
      // 
      // menuItemViewSep1
      // 
      this.menuItemViewSep1.Index = 1;
      this.menuItemViewSep1.Text = "-";
      // 
      // menuItemViewmainStatusStrip
      // 
      this.menuItemViewmainStatusStrip.Checked = true;
      this.menuItemViewmainStatusStrip.Index = 2;
      this.menuItemViewmainStatusStrip.Shortcut = System.Windows.Forms.Shortcut.CtrlB;
      this.menuItemViewmainStatusStrip.Text = "&Status bar";
      this.menuItemViewmainStatusStrip.Click += new System.EventHandler(this.menuItemViewmainStatusStrip_Click);
      // 
      // menuItemViewAlwaysOnTop
      // 
      this.menuItemViewAlwaysOnTop.Index = 3;
      this.menuItemViewAlwaysOnTop.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
      this.menuItemViewAlwaysOnTop.Text = "Always on &top";
      this.menuItemViewAlwaysOnTop.Click += new System.EventHandler(this.menuItemViewAlwaysOnTop_Click);
      // 
      // menuItemZoom
      // 
      this.menuItemZoom.Index = 3;
      this.menuItemZoom.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemZoom1x,
            this.menuItemZoom2x,
            this.menuItemZoom4x,
            this.menuItemZoom8x,
            this.menuItemZoom16x});
      this.menuItemZoom.Text = "&Zoom";
      // 
      // menuItemZoom1x
      // 
      this.menuItemZoom1x.Index = 0;
      this.menuItemZoom1x.RadioCheck = true;
      this.menuItemZoom1x.Shortcut = System.Windows.Forms.Shortcut.Ctrl1;
      this.menuItemZoom1x.Text = "&1x";
      this.menuItemZoom1x.Click += new System.EventHandler(this.menuItemZoom1x_Click);
      // 
      // menuItemZoom2x
      // 
      this.menuItemZoom2x.Checked = true;
      this.menuItemZoom2x.Index = 1;
      this.menuItemZoom2x.RadioCheck = true;
      this.menuItemZoom2x.Shortcut = System.Windows.Forms.Shortcut.Ctrl2;
      this.menuItemZoom2x.Text = "&2x";
      this.menuItemZoom2x.Click += new System.EventHandler(this.menuItemZoom2x_Click);
      // 
      // menuItemZoom4x
      // 
      this.menuItemZoom4x.Index = 2;
      this.menuItemZoom4x.RadioCheck = true;
      this.menuItemZoom4x.Shortcut = System.Windows.Forms.Shortcut.Ctrl3;
      this.menuItemZoom4x.Text = "&4x";
      this.menuItemZoom4x.Click += new System.EventHandler(this.menuItemZoom4x_Click);
      // 
      // menuItemZoom8x
      // 
      this.menuItemZoom8x.Index = 3;
      this.menuItemZoom8x.RadioCheck = true;
      this.menuItemZoom8x.Shortcut = System.Windows.Forms.Shortcut.Ctrl4;
      this.menuItemZoom8x.Text = "&8x";
      this.menuItemZoom8x.Click += new System.EventHandler(this.menuItemZoom8x_Click);
      // 
      // menuItemZoom16x
      // 
      this.menuItemZoom16x.Index = 4;
      this.menuItemZoom16x.RadioCheck = true;
      this.menuItemZoom16x.Shortcut = System.Windows.Forms.Shortcut.Ctrl5;
      this.menuItemZoom16x.Text = "1&6x";
      this.menuItemZoom16x.Click += new System.EventHandler(this.menuItemZoom16x_Click);
      // 
      // menuItemHelp
      // 
      this.menuItemHelp.Index = 4;
      this.menuItemHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemHelpAbout});
      this.menuItemHelp.Text = "&Help";
      // 
      // menuItemHelpAbout
      // 
      this.menuItemHelpAbout.Index = 0;
      this.menuItemHelpAbout.Shortcut = System.Windows.Forms.Shortcut.F1;
      this.menuItemHelpAbout.Text = "&About MOBZoom...";
      this.menuItemHelpAbout.Click += new System.EventHandler(this.menuItemHelpAbout_Click);
      // 
      // pictureBox
      // 
      this.pictureBox.BackColor = System.Drawing.Color.Red;
      this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pictureBox.Location = new System.Drawing.Point(0, 0);
      this.pictureBox.Name = "pictureBox";
      this.pictureBox.Size = new System.Drawing.Size(454, 243);
      this.pictureBox.TabIndex = 2;
      this.pictureBox.TabStop = false;
      this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
      // 
      // mainStatusStrip
      // 
      this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelPrompt,
            this.toolStripStatusLabelPos,
            this.toolStripStatusLabelSize,
            this.toolStripStatusLabelLink});
      this.mainStatusStrip.Location = new System.Drawing.Point(0, 243);
      this.mainStatusStrip.Name = "mainStatusStrip";
      this.mainStatusStrip.Size = new System.Drawing.Size(454, 22);
      this.mainStatusStrip.TabIndex = 3;
      // 
      // toolStripStatusLabelPrompt
      // 
      this.toolStripStatusLabelPrompt.Name = "toolStripStatusLabelPrompt";
      this.toolStripStatusLabelPrompt.Size = new System.Drawing.Size(113, 17);
      this.toolStripStatusLabelPrompt.Spring = true;
      this.toolStripStatusLabelPrompt.Text = "MOBZoom";
      this.toolStripStatusLabelPrompt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // toolStripStatusLabelPos
      // 
      this.toolStripStatusLabelPos.AutoSize = false;
      this.toolStripStatusLabelPos.Name = "toolStripStatusLabelPos";
      this.toolStripStatusLabelPos.Size = new System.Drawing.Size(80, 17);
      this.toolStripStatusLabelPos.Text = "(X, Y)";
      // 
      // toolStripStatusLabelSize
      // 
      this.toolStripStatusLabelSize.AutoSize = false;
      this.toolStripStatusLabelSize.Name = "toolStripStatusLabelSize";
      this.toolStripStatusLabelSize.Size = new System.Drawing.Size(80, 17);
      this.toolStripStatusLabelSize.Text = "WxH";
      // 
      // toolStripStatusLabelLink
      // 
      this.toolStripStatusLabelLink.IsLink = true;
      this.toolStripStatusLabelLink.Name = "toolStripStatusLabelLink";
      this.toolStripStatusLabelLink.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
      this.toolStripStatusLabelLink.Size = new System.Drawing.Size(166, 17);
      this.toolStripStatusLabelLink.Text = "MOBZoom by MOBZystems";
      this.toolStripStatusLabelLink.Click += new System.EventHandler(this.toolStripStatusLabelLink_Click);
      // 
      // ZoomForm
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
      this.ClientSize = new System.Drawing.Size(454, 265);
      this.Controls.Add(this.pictureBox);
      this.Controls.Add(this.mainStatusStrip);
      this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Menu = this.mainMenu;
      this.Name = "ZoomForm";
      this.Text = "MOBZoom";
      ((System.ComponentModel.ISupportInitialize)(this.timerMouse)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
      this.mainStatusStrip.ResumeLayout(false);
      this.mainStatusStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
    #endregion

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new ZoomForm());
    }

    // Show a message box detailing the exception - TODO
    private void HandleException(Exception ex)
    {
      timerMouse.Enabled = false;
      MessageBox.Show(this, ex.Message, "An error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    // Return whether this control or any of its child controls have the specified Windows handle
    private bool AnyControlHasHandle(Control parentControl, IntPtr handle)
    {
      if (parentControl.Handle == handle)
        return true;
      foreach (Control c in parentControl.Controls)
        if (AnyControlHasHandle(c, handle))
          return true;
      return false;
    }

    // When timer fires, update display
    private void timerMouse_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      try
      {
        // When minimized, don't do anything in timer event.
        if (this.WindowState == FormWindowState.Minimized)
          return;

        // When snapshot taken, don't do anything in timer event.
        if (this.frozen)
          return;

        // Get the cursor position - in screen coordinates
        System.Drawing.Point position = Cursor.Position;

        // Store into x and y
        int x = position.X;
        int y = position.Y;

        // If over form, do NOT paint:
        if (AnyControlHasHandle(this, WindowFromPoint(position)))
        {
          toolStripStatusLabelPos.Text = "";
          pictureBox.Refresh();
          return;
        }

        // Get the size of the picture box to display the zoomed image in:
        int width = pictureBox.ClientSize.Width;
        int height = pictureBox.ClientSize.Height;

        // Calculate the distance of the rectange around the cursor position
        // to use as a SOURCE. The rectangle extends x-dx to x+dx, y-dy to y+dy
        int dx = width / this.zoom / 2;
        int dy = height / this.zoom / 2;

        //// Get the handle and position of the desktop window
        IntPtr desktopHandle = GetDesktopWindow();

        // We need this for multiple monitors!
        RECT rect = new RECT();
        rect.left = SystemInformation.VirtualScreen.X;
        rect.top = SystemInformation.VirtualScreen.Y;
        rect.right = rect.left + SystemInformation.VirtualScreen.Width;
        rect.bottom = rect.top + SystemInformation.VirtualScreen.Height;
        //GetWindowRect(desktopHandle, out rect);

        // Make sure we never use a source outside the source rectangle
        if (x < rect.left + dx)
          x = rect.left + dx;
        else if (x > rect.right - dx)
          x = rect.right - dx;

        if (y < rect.top + dy)
          y = rect.top + dy;
        else if (y > rect.bottom - dy)
          y = rect.bottom - dy;

        // Display the coordinates and size in the status bar
        toolStripStatusLabelPos.Text = "(" + x.ToString() + ", " + y.ToString() + ")";
        toolStripStatusLabelSize.Text = (width / zoom).ToString() + " x " + (height / zoom).ToString();

        // If snapshot taken, do nothing:
        if (this.frozen)
          return;

        // Perform the actual blit:
        Graphics g1 = Graphics.FromHwnd(desktopHandle);

        if (this.bitmap != null && this.bitmap.Width == width && this.bitmap.Height == height)
        {
          // We already have a bitmap
        }
        else
        {
          // Create a new one. First, dispose of the old one if necessary
          if (this.bitmap != null)
          {
            this.bitmap.Dispose();
            this.bitmap = null;
          }
          // Create a new bitmap of the right size
          this.bitmap = new Bitmap(width, height, g1);
        }

        IntPtr dc1 = g1.GetHdc();

        try
        {
          // Create a new bitmap:
          Graphics g2 = Graphics.FromImage(this.bitmap); // pictureBox.CreateGraphics();
          IntPtr dc2 = g2.GetHdc();
          try
          {
            StretchBlt(dc2, 0, 0, width, height, dc1, x - width / zoom / 2, y - height / zoom / 2, width / zoom, height / zoom, SRCCOPY);
          }
          finally
          {
            g2.ReleaseHdc(dc2);
            g2.Dispose();
          }
        }
        finally
        {
          g1.ReleaseHdc(dc1);
          g1.Dispose();
        }
        pictureBox.Invalidate();
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
    }

    // Paint the picture box. If we have a bitmap, paint it
    private void pictureBox_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
    {
      if (this.bitmap != null)
        e.Graphics.DrawImage(this.bitmap, 0, 0);
    }

    // Set/Unset Always on Top
    private void menuItemViewAlwaysOnTop_Click(object sender, System.EventArgs e)
    {
      menuItemViewAlwaysOnTop.Checked = !menuItemViewAlwaysOnTop.Checked;
      this.TopMost = menuItemViewAlwaysOnTop.Checked;
    }

    // Set zoom to 1x
    private void menuItemZoom1x_Click(object sender, System.EventArgs e)
    {
      CheckZoomMenuItem(sender);
      this.zoom = 1;
    }

    // Set zoom to 2x
    private void menuItemZoom2x_Click(object sender, System.EventArgs e)
    {
      CheckZoomMenuItem(sender);
      this.zoom = 2;
    }

    // Set zoom to 4x
    private void menuItemZoom4x_Click(object sender, System.EventArgs e)
    {
      CheckZoomMenuItem(sender);
      this.zoom = 4;
    }

    // Set zoom to 8x
    private void menuItemZoom8x_Click(object sender, System.EventArgs e)
    {
      CheckZoomMenuItem(sender);
      this.zoom = 8;
    }

    // Set zoom to 16x
    private void menuItemZoom16x_Click(object sender, System.EventArgs e)
    {
      CheckZoomMenuItem(sender);
      this.zoom = 16;
    }

    // Check a single zoom menu item. Uncheck all others
    private void CheckZoomMenuItem(object item)
    {
      MenuItem menuItem = (MenuItem)item;

      foreach (MenuItem m in menuItemZoom.MenuItems)
      {
        m.Checked = (m == menuItem);
      }
    }

    // Show/hide the status bar
    private void menuItemViewmainStatusStrip_Click(object sender, System.EventArgs e)
    {
      menuItemViewmainStatusStrip.Checked = !menuItemViewmainStatusStrip.Checked;
      mainStatusStrip.Visible = menuItemViewmainStatusStrip.Checked;
    }

    // Copy the image to the clipboard
    private void menuItemEditCopy_Click(object sender, System.EventArgs e)
    {
      if (this.bitmap != null)
      {
        Clipboard.SetDataObject(this.bitmap, true);
        toolStripStatusLabelPrompt.Text = "Copied to clipboard.";
      }
    }

    // Close the form, exit the application
    private void menuItemFileExit_Click(object sender, System.EventArgs e)
    {
      this.Close();
    }

    // Save the current bitmap
    private void menuItemFileSaveAs_Click(object sender, System.EventArgs e)
    {
      // Nothing to save
      if (this.bitmap == null)
        return;

      // New save file dialog
      SaveFileDialog sfd = new SaveFileDialog();
      sfd.Filter =
        "Bitmap files|*.bmp" +
        "|GIF files|*.gif" +
        "|JPEG files|*.jpg;*.jpeg" +
        "|PNG files|*.png" +
        "|Icon files|*.ico";
      sfd.CheckPathExists = true;
      sfd.CreatePrompt = false;
      sfd.OverwritePrompt = true;
      sfd.RestoreDirectory = false;
      sfd.ValidateNames = true;
      sfd.ShowHelp = false;
      sfd.Title = "Select a file name to save";

      if (this.lastSaveDir == null)
      {
        // Never saved before
        sfd.InitialDirectory = Environment.GetFolderPath(System.Environment.SpecialFolder.MyPictures);
        sfd.FilterIndex = 0;
      }
      else
      {
        sfd.InitialDirectory = this.lastSaveDir;
        sfd.FilterIndex = this.lastSaveFilterIndex;
      }

      // OK pressed?
      if (sfd.ShowDialog(this) == DialogResult.OK)
      {
        // Save in a predefined format
        Cursor.Current = Cursors.WaitCursor;
        try
        {
          ImageFormat imageFormat;

          switch (sfd.FilterIndex)
          {
            case 0:
            default:
              imageFormat = ImageFormat.Bmp;
              break;
            case 1:
              imageFormat = ImageFormat.Gif;
              break;
            case 2:
              imageFormat = ImageFormat.Jpeg;
              break;
            case 3:
              imageFormat = ImageFormat.Png;
              break;
            case 4:
              imageFormat = ImageFormat.Icon;
              break;
          }

          // Save the image
          this.bitmap.Save(sfd.FileName, imageFormat);

          // Update status bar
          toolStripStatusLabelPrompt.Text = "Saved " + sfd.FileName + ".";

          // Store last used values for next time:
          this.lastSaveFilterIndex = sfd.FilterIndex;
          FileInfo fileInfo = new FileInfo(sfd.FileName);
          this.lastSaveDir = fileInfo.DirectoryName;
        }
        catch (Exception ex)
        {
          HandleException(ex);
        }
        finally
        {
          Cursor.Current = Cursors.Default;
        }
      }
    }

    // Freeze the current image
    private void menuItemEditFreeze_Click(object sender, System.EventArgs e)
    {
      this.frozen = true;
      menuItemFileSaveAs.Enabled = true;
      menuItemZoom.Enabled = false;
      menuItemEditFreeze.Enabled = false;
      menuItemEditUnfreeze.Enabled = true;
      menuItemViewShrinkToFit.Enabled = true;
      menuItemEditCrop.Enabled = true;
      toolStripStatusLabelPos.Text = "Frozen";
      toolStripStatusLabelSize.Text = this.bitmap.Width.ToString() + " x " + this.bitmap.Height.ToString();
    }

    // Unfreeze the current image
    private void menuItemEditUnfreeze_Click(object sender, System.EventArgs e)
    {
      this.frozen = false;
      menuItemFileSaveAs.Enabled = false;
      menuItemZoom.Enabled = true;
      menuItemEditFreeze.Enabled = true;
      menuItemEditUnfreeze.Enabled = false;
      menuItemViewShrinkToFit.Enabled = true;
      menuItemEditCrop.Enabled = true;
      toolStripStatusLabelPos.Text = "";
    }

    private void menuItemHelpAbout_Click(object sender, System.EventArgs e)
    {
      AboutForm aboutForm = new AboutForm();
      aboutForm.Text = "About MOBZoom";
      aboutForm.labelProduct.Text = "MOBZoom";
      aboutForm.labelCopyright.Text = "(C) 2003-2008, MOBZystems B.V., Amsterdam";
      aboutForm.ShowDialog(this);
    }

    private void SetClientSize(int width, int height)
    {
      // Do this twice. The first time, the main menu may wrap/unwrap, screwing up
      // the calculation
      for (int n = 0; n < 2; n++)
      {
        // Calculate the difference between the current picture box size and the desired picture box size:
        int dx = width - pictureBox.ClientSize.Width;
        int dy = height - pictureBox.ClientSize.Height;

        // Inflate or deflate the form:
        this.Width += dx;
        this.Height += dy;
      }
    }

    // Set the client size of the picture box to the size of the bitmap:
    private void menuItemViewShrinkToFit_Click(object sender, System.EventArgs e)
    {
      if (this.bitmap == null)
        return;

      try
      {
        SetClientSize(this.bitmap.Width, this.bitmap.Height);
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
    }

    // Set the size of the bitmap to the client size of the picture box:
    private void menuItemEditCrop_Click(object sender, System.EventArgs e)
    {
      if (this.bitmap == null)
        return;

      try
      {
        // Calculate difference between current bitmap size and picture box size:
        int dx = pictureBox.ClientSize.Width - this.bitmap.Width;
        int dy = pictureBox.ClientSize.Height - this.bitmap.Height;

        // Inflate or deflate the bitmap:
        if (dx != 0 || dy != 0)
        {
          Bitmap b2 = new Bitmap(pictureBox.ClientSize.Width, pictureBox.ClientSize.Height);
          Graphics g = Graphics.FromImage(b2);
          g.DrawImage(this.bitmap, 0, 0); // , pictureBox.ClientSize.Width, pictureBox.ClientSize.Height);
          g.Dispose();
          this.bitmap.Dispose();
          this.bitmap = b2;
          pictureBox.Invalidate();
        }
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
    }

    // Edit the properties of the current image
    private void menuItemEditProperties_Click(object sender, System.EventArgs e)
    {
      try
      {
        PropertiesForm propertiesForm = new PropertiesForm();

        propertiesForm.IsOriginalSize = false;
        if (this.bitmap != null)
        {
          propertiesForm.ImageWidth = this.bitmap.Width;
          propertiesForm.ImageHeight = this.bitmap.Height;
        }
        else
        {
          propertiesForm.ImageWidth = this.pictureBox.ClientSize.Width;
          propertiesForm.ImageHeight = this.pictureBox.ClientSize.Height;
        }

        if (propertiesForm.ShowDialog(this) == DialogResult.OK)
        {
          if (propertiesForm.IsOriginalSize)
          {
            SetClientSize(propertiesForm.ImageWidth * this.zoom, propertiesForm.ImageHeight * this.zoom);
          }
          else
          {
            SetClientSize(propertiesForm.ImageWidth, propertiesForm.ImageHeight);
          }
        }
      }
      catch (Exception ex)
      {
        HandleException(ex);
      }
    }

    private void toolStripStatusLabelLink_Click(object sender, EventArgs e)
    {
      Process.Start("http://www.mobzystems.com/tools/MOBZoom.aspx");
    }
  }
}