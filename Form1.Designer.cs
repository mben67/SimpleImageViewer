
namespace SimpleImageViewer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            abrirOpenDialogFile = new System.Windows.Forms.ToolStripMenuItem();
            closeStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            viewStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            viewFullSizeStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitFullSizeStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fullScreenStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openFileLocationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            modifyStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            flipHMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            flipVMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveImageStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitSelectToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            selectToolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveSelectionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            aboutStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = System.Drawing.Color.Transparent;
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripMenuItem1, viewStripMenuItem, modifyStripMenuItem, aboutStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(1264, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { abrirOpenDialogFile, closeStripMenuItem, exitStripMenuItem });
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.ShowShortcutKeys = false;
            toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            toolStripMenuItem1.Text = "File";
            // 
            // abrirOpenDialogFile
            // 
            abrirOpenDialogFile.Name = "abrirOpenDialogFile";
            abrirOpenDialogFile.ShowShortcutKeys = false;
            abrirOpenDialogFile.Size = new System.Drawing.Size(117, 22);
            abrirOpenDialogFile.Text = "Open File";
            // 
            // closeStripMenuItem
            // 
            closeStripMenuItem.Name = "closeStripMenuItem";
            closeStripMenuItem.ShowShortcutKeys = false;
            closeStripMenuItem.Size = new System.Drawing.Size(117, 22);
            closeStripMenuItem.Text = "Close File";
            // 
            // exitStripMenuItem
            // 
            exitStripMenuItem.Name = "exitStripMenuItem";
            exitStripMenuItem.ShowShortcutKeys = false;
            exitStripMenuItem.Size = new System.Drawing.Size(117, 22);
            exitStripMenuItem.Text = "Exit";
            // 
            // viewStripMenuItem
            // 
            viewStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { viewFullSizeStripMenuItem, exitFullSizeStripMenuItem, fullScreenStripMenuItem, openFileLocationMenuItem });
            viewStripMenuItem.Name = "viewStripMenuItem";
            viewStripMenuItem.Size = new System.Drawing.Size(44, 20);
            viewStripMenuItem.Text = "View";
            // 
            // viewFullSizeStripMenuItem
            // 
            viewFullSizeStripMenuItem.Name = "viewFullSizeStripMenuItem";
            viewFullSizeStripMenuItem.ShowShortcutKeys = false;
            viewFullSizeStripMenuItem.Size = new System.Drawing.Size(176, 22);
            viewFullSizeStripMenuItem.Text = "View Real Size";
            // 
            // exitFullSizeStripMenuItem
            // 
            exitFullSizeStripMenuItem.Name = "exitFullSizeStripMenuItem";
            exitFullSizeStripMenuItem.ShowShortcutKeys = false;
            exitFullSizeStripMenuItem.Size = new System.Drawing.Size(176, 22);
            exitFullSizeStripMenuItem.Text = "View Adjusted Mode";
            // 
            // fullScreenStripMenuItem
            // 
            fullScreenStripMenuItem.Name = "fullScreenStripMenuItem";
            fullScreenStripMenuItem.ShowShortcutKeys = false;
            fullScreenStripMenuItem.Size = new System.Drawing.Size(176, 22);
            fullScreenStripMenuItem.Text = "FullScreen";
            // 
            // openFileLocationMenuItem
            // 
            openFileLocationMenuItem.Name = "openFileLocationMenuItem";
            openFileLocationMenuItem.ShowShortcutKeys = false;
            openFileLocationMenuItem.Size = new System.Drawing.Size(176, 22);
            openFileLocationMenuItem.Text = "Open File Location";
            // 
            // modifyStripMenuItem
            // 
            modifyStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { flipHMenuItem, flipVMenuItem, saveImageStripMenuItem, exitSelectToolMenuItem, selectToolMenuItem, saveSelectionMenuItem });
            modifyStripMenuItem.Name = "modifyStripMenuItem";
            modifyStripMenuItem.Size = new System.Drawing.Size(39, 20);
            modifyStripMenuItem.Text = "Edit";
            // 
            // flipHMenuItem
            // 
            flipHMenuItem.Name = "flipHMenuItem";
            flipHMenuItem.ShowShortcutKeys = false;
            flipHMenuItem.Size = new System.Drawing.Size(152, 22);
            flipHMenuItem.Text = "Flip Horizontal";
            // 
            // flipVMenuItem
            // 
            flipVMenuItem.Name = "flipVMenuItem";
            flipVMenuItem.ShowShortcutKeys = false;
            flipVMenuItem.Size = new System.Drawing.Size(152, 22);
            flipVMenuItem.Text = "Flip Vertical";
            // 
            // saveImageStripMenuItem
            // 
            saveImageStripMenuItem.Name = "saveImageStripMenuItem";
            saveImageStripMenuItem.ShowShortcutKeys = false;
            saveImageStripMenuItem.Size = new System.Drawing.Size(152, 22);
            saveImageStripMenuItem.Text = "Save Image As";
            // 
            // exitSelectToolMenuItem
            // 
            exitSelectToolMenuItem.Name = "exitSelectToolMenuItem";
            exitSelectToolMenuItem.Size = new System.Drawing.Size(152, 22);
            exitSelectToolMenuItem.Text = "Exit Select Tool";
            // 
            // selectToolMenuItem
            // 
            selectToolMenuItem.Name = "selectToolMenuItem";
            selectToolMenuItem.ShowShortcutKeys = false;
            selectToolMenuItem.Size = new System.Drawing.Size(152, 22);
            selectToolMenuItem.Text = "Select Tool";
            // 
            // saveSelectionMenuItem
            // 
            saveSelectionMenuItem.Name = "saveSelectionMenuItem";
            saveSelectionMenuItem.ShowShortcutKeys = false;
            saveSelectionMenuItem.Size = new System.Drawing.Size(152, 22);
            saveSelectionMenuItem.Text = "Save Selection";
            // 
            // aboutStripMenuItem
            // 
            aboutStripMenuItem.Name = "aboutStripMenuItem";
            aboutStripMenuItem.Size = new System.Drawing.Size(52, 20);
            aboutStripMenuItem.Text = "About";
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBox1.Location = new System.Drawing.Point(0, 24);
            pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(1264, 657);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1264, 681);
            Controls.Add(pictureBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Simple Image Viewer";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        internal System.Windows.Forms.ToolStripMenuItem viewFullSizeStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem fullScreenStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem closeStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem exitStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem abrirOpenDialogFile;
        internal System.Windows.Forms.ToolStripMenuItem flipHMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem flipVMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem saveImageStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem saveSelectionMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem selectToolMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem openFileLocationMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem exitFullSizeStripMenuItem;

        internal System.Windows.Forms.MenuStrip menuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        internal System.Windows.Forms.OpenFileDialog openFileDialog1;
        internal System.Windows.Forms.PictureBox pictureBox1;
        internal System.Windows.Forms.ToolStripMenuItem viewStripMenuItem;
        
        internal System.Windows.Forms.ToolStripMenuItem modifyStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem exitSelectToolMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem aboutStripMenuItem;
    }
}

